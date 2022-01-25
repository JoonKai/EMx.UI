// Decompiled with JetBrains decompiler
// Type: EMx.UI.CanvasExEvent
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;

namespace EMx.UI
{
  public class CanvasExEvent
  {
    private static CanvasExEvent g_pInst;

    public static CanvasExEvent Inst
    {
      get
      {
        if (CanvasExEvent.g_pInst == null)
          CanvasExEvent.g_pInst = new CanvasExEvent();
        return CanvasExEvent.g_pInst;
      }
    }

    public event Action<CanvasEx, object> ObjectSelectEvent;

    public void InvokeObjectSelectEvent(CanvasEx ui, object obj)
    {
      if (this.ObjectSelectEvent == null)
        return;
      this.ObjectSelectEvent(ui, obj);
    }

    private CanvasExEvent()
    {
    }
  }
}
