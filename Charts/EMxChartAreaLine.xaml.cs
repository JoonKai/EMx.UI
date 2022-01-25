// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartAreaLine
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using EMx.Maths;
using EMx.Maths.Viewport;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Charts
{
  public partial class EMxChartAreaLine : EMxChartArea, IComponentConnector
  {
    private WriteableBitmap BackBuffer;
    private int LastMaxSeriesCount = 0;
    protected bool IsOnDrag = false;
    protected System.Windows.Point DragStartPosition = new System.Windows.Point();
    protected System.Windows.Point DragCurrentPosition = new System.Windows.Point();
    protected System.Windows.Point DragStopPosition = new System.Windows.Point();
    protected NumericPoint DragStartZoomOffset = new NumericPoint();
    protected NumericPoint DragCurrentZoomOffset = new NumericPoint();
    private bool _contentLoaded;

    public EMxChartAreaLine()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
      this.LastMaxSeriesCount = 0;
    }

    protected override void OnRender(DrawingContext dc)
    {
      if (this.Chart == null || this.ActualWidth == 0.0 || this.ActualHeight == 0.0)
        return;
      if (this.BackBuffer == null || Math.Round(this.ActualWidth) != (double) this.BackBuffer.PixelWidth || Math.Round(this.ActualHeight) != (double) this.BackBuffer.PixelHeight)
        this.BackBuffer = new WriteableBitmap((int) this.ActualWidth, (int) this.ActualHeight, 72.0, 72.0, PixelFormats.Bgr24, (BitmapPalette) null);
      this.UpdateAxisRange();
      WriteableBitmap backBuffer = this.BackBuffer;
      bool flag = false;
      try
      {
        if (flag = backBuffer.TryLock((Duration) new TimeSpan(0, 0, 1)))
        {
          using (Bitmap bitmap = new Bitmap(backBuffer.PixelWidth, backBuffer.PixelHeight, backBuffer.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, backBuffer.BackBuffer))
          {
            using (Graphics g = Graphics.FromImage((Image) bitmap))
            {
              g.FillRectangle((System.Drawing.Brush) new SolidBrush((this.Chart.Background as SolidColorBrush).Color.ToDrawingColor()), 0, 0, backBuffer.PixelWidth, backBuffer.PixelHeight);
              g.SmoothingMode = this.SmoothMode;
              this.DrawChartAreaLattice(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
              if (this.LastMaxSeriesCount < backBuffer.PixelWidth || this.DuplicationRule == eAreaLineDuplicationRule.TopBottom)
                this.DrawChartArea(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
              else if (this.DuplicationRule == eAreaLineDuplicationRule.AnyPoint)
                this.DrawChartArea_AnyPoint(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
              this.Chart.XAxis.InvalidateVisual();
              this.Chart.YAxis.InvalidateVisual();
            }
          }
          backBuffer.AddDirtyRect(new Int32Rect(0, 0, backBuffer.PixelWidth, backBuffer.PixelHeight));
        }
      }
      finally
      {
        if (flag)
          backBuffer.Unlock();
      }
      dc.DrawImage((ImageSource) backBuffer, new Rect(0.0, 0.0, (double) backBuffer.PixelWidth, (double) backBuffer.PixelHeight));
    }

    protected void DrawChartAreaLattice(Graphics g, int w, int h)
    {
      EMxChartAxis xaxis = this.Chart.XAxis;
      EMxChartAxis yaxis = this.Chart.YAxis;
      Trace.Assert(xaxis != null);
      Trace.Assert(yaxis != null);
      System.Drawing.Pen pen = new System.Drawing.Pen(Colors.DimGray.ToDrawingColor(), 1f);
      xaxis.AutoMeasureAxisUnit();
      List<double> majorUnitPoints1 = xaxis.MajorUnitPoints;
      for (int index = 0; index < majorUnitPoints1.Count; ++index)
      {
        double world = majorUnitPoints1[index];
        bool success;
        int view = (int) xaxis.AxisScale.ToView(world, out success);
        if (success && view > -1 && view < w)
          g.DrawLine(pen, view, 0, view, h - 1);
      }
      yaxis.AutoMeasureAxisUnit();
      List<double> majorUnitPoints2 = yaxis.MajorUnitPoints;
      for (int index = 0; index < majorUnitPoints2.Count; ++index)
      {
        double world = majorUnitPoints2[index];
        bool success;
        int view = (int) yaxis.AxisScale.ToView(world, out success);
        if (success && view > -1 && view < h)
          g.DrawLine(pen, 0, view, w - 1, view);
      }
    }

    protected void DrawChartArea_AnyPoint(Graphics g, int w, int h)
    {
      EMxChartAxis xaxis = this.Chart.XAxis;
      EMxChartAxis yaxis = this.Chart.YAxis;
      Trace.Assert(xaxis != null);
      Trace.Assert(yaxis != null);
      lock (this.Series)
      {
        for (int index = 0; index < this.Series.Count; ++index)
        {
          IEMxChartSeries series = this.Series[index];
          if (series.IsShow)
          {
            System.Drawing.Pen pen = new System.Drawing.Pen(series.LineColor.ToDrawingColor(), series.LineThickness);
            lock (series)
            {
              IList ilist = this.GetIList(series.ItemSource);
              if (ilist != null && ilist.Count > 0)
              {
                Func<object, double> getter = this.GetGetter(series, ilist, true);
                int count = ilist.Count;
                List<System.Drawing.Point> pointList = new List<System.Drawing.Point>();
                for (int x = 0; x < w; ++x)
                {
                  bool success;
                  int world1 = (int) xaxis.AxisScale.ToWorld((double) x, out success);
                  if (success && world1 > -1 && world1 < count)
                  {
                    double world2 = getter(ilist[world1]);
                    int view = (int) yaxis.AxisScale.ToView(world2, out success);
                    if (success)
                      pointList.Add(new System.Drawing.Point(x, view));
                  }
                }
                if (pointList.Count > 1)
                  g.DrawLines(pen, pointList.ToArray());
              }
            }
          }
        }
      }
    }

    protected void DrawChartArea(Graphics g, int w, int h)
    {
      EMxChartAxis xaxis = this.Chart.XAxis;
      EMxChartAxis yaxis = this.Chart.YAxis;
      Trace.Assert(xaxis != null);
      Trace.Assert(yaxis != null);
      lock (this.Series)
      {
        for (int index1 = 0; index1 < this.Series.Count; ++index1)
        {
          IEMxChartSeries series = this.Series[index1];
          if (series.IsShow && series.ItemSource != null)
          {
            System.Drawing.Pen pen = new System.Drawing.Pen(series.LineColor.ToDrawingColor(), series.LineThickness);
            SolidBrush solidBrush = new SolidBrush(series.MarkerColor.ToDrawingColor());
            lock (series.ItemSource)
            {
              IList ilist = this.GetIList(series.ItemSource);
              if (ilist != null && ilist.Count > 0)
              {
                List<System.Drawing.Point> pointList = new List<System.Drawing.Point>();
                Func<object, double> getter = this.GetGetter(series, ilist, true);
                if (ilist.Count < w)
                {
                  for (int index2 = 0; index2 < ilist.Count; ++index2)
                  {
                    double world = getter(ilist[index2]);
                    bool success1;
                    int view1 = (int) xaxis.AxisScale.ToView((double) (index2 + 1), out success1);
                    bool success2;
                    int view2 = (int) yaxis.AxisScale.ToView(world, out success2);
                    if (success1 & success2)
                      pointList.Add(new System.Drawing.Point(view1, view2));
                  }
                }
                else
                {
                  int index3 = 0;
                  for (int index4 = 0; index4 < w; ++index4)
                  {
                    int x = index4;
                    double num1 = double.MinValue;
                    double num2 = double.MaxValue;
                    bool success3;
                    int world = (int) xaxis.AxisScale.ToWorld((double) index4, out success3);
                    if (success3)
                    {
                      int num3 = 0;
                      for (int index5 = index3; index5 <= world; ++index5)
                      {
                        if (index5 > -1 && index5 < ilist.Count)
                        {
                          double val2 = getter(ilist[index5]);
                          num1 = Math.Max(num1, val2);
                          num2 = Math.Min(num2, val2);
                          ++num3;
                        }
                      }
                      if (num3 != 0)
                      {
                        bool success4;
                        int view3 = (int) yaxis.AxisScale.ToView(num2, out success4);
                        bool success5;
                        int view4 = (int) yaxis.AxisScale.ToView(num1, out success5);
                        if (!success4 & success5)
                          pointList.Add(new System.Drawing.Point(x, view4));
                        else if (success4 && !success5)
                          pointList.Add(new System.Drawing.Point(x, view3));
                        else if (success4 || success5)
                        {
                          if (view3 != view4)
                          {
                            if (index3 > -1 && index3 < ilist.Count)
                            {
                              bool success6;
                              int view5 = (int) yaxis.AxisScale.ToView(getter(ilist[index3]), out success6);
                              if (success6)
                                pointList.Add(new System.Drawing.Point(x, view5));
                            }
                            pointList.Add(new System.Drawing.Point(x, view3));
                            pointList.Add(new System.Drawing.Point(x, view4));
                            if (world > -1 && world < ilist.Count)
                            {
                              bool success7;
                              int view6 = (int) yaxis.AxisScale.ToView(getter(ilist[world]), out success7);
                              if (success7)
                                pointList.Add(new System.Drawing.Point(x, view6));
                            }
                          }
                          else
                            pointList.Add(new System.Drawing.Point(x, view3));
                        }
                        index3 = world + 1;
                      }
                    }
                  }
                }
                if (pointList.Count > 1)
                {
                  if ((double) series.LineThickness > 0.0)
                    g.DrawLines(pen, pointList.ToArray());
                  if (series.UseMarkerDrawing && (double) pointList.Count < (double) w * 0.5)
                  {
                    for (int index6 = 0; index6 < pointList.Count; ++index6)
                    {
                      System.Drawing.Point point = pointList[index6];
                      g.FillRectangle((System.Drawing.Brush) solidBrush, point.X - series.MarkerSize / 2, point.Y - series.MarkerSize / 2, series.MarkerSize, series.MarkerSize);
                    }
                  }
                }
                pointList.Clear();
              }
            }
          }
        }
      }
    }

    private IList GetIList(object obj)
    {
      if (!(obj is IList list) && obj is IListSource)
        list = (obj as IListSource).GetList();
      return list;
    }

    private Func<object, double> GetGetter(IEMxChartSeries series, IList data, bool value_type)
    {
      Func<object, double> func = (Func<object, double>) (x => (double) x);
      if (data.Count > 0)
      {
        if (value_type)
        {
          if (series.ValueName != null && series.ValueName.Length > 0)
          {
            PropertyInfo p = Helper.Assem.GetProperty(data[0], series.ValueName);
            if (p != (PropertyInfo) null)
              func = (Func<object, double>) (x => (double) p.GetValue(x));
          }
        }
        else if (series.BaseName != null && series.BaseName.Length > 0)
        {
          PropertyInfo p = Helper.Assem.GetProperty(data[0], series.BaseName);
          if (p != (PropertyInfo) null)
            func = (Func<object, double>) (x => (double) p.GetValue(x));
        }
      }
      return func;
    }

    protected virtual void UpdateAxisRange()
    {
      if (this.Chart == null)
        return;
      EMxChartAxis xaxis = this.Chart.XAxis;
      EMxChartAxis yaxis = this.Chart.YAxis;
      Trace.Assert(xaxis != null);
      Trace.Assert(yaxis != null);
      xaxis.AxisScale.View.Begin = 1.0;
      xaxis.AxisScale.View.End = this.ActualWidth - 1.0;
      xaxis.AxisScale.Setup();
      yaxis.AxisScale.View.Begin = this.ActualHeight - 1.0;
      yaxis.AxisScale.View.End = 1.0;
      yaxis.AxisScale.Setup();
      int val1_1 = 0;
      lock (this.Series)
      {
        for (int index = 0; index < this.Series.Count; ++index)
        {
          IEMxChartSeries emxChartSeries = this.Series[index];
          if (emxChartSeries.IsShow && emxChartSeries.ItemSource != null)
          {
            lock (emxChartSeries.ItemSource)
            {
              IList ilist = this.GetIList(emxChartSeries.ItemSource);
              if (ilist != null)
                val1_1 = Math.Max(val1_1, ilist.Count);
            }
          }
        }
      }
      this.LastMaxSeriesCount = val1_1;
      lock (this.Series)
      {
        if (!xaxis.IsAutoRange && !yaxis.IsAutoRange)
          return;
        bool flag = false;
        for (int index = 0; index < this.Series.Count; ++index)
        {
          IEMxChartSeries emxChartSeries = this.Series[index];
          if (emxChartSeries.IsShow && emxChartSeries.ItemUpdatedTime.Subtract(this.LastUpdateTime).TotalMilliseconds > 0.0)
          {
            flag = true;
            this.LastUpdateTime = DateTime.Now;
            break;
          }
        }
        if (flag)
        {
          double val1_2 = double.MaxValue;
          double val1_3 = double.MinValue;
          if (xaxis.IsAutoRange)
          {
            xaxis.AxisScale.World.Begin = 1.0;
            xaxis.AxisScale.World.End = (double) val1_1;
            xaxis.AxisScale.Setup();
          }
          if (yaxis.IsAutoRange)
          {
            for (int index1 = 0; index1 < this.Series.Count; ++index1)
            {
              IEMxChartSeries series = this.Series[index1];
              if (series.IsShow && series.ItemSource != null)
              {
                lock (series.ItemSource)
                {
                  IList ilist = this.GetIList(series.ItemSource);
                  if (ilist != null)
                  {
                    Func<object, double> getter = this.GetGetter(series, ilist, true);
                    if ((double) ilist.Count < this.ActualWidth || this.DuplicationRule == eAreaLineDuplicationRule.TopBottom)
                    {
                      int count = ilist.Count;
                      for (int index2 = 0; index2 < count; ++index2)
                      {
                        double val2 = getter(ilist[index2]);
                        val1_2 = Math.Min(val1_2, val2);
                        val1_3 = Math.Max(val1_3, val2);
                      }
                    }
                    else if (this.DuplicationRule == eAreaLineDuplicationRule.AnyPoint)
                    {
                      for (int index3 = 0; index3 < this.BackBuffer.PixelWidth; ++index3)
                      {
                        bool success;
                        int world = (int) xaxis.AxisScale.ToWorld((double) index3, out success);
                        if (success && world > -1 && world < ilist.Count)
                        {
                          double val2 = getter(ilist[world]);
                          val1_2 = Math.Min(val1_2, val2);
                          val1_3 = Math.Max(val1_3, val2);
                        }
                      }
                    }
                  }
                }
              }
            }
            yaxis.AxisScale.World.Begin = val1_2;
            yaxis.AxisScale.World.End = val1_3;
            yaxis.AxisScale.Setup();
          }
        }
      }
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if (e.Handled || this.Chart == null || (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control || e.LeftButton != MouseButtonState.Pressed)
        return;
      ZoomableRangeViewport axisScale1 = this.Chart.XAxis.AxisScale;
      ZoomableRangeViewport axisScale2 = this.Chart.YAxis.AxisScale;
      this.DragStartZoomOffset.X = axisScale1.ZoomOffset;
      this.DragStartZoomOffset.Y = axisScale2.ZoomOffset;
      this.DragStartPosition = e.GetPosition((IInputElement) this);
      this.DragCurrentPosition = this.DragStartPosition;
      Mouse.OverrideCursor = Cursors.ScrollAll;
      Mouse.Capture((IInputElement) this);
      this.IsOnDrag = true;
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      if (e.Handled || this.Chart == null || !this.IsOnDrag || e.LeftButton != MouseButtonState.Released)
        return;
      this.DragStopPosition = e.GetPosition((IInputElement) this);
      this.IsOnDrag = false;
      Mouse.OverrideCursor = (Cursor) null;
      Mouse.Capture((IInputElement) null);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (e.Handled || this.Chart == null || !this.IsOnDrag || !this.UseZoomingControl)
        return;
      this.DragCurrentPosition = e.GetPosition((IInputElement) this);
      ZoomableRangeViewport axisScale1 = this.Chart.XAxis.AxisScale;
      ZoomableRangeViewport axisScale2 = this.Chart.YAxis.AxisScale;
      double num1 = this.DragCurrentPosition.X - this.DragStartPosition.X;
      double num2 = this.DragCurrentPosition.Y - this.DragStartPosition.Y;
      double num3 = 2.0 * num1 / axisScale1.View.Length / Math.Max(1E-10, axisScale1.ZoomRatio);
      double num4 = 2.0 * num2 / axisScale2.View.Length / Math.Max(1E-10, axisScale2.ZoomRatio);
      this.DragCurrentZoomOffset.X = this.DragStartZoomOffset.X + num3;
      this.DragCurrentZoomOffset.Y = this.DragStartZoomOffset.Y + num4;
      if (this.DragCurrentZoomOffset.X != axisScale1.ZoomOffset || this.DragCurrentZoomOffset.Y != axisScale2.ZoomOffset)
      {
        axisScale1.ZoomOffset = this.DragCurrentZoomOffset.X;
        axisScale2.ZoomOffset = this.DragCurrentZoomOffset.Y;
        this.Chart.ChartArea.InvalidateVisual();
      }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      base.OnMouseWheel(e);
      if (e.Handled || this.Chart == null || !this.UseZoomingControl || this.IsOnDrag)
        return;
      ZoomableRangeViewport axisScale1 = this.Chart.XAxis.AxisScale;
      ZoomableRangeViewport axisScale2 = this.Chart.YAxis.AxisScale;
      double num1 = (double) Math.Sign(e.Delta) * axisScale1.ZoomRatio * 0.1;
      axisScale1.ZoomRatio += num1;
      double num2 = (double) Math.Sign(e.Delta) * axisScale2.ZoomRatio * 0.1;
      axisScale2.ZoomRatio += num2;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void EMxChartAreaLine_Loaded_1(object sender, RoutedEventArgs e)
    {
      EMxChart parent = this.FindParent<EMxChart>();
      if (parent != null)
        this.Chart = parent;
      this.InvalidateVisual();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/charts/emxchartarealine.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
