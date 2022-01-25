// Decompiled with JetBrains decompiler
// Type: EMx.UI.TimeEvents.WeeklyTimeEventManageDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Logging;
using EMx.TimeEvents;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using EMx.UI.PropertyGrids;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.TimeEvents
{
  public partial class WeeklyTimeEventManageDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty EventManagerProperty = DependencyProperty.Register(nameof (EventManager), typeof (TimeEventManager), typeof (WeeklyTimeEventManageDialog));
    internal TimeEventListItemControl ctrlList;
    internal TimeEventWeeklyTimeTableControl ctrlTable;
    private bool _contentLoaded;

    public Func<WeeklyTimeEvent> CreateTimeEventItem { get; set; }

    public TimeEventManager EventManager
    {
      get => (TimeEventManager) this.GetValue(WeeklyTimeEventManageDialog.EventManagerProperty);
      set
      {
        this.SetValue(WeeklyTimeEventManageDialog.EventManagerProperty, (object) value);
        this.ctrlList.EventManager = value;
        this.ctrlTable.EventManager = value;
      }
    }

    public WeeklyTimeEventManageDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.CreateTimeEventItem = (Func<WeeklyTimeEvent>) (() => new WeeklyTimeEvent());
      this.EventManager = new TimeEventManager();
    }

    private void btnOK_Clicked(object sender, RoutedEventArgs e)
    {
      for (int index = 0; index < this.EventManager.TimeEvents.Count; ++index)
      {
        ITimeEvent timeEvent = this.EventManager.TimeEvents[index];
        string str = timeEvent.ValidateTime();
        if (!str.IsNullOrEmpty())
        {
          int num = (int) this.ShowWarningMessage("Validation Failure", str);
          if (!(timeEvent is WeeklyTimeEvent weeklyTimeEvent4))
            return;
          this.ShowModifyTimeEventItemDialog(weeklyTimeEvent4);
          return;
        }
      }
      this.DialogResult = new bool?(true);
    }

    private void btnCancel_Clicked(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void mnuTimeEvt_Add_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.CreateTimeEventItem == null)
      {
        WeeklyTimeEventManageDialog.log.Error("CreateTimeEventItem is null.");
      }
      else
      {
        WeeklyTimeEvent weeklyTimeEvent = this.CreateTimeEventItem();
        if (this.ShowModifyTimeEventItemDialog(weeklyTimeEvent))
          this.EventManager.TimeEvents.Add((ITimeEvent) weeklyTimeEvent);
        this.ctrlTable.InvalidateVisual();
      }
    }

    private void mnuTimeEvt_Modify_Clicked(object sender, RoutedEventArgs e)
    {
      if (!(this.ctrlList.SelectedItem is WeeklyTimeEvent selectedItem))
        return;
      this.ShowModifyTimeEventItemDialog(selectedItem);
      this.ctrlTable.InvalidateVisual();
    }

    private bool ShowModifyTimeEventItemDialog(WeeklyTimeEvent item)
    {
      if (item == null)
        return false;
      List<object> objectList = new List<object>()
      {
        (object) item
      };
      if (item.ReferenceData != null)
        objectList.Add(item.ReferenceData);
      PropertyGridDialog propertyGridDialog = new PropertyGridDialog();
      propertyGridDialog.Owner = (Window) this;
      propertyGridDialog.UseIndirectMode = true;
      propertyGridDialog.SelectedObject = (object) objectList;
      bool? nullable = propertyGridDialog.ShowDialog();
      bool flag1 = true;
      bool flag2 = nullable.GetValueOrDefault() == flag1 & nullable.HasValue;
      if (flag2)
        item.InvokeAllPropertiesChanged();
      return flag2;
    }

    private void ctrlList_MouseDoubleClick(object sender, MouseButtonEventArgs e) => this.mnuTimeEvt_Modify_Clicked(sender, (RoutedEventArgs) e);

    private void mnuTimeEvt_Remove_Clicked(object sender, RoutedEventArgs e)
    {
      ITimeEvent selectedItem = this.ctrlList.SelectedItem;
      if (selectedItem == null)
        return;
      if (this.ShowQuestionMessage("Manager", string.Format("Would you like to remove '{0}' item?", (object) selectedItem.EventName)) == eDialogButtons.Yes)
        this.EventManager.TimeEvents.Remove(selectedItem);
      this.ctrlTable.InvalidateVisual();
    }

    private void mnuTimeEvt_RemoveAll_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.ShowQuestionMessage("Manager", "Would you like to remove all items?") == eDialogButtons.Yes)
        this.EventManager.TimeEvents.Clear();
      this.ctrlTable.InvalidateVisual();
    }

    private void mnuFile_Import_Clicked(object sender, RoutedEventArgs e)
    {
      string str = this.OpenFileDialog("", "TimeEvent Data(*.ted)|*.ted", "ted");
      if (str.IsNullOrEmpty())
        return;
      if (!this.EventManager.Import(str))
      {
        int num = (int) this.ShowWarningMessage("Manager", "Fail to load time event data.");
      }
      else
        this.ctrlTable.InvalidateVisual();
    }

    private void mnuFile_Export_Clicked(object sender, RoutedEventArgs e)
    {
      string str = this.SaveFileDialog("", "TimeEvent Data(*.ted)|*.ted", "ted");
      if (str.IsNullOrEmpty())
        return;
      if (!this.EventManager.Export(str))
      {
        int num = (int) this.ShowWarningMessage("Manager", "Fail to save time event data.");
      }
      else
        this.ctrlTable.InvalidateVisual();
    }

    private void ctrlTable_ItemDoubleClickEvent(object sender, ITimeEvent item)
    {
      this.ShowModifyTimeEventItemDialog(item as WeeklyTimeEvent);
      this.ctrlTable.InvalidateVisual();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/timeevents/weeklytimeeventmanagedialog.xaml", UriKind.Relative));
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
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuFile_Import_Clicked);
          break;
        case 2:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuFile_Export_Clicked);
          break;
        case 3:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Add_Clicked);
          break;
        case 4:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Modify_Clicked);
          break;
        case 5:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Remove_Clicked);
          break;
        case 6:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_RemoveAll_Clicked);
          break;
        case 7:
          this.ctrlList = (TimeEventListItemControl) target;
          break;
        case 8:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Add_Clicked);
          break;
        case 9:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Modify_Clicked);
          break;
        case 10:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuTimeEvt_Remove_Clicked);
          break;
        case 11:
          this.ctrlTable = (TimeEventWeeklyTimeTableControl) target;
          break;
        case 12:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnOK_Clicked);
          break;
        case 13:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCancel_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
