// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.PartitionMap
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data;
using EMx.Extensions;
using EMx.IO.MxData;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "b6b391bd-d846-4448-bda2-f0ffb45a38e3")]
  public class PartitionMap
  {
    private static ILog log = LogManager.GetLogger();
    [InstanceMember]
    protected MapItemHeader[,] ItemHeaders;

    [InstanceMember]
    public int PartitionWidth { get; set; }

    [InstanceMember]
    public int PartitionHeight { get; set; }

    [InstanceMember]
    public double PartitionRealW { get; set; }

    [InstanceMember]
    public double PartitionRealH { get; set; }

    [InstanceMember]
    public double RealOffsetX { get; set; }

    [InstanceMember]
    public double RealOffsetY { get; set; }

    [InstanceMember]
    public int BytesPerData { get; set; }

    [InstanceMember]
    public int Columns { get; set; }

    [InstanceMember]
    public int Rows { get; set; }

    [InstanceMember]
    public int Width { get; set; }

    [InstanceMember]
    public int Height { get; set; }

    [InstanceMember]
    public string DataStreamName { get; set; }

    [InstanceMember]
    public string StreamName { get; set; }

    [InstanceMember]
    public double ZoomScale { get; set; }

    [InstanceMember]
    public List<long> Histogram { get; set; }

    public MxFile File { get; set; }

    public Mutable<double> ProgressRate { get; set; }

    public PartitionMap()
      : this((MxFile) null)
    {
    }

    public PartitionMap(MxFile file)
    {
      this.ZoomScale = 1.0;
      this.StreamName = "";
      this.DataStreamName = "";
      this.File = file;
      this.ItemHeaders = new MapItemHeader[0, 0];
      this.Histogram = new List<long>();
      this.ProgressRate = new Mutable<double>();
    }

    public NumericRange GetHistogramRange()
    {
      NumericRange numericRange = new NumericRange();
      if (this.Histogram != null)
      {
        for (int index = 0; index < this.Histogram.Count; ++index)
        {
          if ((ulong) this.Histogram[index] > 0UL)
          {
            numericRange.Begin = (double) index;
            break;
          }
        }
        for (int index = this.Histogram.Count - 1; index > -1; --index)
        {
          if ((ulong) this.Histogram[index] > 0UL)
          {
            numericRange.End = (double) index;
            break;
          }
        }
      }
      return numericRange;
    }

    public unsafe void CreatePartitionMap(
      int pw,
      int ph,
      double real_offx,
      double real_offy,
      double real_pw,
      double real_ph,
      List<ushort[]> data,
      int data_w,
      int data_h,
      string stream_name)
    {
      Mutable<double> progressRate = this.ProgressRate;
      progressRate.Value = 0.0;
      int num1 = 2;
      this.StreamName = stream_name;
      this.DataStreamName = stream_name + "_data";
      this.PartitionWidth = pw;
      this.PartitionHeight = ph;
      this.BytesPerData = num1;
      this.PartitionRealW = real_pw;
      this.PartitionRealH = real_ph;
      this.Width = data_w;
      this.Height = data_h;
      this.RealOffsetX = real_offx;
      this.RealOffsetY = real_offy;
      this.Columns = (data_w + pw - 1) / pw;
      this.Rows = (data_h + ph - 1) / ph;
      this.ItemHeaders = new MapItemHeader[this.Rows, this.Columns];
      this.Histogram.Clear();
      this.Histogram.Resize<long>(256, (Func<long>) (() => 0L));
      long num2 = 0;
      using (Stream dataStream = this.File.GetOrCreateDataStream(this.DataStreamName))
      {
        double num3 = 1.0 / (double) this.Rows;
        for (int index1 = 0; index1 < this.Rows; ++index1)
        {
          progressRate.Value += num3;
          int num4 = index1 * ph;
          int num5 = Math.Min(num4 + ph, data_h) - num4;
          for (int index2 = 0; index2 < this.Columns; ++index2)
          {
            int num6 = index2 * pw;
            int num7 = Math.Min(num6 + pw, data_w) - num6;
            byte[] numArray = new byte[num5 * num7 * num1];
            for (int index3 = 0; index3 < num5; ++index3)
            {
              fixed (ushort* numPtr = data[num4 + index3])
                Marshal.Copy((IntPtr) (void*) (numPtr + num6), numArray, index3 * num7 * num1, num7 * num1);
            }
            this.ItemHeaders[index1, index2] = new MapItemHeader()
            {
              BytesOffset = num2,
              Length = numArray.Length,
              X = index2,
              Y = index1,
              RealX = (float) (real_offx + real_pw * (double) index2),
              RealY = (float) (real_offy + real_ph * (double) index1),
              Width = num7,
              Height = num5,
              RealW = (float) real_pw * (float) num7 / (float) pw,
              RealH = (float) real_ph * (float) num5 / (float) ph
            };
            dataStream.Write(numArray, 0, numArray.Length);
            num2 += (long) numArray.Length;
            for (int index4 = 1; index4 < numArray.Length; index4 += 2)
              ++this.Histogram[(int) numArray[index4]];
          }
        }
      }
      Tuple<byte[], List<string>> memory = InstanceSerializer.WriteToMemory((object) this);
      InstanceSerializer.LogSerializationResult(memory.Item2);
      using (Stream dataStream = this.File.GetOrCreateDataStream(this.StreamName))
        dataStream.Write(memory.Item1, 0, memory.Item1.Length);
    }

    public virtual bool IsValidRange(int x, int y) => x >= 0 && y >= 0 && x < this.Columns && y < this.Rows;

    public virtual MapItemHeader Get(int x, int y) => !this.IsValidRange(x, y) ? (MapItemHeader) null : this.ItemHeaders[y, x];

    public virtual bool Set(int x, int y, MapItemHeader header)
    {
      if (!this.IsValidRange(x, y))
        return false;
      this.ItemHeaders[y, x] = header;
      return true;
    }
  }
}
