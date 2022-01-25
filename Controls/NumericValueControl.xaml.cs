// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.NumericValueControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
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
  [InstanceContract(ClassID = "986c44d9-bf1c-4ff8-ac51-50d384b8e084")]
  public partial class NumericValueControl : UserControl, IManagedType, IComponentConnector
  {
    public static DependencyProperty UnitProperty = DependencyProperty.Register(nameof (Unit), typeof (string), typeof (NumericValueControl));
    public static DependencyProperty UnitWidthProperty = DependencyProperty.Register(nameof (UnitWidth), typeof (int), typeof (NumericValueControl), new PropertyMetadata((object) 25));
    public static DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (NumericValueControl));
    internal TextBox txtValue;
    private bool _contentLoaded;

    public virtual string Unit
    {
      get => (string) this.GetValue(NumericValueControl.UnitProperty);
      set => this.SetValue(NumericValueControl.UnitProperty, (object) value);
    }

    public virtual int UnitWidth
    {
      get => (int) this.GetValue(NumericValueControl.UnitWidthProperty);
      set => this.SetValue(NumericValueControl.UnitWidthProperty, (object) value);
    }

    public virtual bool IsReadOnly
    {
      get => (bool) this.GetValue(NumericValueControl.IsReadOnlyProperty);
      set => this.SetValue(NumericValueControl.IsReadOnlyProperty, (object) value);
    }

    public NumericValueControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Unit = "nm";
      this.UnitWidth = 30;
      this.IsReadOnly = false;
    }

    public virtual double GetNumericValue()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
        return this.Dispatcher.Invoke<double>(new Func<double>(this.GetNumericValue));
      double result = 0.0;
      return double.TryParse(this.txtValue.Text.Trim(), out result) ? result : 0.0;
    }

    public virtual bool IsValid()
    {
      double result = 0.0;
      return double.TryParse(this.txtValue.Text.Trim(), out result);
    }

    public virtual void SetNumericValue(double value) => this.txtValue.Text = value.ToString();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/numericvaluecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.txtValue = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
