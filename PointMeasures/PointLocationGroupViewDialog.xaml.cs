// Decompiled with JetBrains decompiler
// Type: EMx.UI.PointMeasures.PointLocationGroupViewDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.Measures.Points;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EMx.UI.PointMeasures
{
  public partial class PointLocationGroupViewDialog : Window, IComponentConnector
  {
    internal Label lblName;
    internal Label lblNumOfPoints;
    internal Label lblWaferSize;
    internal PointLocationGroupViewControl ctrlView;
    private bool _contentLoaded;

    public PointMeasurementLocationGroup PointGroup
    {
      get => this.ctrlView.PointGroup;
      set => this.ctrlView.PointGroup = value;
    }

    public double WaferSize
    {
      get => this.ctrlView.WaferSize;
      set => this.ctrlView.WaferSize = value;
    }

    public PointLocationGroupViewDialog() => this.InitializeComponent();

    private void btnClose_Clicked(object sender, RoutedEventArgs e) => this.Close();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      PointMeasurementLocationGroup pointGroup = this.PointGroup;
      if (pointGroup == null)
        return;
      this.lblName.Content = (object) pointGroup.Name;
      this.lblNumOfPoints.Content = (object) pointGroup.Items.Count;
      this.lblWaferSize.Content = (object) this.WaferSize;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/pointmeasures/pointlocationgroupviewdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.lblName = (Label) target;
          break;
        case 3:
          this.lblNumOfPoints = (Label) target;
          break;
        case 4:
          this.lblWaferSize = (Label) target;
          break;
        case 5:
          this.ctrlView = (PointLocationGroupViewControl) target;
          break;
        case 6:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnClose_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
