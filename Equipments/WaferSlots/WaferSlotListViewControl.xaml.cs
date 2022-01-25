// Decompiled with JetBrains decompiler
// Type: EMx.UI.Equipments.WaferSlots.WaferSlotListViewControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Equipments.Jobs;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EMx.UI.Equipments.WaferSlots
{
  [InstanceContract(ClassID = "6a570be9-78e6-4f2a-9889-4469589d0336")]
  public partial class WaferSlotListViewControl : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static DependencyProperty WaferSlotSourceProperty = DependencyProperty.Register(nameof (WaferSlotSource), typeof (IList<WaferSlotInformation>), typeof (WaferSlotListViewControl));
    private bool _contentLoaded;

    public IList<WaferSlotInformation> WaferSlotSource
    {
      get => (IList<WaferSlotInformation>) this.GetValue(WaferSlotListViewControl.WaferSlotSourceProperty);
      set => this.SetValue(WaferSlotListViewControl.WaferSlotSourceProperty, (object) value);
    }

    public WaferSlotListViewControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
    }

    private void mnuSelectAll_Clicked(object sender, RoutedEventArgs e) => this.SelectAll();

    private void mnuDeselectAll_Clicked(object sender, RoutedEventArgs e) => this.DeSelectAll();

    [QueryableMember(true)]
    public void SelectAll()
    {
      foreach (WaferSlotInformation waferSlotInformation in (IEnumerable<WaferSlotInformation>) this.WaferSlotSource)
        waferSlotInformation.IsSelected = true;
    }

    [QueryableMember(true)]
    public void DeSelectAll()
    {
      foreach (WaferSlotInformation waferSlotInformation in (IEnumerable<WaferSlotInformation>) this.WaferSlotSource)
        waferSlotInformation.IsSelected = false;
    }

    private void mnuSetSlots_Clicked(object sender, RoutedEventArgs e)
    {
    }

    private void mnuSetNames_Clicked(object sender, RoutedEventArgs e)
    {
    }

    private void btnSelectAll_Click(object sender, RoutedEventArgs e) => this.SelectAll();

    private void btnDeSelectAll_Click(object sender, RoutedEventArgs e) => this.DeSelectAll();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/equipments/waferslots/waferslotlistviewcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuSelectAll_Clicked);
          break;
        case 2:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuDeselectAll_Clicked);
          break;
        case 3:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuSetSlots_Clicked);
          break;
        case 4:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuSetNames_Clicked);
          break;
        case 5:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSelectAll_Click);
          break;
        case 6:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnDeSelectAll_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
