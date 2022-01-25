// Decompiled with JetBrains decompiler
// Type: EMx.UI.Data.PolynomialInterpolationDataControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Extensions;
using EMx.Maths.Numerics;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Data
{
  [InstanceContract(ClassID = "39820a05-3b74-4a32-8469-52c11df2e22c")]
  public partial class PolynomialInterpolationDataControl : 
    UserControl,
    IManagedType,
    IComponentConnector
  {
    public static DependencyProperty HighOrderVisibilityProperty = DependencyProperty.Register(nameof (HighOrderVisibility), typeof (Visibility), typeof (PolynomialInterpolationDataControl));
    internal TextBox txtTitle;
    internal TextBox txtIntercept;
    internal TextBox txtCoE1;
    internal TextBox txtCoE2;
    internal TextBox txtCoE3;
    private bool _contentLoaded;

    [InstanceMember]
    [GridViewItem(true)]
    public Visibility HighOrderVisibility
    {
      get => (Visibility) this.GetValue(PolynomialInterpolationDataControl.HighOrderVisibilityProperty);
      set => this.SetValue(PolynomialInterpolationDataControl.HighOrderVisibilityProperty, (object) value);
    }

    public virtual string Title
    {
      get => this.txtTitle.Text;
      set => this.txtTitle.Text = value;
    }

    public PolynomialInterpolationDataControl()
    {
      this.InitializeComponent();
      this.HighOrderVisibility = Visibility.Visible;
      this.DataContext = (object) this;
    }

    public virtual void CtrlToData(PolynomialInterpolation3 poly)
    {
      poly.CoE1 = this.txtCoE1.Text.ToDouble(0.0);
      poly.CoE2 = this.txtCoE2.Text.ToDouble(0.0);
      poly.CoE3 = this.txtCoE3.Text.ToDouble(0.0);
      poly.Intercept = this.txtIntercept.Text.ToDouble(0.0);
    }

    public virtual void DataToCtrl(PolynomialInterpolation3 poly)
    {
      this.txtCoE1.Text = poly.CoE1.ToString();
      this.txtCoE2.Text = poly.CoE2.ToString();
      this.txtCoE3.Text = poly.CoE3.ToString();
      this.txtIntercept.Text = poly.Intercept.ToString();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/data/polynomialinterpolationdatacontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtTitle = (TextBox) target;
          break;
        case 2:
          this.txtIntercept = (TextBox) target;
          break;
        case 3:
          this.txtCoE1 = (TextBox) target;
          break;
        case 4:
          this.txtCoE2 = (TextBox) target;
          break;
        case 5:
          this.txtCoE3 = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
