// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartArea
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Controls;

namespace EMx.UI.Charts
{
  public abstract class EMxChartArea : UserControl
  {
    protected DateTime LastUpdateTime;

    public List<IEMxChartSeries> Series { get; set; }

    protected EMxChart Chart { get; set; }

    public eAreaLineDuplicationRule DuplicationRule { get; set; }

    public bool UseZoomingControl { get; set; }

    public virtual bool UseTraceFunction { get; set; }

    public SmoothingMode SmoothMode { get; set; }

    public string XAxisDisplayFormat { get; set; }

    public string YAxisDisplayformat { get; set; }

    protected EMxChartArea()
    {
      this.Series = new List<IEMxChartSeries>();
      this.DuplicationRule = eAreaLineDuplicationRule.AnyPoint;
      this.LastUpdateTime = new DateTime();
      this.SmoothMode = SmoothingMode.AntiAlias;
      this.XAxisDisplayFormat = "0.00";
      this.YAxisDisplayformat = "Auto";
      this.UseZoomingControl = true;
      this.InitializeContextMenu();
    }

    public virtual void InvalidModels()
    {
      this.LastUpdateTime = DateTime.MinValue;
      this.InvalidateVisual();
    }

    protected virtual ContextMenu InitializeContextMenu()
    {
      ContextMenu contextMenu = new ContextMenu();
      this.ContextMenu = contextMenu;
      return contextMenu;
    }
  }
}
