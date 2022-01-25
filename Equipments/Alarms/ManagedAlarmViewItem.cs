// Decompiled with JetBrains decompiler
// Type: EMx.UI.Equipments.Alarms.ManagedAlarmViewItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.Alarms;
using EMx.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace EMx.UI.Equipments.Alarms
{
  public class ManagedAlarmViewItem : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public IManagedAlarm Alarm { get; set; }

    public string AlarmName => this.Alarm.AlarmName;

    public string AlarmCode => this.Alarm.AlarmCode;

    public string AlarmMessages => this.Alarm.AlarmMessages.AggregateText<string>("\r\n", (Func<string, string>) (x => x));

    public string Status => this.Alarm.Status == eAlarmStatus.Set ? "Alarm Set" : "Clear";

    public bool IsAlarmSet => this.Alarm != null && this.Alarm.Status == eAlarmStatus.Set;

    public Brush StatusBrush => this.Alarm.Status == eAlarmStatus.Set ? (Brush) Brushes.Red : (Brush) Brushes.White;

    public Brush StatusForeBrush => this.Alarm.Status == eAlarmStatus.Set ? (Brush) Brushes.White : (Brush) Brushes.Black;

    public ManagedAlarmViewItem(IManagedAlarm alarm) => this.Alarm = alarm;

    public void Invalidate() => this.PropertyChanged.InvokePropertiesChanged((object) this, "AlarmName", "AlarmCode", "AlarmMessages", "Status", "IsAlarmSet", "StatusBrush", "StatusForeBrush");
  }
}
