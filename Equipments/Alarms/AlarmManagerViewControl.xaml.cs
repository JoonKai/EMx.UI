// Decompiled with JetBrains decompiler
// Type: EMx.UI.Equipments.Alarms.AlarmManagerViewControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Equipments.Alarms;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using EMx.UI.MxControls.Props;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EMx.UI.Equipments.Alarms
{
  [InstanceContract(ClassID = "540782ba-5331-4872-8aff-bb071e2c3907")]
  public partial class AlarmManagerViewControl : 
    UserControl,
    IManagedType,
    IComponentConnector,
    IStyleConnector
  {
    private static ILog log = LogManager.GetLogger();
    private ObservableCollection<ManagedAlarmViewItem> ViewItems;
    internal ListView ctrlItems;
    private bool _contentLoaded;

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public ManagedAlarmManager AlarmManager { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Alarm")]
    public bool UseActivatedFiltering { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI")]
    public double PopupDialogItemFontSize { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI")]
    public eMxFontWeight PopupDialogItemFontWeight { get; set; }

    public AlarmManagerViewControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.PopupDialogItemFontSize = 20.0;
      this.PopupDialogItemFontWeight = eMxFontWeight.Medium;
      this.ViewItems = new ObservableCollection<ManagedAlarmViewItem>();
      this.ctrlItems.ItemsSource = (IEnumerable) this.ViewItems;
    }

    [LinkTrigger(eLinkTrigger.Unlinked)]
    private void OnUnlinked()
    {
      if (this.AlarmManager == null)
        return;
      this.AlarmManager.AlarmChanagedEvent -= new Action<ManagedAlarmManager, IManagedAlarm>(this.OnAlarmChanged);
    }

    [LinkTrigger(eLinkTrigger.Linked)]
    private void OnLinked()
    {
      if (this.AlarmManager == null)
        return;
      this.AlarmManager.AlarmChanagedEvent += new Action<ManagedAlarmManager, IManagedAlarm>(this.OnAlarmChanged);
    }

    private void OnAlarmChanged(ManagedAlarmManager mgr, IManagedAlarm alarm)
    {
      if (mgr == null || alarm == null)
        return;
      this.Dispatcher.BeginAction(0, new Action(this.UpdateAlarmItems));
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) => this.UpdateAlarmItems();

    private void UpdateAlarmItems()
    {
      List<IManagedAlarm> allAlarms = this.AlarmManager.GetAllAlarms();
      for (int index = 0; index < allAlarms.Count; ++index)
      {
        IManagedAlarm a = allAlarms[index];
        ManagedAlarmViewItem managedAlarmViewItem = this.ViewItems.FirstOrDefault<ManagedAlarmViewItem>((Func<ManagedAlarmViewItem, bool>) (x => x.Alarm == a));
        if (managedAlarmViewItem == null)
        {
          managedAlarmViewItem = new ManagedAlarmViewItem(a);
          this.ViewItems.Add(managedAlarmViewItem);
        }
        if (this.UseActivatedFiltering && !managedAlarmViewItem.IsAlarmSet)
          this.ViewItems.Remove(managedAlarmViewItem);
      }
      for (int index = 0; index < this.ViewItems.Count; ++index)
        this.ViewItems[index].Invalidate();
    }

    private void TryClearAlarm_Clicked(object sender, RoutedEventArgs e)
    {
      if (!(sender is Button button) || !(button.Tag is ManagedAlarmViewItem tag) || tag.Alarm == null)
        return;
      IManagedAlarm alarm = tag.Alarm;
      List<string> list = alarm.ClearAlternatives.Select<IAlarmClearAlternative, string>((Func<IAlarmClearAlternative, string>) (x => x.AlternativeName)).ToList<string>();
      SelectListItemDialog selectListItemDialog = new SelectListItemDialog();
      selectListItemDialog.Owner = Application.Current.MainWindow;
      selectListItemDialog.Title = "Select Alternative To Clear Alarm";
      selectListItemDialog.ItemFontSize = this.PopupDialogItemFontSize;
      selectListItemDialog.ItemFontWeight = this.PopupDialogItemFontWeight;
      selectListItemDialog.ItemSource = (IEnumerable) list;
      bool? nullable = selectListItemDialog.ShowDialog();
      bool flag1 = true;
      if (!(nullable.GetValueOrDefault() == flag1 & nullable.HasValue))
        return;
      IAlarmClearAlternative clearAlternative = alarm.ClearAlternatives[selectListItemDialog.SelectedIndex];
      AlarmManagerViewControl.log.Debug("Clear to clear alarm[{0}] with alternative[{1}]", (object) alarm.AlarmName, (object) clearAlternative.AlternativeName);
      WaitingDialog waitingDialog = WaitingDialog.ShowWaitingDialog((Window) null, (Var<bool>) null, true);
      bool flag2 = alarm.ClearAlarm(clearAlternative);
      waitingDialog.ReqClose.Value = true;
      if (flag2)
      {
        int num1 = (int) Application.Current.MainWindow.ShowNormalMessage("Alarm", "Clear success.");
      }
      else
      {
        int num2 = (int) Application.Current.MainWindow.ShowNormalMessage("Alarm", "Fail to clear.");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/equipments/alarms/alarmmanagerviewcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
          break;
        case 2:
          this.ctrlItems = (ListView) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 3)
        return;
      ((ButtonBase) target).Click += new RoutedEventHandler(this.TryClearAlarm_Clicked);
    }
  }
}
