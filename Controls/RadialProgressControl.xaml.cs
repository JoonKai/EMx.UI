// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.RadialProgressControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Controls
{
  public partial class RadialProgressControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (double), typeof (RadialProgressControl), new PropertyMetadata(new PropertyChangedCallback(RadialProgressControl.OnPropertiesChanged)));
    public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (double), typeof (RadialProgressControl), new PropertyMetadata(new PropertyChangedCallback(RadialProgressControl.OnPropertiesChanged)));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (RadialProgressControl), new PropertyMetadata(new PropertyChangedCallback(RadialProgressControl.OnPropertiesChanged)));
    public static readonly DependencyProperty RadialColorProperty = DependencyProperty.Register(nameof (RadialColor), typeof (Brush), typeof (RadialProgressControl), new PropertyMetadata(new PropertyChangedCallback(RadialProgressControl.OnPropertiesChanged)));
    public static readonly DependencyProperty RadialThicknessProperty = DependencyProperty.Register(nameof (RadialThickness), typeof (int), typeof (RadialProgressControl), new PropertyMetadata(new PropertyChangedCallback(RadialProgressControl.OnPropertiesChanged)));
    internal RadialArcControl ctrlArc;
    internal Label lblProgress;
    private bool _contentLoaded;

    public double MinValue
    {
      get => (double) this.GetValue(RadialProgressControl.MinValueProperty);
      set => this.SetValue(RadialProgressControl.MinValueProperty, (object) value);
    }

    public double MaxValue
    {
      get => (double) this.GetValue(RadialProgressControl.MaxValueProperty);
      set => this.SetValue(RadialProgressControl.MaxValueProperty, (object) value);
    }

    public double Value
    {
      get => (double) this.GetValue(RadialProgressControl.ValueProperty);
      set => this.SetValue(RadialProgressControl.ValueProperty, (object) value);
    }

    public Brush RadialColor
    {
      get => (Brush) this.GetValue(RadialProgressControl.RadialColorProperty);
      set => this.SetValue(RadialProgressControl.RadialColorProperty, (object) value);
    }

    public int RadialThickness
    {
      get => (int) this.GetValue(RadialProgressControl.RadialThicknessProperty);
      set => this.SetValue(RadialProgressControl.RadialThicknessProperty, (object) value);
    }

    public RadialProgressControl()
    {
      this.InitializeComponent();
      this.RadialColor = "#F2F2F2".ToBrush();
      this.DataContext = (object) this;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);
      double num1 = this.MaxValue - this.MinValue;
      double num2 = 0.0;
      if (num1 > 0.0)
      {
        num2 = (this.Value - this.MinValue) / num1;
        this.ctrlArc.Length = 2.0 * Math.PI * num2;
      }
      this.lblProgress.Content = (object) string.Format("{0:0.0}%", (object) (100.0 * num2));
    }

    protected static void OnPropertiesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is RadialProgressControl radialProgressControl))
        return;
      radialProgressControl.InvalidateVisual();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/radialprogresscontrol.xaml", UriKind.Relative));
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
          this.ctrlArc = (RadialArcControl) target;
          break;
        case 2:
          this.lblProgress = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
