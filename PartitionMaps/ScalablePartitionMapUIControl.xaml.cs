// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.ScalablePartitionMapUIControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Extensions;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using EMx.UI.ColorConverters;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using EMx.UI.PartitionMaps.MapPins;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "088e7cf7-03e4-4c6e-a931-b845bb682700")]
  public partial class ScalablePartitionMapUIControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty RangeBarWidthProperty = DependencyProperty.Register(nameof (RangeBarWidth), typeof (double), typeof (PartitionMapUIControl), new PropertyMetadata((object) 90.0));
    protected Point LastContextMenuPoint;
    internal ColumnDefinition colFirst;
    internal ScalablePartitionMapControl ctrlPMap;
    internal MapRangeControl ctrlRangeBar;
    internal Label txtMousePos;
    internal Label txtHoverValue;
    internal Label txtScale;
    private bool _contentLoaded;

    public double RangeBarWidth
    {
      get => (double) this.GetValue(ScalablePartitionMapUIControl.RangeBarWidthProperty);
      set => this.SetValue(ScalablePartitionMapUIControl.RangeBarWidthProperty, (object) value);
    }

    [DesignableMember(true)]
    public List<PartitionMap> PartitionMaps
    {
      get => this.ctrlPMap.PartitionMaps;
      set
      {
        this.ctrlPMap.PartitionMaps = value;
        this.ctrlPMap.ResetView();
        this.OnRangeBarChanged(this.ctrlRangeBar.Range);
      }
    }

    [DesignableMember(true)]
    public MapPinGroup PartitionMapPins
    {
      get => this.ctrlPMap.PartitionMapPins;
      set
      {
        this.ctrlPMap.PartitionMapPins = value;
        this.ctrlPMap.InvalidateVisual();
      }
    }

    public ScalablePartitionMapControl MapControl => this.ctrlPMap;

    public event Action<ScalablePartitionMapControl, NumericPoint> CenterPositionChangedEvent;

    public event Action<ScalablePartitionMapControl, double> ZoomRationChangedEvent;

    public ScalablePartitionMapUIControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.ctrlPMap.CenterPositionChangedEvent += new Action<ScalablePartitionMapControl, NumericPoint>(this.OnCenterPositionChanged);
      this.ctrlPMap.ZoomRationChangedEvent += new Action<ScalablePartitionMapControl, double>(this.OnZoomRatioChanged);
      this.ctrlRangeBar.Range = this.ctrlPMap.Range;
      this.ctrlRangeBar.ColorConveter = (IColorConverter) this.ctrlPMap.ColorConverter;
      this.ctrlRangeBar.DisplayFormat = "#,##0";
      this.ctrlPMap.Range.RangeChanged += new Action<MapRange>(this.OnRangeBarChanged);
      this.ctrlPMap.AllowSmoothZooming = true;
    }

    private void OnRangeBarChanged(MapRange range)
    {
      PartitionMap currentPartitionMap = this.ctrlPMap.CurrentPartitionMap;
      if (currentPartitionMap == null)
        return;
      List<Color> list = this.ctrlRangeBar.ColorConveter.GetColors().ToList<Color>();
      if (list.Count < 1)
        return;
      List<long> histogram1 = currentPartitionMap.Histogram;
      if (histogram1 == null || histogram1.Count == 0)
        return;
      List<long> histogram2 = new List<long>();
      double num1 = 0.0;
      if (range.IgnoreOutOfRange)
        num1 = range.Begin / 256.0;
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        long num2 = 0;
        double num3 = (range.Begin + range.Delta * (double) (index1 + 1) / (double) list.Count) / 256.0;
        for (int index2 = 0; index2 < histogram1.Count; ++index2)
        {
          if (num1 <= (double) index2 && (double) index2 < num3)
            num2 += histogram1[index2];
        }
        num1 = num3;
        histogram2.Add(num2);
      }
      range.SetHistogram(histogram2);
      this.ctrlRangeBar.InvalidateVisual();
    }

    private void OnCenterPositionChanged(ScalablePartitionMapControl pmap, NumericPoint pt)
    {
      Action<ScalablePartitionMapControl, NumericPoint> positionChangedEvent = this.CenterPositionChangedEvent;
      if (positionChangedEvent == null)
        return;
      positionChangedEvent(pmap, pt);
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      this.ctrlPMap.InvalidateVisual();
      this.ctrlRangeBar.InvalidateVisual();
      base.OnRender(drawingContext);
    }

    private void OnZoomRatioChanged(ScalablePartitionMapControl m, double ratio)
    {
      this.txtScale.Content = (object) string.Format("Zoom {0:0.0}%, Source {1:0.0}%", (object) (this.ctrlPMap.ZoomRatio * 100.0), (object) (this.ctrlPMap.SourceZoomRatio * 100.0));
      Action<ScalablePartitionMapControl, double> rationChangedEvent = this.ZoomRationChangedEvent;
      if (rationChangedEvent == null)
        return;
      rationChangedEvent(m, ratio);
    }

    private void ctrlPMap_MouseMove(object sender, MouseEventArgs e)
    {
      Point position = e.GetPosition((IInputElement) this.ctrlPMap);
      NumericVector usingViewPosition = this.ctrlPMap.GetValueUsingViewPosition(new NumericPoint(position.X, position.Y));
      if (usingViewPosition != null)
      {
        this.txtMousePos.Content = (object) string.Format("Pixel Position: (X{0:0}, Y{1:0})", (object) usingViewPosition.X, (object) -usingViewPosition.Y);
        this.txtHoverValue.Content = (object) string.Format("Value: {0:#,##0}", (object) usingViewPosition.Z);
      }
      else
      {
        this.txtMousePos.Content = (object) "";
        this.txtHoverValue.Content = (object) "";
      }
    }

    private void cmnuPin_CreatePinHere_Clicked(object sender, RoutedEventArgs e)
    {
      Point contextMenuPoint = this.LastContextMenuPoint;
      NumericVector usingViewPosition = this.ctrlPMap.GetValueUsingViewPosition(new NumericPoint(contextMenuPoint.X, contextMenuPoint.Y));
      if (usingViewPosition == null)
        return;
      string text = Application.Current.MainWindow.ShowInputMessage("Pin Name", "Please type the pin name.");
      if (text.IsNullOrEmpty())
        return;
      PartitionMap currentPartitionMap = this.ctrlPMap.CurrentPartitionMap;
      this.PartitionMapPins.Items.Add(new MapPinItem()
      {
        LocationX = usingViewPosition.X,
        LocationY = usingViewPosition.Y,
        PinName = text
      });
      this.ctrlPMap.InvalidateVisual();
    }

    private void cmnuPin_FindNearestPin_Clicked(object sender, RoutedEventArgs e) => this.MoveToNearestPin();

    public virtual void MoveToNearestPin()
    {
      MapPinItem nearest = this.PartitionMapPins.FindNearest(this.ctrlPMap.ConvertViewToRealPosition(new NumericPoint(Mouse.GetPosition((IInputElement) this.ctrlPMap))));
      if (nearest == null)
        return;
      this.ctrlPMap.MoveCenterPositionTo(nearest.LocationX, nearest.LocationY);
    }

    private void Grid_ContextMenuOpening(object sender, ContextMenuEventArgs e) => this.LastContextMenuPoint = Mouse.GetPosition((IInputElement) this);

    public void SetCenterPosition(NumericPoint arg2) => this.ctrlPMap.SetCenterPosition(arg2);

    public void SetZoom(double arg2)
    {
      this.txtScale.Content = (object) string.Format("Zoom {0:0.0}%, Source {1:0.0}%", (object) (this.ctrlPMap.ZoomRatio * 100.0), (object) (this.ctrlPMap.SourceZoomRatio * 100.0));
      this.ctrlPMap.SetZoom(arg2);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/partitionmaps/scalablepartitionmapuicontrol.xaml", UriKind.Relative));
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
          ((FrameworkElement) target).ContextMenuOpening += new ContextMenuEventHandler(this.Grid_ContextMenuOpening);
          break;
        case 2:
          this.colFirst = (ColumnDefinition) target;
          break;
        case 3:
          this.ctrlPMap = (ScalablePartitionMapControl) target;
          break;
        case 4:
          ((MenuItem) target).Click += new RoutedEventHandler(this.cmnuPin_CreatePinHere_Clicked);
          break;
        case 5:
          ((MenuItem) target).Click += new RoutedEventHandler(this.cmnuPin_FindNearestPin_Clicked);
          break;
        case 6:
          this.ctrlRangeBar = (MapRangeControl) target;
          break;
        case 7:
          this.txtMousePos = (Label) target;
          break;
        case 8:
          this.txtHoverValue = (Label) target;
          break;
        case 9:
          this.txtScale = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
