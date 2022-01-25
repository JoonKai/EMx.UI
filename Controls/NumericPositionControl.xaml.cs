// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.NumericPositionControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Maths;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

namespace EMx.UI.Controls
{
  public partial class NumericPositionControl : UserControl, IComponentConnector
  {
    public static DependencyProperty UnitProperty = DependencyProperty.Register(nameof (UnitFirst), typeof (string), typeof (NumericRangeControl));
    public static DependencyProperty UnitSecondProperty = DependencyProperty.Register(nameof (UnitSecond), typeof (string), typeof (NumericRangeControl));
    internal DoubleUpDown txtBegin;
    internal DoubleUpDown txtEnd;
    private bool _contentLoaded;

    public virtual string UnitFirst
    {
      get => (string) this.GetValue(NumericPositionControl.UnitProperty);
      set => this.SetValue(NumericPositionControl.UnitProperty, (object) value);
    }

    public virtual string UnitSecond
    {
      get => (string) this.GetValue(NumericPositionControl.UnitSecondProperty);
      set => this.SetValue(NumericPositionControl.UnitSecondProperty, (object) value);
    }

    public event NumericPositionControl.DoubleEventHandler FirstValueKeyUpEvent;

    public event NumericPositionControl.DoubleEventHandler SecondValueKeyUpEvent;

    public NumericPositionControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.UnitFirst = "X";
      this.UnitSecond = "Y";
    }

    public virtual double GetBegin() => this.Dispatcher.Thread != Thread.CurrentThread ? this.Dispatcher.Invoke<double>(new Func<double>(this.GetBegin)) : this.txtBegin.Value.Value;

    public virtual double GetEnd() => this.Dispatcher.Thread != Thread.CurrentThread ? this.Dispatcher.Invoke<double>(new Func<double>(this.GetEnd)) : this.txtEnd.Value.Value;

    public virtual NumericPoint GetPoint() => !this.IsValid() ? (NumericPoint) null : new NumericPoint(this.GetBegin(), this.GetEnd());

    public virtual void SetFirst(double value) => this.txtBegin.Text = value.ToString();

    public virtual void SetSecond(double value) => this.txtEnd.Text = value.ToString();

    public virtual void SetValue(double begin, double end)
    {
      this.SetFirst(begin);
      this.SetSecond(end);
    }

    public virtual bool IsValid()
    {
      double result = 0.0;
      return double.TryParse(this.txtBegin.Text.Trim(), out result) && double.TryParse(this.txtEnd.Text.Trim(), out result);
    }

    private void txtBegin_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      NumericPositionControl.DoubleEventHandler firstValueKeyUpEvent = this.FirstValueKeyUpEvent;
      if (firstValueKeyUpEvent != null)
        firstValueKeyUpEvent(this.txtBegin.Value.Value);
    }

    private void txtEnd_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      NumericPositionControl.DoubleEventHandler secondValueKeyUpEvent = this.SecondValueKeyUpEvent;
      if (secondValueKeyUpEvent != null)
        secondValueKeyUpEvent(this.txtEnd.Value.Value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/numericpositioncontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtBegin = (DoubleUpDown) target;
          this.txtBegin.KeyUp += new KeyEventHandler(this.txtBegin_KeyUp);
          break;
        case 2:
          this.txtEnd = (DoubleUpDown) target;
          this.txtEnd.KeyUp += new KeyEventHandler(this.txtEnd_KeyUp);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public delegate void DoubleEventHandler(double value);
  }
}
