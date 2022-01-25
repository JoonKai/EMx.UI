// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.NumericRangeControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Maths;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Controls
{
  [InstanceContract(ClassID = "4fe8372c-9067-4ef2-9ad8-71e93651663f")]
  public partial class NumericRangeControl : UserControl, IManagedType, IComponentConnector
  {
    public static DependencyProperty UnitProperty = DependencyProperty.Register(nameof (Unit), typeof (string), typeof (NumericRangeControl));
    internal TextBox txtBegin;
    internal TextBox txtEnd;
    private bool _contentLoaded;

    public virtual string Unit
    {
      get => (string) this.GetValue(NumericRangeControl.UnitProperty);
      set => this.SetValue(NumericRangeControl.UnitProperty, (object) value);
    }

    public NumericRangeControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Unit = "nm";
    }

    public virtual double GetBegin()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
        return this.Dispatcher.Invoke<double>(new Func<double>(this.GetBegin));
      double result = 0.0;
      return double.TryParse(this.txtBegin.Text.Trim(), out result) ? result : 0.0;
    }

    public virtual double GetEnd()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
        return this.Dispatcher.Invoke<double>(new Func<double>(this.GetEnd));
      double result = 0.0;
      return double.TryParse(this.txtEnd.Text.Trim(), out result) ? result : 0.0;
    }

    public virtual NumericRange GetRange() => !this.IsValid() ? (NumericRange) null : new NumericRange(this.GetBegin(), this.GetEnd());

    public virtual void SetBegin(double value) => this.txtBegin.Text = value.ToString();

    public virtual void SetEnd(double value) => this.txtEnd.Text = value.ToString();

    public virtual void SetRange(double begin, double end)
    {
      this.SetBegin(begin);
      this.SetEnd(end);
    }

    public virtual bool IsValid()
    {
      double result = 0.0;
      return double.TryParse(this.txtBegin.Text.Trim(), out result) && double.TryParse(this.txtEnd.Text.Trim(), out result);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/numericrangecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtBegin = (TextBox) target;
          break;
        case 2:
          this.txtEnd = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
