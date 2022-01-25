// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.ScalablePartitionMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data;
using EMx.Extensions;
using EMx.Graphics.PNGs;
using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using EMx.UI.ColorConverters;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using EMx.UI.PartitionMaps.MapPins;
using EMx.UI.Properties;
using EMx.UI.WPFs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "6f6863a9-c436-42d6-9280-b7a1bb302fe6")]
  public partial class ScalablePartitionMapControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private const int MaxColorValue = 65535;
    protected List<int> ColorTable;
    protected Dictionary<Point, ushort[]> BmpBuffer;
    protected WriteableBitmap WBuffer;
    protected PartitionMap PreviousPartitionMap;
    protected double PreviousZoomRatio = 0.0;
    protected ToolTip TooltipControl;
    private bool _contentLoaded;

    public PredefinedColorsGradientConverter ColorConverter { get; set; }

    public virtual MapRange Range { get; protected set; }

    protected UIElementDragEventHelper DragHandler { get; set; }

    public NumericPoint CenterPosition { get; protected set; }

    protected NumericPoint CenterPositionDragStart { get; set; }

    public List<PartitionMap> PartitionMaps { get; set; }

    public MapPinGroup PartitionMapPins { get; set; }

    public double ZoomRatio { get; set; }

    public double SourceZoomRatio { get; set; }

    public bool AllowSmoothZooming { get; set; }

    public event Action<ScalablePartitionMapControl, NumericPoint> CenterPositionChangedEvent;

    protected virtual void InvokeCenterPositionChanged()
    {
      Action<ScalablePartitionMapControl, NumericPoint> positionChangedEvent = this.CenterPositionChangedEvent;
      if (positionChangedEvent == null)
        return;
      positionChangedEvent(this, this.CenterPosition);
    }

    public event Action<ScalablePartitionMapControl, double> ZoomRationChangedEvent;

    protected virtual void InvokeZoomRatioChanged()
    {
      Action<ScalablePartitionMapControl, double> rationChangedEvent = this.ZoomRationChangedEvent;
      if (rationChangedEvent == null)
        return;
      rationChangedEvent(this, this.ZoomRatio);
    }

    protected RenderingFigureTooltipManager TooltipManager { get; set; }

    public ScalablePartitionMapControl()
    {
      this.InitializeComponent();
      this.ZoomRatio = 1.0;
      this.PartitionMaps = new List<PartitionMap>();
      this.PartitionMapPins = new MapPinGroup();
      this.WBuffer = (WriteableBitmap) null;
      this.BmpBuffer = new Dictionary<Point, ushort[]>();
      this.TooltipControl = new ToolTip();
      this.TooltipManager = new RenderingFigureTooltipManager();
      this.CenterPositionDragStart = new NumericPoint();
      this.CenterPosition = new NumericPoint();
      this.DragHandler = new UIElementDragEventHelper();
      this.DragHandler.DragDirection = UIElementDragEventHelper.eDragDirection.Both;
      this.DragHandler.Register((UIElement) this);
      this.DragHandler.DragStart += new Action<UIElementDragEventHelper, UIElementDragStartArgs>(this.OnDragStart);
      this.DragHandler.DragMoving += new Action<UIElementDragEventHelper, Point>(this.OnDragMoving);
      this.ColorConverter = new PredefinedColorsGradientConverter();
      this.ColorTable = new List<int>();
      this.ColorTable.Resize<int>(65536, (Func<int>) (() => 0));
      this.Range = new MapRange();
      this.Range.UseAutoRange = false;
      this.Range.Set(0.0, (double) ushort.MaxValue);
      this.Range.UseDistribution = true;
      this.Range.InclusiveBegin = true;
      this.Range.InclusiveEnd = true;
      this.Range.RangeChanged += new Action<MapRange>(this.OnRangeValueChanged);
      this.OnRangeValueChanged(this.Range);
      this.AllowSmoothZooming = true;
      this.PreviousZoomRatio = 0.0;
      this.PreviousPartitionMap = (PartitionMap) null;
      this.SnapsToDevicePixels = true;
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
    }

    public PartitionMap CurrentPartitionMap
    {
      get
      {
        if (this.PartitionMaps.Count > 0)
        {
          for (int index = 0; index < this.PartitionMaps.Count; ++index)
          {
            PartitionMap partitionMap = this.PartitionMaps[index];
            if (this.ZoomRatio >= partitionMap.ZoomScale)
              return partitionMap;
          }
          PartitionMap partitionMap1 = this.PartitionMaps.First<PartitionMap>();
          PartitionMap partitionMap2 = this.PartitionMaps.Last<PartitionMap>();
          if (this.ZoomRatio > partitionMap1.ZoomScale)
            return partitionMap1;
          if (this.ZoomRatio < partitionMap2.ZoomScale)
            return partitionMap2;
        }
        return (PartitionMap) null;
      }
    }

    public virtual NumericPoint ConvertViewToRealPosition(NumericPoint view_pt)
    {
      PartitionMap previousPartitionMap = this.PreviousPartitionMap;
      if (previousPartitionMap == null || this.WBuffer == null)
        return (NumericPoint) null;
      int partitionWidth = previousPartitionMap.PartitionWidth;
      int partitionHeight = previousPartitionMap.PartitionHeight;
      double partitionRealW = previousPartitionMap.PartitionRealW;
      double partitionRealH = previousPartitionMap.PartitionRealH;
      int pixelWidth = this.WBuffer.PixelWidth;
      int pixelHeight = this.WBuffer.PixelHeight;
      double x = view_pt.X;
      double y = view_pt.Y;
      double num1 = (x - (double) (pixelWidth / 2)) * partitionRealW / (double) partitionWidth;
      double num2 = (y - (double) (pixelHeight / 2)) * partitionRealH / (double) partitionHeight;
      if (this.AllowSmoothZooming)
      {
        double num3 = previousPartitionMap.ZoomScale / this.ZoomRatio;
        num1 *= num3;
        num2 *= num3;
      }
      return new NumericPoint(Math.Floor(this.CenterPosition.X + num1), Math.Floor(this.CenterPosition.Y + num2));
    }

    public virtual NumericVector GetValueUsingViewPosition(NumericPoint view_pos)
    {
      PartitionMap previousPartitionMap = this.PreviousPartitionMap;
      if (previousPartitionMap == null || this.WBuffer == null)
        return (NumericVector) null;
      NumericPoint realPosition = this.ConvertViewToRealPosition(view_pos);
      if (realPosition == null)
        return (NumericVector) null;
      int partitionWidth = previousPartitionMap.PartitionWidth;
      int partitionHeight = previousPartitionMap.PartitionHeight;
      double partitionRealW = previousPartitionMap.PartitionRealW;
      double partitionRealH = previousPartitionMap.PartitionRealH;
      double x1 = realPosition.X;
      double y1 = realPosition.Y;
      double num1 = Math.Floor(x1 - previousPartitionMap.RealOffsetX);
      double num2 = Math.Floor(y1 - previousPartitionMap.RealOffsetY);
      if (num1 < 0.0 || num2 < 0.0)
        return (NumericVector) null;
      int x2 = (int) (num1 / partitionRealW);
      int y2 = (int) (num2 / partitionRealH);
      MapItemHeader mapItemHeader = previousPartitionMap.Get(x2, y2);
      if (mapItemHeader == null)
        return (NumericVector) null;
      Point key = new Point((double) mapItemHeader.X, (double) mapItemHeader.Y);
      if (this.BmpBuffer.ContainsKey(key))
      {
        ushort[] numArray = this.BmpBuffer[key];
        int num3 = (int) (Math.Floor(num1 - (double) x2 * partitionRealW) * (double) partitionWidth / partitionRealW);
        int index = (int) (Math.Floor(num2 - (double) y2 * partitionRealH) * (double) partitionHeight / partitionRealH) * mapItemHeader.Width + num3;
        if (index > -1 && index < numArray.Length)
        {
          ushort num4 = numArray[index];
          return new NumericVector(x1, y1, (double) num4);
        }
      }
      return (NumericVector) null;
    }

    internal void SetCenterPosition(NumericPoint arg2)
    {
      this.CenterPosition = arg2;
      this.InvalidateVisual();
    }

    internal void SetZoom(double arg2)
    {
    }

    private void OnDragMoving(UIElementDragEventHelper dh, Point delta)
    {
      if (Helper.Data.AnyInvalid(this.ActualHeight) || this.ActualHeight == 0.0)
        return;
      PartitionMap previousPartitionMap = this.PreviousPartitionMap;
      if (previousPartitionMap == null)
        return;
      double x = delta.X;
      double y = delta.Y;
      PartitionMap currentPartitionMap = this.CurrentPartitionMap;
      if (this.AllowSmoothZooming && currentPartitionMap != null)
      {
        double num = currentPartitionMap.ZoomScale / this.ZoomRatio;
        x *= num;
        y *= num;
      }
      double num1 = x * previousPartitionMap.PartitionRealW / (double) previousPartitionMap.PartitionWidth;
      double num2 = y * previousPartitionMap.PartitionRealH / (double) previousPartitionMap.PartitionHeight;
      this.CenterPosition.X = Math.Floor(this.CenterPositionDragStart.X - num1 + 0.5);
      this.CenterPosition.Y = Math.Floor(this.CenterPositionDragStart.Y - num2 + 0.5);
      this.InvalidateVisual();
      this.InvokeCenterPositionChanged();
    }

    private void OnDragStart(UIElementDragEventHelper dh, UIElementDragStartArgs args) => this.CenterPositionDragStart.CopyFrom((NumericPoint<double>) this.CenterPosition);

    private void OnRangeValueChanged(MapRange range)
    {
      lock (this)
      {
        this.Range.Distributions.Resize<long>(this.ColorConverter.ColorSet.Count, (Func<long>) (() => 100L));
        for (int index = 0; index <= (int) ushort.MaxValue; ++index)
          this.ColorTable[index] = this.ColorConverter.ValueToColor(this.Range.Begin, this.Range.End, (double) index).ToInt();
      }
      this.InvalidateVisual();
    }

    public void ResetView()
    {
      lock (this)
      {
        this.SourceZoomRatio = 1.0;
        this.PreviousZoomRatio = 0.0;
        this.PreviousPartitionMap = (PartitionMap) null;
        this.BmpBuffer.Clear();
        this.CenterPosition.Set(0.0, 0.0);
        this.CenterPositionDragStart.CopyFrom((NumericPoint<double>) this.CenterPosition);
      }
      this.InvokeCenterPositionChanged();
      this.InvalidateVisual();
    }

    public void MoveCenterPositionTo(double x, double y)
    {
      lock (this)
        this.CenterPosition.Set(x, y);
      this.InvokeCenterPositionChanged();
      this.InvalidateVisual();
    }

    protected override unsafe void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      PartitionMap currentPartitionMap = this.CurrentPartitionMap;
      if (currentPartitionMap == null || !currentPartitionMap.File.IsOpened)
        return;
      dc.PushClip((Geometry) new RectangleGeometry(new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)));
      int num1 = (int) Math.Round(this.ActualWidth);
      int num2 = (int) Math.Round(this.ActualHeight);
      int partitionWidth = currentPartitionMap.PartitionWidth;
      int partitionHeight = currentPartitionMap.PartitionHeight;
      double partitionRealW = currentPartitionMap.PartitionRealW;
      double partitionRealH = currentPartitionMap.PartitionRealH;
      double num3 = this.ZoomRatio / currentPartitionMap.ZoomScale;
      if (this.WBuffer == null || num1 != this.WBuffer.PixelWidth || num2 != this.WBuffer.PixelHeight)
        this.WBuffer = new WriteableBitmap(num1, num2, 72.0, 72.0, PixelFormats.Bgr32, (BitmapPalette) null);
      int pixelWidth = this.WBuffer.PixelWidth;
      int pixelHeight = this.WBuffer.PixelHeight;
      double num4 = Math.Floor(this.CenterPosition.X) - currentPartitionMap.RealOffsetX - (double) pixelWidth / 2.0 * currentPartitionMap.PartitionRealW / (double) partitionWidth;
      double num5 = Math.Floor(this.CenterPosition.Y) - currentPartitionMap.RealOffsetY - (double) pixelHeight / 2.0 * currentPartitionMap.PartitionRealH / (double) partitionHeight;
      double num6 = num4 * (double) partitionWidth / currentPartitionMap.PartitionRealW;
      double num7 = num5 * (double) partitionHeight / currentPartitionMap.PartitionRealH;
      if (this.PreviousPartitionMap != currentPartitionMap)
      {
        this.BmpBuffer.Clear();
      }
      else
      {
        List<Point> list = this.BmpBuffer.Keys.ToList<Point>();
        double num8 = (num6 + (double) num1 / 2.0) / (double) partitionWidth;
        double num9 = (num7 + (double) num2 / 2.0) / (double) partitionHeight;
        double num10 = (double) num1 / (double) partitionWidth * 3.0 / 5.0;
        double num11 = (double) num2 / (double) partitionHeight * 3.0 / 5.0;
        for (int index = 0; index < list.Count; ++index)
        {
          Point key = list[index];
          double num12 = Math.Abs(key.X - num8);
          double num13 = Math.Abs(key.Y - num9);
          if (num12 > num10 || num13 > num11)
          {
            this.BmpBuffer[key] = (ushort[]) null;
            this.BmpBuffer.Remove(key);
          }
        }
      }
      bool flag = false;
      try
      {
        lock (this)
        {
          if (flag = this.WBuffer.TryLock((Duration) new TimeSpan(0, 0, 1)))
          {
            this.WBuffer.Clear(Colors.White);
            int* pointer = (int*) this.WBuffer.BackBuffer.ToPointer();
            using (Stream dataStream = currentPartitionMap.File.GetDataStream(currentPartitionMap.DataStreamName, true))
            {
              if (dataStream == null)
                return;
              for (int y = 0; y < currentPartitionMap.Rows; ++y)
              {
                int num14 = (int) ((double) (y * partitionHeight) - num7);
                if (num14 <= num2 && num14 + partitionHeight >= 0)
                {
                  for (int x = 0; x < currentPartitionMap.Columns; ++x)
                  {
                    int num15 = (int) ((double) (x * partitionWidth) - num6);
                    if (num15 <= num1 && num15 + partitionWidth >= 0)
                    {
                      MapItemHeader mapItemHeader = currentPartitionMap.Get(x, y);
                      if (mapItemHeader != null)
                      {
                        Point key = new Point((double) mapItemHeader.X, (double) mapItemHeader.Y);
                        ushort[] numArray;
                        if (this.BmpBuffer.ContainsKey(key))
                        {
                          numArray = this.BmpBuffer[key];
                        }
                        else
                        {
                          dataStream.Position = mapItemHeader.BytesOffset;
                          byte[] source = dataStream.ReadBytes(mapItemHeader.Length);
                          numArray = new ushort[mapItemHeader.Length / 2];
                          fixed (ushort* numPtr = numArray)
                            Marshal.Copy(source, 0, (IntPtr) (void*) numPtr, mapItemHeader.Length);
                          this.BmpBuffer[key] = numArray;
                        }
                        if (numArray == null || numArray.Length != mapItemHeader.Length / 2)
                        {
                          ScalablePartitionMapControl.log.Warn("Fail to read data : {0} / {1}", (object) numArray.Length, (object) mapItemHeader.Length);
                        }
                        else
                        {
                          int width = mapItemHeader.Width;
                          int height = mapItemHeader.Height;
                          int num16 = num15 < 0 ? -num15 : 0;
                          int num17 = num14 < 0 ? -num14 : 0;
                          int num18 = num15 + width > num1 ? num1 - num15 : width - num16;
                          int num19 = num14 + height > num2 ? num2 - num14 : height - num17;
                          int num20 = num15 + num16;
                          int num21 = num14 + num17;
                          for (int index1 = 0; index1 < num19; ++index1)
                          {
                            int* numPtr = pointer + num1 * (num21 + index1);
                            int num22 = width * (num17 + index1) + num16;
                            for (int index2 = 0; index2 < num18; ++index2)
                              numPtr[num20 + index2] = this.ColorTable[(int) numArray[num22 + index2]];
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            this.WBuffer.AddDirtyRect(new Int32Rect(0, 0, num1, num2));
            if (this.AllowSmoothZooming)
            {
              dc.PushTransform((Transform) new ScaleTransform(num3, num3, (double) (num1 / 2), (double) (num2 / 2)));
              dc.DrawImage((ImageSource) this.WBuffer, new Rect(0.0, 0.0, (double) num1, (double) num2));
              dc.Pop();
            }
            else
              dc.DrawImage((ImageSource) this.WBuffer, new Rect(0.0, 0.0, (double) num1, (double) num2));
            List<MapPinItem> mapPinItemList = new List<MapPinItem>();
            this.TooltipManager.Clear();
            lock (this.PartitionMapPins)
              mapPinItemList.AddRange((IEnumerable<MapPinItem>) this.PartitionMapPins.Items);
            BitmapSource imageSource = Resources.map_pin30.ToImageSource();
            double width1 = imageSource.Width;
            double height1 = imageSource.Height;
            NumericPoint realPosition = this.ConvertViewToRealPosition(new NumericPoint((double) (num1 / 2), (double) (num2 / 2)));
            if (realPosition != null)
            {
              for (int index = 0; index < mapPinItemList.Count; ++index)
              {
                MapPinItem mapPinItem = mapPinItemList[index];
                double num23 = mapPinItem.LocationX - realPosition.X;
                double num24 = mapPinItem.LocationY - realPosition.Y;
                double num25 = (double) (num1 / 2) + num23 * (double) partitionWidth / partitionRealW * num3;
                double num26 = (double) (num2 / 2) + num24 * (double) partitionHeight / partitionRealH * num3;
                if (num25 >= 0.0 && num25 < (double) pixelWidth && num26 >= 0.0 && num26 < (double) pixelHeight)
                {
                  Rect rect = new Rect(num25 - width1 / 2.0, num26 - height1, width1, height1);
                  dc.DrawImage((ImageSource) imageSource, rect);
                  this.TooltipManager.AddTooltip(rect, mapPinItem.PinName);
                }
              }
            }
          }
        }
      }
      finally
      {
        if (flag)
          this.WBuffer.Unlock();
      }
      dc.Pop();
      this.SourceZoomRatio = currentPartitionMap.ZoomScale;
      if (this.PreviousZoomRatio != this.ZoomRatio)
        this.InvokeZoomRatioChanged();
      this.PreviousPartitionMap = currentPartitionMap;
      this.PreviousZoomRatio = this.ZoomRatio;
    }

    public virtual unsafe bool ExportEntireImageAsFile(
      string file_path,
      Mutable<bool> run,
      Mutable<double> pro)
    {
      PartitionMap previousPartitionMap = this.PreviousPartitionMap;
      if (previousPartitionMap == null || previousPartitionMap.File == null || !previousPartitionMap.File.IsOpened)
        return false;
      int pw = previousPartitionMap.PartitionWidth;
      int partitionHeight = previousPartitionMap.PartitionHeight;
      pro.Value = 0.0;
      SimplePngExporter simplePngExporter = new SimplePngExporter(previousPartitionMap.Width, previousPartitionMap.Height, ePngColorType.RGBAlpha);
      try
      {
        lock (this)
        {
          using (Stream stream = simplePngExporter.OpenData(file_path))
          {
            using (Stream dataStream = previousPartitionMap.File.GetDataStream(previousPartitionMap.DataStreamName, true))
            {
              if (dataStream == null || stream == null)
                return false;
              List<byte[]> lines = new List<byte[]>();
              for (int index = 0; index < partitionHeight; ++index)
                lines.Add(new byte[previousPartitionMap.Width * 4]);
              List<uint> rgba_palette = new List<uint>();
              for (int index = 0; index < this.ColorTable.Count; ++index)
              {
                uint num1 = (uint) (this.ColorTable[index] & -1);
                uint num2 = (uint) ((int) (num1 >> 16) & (int) byte.MaxValue | (int) num1 & -16711936 | (int) num1 << 16 & 16711680);
                rgba_palette.Add(num2);
              }
              int num3 = 0;
              for (int y = 0; y < previousPartitionMap.Rows; ++y)
              {
                pro.Value = ((double) y + 1.0) / (double) previousPartitionMap.Rows;
                List<byte[]> parts = new List<byte[]>();
                List<MapItemHeader> hds = new List<MapItemHeader>();
                for (int x = 0; x < previousPartitionMap.Columns; ++x)
                {
                  MapItemHeader mapItemHeader = previousPartitionMap.Get(x, y);
                  if (mapItemHeader != null)
                  {
                    hds.Add(mapItemHeader);
                    dataStream.Position = mapItemHeader.BytesOffset;
                    byte[] numArray = dataStream.ReadBytes(mapItemHeader.Length);
                    parts.Add(numArray);
                  }
                }
                Parallel.For(0, partitionHeight, (Action<int>) (j =>
                {
                  fixed (byte* numPtr19 = lines[j])
                  {
                    for (int index19 = 0; index19 < parts.Count; ++index19)
                    {
                      MapItemHeader mapItemHeader = hds[index19];
                      if (j < mapItemHeader.Height)
                      {
                        int num40 = j * mapItemHeader.Width;
                        fixed (byte* numPtr20 = parts[index19])
                        {
                          int num41 = Math.Min(pw, mapItemHeader.Width);
                          int num42 = index19 * pw;
                          for (int index20 = 0; index20 < num41; ++index20)
                          {
                            uint num43 = rgba_palette[(int) ((ushort*) numPtr20)[num40 + index20]];
                            *(int*) (numPtr19 + ((IntPtr) (index19 * pw + index20) * 4).ToInt64()) = (int) num43;
                          }
                        }
                      }
                      else
                        break;
                    }
                  }
                }));
                for (int index = 0; index < partitionHeight; ++index)
                {
                  if (num3++ < previousPartitionMap.Height)
                  {
                    stream.WriteByte((byte) 0);
                    stream.Write(lines[index], 0, lines[index].Length);
                  }
                }
              }
            }
          }
          return simplePngExporter.CloseData();
        }
      }
      catch (Exception ex)
      {
        ScalablePartitionMapControl.log.Error(ex, ex.Message);
        return false;
      }
    }

    public virtual bool AccumulatePartialRawData(
      Mutable<bool> run,
      Mutable<double> pro,
      int offx,
      int offy,
      int w,
      int h,
      Action<ushort[], int> accumulator)
    {
      PartitionMap previousPartitionMap = this.PreviousPartitionMap;
      if (previousPartitionMap == null || previousPartitionMap.File == null || !previousPartitionMap.File.IsOpened || offx < 0 || offy < 0 || w <= 0 || h <= 0)
        return false;
      int partitionWidth = previousPartitionMap.PartitionWidth;
      int partitionHeight = previousPartitionMap.PartitionHeight;
      pro.Value = 0.0;
      try
      {
        int num1 = offx / partitionWidth;
        int num2 = offy / partitionHeight;
        int num3 = (offx + w + partitionWidth - 1) / partitionWidth;
        int num4 = (offy + h + partitionHeight - 1) / partitionHeight;
        int num5 = num3 - num1;
        int num6 = num4 - num2;
        int sourceIndex = offx % partitionWidth;
        lock (this)
        {
          using (Stream dataStream = previousPartitionMap.File.GetDataStream(previousPartitionMap.DataStreamName, true))
          {
            if (dataStream == null)
              return false;
            int length = num5 * partitionWidth;
            List<ushort[]> numArrayList = new List<ushort[]>();
            for (int index = 0; index < partitionHeight; ++index)
              numArrayList.Add(new ushort[length]);
            for (int y = num2; y < num4; ++y)
            {
              pro.Value = 1.0 * (double) (y + 1) / (double) num6;
              for (int x = num1; x < num3; ++x)
              {
                MapItemHeader mapItemHeader = previousPartitionMap.Get(x, y);
                if (mapItemHeader == null)
                {
                  ScalablePartitionMapControl.log.Error("Fail to get block({0},{1}).", (object) x, (object) y);
                  return false;
                }
                int dstOffset = (x - num1) * partitionWidth * 2;
                int count = mapItemHeader.Width * 2;
                dataStream.Position = mapItemHeader.BytesOffset;
                byte[] numArray = dataStream.ReadBytes(mapItemHeader.Length);
                for (int index = 0; index < mapItemHeader.Height; ++index)
                  Buffer.BlockCopy((Array) numArray, count * index, (Array) numArrayList[index], dstOffset, count);
              }
              ushort[] numArray1 = new ushort[w];
              for (int index = 0; index < partitionHeight; ++index)
              {
                int num7 = y * partitionHeight + index;
                if (num7 >= offy && num7 < offy + h)
                {
                  Array.Copy((Array) numArrayList[index], sourceIndex, (Array) numArray1, 0, w);
                  Array.Clear((Array) numArrayList[index], 0, length);
                  accumulator(numArray1, num7);
                }
              }
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        ScalablePartitionMapControl.log.Error(ex, ex.Message);
        return false;
      }
    }

    private void UserControl_MouseMove(object sender, MouseEventArgs e)
    {
      RenderingFigureTooltip renderingFigureTooltip = this.TooltipManager.Find(e.GetPosition((IInputElement) this));
      if (renderingFigureTooltip != null)
      {
        this.TooltipControl.Content = (object) renderingFigureTooltip.Tooltip;
        this.TooltipControl.IsOpen = true;
        this.ToolTip = (object) this.TooltipControl;
      }
      else
      {
        this.TooltipControl.IsOpen = false;
        this.ToolTip = (object) null;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/partitionmaps/scalablepartitionmapcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((UIElement) target).MouseMove += new MouseEventHandler(this.UserControl_MouseMove);
      else
        this._contentLoaded = true;
    }
  }
}
