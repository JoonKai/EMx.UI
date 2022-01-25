// Decompiled with JetBrains decompiler
// Type: EMx.UI.Compositions.CompositionRatioSimulationControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Maths;
using EMx.Physics.AlCompositions;
using EMx.Serialization;
using EMx.UI.Charts;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EMx.UI.Compositions
{
  [InstanceContract(ClassID = "2701705e-4699-4f62-9a5c-f1c0ac14b6ab")]
  public partial class CompositionRatioSimulationControl : 
    UserControl,
    IManagedType,
    IComponentConnector
  {
    protected AlCompositionFormulaData Formula;
    internal TextBox txtE1;
    internal TextBox txtE2;
    internal TextBox txtE3;
    internal TextBox txtBegin;
    internal TextBox txtEnd;
    internal TextBox txtStep;
    internal EMxChart ctrlChart;
    private bool _contentLoaded;

    public CompositionRatioSimulationControl()
    {
      this.InitializeComponent();
      this.Formula = new AlCompositionFormulaData();
    }

    private void mnuCalc_Clicked(object sender, RoutedEventArgs e)
    {
      double num1 = this.txtBegin.Text.ToDouble(double.NaN);
      double num2 = this.txtEnd.Text.ToDouble(double.NaN);
      double num3 = this.txtStep.Text.ToDouble(double.NaN);
      if (Helper.Data.AnyInvalid(num1, num2, num3) || num3 == 0.0)
      {
        int num4 = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", "Invalid Wavelength Range.");
      }
      else
      {
        double num5 = this.txtE1.Text.ToDouble(double.NaN);
        double num6 = this.txtE2.Text.ToDouble(double.NaN);
        double num7 = this.txtE3.Text.ToDouble(double.NaN);
        if (Helper.Data.AnyInvalid(num5, num6, num7))
        {
          int num8 = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", "Invalid Coefficient parameters.");
        }
        else
        {
          this.Formula.E1 = num5;
          this.Formula.E2 = num6;
          this.Formula.E3 = num7;
          List<NumericPoint> points = new List<NumericPoint>();
          for (double num9 = num1; num9 <= num2; num9 += num3)
          {
            double num10 = this.Formula.Calculate(num9);
            if (!num10.IsInvalid())
              points.AddXY(num9, num10 * 100.0);
          }
          EMxChartScatterSeries getScatterSeries = this.ctrlChart.ChartArea.CreateOrGetScatterSeries(0);
          getScatterSeries.SetItemSource((object) points, "X", "Y");
          getScatterSeries.UseMarkerDrawing = true;
          this.ctrlChart.InvalidModels();
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/compositions/compositionratiosimulationcontrol.xaml", UriKind.Relative));
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
          this.txtE1 = (TextBox) target;
          break;
        case 2:
          this.txtE2 = (TextBox) target;
          break;
        case 3:
          this.txtE3 = (TextBox) target;
          break;
        case 4:
          this.txtBegin = (TextBox) target;
          break;
        case 5:
          this.txtEnd = (TextBox) target;
          break;
        case 6:
          this.txtStep = (TextBox) target;
          break;
        case 7:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.mnuCalc_Clicked);
          break;
        case 8:
          this.ctrlChart = (EMxChart) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
