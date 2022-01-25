// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartAreaScatter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using EMx.Maths.Viewport;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Charts
{
  public partial class EMxChartAreaScatter : EMxChartArea, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private WriteableBitmap BackBuffer;
    protected System.Windows.Point MouseTracePosition;
    protected bool IsOnDrag = false;
    protected System.Windows.Point DragStartPosition = new System.Windows.Point();
    protected System.Windows.Point DragCurrentPosition = new System.Windows.Point();
    protected System.Windows.Point DragStopPosition = new System.Windows.Point();
    protected NumericPoint DragStartZoomOffset = new NumericPoint();
    protected NumericPoint DragCurrentZoomOffset = new NumericPoint();
    private bool _contentLoaded;

    protected List<NumericPoint> TracedPoints { get; set; }

    protected System.Drawing.Pen TraceLinePen { get; set; }

    protected Font TraceInfoFont { get; set; }

    public event Action<EMxChartAreaScatter, List<NumericPoint>> TracedPointsUpdateEvent;

    protected void InvokeTracedPointsUpdateEvent()
    {
      if (this.TracedPointsUpdateEvent == null)
        return;
      this.TracedPointsUpdateEvent(this, this.TracedPoints);
    }

    public EMxChartAreaScatter()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
      this.DuplicationRule = eAreaLineDuplicationRule.TopBottom;
      this.MouseTracePosition.X = -1.0;
      this.TracedPoints = new List<NumericPoint>();
      this.UseTraceFunction = true;
      this.TraceLinePen = new System.Drawing.Pen(System.Drawing.Brushes.Orange, 1f);
      this.TraceInfoFont = new Font("Tahoma", 8f);
    }

    protected override ContextMenu InitializeContextMenu()
    {
      ContextMenu mnu1 = base.InitializeContextMenu();
      MenuItem mnu2 = mnu1.AddSubMenu("Trace", (RoutedEventHandler) null);
      mnu2.AddSubMenu("Enable", new RoutedEventHandler(this.OnTrace_EnableTraceFeature));
      mnu2.AddSubMenu("Disable", new RoutedEventHandler(this.OnTrace_DisableTraceFeature));
      MenuItem mnu3 = mnu1.AddSubMenu("Export", (RoutedEventHandler) null);
      mnu3.AddSubMenu("All Graphes", new RoutedEventHandler(this.OnExport_AllGraphes));
      mnu3.AddSubMenu("Select Graphes", new RoutedEventHandler(this.OnExport_SelectedGraphes));
      mnu3.AddSubMenu("Image", new RoutedEventHandler(this.OnExport_ImageFile));
      mnu3.Items.Add((object) new Separator());
      mnu3.AddSubMenu("All Graphes to Clipboard", new RoutedEventHandler(this.OnExport_AllGraphesToClipboard));
      mnu3.AddSubMenu("Select Graphes to Clipboard", new RoutedEventHandler(this.OnExport_SelectedGraphesToClipboard));
      MenuItem mnu4 = mnu1.AddSubMenu("Marker", (RoutedEventHandler) null);
      mnu4.AddSubMenu("Show", new RoutedEventHandler(this.OnMarker_Show));
      mnu4.AddSubMenu("Hide", new RoutedEventHandler(this.OnMarker_Hide));
      mnu4.AddSubMenu("Change Point Size", new RoutedEventHandler(this.OnMarker_ChangeSize));
      MenuItem mnu5 = mnu1.AddSubMenu("Line", (RoutedEventHandler) null);
      mnu5.AddSubMenu("Thickness", new RoutedEventHandler(this.OnLine_ChangeThickness));
      mnu5.AddSubMenu("Color", new RoutedEventHandler(this.OnLine_ChangeColor));
      MenuItem mnu6 = mnu1.AddSubMenu("Zoom", (RoutedEventHandler) null);
      mnu6.AddSubMenu("Reset", new RoutedEventHandler(this.OnZoom_Reset_Clicked));
      mnu6.AddSubMenu("200%", new RoutedEventHandler(this.OnZoom_200_Clicked));
      mnu6.AddSubMenu("500%", new RoutedEventHandler(this.OnZoom_500_Clicked));
      return mnu1;
    }

    private void OnTrace_DisableTraceFeature(object sender, RoutedEventArgs e)
    {
      this.UseTraceFunction = false;
      this.InvalidateVisual();
    }

    private void OnTrace_EnableTraceFeature(object sender, RoutedEventArgs e)
    {
      this.UseTraceFunction = true;
      this.InvalidateVisual();
    }

    private void OnExport_AllGraphes(object sender, RoutedEventArgs e) => this.ExportGraphesWithSerieses(this.Series.Where<IEMxChartSeries>((Func<IEMxChartSeries, bool>) (x => x.ItemSource != null)).ToList<IEMxChartSeries>());

    private void OnExport_SelectedGraphes(object sender, RoutedEventArgs e)
    {
      List<SelectMultiItemsData> source = new List<SelectMultiItemsData>();
      for (int index = 0; index < this.Series.Count; ++index)
      {
        string str = this.Series[index].SeriesName.DefaultMessage(string.Format("Series {0}", (object) (index + 1)));
        source.Add(new SelectMultiItemsData()
        {
          IsSelected = true,
          Message = str,
          Object = (object) this.Series[index]
        });
      }
      SelectMultiItemsDialog multiItemsDialog = new SelectMultiItemsDialog();
      multiItemsDialog.Owner = Application.Current.MainWindow;
      multiItemsDialog.Items = source;
      multiItemsDialog.Title = "Select graph serieses to be exporting.";
      bool? nullable = multiItemsDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      List<IEMxChartSeries> list = source.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, IEMxChartSeries>((Func<SelectMultiItemsData, IEMxChartSeries>) (x => x.Object as IEMxChartSeries)).ToList<IEMxChartSeries>();
      if (list.Count > 0)
        this.ExportGraphesWithSerieses(list);
    }

    private void OnExport_AllGraphesToClipboard(object sender, RoutedEventArgs e) => Clipboard.SetText(this.MakeExportContents(this.Series.Where<IEMxChartSeries>((Func<IEMxChartSeries, bool>) (x => x.ItemSource != null)).ToList<IEMxChartSeries>(), "\t"));

    private void OnExport_SelectedGraphesToClipboard(object sender, RoutedEventArgs e)
    {
      List<SelectMultiItemsData> source = new List<SelectMultiItemsData>();
      for (int index = 0; index < this.Series.Count; ++index)
      {
        string str = this.Series[index].SeriesName.DefaultMessage(string.Format("Series {0}", (object) (index + 1)));
        source.Add(new SelectMultiItemsData()
        {
          IsSelected = true,
          Message = str,
          Object = (object) this.Series[index]
        });
      }
      SelectMultiItemsDialog multiItemsDialog = new SelectMultiItemsDialog();
      multiItemsDialog.Owner = Application.Current.MainWindow;
      multiItemsDialog.Items = source;
      multiItemsDialog.Title = "Select graph serieses to be exporting.";
      bool? nullable = multiItemsDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      List<IEMxChartSeries> list = source.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, IEMxChartSeries>((Func<SelectMultiItemsData, IEMxChartSeries>) (x => x.Object as IEMxChartSeries)).ToList<IEMxChartSeries>();
      if (list.Count > 0)
        Clipboard.SetText(this.MakeExportContents(list, "\t"));
    }

    private void ExportGraphesWithSerieses(List<IEMxChartSeries> series)
    {
      if (series == null || series.Count == 0)
        return;
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "CSV File(*.csv)|*.csv|Text Files(*.txt)|*.txt|All Files(*.*)|*.*";
      saveFileDialog.RestoreDirectory = true;
      bool? nullable = saveFileDialog.ShowDialog(Application.Current.MainWindow);
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      string fileName = saveFileDialog.FileName;
      string delim = ",";
      if (saveFileDialog.FileName.EndsWith("txt", StringComparison.CurrentCultureIgnoreCase))
        delim = "\t";
      EMxChartAreaScatter.log.Info("Try to save graph data. Path({0}) Delimiter({1})", (object) fileName, (object) delim);
      string contents = this.MakeExportContents(series, delim);
      File.WriteAllText(fileName, contents);
      EMxChartAreaScatter.log.Info("Complete Save graph data. Path({0}) Delimiter({1})", (object) fileName, (object) delim);
    }

    private string MakeExportContents(List<IEMxChartSeries> series, string delim)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(series.AggregateText<IEMxChartSeries>(delim, (Func<IEMxChartSeries, string>) (x => string.Format("{0}{1}", (object) x.SeriesName, (object) delim))));
      stringBuilder.AppendLine(series.AggregateText<IEMxChartSeries>(delim, (Func<IEMxChartSeries, string>) (x => string.Format("{0}{1}{2}", (object) x.BaseName.DefaultMessage(this.Chart.XAxisAlias), (object) delim, (object) x.ValueName.DefaultMessage(this.Chart.YAxisAlias)))));
      List<EMxChartAreaScatter.ExportGraphSeriesData> list = series.Select<IEMxChartSeries, EMxChartAreaScatter.ExportGraphSeriesData>((Func<IEMxChartSeries, EMxChartAreaScatter.ExportGraphSeriesData>) (x =>
      {
        IList ilist1 = this.GetIList(x.BaseItemSource);
        IList ilist2 = this.GetIList(x.ItemSource);
        return new EMxChartAreaScatter.ExportGraphSeriesData()
        {
          BaseItemSource = ilist1,
          ValueItemSource = ilist2,
          BaseGetter = this.GetGetter(x, ilist1, false),
          ValueGetter = this.GetGetter(x, ilist2, true)
        };
      })).ToList<EMxChartAreaScatter.ExportGraphSeriesData>();
      int num1 = list.Max<EMxChartAreaScatter.ExportGraphSeriesData>((Func<EMxChartAreaScatter.ExportGraphSeriesData, int>) (x => x.BaseItemSource.Count));
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          EMxChartAreaScatter.ExportGraphSeriesData exportGraphSeriesData = list[index2];
          if (index2 > 0)
            stringBuilder.Append(delim);
          if (index1 < exportGraphSeriesData.BaseItemSource.Count)
          {
            double num2 = exportGraphSeriesData.BaseGetter(exportGraphSeriesData.BaseItemSource[index1]);
            double num3 = exportGraphSeriesData.ValueGetter(exportGraphSeriesData.ValueItemSource[index1]);
            stringBuilder.AppendFormat("{0}{1}{2}", (object) num2, (object) delim, (object) num3);
          }
          else
            stringBuilder.AppendFormat("{0}", (object) delim);
        }
        stringBuilder.AppendLine();
      }
      return stringBuilder.ToString();
    }

    private void OnExport_ImageFile(object sender, RoutedEventArgs e) => this.FindParent<EMxChart>().CaptureImage();

    private void OnMarker_Show(object sender, RoutedEventArgs e)
    {
      foreach (IEMxChartSeries emxChartSeries in this.Series)
      {
        emxChartSeries.UseMarkerDrawing = true;
        if (emxChartSeries.MarkerSize <= 0)
          emxChartSeries.MarkerSize = 1;
      }
      this.InvalidModels();
    }

    private void OnMarker_Hide(object sender, RoutedEventArgs e)
    {
      foreach (IEMxChartSeries emxChartSeries in this.Series)
        emxChartSeries.UseMarkerDrawing = false;
      this.InvalidModels();
    }

    private void OnMarker_ChangeSize(object sender, RoutedEventArgs e)
    {
      if (this.Series.Count == 0)
        return;
      int num1 = this.Series.Max<IEMxChartSeries>((Func<IEMxChartSeries, int>) (x => x.MarkerSize));
      Window mainWindow = Application.Current.MainWindow;
      if (mainWindow == null)
        return;
      string s = mainWindow.ShowInputMessage("Change Marker Size", "Please type the size of marker point.", num1.ToString());
      int n = 0;
      if (int.TryParse(s, out n))
      {
        this.Series.ForEach((Action<IEMxChartSeries>) (x => x.MarkerSize = n));
        this.InvalidModels();
      }
      else
      {
        int num2 = (int) mainWindow.ShowWarningMessage("Manager", "Invalid number format.");
      }
    }

    private void OnLine_ChangeColor(object sender, RoutedEventArgs e)
    {
      if (this.Series.Count == 0)
        return;
      string rrggbbText = this.Series.First<IEMxChartSeries>().LineColor.ToRRGGBBText("");
      Window mainWindow = Application.Current.MainWindow;
      if (mainWindow == null)
        return;
      string rrggbb = mainWindow.ShowInputMessage("Change Line Color", "Please type the color as RRGGBB format ex)FF0000 for red.", rrggbbText);
      if (!string.IsNullOrWhiteSpace(rrggbb))
      {
        try
        {
          System.Windows.Media.Color new_clr = rrggbb.ToColor();
          this.Series.ForEach((Action<IEMxChartSeries>) (x => x.LineColor = new_clr));
          this.InvalidModels();
        }
        catch (Exception ex)
        {
          EMxChartAreaScatter.log.Error(ex, ex.Message);
          int num = (int) mainWindow.ShowWarningMessage("Manager", "Invalid color format : " + rrggbb);
        }
      }
      else
      {
        int num1 = (int) mainWindow.ShowWarningMessage("Manager", "Invalid color format.");
      }
    }

    private void OnLine_ChangeThickness(object sender, RoutedEventArgs e)
    {
      if (this.Series.Count == 0)
        return;
      float num1 = this.Series.Max<IEMxChartSeries>((Func<IEMxChartSeries, float>) (x => x.LineThickness));
      Window mainWindow = Application.Current.MainWindow;
      if (mainWindow == null)
        return;
      string s = mainWindow.ShowInputMessage("Change Line Thickness", "Please type the thickness of line.", num1.ToString());
      int n = 0;
      if (int.TryParse(s, out n))
      {
        this.Series.ForEach((Action<IEMxChartSeries>) (x => x.LineThickness = (float) n));
        this.InvalidModels();
      }
      else
      {
        int num2 = (int) mainWindow.ShowWarningMessage("Manager", "Invalid number format.");
      }
    }

    private void OnZoom_Reset_Clicked(object sender, RoutedEventArgs e)
    {
      this.Chart.XAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.XAxis.AxisScale.ZoomRatio = 1.0;
      this.Chart.YAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.YAxis.AxisScale.ZoomRatio = 1.0;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnZoom_500_Clicked(object sender, RoutedEventArgs e)
    {
      this.Chart.XAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.XAxis.AxisScale.ZoomRatio = 5.0;
      this.Chart.YAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.YAxis.AxisScale.ZoomRatio = 5.0;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnZoom_200_Clicked(object sender, RoutedEventArgs e)
    {
      this.Chart.XAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.XAxis.AxisScale.ZoomRatio = 2.0;
      this.Chart.YAxis.AxisScale.ZoomOffset = 0.0;
      this.Chart.YAxis.AxisScale.ZoomRatio = 2.0;
      this.Chart.ChartArea.InvalidateVisual();
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
            using (Graphics g = Graphics.FromImage((System.Drawing.Image) bitmap))
            {
              g.FillRectangle((System.Drawing.Brush) new SolidBrush((this.Chart.Background as SolidColorBrush).Color.ToDrawingColor()), 0, 0, backBuffer.PixelWidth, backBuffer.PixelHeight);
              g.SmoothingMode = this.SmoothMode;
              this.DrawChartAreaLattice(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
              this.DrawChartArea(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
              if (this.UseTraceFunction && this.MouseTracePosition.X >= 0.0)
              {
                g.DrawLine(this.TraceLinePen, new PointF((float) this.MouseTracePosition.X, 0.0f), new PointF((float) this.MouseTracePosition.X, (float) this.ActualHeight));
                int num = 5;
                List<EMxChartAreaScatter.TraceNamedPoint> list = new List<EMxChartAreaScatter.TraceNamedPoint>();
                for (int index = 0; index < this.TracedPoints.Count; ++index)
                {
                  NumericPoint tracedPoint = this.TracedPoints[index];
                  if (tracedPoint != null && (index >= this.Series.Count || this.Series[index].IsShow) && (index >= this.Series.Count || !this.Series[index].DisableTracing))
                  {
                    string displayText1 = this.Chart.XAxis.GetDisplayText(tracedPoint.X, this.XAxisDisplayFormat);
                    string displayText2 = this.Chart.YAxis.GetDisplayText(tracedPoint.Y, this.YAxisDisplayformat);
                    list.Add(new EMxChartAreaScatter.TraceNamedPoint(string.Format("{0}({1}, {2})", index >= this.Series.Count || string.IsNullOrWhiteSpace(this.Series[index].SeriesName) ? (object) ("P" + (object) (index + 1)) : (object) this.Series[index].SeriesName, (object) displayText1, (object) displayText2), tracedPoint.X, tracedPoint.Y)
                    {
                      Index = index
                    });
                  }
                }
                list.SortExt<EMxChartAreaScatter.TraceNamedPoint>((Comparison<EMxChartAreaScatter.TraceNamedPoint>) ((a, b) => b.Y.CompareTo(a.Y)));
                if (list.Count > 10)
                {
                  list.RemoveRange(5, list.Count - 10);
                  list.Insert(5, new EMxChartAreaScatter.TraceNamedPoint("...", 0.0, 0.0)
                  {
                    Index = 5
                  });
                }
                foreach (EMxChartAreaScatter.TraceNamedPoint traceNamedPoint in list)
                {
                  System.Windows.Media.Color color = traceNamedPoint.Index <= -1 || traceNamedPoint.Index >= this.Series.Count ? Colors.DimGray : this.Series[traceNamedPoint.Index].LineColor;
                  g.DrawString(traceNamedPoint.Name, this.TraceInfoFont, (System.Drawing.Brush) new SolidBrush(color.ToDrawingColor()), new PointF(5f, (float) num));
                  num += 17;
                }
                this.InvokeTracedPointsUpdateEvent();
              }
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
      System.Drawing.Pen pen = new System.Drawing.Pen(System.Windows.Media.Color.FromRgb((byte) 200, (byte) 200, (byte) 200).ToDrawingColor(), 1f);
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

    protected void DrawChartArea(Graphics g, int w, int h)
    {
      EMxChartAxis xaxis = this.Chart.XAxis;
      EMxChartAxis yaxis = this.Chart.YAxis;
      Trace.Assert(xaxis != null);
      Trace.Assert(yaxis != null);
      this.TracedPoints.Resize<NumericPoint>(this.Series.Count, (Func<NumericPoint>) (() => new NumericPoint()));
      lock (this.Series)
      {
        for (int index1 = 0; index1 < this.Series.Count; ++index1)
        {
          IEMxChartSeries series = this.Series[index1];
          if (series.IsShow && series.BaseItemSource != null && series.ItemSource != null)
          {
            System.Drawing.Pen pen = new System.Drawing.Pen(series.LineColor.ToDrawingColor(), series.LineThickness);
            SolidBrush solidBrush = new SolidBrush(series.MarkerColor.ToDrawingColor());
            int index2 = -1;
            double num1 = double.MaxValue;
            lock (series.ItemSource)
            {
              lock (series.BaseItemSource)
              {
                IList ilist1 = this.GetIList(series.BaseItemSource);
                IList ilist2 = this.GetIList(series.ItemSource);
                if (ilist2 != null && ilist1 != null && ilist2.Count > 0 && ilist1.Count > 0)
                {
                  List<System.Drawing.Point> pointList = new List<System.Drawing.Point>();
                  Func<object, double> getter1 = this.GetGetter(series, ilist2, true);
                  Func<object, double> getter2 = this.GetGetter(series, ilist1, false);
                  int minValue = int.MinValue;
                  int num2 = int.MaxValue;
                  int num3 = int.MinValue;
                  bool flag = false;
                  int index3 = -1;
                  int num4 = Math.Min(ilist2.Count, ilist1.Count);
                  for (int index4 = 0; index4 < num4; ++index4)
                  {
                    double world1 = getter1(ilist2[index4]);
                    double world2 = getter2(ilist1[index4]);
                    bool success1;
                    int view1 = (int) xaxis.AxisScale.ToView(world2, out success1);
                    bool success2;
                    int view2 = (int) yaxis.AxisScale.ToView(world1, out success2);
                    if (success1 & success2)
                    {
                      num2 = Math.Min(num2, view2);
                      num3 = Math.Max(num3, view2);
                      if (view1 == minValue)
                      {
                        if (!flag)
                          index3 = index4;
                        flag = true;
                      }
                      else
                      {
                        if (num2 == num3)
                        {
                          pointList.Add(new System.Drawing.Point(view1, num3));
                        }
                        else
                        {
                          if (index3 > -1 && index3 < ilist2.Count)
                          {
                            bool success3;
                            int view3 = (int) yaxis.AxisScale.ToView(getter1(ilist2[index3]), out success3);
                            if (success3)
                              pointList.Add(new System.Drawing.Point(view1, view3));
                          }
                          pointList.Add(new System.Drawing.Point(view1, num2));
                          pointList.Add(new System.Drawing.Point(view1, num3));
                          bool success4;
                          int view4 = (int) yaxis.AxisScale.ToView(getter1(ilist2[index4]), out success4);
                          if (success4)
                            pointList.Add(new System.Drawing.Point(view1, view4));
                        }
                        flag = false;
                        index3 = -1;
                        num2 = int.MaxValue;
                        num3 = int.MinValue;
                      }
                      if (this.UseTraceFunction)
                      {
                        double num5 = Math.Abs(this.MouseTracePosition.X - (double) view1);
                        if (num5 <= num1)
                        {
                          num1 = num5;
                          index2 = index4;
                        }
                      }
                    }
                  }
                  if (pointList.Count > 1)
                  {
                    if ((double) series.LineThickness > 0.0)
                      g.DrawLines(pen, pointList.ToArray());
                    if (series.UseMarkerDrawing)
                    {
                      for (int index5 = 0; index5 < pointList.Count; ++index5)
                      {
                        System.Drawing.Point point = pointList[index5];
                        g.FillRectangle((System.Drawing.Brush) solidBrush, point.X - series.MarkerSize / 2, point.Y - series.MarkerSize / 2, series.MarkerSize, series.MarkerSize);
                      }
                    }
                  }
                  if (this.UseTraceFunction && index2 != -1)
                  {
                    double y = getter1(ilist2[index2]);
                    double x = getter2(ilist1[index2]);
                    this.TracedPoints[index1].Set(x, y);
                  }
                  pointList.Clear();
                }
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

    private Func<object, double> GetGetter(IEMxChartSeries series, IList data, bool yvalue)
    {
      Func<object, double> func = (Func<object, double>) (x => Convert.ToDouble(x));
      if (data.Count > 0)
      {
        if (yvalue)
        {
          if (series.ValueName != null && series.ValueName.Length > 0)
          {
            PropertyInfo p = Helper.Assem.GetProperty(data[0], series.ValueName);
            if (p != (PropertyInfo) null)
              func = (Func<object, double>) (x => (double) p.GetValue(x));
            else if (data is DataView)
              func = (Func<object, double>) (x =>
              {
                // ISSUE: reference to a compiler-generated field
                if (EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__1 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (double), typeof (EMxChartAreaScatter)));
                }
                // ISSUE: reference to a compiler-generated field
                Func<CallSite, object, double> target = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__1.Target;
                // ISSUE: reference to a compiler-generated field
                CallSite<Func<CallSite, object, double>> p1 = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__1;
                // ISSUE: reference to a compiler-generated field
                if (EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__0 == null)
                {
                  // ISSUE: reference to a compiler-generated field
                  EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof (EMxChartAreaScatter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                  {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                  }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                object obj = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__0.Target((CallSite) EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__0, x, series.ValueName);
                return target((CallSite) p1, obj);
              });
          }
        }
        else if (series.BaseName != null && series.BaseName.Length > 0)
        {
          PropertyInfo p = Helper.Assem.GetProperty(data[0], series.BaseName);
          if (p != (PropertyInfo) null)
            func = (Func<object, double>) (x => (double) p.GetValue(x));
          else if (data is DataView)
            func = (Func<object, double>) (x =>
            {
              // ISSUE: reference to a compiler-generated field
              if (EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__3 == null)
              {
                // ISSUE: reference to a compiler-generated field
                EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (double), typeof (EMxChartAreaScatter)));
              }
              // ISSUE: reference to a compiler-generated field
              Func<CallSite, object, double> target = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__3.Target;
              // ISSUE: reference to a compiler-generated field
              CallSite<Func<CallSite, object, double>> p3 = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__3;
              // ISSUE: reference to a compiler-generated field
              if (EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__2 == null)
              {
                // ISSUE: reference to a compiler-generated field
                EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof (EMxChartAreaScatter), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
                {
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                  CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                }));
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              object obj = EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__2.Target((CallSite) EMxChartAreaScatter.\u003C\u003Eo__42.\u003C\u003Ep__2, x, series.BaseName);
              return target((CallSite) p3, obj);
            });
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
      yaxis.AxisScale.View.Begin = this.ActualHeight - 1.0;
      yaxis.AxisScale.View.End = 1.0;
      lock (this.Series)
      {
        if (xaxis.IsAutoRange || yaxis.IsAutoRange)
        {
          List<DateTime> list = this.Series.Select<IEMxChartSeries, DateTime>((Func<IEMxChartSeries, DateTime>) (x => x.ItemUpdatedTime)).ToList<DateTime>();
          list.Add(xaxis.AxisScale.ConvertChangedTime);
          list.Add(yaxis.AxisScale.ConvertChangedTime);
          if (list.Any<DateTime>((Func<DateTime, bool>) (x => x.Subtract(this.LastUpdateTime).TotalMilliseconds > 0.0)))
          {
            this.LastUpdateTime = DateTime.Now;
            if (xaxis.IsAutoRange)
            {
              double num1 = double.MaxValue;
              double num2 = double.MinValue;
              bool flag = false;
              for (int index1 = 0; index1 < this.Series.Count; ++index1)
              {
                IEMxChartSeries series = this.Series[index1];
                if (series.IsShow && series.BaseItemSource != null)
                {
                  lock (series.BaseItemSource)
                  {
                    IList ilist = this.GetIList(series.BaseItemSource);
                    if (ilist != null && ilist.Count > 0)
                    {
                      Func<object, double> getter = this.GetGetter(series, ilist, false);
                      int count = ilist.Count;
                      for (int index2 = 0; index2 < count; ++index2)
                      {
                        double val2 = getter(ilist[index2]);
                        if (val2 > 0.0 || !xaxis.AxisScale.IsLogScale)
                        {
                          num1 = Math.Min(num1, val2);
                          num2 = Math.Max(num2, val2);
                          flag = true;
                        }
                      }
                    }
                  }
                }
              }
              if (flag)
                xaxis.AxisScale.World.Set(num1, num2);
              else
                xaxis.AxisScale.World.Set(1.0, 100.0);
            }
            if (yaxis.IsAutoRange)
            {
              double num3 = double.MaxValue;
              double num4 = double.MinValue;
              bool flag = false;
              for (int index3 = 0; index3 < this.Series.Count; ++index3)
              {
                IEMxChartSeries series = this.Series[index3];
                if (series.IsShow && series.ItemSource != null)
                {
                  lock (series.ItemSource)
                  {
                    IList ilist = this.GetIList(series.ItemSource);
                    if (ilist != null && ilist.Count > 0)
                    {
                      Func<object, double> getter = this.GetGetter(series, ilist, true);
                      int count = ilist.Count;
                      for (int index4 = 0; index4 < count; ++index4)
                      {
                        double val2 = getter(ilist[index4]);
                        if (val2 > 0.0 || !yaxis.AxisScale.IsLogScale)
                        {
                          num3 = Math.Min(num3, val2);
                          num4 = Math.Max(num4, val2);
                          flag = true;
                        }
                      }
                    }
                  }
                }
              }
              if (flag)
                yaxis.AxisScale.World.Set(num3, num4);
              else
                yaxis.AxisScale.World.Set(1.0, 100.0);
            }
          }
        }
      }
      bool flag1 = xaxis.AxisScale.Setup();
      bool flag2 = yaxis.AxisScale.Setup();
      if (flag1 && flag2)
        return;
      Trace.WriteLine("error");
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
      if (e.Handled || this.Chart == null)
        return;
      if (this.IsOnDrag && this.UseZoomingControl)
      {
        this.DragCurrentPosition = e.GetPosition((IInputElement) this);
        ZoomableRangeViewport axisScale1 = this.Chart.XAxis.AxisScale;
        ZoomableRangeViewport axisScale2 = this.Chart.YAxis.AxisScale;
        double num1 = this.DragCurrentPosition.X - this.DragStartPosition.X;
        double num2 = this.DragCurrentPosition.Y - this.DragStartPosition.Y;
        double num3 = 2.0 * num1 / axisScale1.View.Length / Math.Max(1E-10, axisScale1.ZoomRatio);
        double num4 = 2.0 * num2 / axisScale2.View.Length / Math.Max(1E-10, axisScale2.ZoomRatio);
        this.DragCurrentZoomOffset.X = this.DragStartZoomOffset.X + num3;
        this.DragCurrentZoomOffset.Y = this.DragStartZoomOffset.Y + num4;
        if (this.DragCurrentZoomOffset.X == axisScale1.ZoomOffset && this.DragCurrentZoomOffset.Y == axisScale2.ZoomOffset)
          return;
        axisScale1.ZoomOffset = this.DragCurrentZoomOffset.X;
        axisScale2.ZoomOffset = this.DragCurrentZoomOffset.Y;
        this.Chart.ChartArea.InvalidateVisual();
      }
      else
      {
        this.MouseTracePosition = e.GetPosition((IInputElement) this);
        this.InvalidateVisual();
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
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/charts/emxchartareascatter.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;

    private class ExportGraphSeriesData
    {
      public IList BaseItemSource { get; set; }

      public IList ValueItemSource { get; set; }

      public Func<object, double> BaseGetter { get; set; }

      public Func<object, double> ValueGetter { get; set; }
    }

    private class TraceNamedPoint : NumericPoint
    {
      public string Name { get; set; }

      public int Index { get; set; }

      public TraceNamedPoint(string name, double x, double y)
        : base(x, y)
      {
        this.Name = name;
      }
    }
  }
}
