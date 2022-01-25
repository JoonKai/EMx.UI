// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.PartitionMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using EMx.UI.ColorConverters;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "a3cdaf6d-2a0b-4134-83d6-f99c691e7149")]
  public partial class PartitionMapControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private const int MaxColorValue = 65535;
    protected List<int> ColorTable;
    protected Dictionary<Point, byte[]> BmpBuffer;
    protected WriteableBitmap BackBuffer;
    private bool _contentLoaded;

    public PredefinedColorsGradientConverter ColorConverter { get; set; }

    public virtual MapRange Range { get; protected set; }

    public PartitionMap PMap { get; set; }

    protected UIElementDragEventHelper DragHandler { get; set; }

    public NumericPoint ImageOffset { get; protected set; }

    protected NumericPoint ImageOffsetDragStart { get; set; }

    public event Action<PartitionMapControl, NumericPoint> ImageOffsetChanged;

    protected virtual void InvokeImageOffsetChanged()
    {
      if (this.ImageOffsetChanged == null)
        return;
      this.ImageOffsetChanged(this, this.ImageOffset);
    }

    public PartitionMapControl()
    {
      this.InitializeComponent();
      this.BackBuffer = (WriteableBitmap) null;
      this.BmpBuffer = new Dictionary<Point, byte[]>();
      this.ImageOffsetDragStart = new NumericPoint();
      this.ImageOffset = new NumericPoint();
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
      this.SnapsToDevicePixels = true;
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
    }

    private void OnDragMoving(UIElementDragEventHelper dh, Point delta)
    {
      if (Helper.Data.AnyInvalid(this.ActualHeight) || this.ActualHeight == 0.0)
        return;
      this.ImageOffset.X = this.ImageOffsetDragStart.X - delta.X;
      this.ImageOffset.Y = this.ImageOffsetDragStart.Y - delta.Y;
      this.InvalidateVisual();
      this.InvokeImageOffsetChanged();
    }

    private void OnDragStart(UIElementDragEventHelper dh, UIElementDragStartArgs args) => this.ImageOffsetDragStart.CopyFrom((NumericPoint<double>) this.ImageOffset);

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
        this.BmpBuffer.Clear();
        this.ImageOffset.Set(0.0, 0.0);
        this.ImageOffsetDragStart.CopyFrom((NumericPoint<double>) this.ImageOffset);
      }
      this.InvokeImageOffsetChanged();
      this.InvalidateVisual();
    }

    public NumericPoint GetCenterPixelPosition()
    {
      NumericPoint numericPoint = new NumericPoint(this.ImageOffset.X, this.ImageOffset.Y);
      numericPoint.X += (double) (this.BackBuffer.PixelWidth / 2);
      numericPoint.Y += (double) (this.BackBuffer.PixelHeight / 2);
      return numericPoint;
    }

    protected override unsafe void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      if (this.PMap == null || !this.PMap.File.IsOpened)
        return;
      dc.PushClip((Geometry) new RectangleGeometry(new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight)));
      PartitionMap pmap = this.PMap;
      int num1 = (int) Math.Round(this.ActualWidth);
      int num2 = (int) Math.Round(this.ActualHeight);
      int partitionWidth = this.PMap.PartitionWidth;
      int partitionHeight = this.PMap.PartitionHeight;
      int x1 = (int) this.ImageOffset.X;
      int y1 = (int) this.ImageOffset.Y;
      if (this.BackBuffer == null || num1 != this.BackBuffer.PixelWidth || num2 != this.BackBuffer.PixelHeight)
        this.BackBuffer = new WriteableBitmap(num1, num2, 72.0, 72.0, PixelFormats.Bgr32, (BitmapPalette) null);
      List<Point> list = this.BmpBuffer.Keys.ToList<Point>();
      double num3 = ((double) x1 + (double) num1 / 2.0) / (double) partitionWidth;
      double num4 = ((double) y1 + (double) num2 / 2.0) / (double) partitionHeight;
      double num5 = (double) num1 / (double) partitionWidth * 3.0 / 5.0;
      double num6 = (double) num2 / (double) partitionHeight * 3.0 / 5.0;
      for (int index = 0; index < list.Count; ++index)
      {
        Point key = list[index];
        double num7 = Math.Abs(key.X - num3);
        double num8 = Math.Abs(key.Y - num4);
        if (num7 > num5 || num8 > num6)
        {
          this.BmpBuffer[key] = (byte[]) null;
          this.BmpBuffer.Remove(key);
        }
      }
      bool flag = false;
      try
      {
        lock (this)
        {
          if (flag = this.BackBuffer.TryLock((Duration) new TimeSpan(0, 0, 1)))
          {
            this.BackBuffer.Clear(Colors.White);
            int* pointer = (int*) this.BackBuffer.BackBuffer.ToPointer();
            using (Stream dataStream = this.PMap.File.GetDataStream(pmap.DataStreamName, true))
            {
              if (dataStream == null)
                return;
              for (int y2 = 0; y2 < this.PMap.Rows; ++y2)
              {
                int num9 = y2 * partitionHeight - y1;
                if (num9 <= num2 && num9 + partitionHeight >= 0)
                {
                  for (int x2 = 0; x2 < this.PMap.Columns; ++x2)
                  {
                    int num10 = x2 * partitionWidth - x1;
                    if (num10 <= num1 && num10 + partitionWidth >= 0)
                    {
                      MapItemHeader mapItemHeader = pmap.Get(x2, y2);
                      if (mapItemHeader != null)
                      {
                        Point key = new Point((double) mapItemHeader.X, (double) mapItemHeader.Y);
                        byte[] numArray;
                        if (this.BmpBuffer.ContainsKey(key))
                        {
                          numArray = this.BmpBuffer[key];
                        }
                        else
                        {
                          dataStream.Position = mapItemHeader.BytesOffset;
                          numArray = dataStream.ReadBytes(mapItemHeader.Length);
                          this.BmpBuffer[key] = numArray;
                        }
                        if (numArray.Length != mapItemHeader.Length)
                        {
                          PartitionMapControl.log.Warn("Fail to read data : {0} / {1}", (object) numArray.Length, (object) mapItemHeader.Length);
                        }
                        else
                        {
                          int width = mapItemHeader.Width;
                          int height = mapItemHeader.Height;
                          int num11 = num10 < 0 ? -num10 : 0;
                          int num12 = num9 < 0 ? -num9 : 0;
                          int num13 = num10 + width > num1 ? num1 - num10 : width - num11;
                          int num14 = num9 + height > num2 ? num2 - num9 : height - num12;
                          int num15 = num10 + num11;
                          int num16 = num9 + num12;
                          fixed (byte* numPtr1 = numArray)
                          {
                            for (int index1 = 0; index1 < num14; ++index1)
                            {
                              int* numPtr2 = pointer + num1 * (num16 + index1);
                              ushort* numPtr3 = (ushort*) (numPtr1 + ((IntPtr) (width * (num12 + index1)) * 2).ToInt64());
                              for (int index2 = 0; index2 < num13; ++index2)
                                numPtr2[num15 + index2] = this.ColorTable[(int) numPtr3[num11 + index2]];
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            this.BackBuffer.AddDirtyRect(new Int32Rect(0, 0, num1, num2));
            dc.DrawImage((ImageSource) this.BackBuffer, new Rect(0.0, 0.0, (double) num1, (double) num2));
          }
        }
      }
      finally
      {
        if (flag)
          this.BackBuffer.Unlock();
      }
      dc.Pop();
    }

    public virtual unsafe WriteableBitmap GetPartitionImage(int part_x, int part_y)
    {
      PartitionMap pmap = this.PMap;
      if (pmap == null || pmap.File == null || !pmap.File.IsOpened)
        return (WriteableBitmap) null;
      MapItemHeader mapItemHeader = pmap.Get(part_x, part_y);
      if (mapItemHeader == null)
        return (WriteableBitmap) null;
      int width = mapItemHeader.Width;
      int height = mapItemHeader.Height;
      WriteableBitmap writeableBitmap = new WriteableBitmap(mapItemHeader.Width, mapItemHeader.Height, 72.0, 72.0, PixelFormats.Bgr32, (BitmapPalette) null);
      bool flag = false;
      try
      {
        lock (this)
        {
          if (flag = writeableBitmap.TryLock((Duration) new TimeSpan(0, 0, 1)))
          {
            int* pointer = (int*) writeableBitmap.BackBuffer.ToPointer();
            using (Stream dataStream = this.PMap.File.GetDataStream(pmap.DataStreamName, true))
            {
              if (dataStream == null)
                return (WriteableBitmap) null;
              dataStream.Position = mapItemHeader.BytesOffset;
              byte[] numArray = dataStream.ReadBytes(mapItemHeader.Length);
              if (numArray.Length != mapItemHeader.Length)
              {
                PartitionMapControl.log.Warn("Fail to read data : {0} / {1}", (object) numArray.Length, (object) mapItemHeader.Length);
                return (WriteableBitmap) null;
              }
              fixed (byte* numPtr1 = numArray)
              {
                for (int index1 = 0; index1 < height; ++index1)
                {
                  int* numPtr2 = pointer + width * index1;
                  ushort* numPtr3 = (ushort*) (numPtr1 + ((IntPtr) (width * index1) * 2).ToInt64());
                  for (int index2 = 0; index2 < width; ++index2)
                    numPtr2[index2] = this.ColorTable[(int) numPtr3[index2]];
                }
              }
            }
          }
        }
      }
      finally
      {
        if (flag)
          writeableBitmap.Unlock();
      }
      return writeableBitmap;
    }

    public virtual unsafe WriteableBitmap GetEntireImage()
    {
      PartitionMap pmap = this.PMap;
      if (pmap == null || pmap.File == null || !pmap.File.IsOpened)
        return (WriteableBitmap) null;
      int partitionWidth = pmap.PartitionWidth;
      int partitionHeight = pmap.PartitionHeight;
      WriteableBitmap bmp = new WriteableBitmap(pmap.Width, pmap.Height, 72.0, 72.0, PixelFormats.Bgr32, (BitmapPalette) null);
      bool flag1 = false;
      try
      {
        lock (this)
        {
          if (flag1 = bmp.TryLock((Duration) new TimeSpan(0, 0, 1)))
          {
            bmp.Clear(Colors.White);
            IntPtr backBuffer = bmp.BackBuffer;
            int* pointer1 = (int*) backBuffer.ToPointer();
            using (Stream dataStream = this.PMap.File.GetDataStream(pmap.DataStreamName, true))
            {
              if (dataStream == null)
                return (WriteableBitmap) null;
              for (int index1 = 0; index1 < pmap.Rows; ++index1)
              {
                for (int index2 = 0; index2 < pmap.Columns; ++index2)
                {
                  MapItemHeader mapItemHeader = pmap.Get(index2, index1);
                  if (mapItemHeader != null)
                  {
                    WriteableBitmap partitionImage = this.GetPartitionImage(index2, index1);
                    if (partitionImage != null)
                    {
                      bool flag2;
                      if (flag2 = partitionImage.TryLock((Duration) new TimeSpan(0, 0, 1)))
                      {
                        backBuffer = partitionImage.BackBuffer;
                        int* pointer2 = (int*) backBuffer.ToPointer();
                        for (int index3 = 0; index3 < mapItemHeader.Height; ++index3)
                        {
                          int* numPtr1 = pointer1 + (index1 * partitionHeight + index3) * pmap.Width + index2 * partitionWidth;
                          int* numPtr2 = pointer2 + mapItemHeader.Width * index3;
                          for (int index4 = 0; index4 < mapItemHeader.Width; ++index4)
                            numPtr1[index4] = numPtr2[index4];
                        }
                      }
                      if (flag2)
                        partitionImage.Unlock();
                    }
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
        if (flag1)
          bmp.Unlock();
      }
      return bmp;
    }

    private void mnuSaveAsImage_Clicked(object sender, RoutedEventArgs e)
    {
      string str = Application.Current.MainWindow.SaveFileDialog((string) null, "PNG Image(*.png)|*.png", "png");
      if (string.IsNullOrWhiteSpace(str))
        return;
      try
      {
        this.GetEntireImage().SaveFileAsPng(str);
        if (Application.Current.MainWindow.ShowNormalMessage("Manager", "Complete Export.\r\nDo you like to open the exported image?", eDialogButtons.YesNo) != eDialogButtons.Yes)
          return;
        Process.Start(str);
      }
      catch (Exception ex)
      {
        PartitionMapControl.log.Error(ex, ex.Message);
        int num1 = (int) GC.WaitForFullGCComplete();
        int num2 = (int) Application.Current.MainWindow.ShowErrorMessage("Error", ex.Message);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/partitionmaps/partitionmapcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((MenuItem) target).Click += new RoutedEventHandler(this.mnuSaveAsImage_Clicked);
      else
        this._contentLoaded = true;
    }
  }
}
