// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.WaferMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Equipments.ProcessedData;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using EMx.Maths.Numerics;
using EMx.Serialization;
using EMx.Texts;
using EMx.UI.Arrows;
using EMx.UI.ColorConverters;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMx.UI.Maps
{
  [InstanceContract(ClassID = "0be7c2f1-25fe-4e51-9176-76985c011608")]
  public partial class WaferMapControl : 
    UserControl,
    IManagedType,
    INotifyPropertyChanged,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty RangeBarWidthProperty = DependencyProperty.Register(nameof (RangeBarWidth), typeof (double), typeof (WaferMapControl), new PropertyMetadata((object) 90.0));
    protected Visibility _ShowCrossLine;
    internal ColumnDefinition colFirst;
    internal Grid ctrlMapGrid;
    internal Image ctrlMapImage;
    internal Canvas ctrlCanvas;
    internal Border FocusedRectBorder;
    internal Rectangle FocusedRect;
    internal CircleBaseArrow ctrlArrow;
    internal Line ctrlHoriLine;
    internal Line ctrlVertiLine;
    internal MapRangeControl ctrlRangeBar;
    private bool _contentLoaded;

    public double RangeBarWidth
    {
      get => (double) this.GetValue(WaferMapControl.RangeBarWidthProperty);
      set => this.SetValue(WaferMapControl.RangeBarWidthProperty, (object) value);
    }

    public double RangeBarBegin
    {
      get => this.ctrlRangeBar.Range.Begin;
      set => this.ctrlRangeBar.Range.Begin = value;
    }

    public double RangeBarEnd
    {
      get => this.ctrlRangeBar.Range.End;
      set => this.ctrlRangeBar.Range.End = value;
    }

    public void RangeBarChanged() => this.ctrlRangeBar.Range.InvokeRangeChanged();

    public virtual bool UseAutoRange
    {
      get => this.Converter.Range.UseAutoRange;
      set => this.Converter.Range.UseAutoRange = value;
    }

    public List<ContextMenuRegistrationItem> ContextRegItems { get; set; }

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public virtual ConvertMapToImage Converter { get; set; }

    [DesignableMember(true)]
    public virtual IColorConverter ColorConveter => this.Converter.ColorConveter;

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public IMapData MapData { get; set; }

    public bool DisableHovering { get; set; }

    public bool DisableDragDrop { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public virtual Visibility ShowCrossLine
    {
      get => this._ShowCrossLine;
      set => this.PropertyChanged.InvokePropertyChangedEvent<Visibility>((object) this, ref this._ShowCrossLine, value);
    }

    public event Action<WaferMapControl, Point> MapItemHovered;

    protected virtual void InvokeMapItemHovered(Point index)
    {
      if (this.MapItemHovered == null)
        return;
      this.MapItemHovered(this, index);
    }

    public event Action<WaferMapControl, Point> MapItemClick;

    protected virtual void InvokeMapItemClick(Point pt)
    {
      if (this.MapItemClick == null)
        return;
      this.MapItemClick(this, pt);
    }

    public event Action<WaferMapControl, Point> MapItemDoubleClick;

    protected virtual void InvokeMapItemDoubleClick(Point pt)
    {
      if (this.MapItemDoubleClick == null)
        return;
      this.MapItemDoubleClick(this, pt);
    }

    public virtual bool IsFixedFocusPoint { get; set; }

    public Point FixedFocusPoint { get; set; }

    protected UIElementDragEventHelper DragHandler { get; set; }

    protected List<Border> SelectedPoints { get; set; }

    protected NumericPoint DragStartPoint { get; set; }

    protected NumericPoint DragEndPoint { get; set; }

    public List<NumericPoint> SelectedIndices { get; protected set; }

    public event Action<WaferMapControl> ClearedSelectedPointsEvent;

    public event Action<WaferMapControl, List<NumericPoint>> UpdatedSelectedPointsEvent;

    protected virtual void InvokeClearedSelectedPoints()
    {
      if (this.ClearedSelectedPointsEvent == null)
        return;
      this.ClearedSelectedPointsEvent(this);
    }

    protected virtual void InvokeUpdatedSelectedPoints()
    {
      if (this.UpdatedSelectedPointsEvent == null)
        return;
      this.UpdatedSelectedPointsEvent(this, this.SelectedIndices);
    }

    public WaferMapControl()
    {
      try
      {
        this.InitializeComponent();
        RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
        RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
        this.DataContext = (object) this;
        this.SizeChanged += new SizeChangedEventHandler(this.OnControlSizeChanged);
        this.ShowCrossLine = Visibility.Hidden;
        this.ContextRegItems = new List<ContextMenuRegistrationItem>();
        this.Converter = new ConvertMapToImage();
        if (this.Background is SolidColorBrush)
          this.Converter.BackgroundColor = (this.Background as SolidColorBrush).Color;
        this.ctrlRangeBar.ColorConveter = this.Converter.ColorConveter;
        this.ctrlRangeBar.Range = this.Converter.Range;
        this.ctrlRangeBar.Range.RangeChanged += new Action<MapRange>(this.OnRangeChanged);
        this.DragHandler = new UIElementDragEventHelper();
        this.DragHandler.DragDirection = UIElementDragEventHelper.eDragDirection.Both;
        this.DragHandler.Register((UIElement) this.ctrlMapGrid);
        this.DragHandler.DragStart += new Action<UIElementDragEventHelper, UIElementDragStartArgs>(this.OnDragStart);
        this.DragHandler.DragMoving += new Action<UIElementDragEventHelper, Point>(this.OnDragMoving);
        this.DragHandler.DragEnd += new Action<UIElementDragEventHelper>(this.OnDragEnd);
        this.SelectedIndices = new List<NumericPoint>();
        this.SelectedPoints = new List<Border>();
        this.DragStartPoint = new NumericPoint();
        this.DragEndPoint = new NumericPoint();
        this.ContextRegItems.Add(new ContextMenuRegistrationItem("Export/As Image", (object) this, new RoutedEventHandler(this.mnuSaveImage_Clicked), (Func<object, bool>) (x => this.MapData != null)));
        this.ContextRegItems.Add(new ContextMenuRegistrationItem("Export/File/Select Points", (object) this, new RoutedEventHandler(this.mnuSaveSelectedPoints_Clicked), (Func<object, bool>) (x => this.MapData != null)));
        this.ContextRegItems.Add(new ContextMenuRegistrationItem("Export/File/All Points", (object) this, new RoutedEventHandler(this.mnuSaveAllPoints_Clicked), (Func<object, bool>) (x => this.MapData != null)));
        this.ContextRegItems.Add(new ContextMenuRegistrationItem("Export/Clipboard/Select Points", (object) this, new RoutedEventHandler(this.mnuSaveSelectedPointsClipboard_Clicked), (Func<object, bool>) (x => this.MapData != null)));
        this.ContextRegItems.Add(new ContextMenuRegistrationItem("Export/Clipboard/All Points", (object) this, new RoutedEventHandler(this.mnuSaveAllPointsClipboard_Clicked), (Func<object, bool>) (x => this.MapData != null)));
      }
      catch (Exception ex)
      {
        WaferMapControl.log.Error(ex.ToString());
      }
    }

    public virtual Point ConvertMousePointToMapPoint(MouseEventArgs e) => this.MapData == null ? new Point(0.0, 0.0) : this.ConvertMousePointToMapPoint(e.GetPosition((IInputElement) this.ctrlMapImage));

    protected virtual Point ConvertMousePointToMapPoint(Point pt) => this.ctrlMapImage.Width == 0.0 || this.ctrlMapImage.Height == 0.0 || this.MapData == null ? new Point(0.0, 0.0) : new Point(0.5 + (pt.X - this.ctrlMapImage.Width / 2.0) * (double) this.MapData.ColCapacity / this.ctrlMapImage.Width, 0.5 + (this.ctrlMapImage.Height / 2.0 - pt.Y) * (double) this.MapData.RowCapacity / this.ctrlMapImage.Height);

    private void OnMouseClickOnMapImage(object sender, MouseButtonEventArgs e)
    {
      Point mapPoint = this.ConvertMousePointToMapPoint((MouseEventArgs) e);
      if (e.ClickCount == 2)
      {
        this.IsFixedFocusPoint = true;
        this.FixedFocusPoint = mapPoint;
        this.FocusedRect.StrokeThickness = 3.0;
        this.FocusedRectBorder.BorderBrush = (Brush) Brushes.Black;
        this.InvokeMapItemDoubleClick(mapPoint);
      }
      else
      {
        if (this.DisableHovering)
          return;
        if (this.SelectedIndices.Count > 0)
        {
          this.DragStartPoint = new NumericPoint();
          this.DragEndPoint = new NumericPoint();
          this.SelectedIndices = new List<NumericPoint>();
          this.UpdateSelectedPoints();
          this.InvokeClearedSelectedPoints();
        }
        this.InvokeMapItemClick(mapPoint);
        this.IsFixedFocusPoint = false;
        this.FocusedRect.StrokeThickness = 2.0;
        this.FocusedRectBorder.BorderBrush = (Brush) null;
        this.ctrlMapImage_MouseMove(sender, (MouseEventArgs) e);
      }
    }

    public void OnSearchMapImage(Point pos)
    {
      Point offset = this.ctrlMapImage.TranslatePoint(new Point(0.0, 0.0), (UIElement) this.ctrlCanvas);
      if (this.MapData != null && !this.IsFixedFocusPoint && !this.DisableHovering)
      {
        Point mapPoint = this.ConvertMousePointToMapPoint(pos);
        if (this.UpdateRectaglePosition((FrameworkElement) this.FocusedRectBorder, mapPoint, offset, 2))
        {
          this.FocusedRect.Stroke = this.ColorConveter.ValueToColor(this.Converter.Range.Begin, this.Converter.Range.End, this.Converter.Getter(this.MapData.GetObject((int) mapPoint.Y, (int) mapPoint.X))).ToComplementaryColor().ToBrush();
          this.InvokeMapItemHovered(mapPoint);
        }
      }
      this.IsFixedFocusPoint = true;
      this.FixedFocusPoint = pos;
      this.FocusedRect.StrokeThickness = 3.0;
      this.FocusedRectBorder.BorderBrush = (Brush) Brushes.Black;
    }

    private void OnRangeChanged(MapRange obj) => this.InvalidModels();

    public virtual void InvalidModels()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
        this.Dispatcher.Invoke(new Action(this.UpdateImages));
      else
        this.UpdateImages();
    }

    private void UpdateImages()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      this.Converter.UpdateImage(this.MapData);
      stopwatch.Stop();
      if (stopwatch.ElapsedMilliseconds >= 200L)
        WaferMapControl.log.Info("Process time to convert WaferMap to Image : {0}ms", (object) stopwatch.ElapsedMilliseconds);
      this.ctrlMapImage.Source = (ImageSource) this.Converter.GetImageSource();
      this.ctrlRangeBar.InvalidateVisual();
      this.OnControlSizeChanged((object) this, (SizeChangedEventArgs) null);
    }

    private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
    {
      double actualWidth = this.colFirst.ActualWidth;
      double actualHeight = this.ctrlRangeBar.ActualHeight;
      if (Helper.Data.AnyInvalid(actualWidth, actualHeight))
        return;
      int num1 = 11;
      double num2 = Math.Min(actualWidth, actualHeight) - (double) num1;
      if (num2 < 0.0)
        return;
      this.ctrlMapImage.Width = num2;
      this.ctrlMapImage.Height = num2;
      this.UpdateSelectedPoints();
    }

    private void mnuSaveImage_Clicked(object sender, RoutedEventArgs e) => this.CaptureImage();

    private void mnuSaveSelectedPoints_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.MapData == null || this.SelectedIndices.Count == 0)
        return;
      List<MapItemData> points = new List<MapItemData>();
      for (int index = 0; index < this.SelectedIndices.Count; ++index)
      {
        NumericPoint selectedIndex = this.SelectedIndices[index];
        int x = (int) selectedIndex.X;
        int y = (int) selectedIndex.Y;
        if (this.MapData.Has(y, x))
        {
          object obj = this.MapData.GetObject(y, x);
          points.Add(new MapItemData(y, x, obj));
        }
      }
      this.ExportSelectedPoints(points, false, true, true);
    }

    private void mnuSaveAllPoints_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.MapData == null)
        return;
      this.ExportSelectedPoints(this.MapData.ToList(), false, true, true);
    }

    private void mnuSaveSelectedPointsClipboard_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.MapData == null || this.SelectedIndices.Count == 0)
        return;
      List<MapItemData> points = new List<MapItemData>();
      for (int index = 0; index < this.SelectedIndices.Count; ++index)
      {
        NumericPoint selectedIndex = this.SelectedIndices[index];
        int x = (int) selectedIndex.X;
        int y = (int) selectedIndex.Y;
        if (this.MapData.Has(y, x))
        {
          object obj = this.MapData.GetObject(y, x);
          points.Add(new MapItemData(y, x, obj));
        }
      }
      this.ExportSelectedPoints(points, true, false, true);
    }

    private void mnuSaveAllPointsClipboard_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.MapData == null)
        return;
      this.ExportSelectedPoints(this.MapData.ToList(), true, false, true);
    }

    protected void ExportSelectedPoints(
      List<MapItemData> points,
      bool use_clipboard,
      bool use_file,
      bool select_direction)
    {
      if (points == null || points.Count == 0 || !use_clipboard && !use_file)
        return;
      Window mainWindow = Application.Current.MainWindow;
      if (select_direction)
      {
        SelectPlaneDirectionDialog planeDirectionDialog = new SelectPlaneDirectionDialog();
        planeDirectionDialog.Owner = mainWindow;
        bool? nullable = planeDirectionDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        Comparison<MapItemData> directionWeightFucntion = Helper.Data.CreatePlaneDirectionWeightFucntion<MapItemData>(planeDirectionDialog.PlaneDirection, (Func<MapItemData, IComparable>) (x => (IComparable) x.X), (Func<MapItemData, IComparable>) (x => (IComparable) x.Y));
        points = new List<MapItemData>((IEnumerable<MapItemData>) points);
        points.SortExt<MapItemData>(directionWeightFucntion);
      }
      Func<string, string> func = (Func<string, string>) (delim =>
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("X{0}Y{0}{1}\r\n", (object) delim, (object) this.MapData.DataName.WhenNullOrEmpty("Value"));
        for (int index = 0; index < points.Count; ++index)
        {
          MapItemData point = points[index];
          stringBuilder.AppendFormat("{1}{0}{2}{0}{3}\r\n", (object) delim, (object) point.X, (object) point.Y, point.Value);
        }
        return stringBuilder.ToString();
      });
      if (use_clipboard)
      {
        try
        {
          Clipboard.SetText(func("\t"));
          int num = (int) mainWindow.ShowNormalMessage("Manager", "Complete!");
        }
        catch (Exception ex)
        {
          WaferMapControl.log.Error(ex, ex.Message);
        }
      }
      if (!use_file)
        return;
      try
      {
        string str1 = mainWindow.SaveFileDialog(0, "CSV File(*.csv)|*.csv|Text Files(*.txt)|*.txt|All Files(*.*)|*.*", "csv");
        if (!str1.IsNullOrEmpty())
        {
          string str2 = str1.EndsWith("txt", StringComparison.CurrentCultureIgnoreCase) ? "\t" : ",";
          string contents = func(str2);
          File.WriteAllText(str1, contents);
          if (mainWindow.ShowQuestionMessage("Manager", "Would you like to open a exported file?\r\n - {0}".At((object) str1)) == eDialogButtons.Yes)
            Process.Start(str1);
        }
      }
      catch (Exception ex)
      {
        WaferMapControl.log.Error(ex, ex.Message);
      }
    }

    private void ctrlMapImage_MouseMove(object sender, MouseEventArgs e)
    {
      Point offset = this.ctrlMapImage.TranslatePoint(new Point(0.0, 0.0), (UIElement) this.ctrlCanvas);
      if (this.MapData != null && !this.IsFixedFocusPoint && !this.DisableHovering)
      {
        Point mapPoint = this.ConvertMousePointToMapPoint(e);
        if (this.UpdateRectaglePosition((FrameworkElement) this.FocusedRectBorder, mapPoint, offset, 2))
        {
          this.FocusedRect.Stroke = this.ColorConveter.ValueToColor(this.Converter.Range.Begin, this.Converter.Range.End, this.Converter.Getter(this.MapData.GetObject((int) mapPoint.Y, (int) mapPoint.X))).ToComplementaryColor().ToBrush();
          this.InvokeMapItemHovered(mapPoint);
        }
      }
      if (this.ShowCrossLine != Visibility.Visible || this.IsFixedFocusPoint)
        return;
      Point position = e.GetPosition((IInputElement) this.ctrlCanvas);
      this.ctrlHoriLine.X1 = 0.0;
      this.ctrlHoriLine.X2 = this.ctrlCanvas.ActualWidth;
      this.ctrlHoriLine.Y1 = position.Y;
      this.ctrlHoriLine.Y2 = position.Y;
      this.ctrlVertiLine.Y1 = 0.0;
      this.ctrlVertiLine.Y2 = this.ctrlCanvas.ActualHeight;
      this.ctrlVertiLine.X1 = position.X;
      this.ctrlVertiLine.X2 = position.X;
    }

    private bool UpdateRectaglePosition(
      FrameworkElement elem,
      Point map_index,
      Point offset,
      int margin)
    {
      int x = (int) Math.Floor(map_index.X);
      if (this.MapData.Has((int) Math.Floor(map_index.Y), x) && this.Converter.Getter != null && !this.DisableHovering)
      {
        elem.Visibility = Visibility.Visible;
        if (this.MapData.ColCapacity == 0 || this.MapData.RowCapacity == 0 || this.MapData.RowCount == 0 || this.MapData.ColCount == 0)
          return false;
        double num1 = this.ctrlMapImage.Width / (double) this.MapData.ColCapacity;
        double num2 = this.ctrlMapImage.Height / (double) this.MapData.RowCapacity;
        double num3 = (Math.Floor(map_index.X) - 0.5) * num1 + this.ctrlMapImage.Width / 2.0 + offset.X;
        double num4 = (-0.5 - Math.Floor(map_index.Y)) * num2 + this.ctrlMapImage.Height / 2.0 + offset.Y;
        elem.Width = num1 + (double) (margin * 2);
        elem.Height = num2 + (double) (margin * 2);
        Canvas.SetLeft((UIElement) elem, num3 - (double) margin);
        Canvas.SetTop((UIElement) elem, num4 - (double) margin);
        return true;
      }
      elem.Visibility = Visibility.Hidden;
      return false;
    }

    protected bool IsOnDraging { get; set; }

    private void OnDragStart(UIElementDragEventHelper dh, UIElementDragStartArgs args)
    {
      if (this.DisableDragDrop)
      {
        args.IsAbortDrag = true;
      }
      else
      {
        this.ctrlMapImage.TranslatePoint(new Point(0.0, 0.0), (UIElement) this.ctrlMapGrid);
        Point mapPoint = this.ConvertMousePointToMapPoint(this.ctrlMapGrid.TranslatePoint(dh.MouseCurrentPosition, (UIElement) this.ctrlMapImage));
        int x = (int) Math.Floor(mapPoint.X);
        int y = (int) Math.Floor(mapPoint.Y);
        this.IsOnDraging = this.MapData != null && this.MapData.Has(y, x) && this.Converter.Getter != null;
      }
    }

    private void OnDragMoving(UIElementDragEventHelper dh, Point delta)
    {
      if (!this.IsOnDraging)
        return;
      if (Helper.Data.AnyInvalid(this.ActualHeight) || this.ActualHeight == 0.0)
        return;
      if (!this.DisableDragDrop)
      {
        this.ctrlArrow.Visibility = Visibility.Visible;
        CircleBaseArrow ctrlArrow = this.ctrlArrow;
        Point mouseBeginPosition = dh.MouseBeginPosition;
        double x1 = mouseBeginPosition.X;
        mouseBeginPosition = dh.MouseBeginPosition;
        double y1 = mouseBeginPosition.Y;
        double x2 = dh.MouseCurrentPosition.X;
        double y2 = dh.MouseCurrentPosition.Y;
        ctrlArrow.SetLocation(x1, y1, x2, y2);
      }
      if (this.MapData == null || this.IsFixedFocusPoint)
        return;
      Point offset = this.ctrlMapImage.TranslatePoint(new Point(0.0, 0.0), (UIElement) this.ctrlMapGrid);
      Point mapPoint = this.ConvertMousePointToMapPoint(this.ctrlMapGrid.TranslatePoint(dh.MouseCurrentPosition, (UIElement) this.ctrlMapImage));
      if (this.UpdateRectaglePosition((FrameworkElement) this.FocusedRectBorder, mapPoint, offset, 2))
      {
        this.FocusedRect.Stroke = this.ColorConveter.ValueToColor(this.Converter.Range.Begin, this.Converter.Range.End, this.Converter.Getter(this.MapData.GetObject((int) mapPoint.Y, (int) mapPoint.X))).ToComplementaryColor().ToBrush();
        this.InvokeMapItemHovered(mapPoint);
      }
    }

    private void OnDragEnd(UIElementDragEventHelper dh)
    {
      if (!this.IsOnDraging || this.DisableDragDrop)
        return;
      this.ctrlArrow.Visibility = Visibility.Hidden;
      this.IsOnDraging = false;
      Point point1 = this.ctrlMapGrid.TranslatePoint(dh.MouseBeginPosition, (UIElement) this.ctrlMapImage);
      Point point2 = this.ctrlMapGrid.TranslatePoint(dh.MouseCurrentPosition, (UIElement) this.ctrlMapImage);
      this.DragStartPoint = new NumericPoint(point1.X / this.ctrlMapImage.Width, point1.Y / this.ctrlMapImage.Height);
      this.DragEndPoint = new NumericPoint(point2.X / this.ctrlMapImage.Width, point2.Y / this.ctrlMapImage.Height);
      this.UpdateSelectedPoints();
    }

    protected virtual void UpdateSelectedPoints()
    {
      if (this.MapData == null)
        return;
      foreach (UIElement selectedPoint in this.SelectedPoints)
        this.ctrlCanvas.Children.Remove(selectedPoint);
      if (this.DisableHovering)
        return;
      Point pt1 = new Point(this.DragStartPoint.X * this.ctrlMapImage.Width, this.DragStartPoint.Y * this.ctrlMapImage.Height);
      Point pt2 = new Point(this.DragEndPoint.X * this.ctrlMapImage.Width, this.DragEndPoint.Y * this.ctrlMapImage.Height);
      Point mapPoint1 = this.ConvertMousePointToMapPoint(pt1);
      Point mapPoint2 = this.ConvertMousePointToMapPoint(pt2);
      LinearInterpolation linearInterpolation1 = new LinearInterpolation();
      linearInterpolation1.Points.Add(new NumericPoint(mapPoint1));
      linearInterpolation1.Points.Add(new NumericPoint(mapPoint2));
      linearInterpolation1.SortPoints();
      LinearInterpolation linearInterpolation2 = new LinearInterpolation();
      linearInterpolation2.Points.Add(new NumericPoint(mapPoint1.SwapXY()));
      linearInterpolation2.Points.Add(new NumericPoint(mapPoint2.SwapXY()));
      linearInterpolation2.SortPoints();
      List<NumericPoint> numericPointList = new List<NumericPoint>();
      double num1 = mapPoint2.X - mapPoint1.X;
      double num2 = mapPoint2.Y - mapPoint1.Y;
      int num3;
      if (num1 != 0.0 || num2 != 0.0)
        num3 = Helper.Data.AnyInvalid(num1, num2) ? 1 : 0;
      else
        num3 = 1;
      if (num3 != 0)
        return;
      if (Math.Abs(num1) > Math.Abs(num2))
      {
        int num4 = mapPoint1.X.Floor();
        while (true)
        {
          if ((Math.Floor(mapPoint1.X) - (double) num4) * (Math.Floor(mapPoint2.X) - (double) num4) <= 0.0 || numericPointList.Count <= 0)
          {
            numericPointList.Add(new NumericPoint((double) num4, linearInterpolation1.Interpolate((double) num4)));
            num4 += Math.Sign(num1);
          }
          else
            break;
        }
      }
      else
      {
        int num5 = mapPoint1.Y.Floor();
        while (true)
        {
          if ((Math.Floor(mapPoint1.Y) - (double) num5) * (Math.Floor(mapPoint2.Y) - (double) num5) <= 0.0 || numericPointList.Count <= 0)
          {
            numericPointList.Add(new NumericPoint((double) num5, linearInterpolation2.Interpolate((double) num5)).SwapXY());
            num5 += Math.Sign(num2);
          }
          else
            break;
        }
      }
      for (int index = numericPointList.Count - 1; index >= 0; --index)
      {
        NumericPoint numericPoint = numericPointList[index];
        if (!this.MapData.Has((int) Math.Floor(numericPoint.Y), (int) Math.Floor(numericPoint.X)))
          numericPointList.RemoveAt(index);
      }
      PredefinedColorsGradientConverter gradientConverter = new PredefinedColorsGradientConverter();
      gradientConverter.SetColors((IEnumerable<int>) new int[7]
      {
        16646144,
        16751616,
        16121600,
        3993870,
        131045,
        9471,
        12910847
      });
      Point offset = this.ctrlMapImage.TranslatePoint(new Point(0.0, 0.0), (UIElement) this.ctrlCanvas);
      for (int index = 0; index < numericPointList.Count; ++index)
      {
        NumericPoint numericPoint = numericPointList[index];
        Rectangle rectangle = new Rectangle();
        rectangle.Fill = (Brush) null;
        rectangle.Stroke = (Brush) Brushes.Red;
        rectangle.StrokeThickness = 2.0;
        rectangle.Stroke = gradientConverter.ValueToColor(0.0, (double) (numericPointList.Count - 1), (double) index).ToBrush();
        rectangle.ToolTip = (object) string.Format("P{0}", (object) (index + 1));
        Border border1 = new Border();
        border1.Child = (UIElement) rectangle;
        border1.BorderBrush = (Brush) Brushes.Black;
        border1.BorderThickness = new Thickness(1.0);
        Border border2 = border1;
        this.UpdateRectaglePosition((FrameworkElement) border2, numericPoint.ToFloorPoint().ToPoint(), offset, 0);
        this.SelectedPoints.Add(border2);
        this.ctrlCanvas.Children.Add((UIElement) border2);
      }
      this.SelectedIndices = numericPointList;
      this.InvokeUpdatedSelectedPoints();
    }

    private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      if (!(sender is Grid grid))
        return;
      ContextMenu contextMenu = new ContextMenu();
      Dictionary<string, MenuItem> dictionary = new Dictionary<string, MenuItem>();
      for (int index1 = 0; index1 < this.ContextRegItems.Count; ++index1)
      {
        ContextMenuRegistrationItem contextRegItem = this.ContextRegItems[index1];
        StructuredName structuredName = new StructuredName(contextRegItem.Path, "/");
        for (int index2 = 0; index2 < structuredName.Count; ++index2)
        {
          string backwardNodes = structuredName.GetBackwardNodes(index2);
          if (!dictionary.ContainsKey(backwardNodes))
          {
            MenuItem menuItem1 = new MenuItem();
            menuItem1.Header = (object) structuredName[index2];
            MenuItem menuItem2 = menuItem1;
            dictionary[backwardNodes] = menuItem2;
            if (index2 == 0)
            {
              contextMenu.Items.Add((object) menuItem2);
            }
            else
            {
              MenuItem menuItem3 = dictionary.SafeGet<string, MenuItem>(structuredName.GetBackwardNodes(index2, false));
              if (menuItem3 != null)
              {
                if (structuredName[index2].Equals("-"))
                  menuItem3.Items.Add((object) new Separator());
                else
                  menuItem3.Items.Add((object) menuItem2);
              }
            }
            if (index2 == structuredName.Count - 1)
            {
              if (contextRegItem.Handler != null)
                menuItem2.Click += contextRegItem.Handler;
              if (contextRegItem.Activation != null)
                menuItem2.IsEnabled = contextRegItem.Activation(contextRegItem.Sender);
            }
          }
        }
      }
      grid.ContextMenu = contextMenu;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/maps/wafermapcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).ContextMenuOpening += new ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
          break;
        case 2:
          this.colFirst = (ColumnDefinition) target;
          break;
        case 3:
          this.ctrlMapGrid = (Grid) target;
          this.ctrlMapGrid.MouseMove += new MouseEventHandler(this.ctrlMapImage_MouseMove);
          this.ctrlMapGrid.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnMouseClickOnMapImage);
          break;
        case 4:
          this.ctrlMapImage = (Image) target;
          break;
        case 5:
          this.ctrlCanvas = (Canvas) target;
          break;
        case 6:
          this.FocusedRectBorder = (Border) target;
          break;
        case 7:
          this.FocusedRect = (Rectangle) target;
          break;
        case 8:
          this.ctrlArrow = (CircleBaseArrow) target;
          break;
        case 9:
          this.ctrlHoriLine = (Line) target;
          break;
        case 10:
          this.ctrlVertiLine = (Line) target;
          break;
        case 11:
          this.ctrlRangeBar = (MapRangeControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
