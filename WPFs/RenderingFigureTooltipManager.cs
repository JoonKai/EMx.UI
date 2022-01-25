// Decompiled with JetBrains decompiler
// Type: EMx.UI.WPFs.RenderingFigureTooltipManager
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EMx.UI.WPFs
{
  public class RenderingFigureTooltipManager
  {
    public List<RenderingFigureTooltip> Items { get; set; }

    public RenderingFigureTooltipManager() => this.Items = new List<RenderingFigureTooltip>();

    public virtual void Clear()
    {
      lock (this)
        this.Items.Clear();
    }

    public virtual void AddTooltip(Rect rc, string message)
    {
      lock (this)
        this.Items.Add(new RenderingFigureTooltip()
        {
          Area = rc,
          Tooltip = message
        });
    }

    public virtual RenderingFigureTooltip Find(Point pt)
    {
      lock (this)
        return this.Items.LastOrDefault<RenderingFigureTooltip>((Func<RenderingFigureTooltip, bool>) (x => x.Area.Contains(pt)));
    }
  }
}
