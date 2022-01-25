// Decompiled with JetBrains decompiler
// Type: EMx.UI.TimeEvents.WeeklyTimeEventDialogHandle`1
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Logging;
using EMx.Serialization;
using EMx.TimeEvents;
using System;
using System.Windows;

namespace EMx.UI.TimeEvents
{
  [InstanceContract(ClassID = "3b801cbc-682b-457a-8dc4-dfb71e700f2b")]
  public class WeeklyTimeEventDialogHandle<T> : IManagedType where T : class, new()
  {
    private static ILog log = LogManager.GetLogger();

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public TimeEventManager Manager { get; set; }

    [QueryableMember(true)]
    public void ShowWeeklyTimeEventDailog()
    {
      if (this.Manager == null)
      {
        WeeklyTimeEventDialogHandle<T>.log.Warn("Manager is null.");
      }
      else
      {
        WeeklyTimeEventManageDialog eventManageDialog = new WeeklyTimeEventManageDialog();
        eventManageDialog.CreateTimeEventItem = (Func<WeeklyTimeEvent>) (() =>
        {
          return new WeeklyTimeEvent()
          {
            ReferenceData = (object) new T()
          };
        });
        eventManageDialog.Owner = Application.Current.MainWindow;
        eventManageDialog.EventManager = this.Manager;
        bool? nullable = eventManageDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        this.Manager.Export();
      }
    }
  }
}
