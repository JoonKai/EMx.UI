// Decompiled with JetBrains decompiler
// Type: EMx.UI.TimeEvents.TimeEventListItemControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.TimeEvents;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.TimeEvents
{
  public partial class TimeEventListItemControl : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty EventManagerProperty = DependencyProperty.Register(nameof (EventManager), typeof (TimeEventManager), typeof (TimeEventListItemControl));
    internal DataGrid ctrlGrid;
    private bool _contentLoaded;

    public TimeEventManager EventManager
    {
      get => (TimeEventManager) this.GetValue(TimeEventListItemControl.EventManagerProperty);
      set => this.SetValue(TimeEventListItemControl.EventManagerProperty, (object) value);
    }

    public ITimeEvent SelectedItem => this.ctrlGrid.SelectedItem as ITimeEvent;

    public TimeEventListItemControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/timeevents/timeeventlistitemcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.ctrlGrid = (DataGrid) target;
      else
        this._contentLoaded = true;
    }
  }
}
