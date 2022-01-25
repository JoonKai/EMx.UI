// Decompiled with JetBrains decompiler
// Type: EMx.UI.TimeEvents.DailyTimeEventItemLocation
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.TimeEvents;
using System.Windows;

namespace EMx.UI.TimeEvents
{
  public class DailyTimeEventItemLocation
  {
    public ITimeEvent TimeEvent { get; set; }

    public Rect Location { get; set; }

    public DailyTimeEventItemLocation(ITimeEvent evt, Rect loc)
    {
      this.TimeEvent = evt;
      this.Location = loc;
    }
  }
}
