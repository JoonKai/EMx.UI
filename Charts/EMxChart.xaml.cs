// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChart
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Charts
{
  public partial class EMxChart : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ChartTitleProperty = DependencyProperty.Register(nameof (ChartTitle), typeof (string), typeof (EMxChart), (PropertyMetadata) new UIPropertyMetadata());
    public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (EMxChartAxis), typeof (EMxChart), (PropertyMetadata) new UIPropertyMetadata());
    public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (EMxChartAxis), typeof (EMxChart), (PropertyMetadata) new UIPropertyMetadata());
    public static readonly DependencyProperty ChartAreaProperty = DependencyProperty.Register(nameof (ChartArea), typeof (EMxChartArea), typeof (EMxChart), (PropertyMetadata) new UIPropertyMetadata());
    internal Grid ctrlGrid;
    internal TextBlock txtBottom;
    internal TextBlock txtLeft;
    private bool _contentLoaded;

    public EMxChartLayout Layout { get; set; }

    public string ChartTitle
    {
      get => (string) this.GetValue(EMxChart.ChartTitleProperty);
      set => this.SetValue(EMxChart.ChartTitleProperty, (object) value);
    }

    public EMxChartAxis XAxis
    {
      get => (EMxChartAxis) this.GetValue(EMxChart.XAxisProperty);
      set => this.SetValue(EMxChart.XAxisProperty, (object) value);
    }

    public EMxChartAxis YAxis
    {
      get => (EMxChartAxis) this.GetValue(EMxChart.YAxisProperty);
      set => this.SetValue(EMxChart.YAxisProperty, (object) value);
    }

    public EMxChartArea ChartArea
    {
      get => (EMxChartArea) this.GetValue(EMxChart.ChartAreaProperty);
      set => this.SetValue(EMxChart.ChartAreaProperty, (object) value);
    }

    public virtual string XAxisAlias
    {
      get => this.XAxis.AxisAlias;
      set => this.XAxis.AxisAlias = value;
    }

    public virtual string YAxisAlias
    {
      get => this.YAxis.AxisAlias;
      set => this.YAxis.AxisAlias = value;
    }

    public EMxChart()
    {
      this.InitializeComponent();
      this.XAxis = new EMxChartAxis()
      {
        AxisPosition = eChartAxisPosition.Bottom,
        AxisAlias = "X",
        AxisDataType = eChartAxisDataType.Numeric
      };
      this.YAxis = new EMxChartAxis()
      {
        AxisPosition = eChartAxisPosition.Left,
        AxisAlias = "Y",
        AxisDataType = eChartAxisDataType.Numeric
      };
      this.ChartArea = (EMxChartArea) new EMxChartAreaScatter();
      this.ChartTitle = "";
      this.Background = (Brush) Brushes.White;
      this.Layout = new EMxChartLayout();
      this.Layout.Chart = this;
    }

    public virtual void InvalidModels()
    {
      this.XAxis.InvalidateVisual();
      this.YAxis.InvalidateVisual();
      this.ChartArea.InvalidModels();
    }

    public virtual IEMxChartSeries GetOrCreateSeries(int index)
    {
      if (index >= this.ChartArea.Series.Count)
      {
        for (int count = this.ChartArea.Series.Count; count <= index; ++count)
          this.ChartArea.Series.Add((IEMxChartSeries) new EMxChartScatterSeries());
      }
      return this.ChartArea.Series[index];
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/charts/emxchart.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ctrlGrid = (Grid) target;
          break;
        case 2:
          this.txtBottom = (TextBlock) target;
          break;
        case 3:
          this.txtLeft = (TextBlock) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
