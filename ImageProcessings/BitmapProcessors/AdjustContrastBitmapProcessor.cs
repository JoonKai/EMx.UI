// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.AdjustContrastBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "86a5ca7b-99ff-4e44-97ef-71e9ca29bf05")]
  public class AdjustContrastBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    [Description("Range : 0 ~ 50, Percentage unit.")]
    public virtual double UpDownCuttingRatio { get; set; }

    public AdjustContrastBitmapProcessor() => this.UpDownCuttingRatio = 0.1;

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      double width = bmp.Width;
      double height = bmp.Height;
      List<byte> byteList = bmp.ToByteList();
      byteList.Sort();
      int num1 = byteList.Count - 1;
      byte lower = byteList[(int) (this.UpDownCuttingRatio * (double) num1 / 100.0)];
      int diff = (int) byteList[(int) ((100.0 - this.UpDownCuttingRatio) * (double) num1 / 100.0)] - (int) lower;
      if (diff > 0)
        return bmp.SelectEx((Func<byte, byte, byte, SimpleRGB>) ((r, g, b) =>
        {
          byte num3 = (byte) Math.Max(0, Math.Min((int) byte.MaxValue, ((int) r - (int) lower) * (int) byte.MaxValue / diff));
          return SimpleRGB.From(num3, num3, num3);
        }));
      AdjustContrastBitmapProcessor.log.Error("Invalid range. UpDownCuttingRatio({0})", (object) this.UpDownCuttingRatio);
      return bmp;
    }

    public override string ToString() => string.Format("Contrast[{0}] : Cutting({1:0.00})", (object) this.Name, (object) this.UpDownCuttingRatio);
  }
}
