// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.MapRange
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Maths;
using EMx.Serialization;
using System;
using System.Collections.Generic;

namespace EMx.UI.Maps
{
  [InstanceContract(ClassID = "cb91f496-8199-4c67-9936-52471aeb199a")]
  public class MapRange : NumericRange, IManagedType
  {
    public virtual bool UseAutoRange { get; set; }

    public virtual bool UseDistribution { get; set; }

    public virtual List<long> Distributions { get; set; }

    public virtual bool IgnoreOutOfRange { get; set; }

    public event Action<MapRange> RangeChanged;

    public virtual void InvokeRangeChanged()
    {
      if (this.RangeChanged == null)
        return;
      this.RangeChanged(this);
    }

    public MapRange()
    {
      this.UseAutoRange = true;
      this.Distributions = new List<long>();
      this.UseDistribution = true;
      this.IgnoreOutOfRange = false;
    }

    public virtual void SetHistogram(List<long> histogram)
    {
      lock (this)
      {
        List<long> distributions = this.Distributions;
        distributions.Clear();
        distributions.AddRange((IEnumerable<long>) histogram);
      }
    }

    public virtual NumericRange GetHistogramRange(int index)
    {
      if (index < 0 || index >= this.Distributions.Count)
        return (NumericRange) null;
      lock (this)
      {
        int count = this.Distributions.Count;
        if (count <= 1)
          return new NumericRange(this.Begin, this.End);
        double num = this.Delta / (double) count;
        return new NumericRange(this.Begin + num * (double) index, this.Begin + num * (double) (index + 1));
      }
    }

    public virtual void Distribute(int distributions, List<double> items)
    {
      lock (this)
      {
        List<long> distributions1 = this.Distributions;
        distributions1.Clear();
        if (items.Count == 0)
          return;
        if (this.UseAutoRange)
        {
          items.Sort();
          this.Begin = items[items.Count * 125 / 1000];
          this.End = items[items.Count * 875 / 1000];
        }
        if (!this.UseDistribution)
          return;
        double num1 = this.Delta;
        if (num1 == 0.0)
          num1 = 1.0;
        double num2 = num1 / (double) distributions;
        int num3 = distributions - 1;
        for (int index = 0; index < distributions; ++index)
          distributions1.Add(0L);
        foreach (double d in items)
        {
          if (!double.IsNaN(d) && !double.IsInfinity(d) && (!this.IgnoreOutOfRange || d >= this.Begin && d <= this.End))
          {
            int index = (int) Math.Max(0.0, Math.Min((double) num3, (d - this.Begin) / num2));
            ++distributions1[index];
          }
        }
      }
    }

    public virtual void Distribute(int distributions, List<byte> items, bool ignoreOutOfRange = false)
    {
      lock (this)
      {
        List<long> distributions1 = this.Distributions;
        distributions1.Clear();
        if (items.Count == 0)
          return;
        if (this.UseAutoRange)
        {
          items.Sort();
          this.Begin = (double) items[items.Count * 125 / 1000];
          this.End = (double) items[items.Count * 875 / 1000];
        }
        if (!this.UseDistribution)
          return;
        double num1 = this.Delta;
        if (num1 == 0.0)
          num1 = 1.0;
        double num2 = num1 / (double) distributions;
        int num3 = distributions - 1;
        for (int index = 0; index < distributions; ++index)
          distributions1.Add(0L);
        foreach (byte num4 in items)
        {
          if (!double.IsNaN((double) num4) && !double.IsInfinity((double) num4) && (!ignoreOutOfRange || (double) num4 >= this.Begin && (double) num4 <= this.End))
          {
            int index = (int) Math.Max(0.0, Math.Min((double) num3, ((double) num4 - this.Begin) / num2));
            ++distributions1[index];
          }
        }
      }
    }
  }
}
