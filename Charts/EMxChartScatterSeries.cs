// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartScatterSeries
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.UI.Extensions;
using System;
using System.Windows.Media;

namespace EMx.UI.Charts
{
  public class EMxChartScatterSeries : IEMxChartSeries
  {
    public string SeriesName { get; set; }

    public string ValueName { get; protected set; }

    public string BaseName { get; protected set; }

    public object ItemSource { get; protected set; }

    public object BaseItemSource { get; protected set; }

    public float LineThickness { get; set; }

    public float SpotSize { get; set; }

    public Color LineColor { get; set; }

    public DateTime ItemUpdatedTime { get; protected set; }

    public bool IsShow { get; set; }

    public bool DisableTracing { get; set; }

    public bool UseMarkerDrawing { get; set; }

    public int MarkerSize { get; set; }

    public Color MarkerColor { get; set; }

    public EMxChartScatterSeries()
    {
      this.ItemSource = (object) null;
      this.BaseItemSource = (object) null;
      this.ValueName = (string) null;
      this.BaseName = (string) null;
      this.SeriesName = "";
      this.ItemUpdatedTime = DateTime.Now;
      this.LineColor = "4A7EBB".ToColor();
      this.IsShow = true;
      this.SpotSize = 0.0f;
      this.LineThickness = 2f;
      this.UseMarkerDrawing = false;
      this.MarkerSize = 5;
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

    public virtual void SetBaseItemSource(object itemsource, string name)
    {
      if (this.BaseItemSource != null)
      {
        lock (this.BaseItemSource)
        {
          if (this.BaseItemSource == itemsource)
            return;
          this.BaseItemSource = itemsource;
          this.ItemUpdatedTime = DateTime.Now;
          this.BaseName = name;
        }
      }
      else
      {
        this.BaseItemSource = itemsource;
        this.ItemUpdatedTime = DateTime.Now;
        this.BaseName = name;
      }
    }

    public virtual void SetItemSource(object itemsource, string base_name, string value_name)
    {
      this.SetItemSource(itemsource, value_name);
      this.SetBaseItemSource(itemsource, base_name);
    }

    public virtual void DetachSource()
    {
      this.SetItemSource((object) null, (string) null);
      this.SetBaseItemSource((object) null, (string) null);
    }
  }
}
