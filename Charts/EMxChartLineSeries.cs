// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartLineSeries
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.UI.Extensions;
using System;
using System.Windows.Media;

namespace EMx.UI.Charts
{
  public class EMxChartLineSeries : IEMxChartSeries
  {
    public string SeriesName { get; set; }

    public string ValueName { get; protected set; }

    public object ItemSource { get; protected set; }

    public object BaseItemSource { get; set; }

    public string BaseName { get; protected set; }

    public float LineThickness { get; set; }

    public Color LineColor { get; set; }

    public bool IsShow { get; set; }

    public DateTime ItemUpdatedTime { get; protected set; }

    public bool DisableTracing { get; set; }

    public bool UseMarkerDrawing { get; set; }

    public int MarkerSize { get; set; }

    public Color MarkerColor { get; set; }

    public EMxChartLineSeries()
    {
      this.ItemSource = (object) null;
      this.ValueName = (string) null;
      this.SeriesName = "";
      this.ItemUpdatedTime = DateTime.Now;
      this.LineColor = "4A7EBB".ToColor();
      this.IsShow = true;
      this.LineThickness = 2f;
      this.UseMarkerDrawing = false;
      this.MarkerSize = 3;
      this.MarkerColor = Colors.Red;
    }

    public virtual void SetItemSource(object itemsource, string name)
    {
      if (this.ItemSource != null)
      {
        lock (this.ItemSource)
        {
          if (this.ItemSource == itemsource)
            return;
          this.ItemSource = itemsource;
          this.ItemUpdatedTime = DateTime.Now;
          this.ValueName = name;
        }
      }
      else
      {
        this.ItemSource = itemsource;
        this.ItemUpdatedTime = DateTime.Now;
        this.ValueName = name;
      }
    }

    public virtual void DetachSource() => this.SetItemSource((object) null, (string) null);
  }
}
