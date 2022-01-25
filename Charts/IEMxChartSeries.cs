// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.IEMxChartSeries
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Windows.Media;

namespace EMx.UI.Charts
{
  public interface IEMxChartSeries
  {
    string SeriesName { get; set; }

    string ValueName { get; }

    string BaseName { get; }

    object ItemSource { get; }

    object BaseItemSource { get; }

    float LineThickness { get; set; }

    Color LineColor { get; set; }

    bool IsShow { get; set; }

    bool UseMarkerDrawing { get; set; }

    int MarkerSize { get; set; }

    Color MarkerColor { get; set; }

    bool DisableTracing { get; set; }

    DateTime ItemUpdatedTime { get; }

    void SetItemSource(object itemsource, string name);

    void DetachSource();
  }
}
