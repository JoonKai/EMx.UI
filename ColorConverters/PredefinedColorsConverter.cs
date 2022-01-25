// Decompiled with JetBrains decompiler
// Type: EMx.UI.ColorConverters.PredefinedColorsConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Logging;
using EMx.Maths.Viewport;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace EMx.UI.ColorConverters
{
  [InstanceContract(ClassID = "a5834bb1-b3b8-48d0-af89-ff1d347f4223")]
  public class PredefinedColorsConverter : IColorConverter, IManagedType
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly int[] DefaultColorSet1 = new int[32]
    {
      16711935,
      15663359,
      13500671,
      11337983,
      9176319,
      7012607,
      5374207,
      2687207,
      (int) byte.MaxValue,
      10471,
      16829,
      20884,
      25971,
      32082,
      36393,
      41488,
      46592,
      54016,
      60160,
      65288,
      3800832,
      7077632,
      10878720,
      13565696,
      16776960,
      16767744,
      16761600,
      16750080,
      16744960,
      16733440,
      16728320,
      16711680
    };
    public static readonly int[] DefaultColorSet2 = new int[40]
    {
      128,
      69508,
      204680,
      340108,
      475536,
      610964,
      746648,
      948124,
      1084065,
      1285541,
      1419681,
      1814921,
      2210161,
      2605400,
      2868555,
      3329330,
      4313136,
      5296942,
      6411563,
      7591977,
      8772391,
      10083620,
      11460642,
      12902943,
      14411037,
      15260954,
      15386904,
      15578133,
      15769106,
      15894287,
      16084492,
      16274441,
      16398598,
      16588035,
      16711680,
      16123398,
      14750483,
      13442333,
      12133412,
      10824234
    };

    public virtual List<Color> ColorSet { get; set; }

    protected NumericRangeViewport ViewPort { get; set; }

    public Func<double, double> ValueMappingFunc { get; set; }

    public Func<double, double> ValueReverseMappingFunc { get; set; }

    public virtual bool UseCustomBackground { get; set; }

    public virtual Color CustomBackground { get; set; }

    public PredefinedColorsConverter()
    {
      this.ColorSet = new List<Color>();
      this.ViewPort = new NumericRangeViewport();
      this.SetLinearMap();
      this.SetColors((IEnumerable<int>) PredefinedColorsConverter.DefaultColorSet2);
    }

    public virtual Color ValueToColor(double begin, double end, double value)
    {
      double num = 0.0;
      double view = this.ValueMappingFunc(value);
      double val1 = this.ValueMappingFunc(begin);
      double val2 = this.ValueMappingFunc(end);
      double begin1 = Math.Min(val1, val2);
      double end1 = Math.Max(val1, val2);
      lock (this.ViewPort)
      {
        this.ViewPort.World.Set(0.0, (double) this.ColorSet.Count);
        this.ViewPort.View.Set(begin1, end1);
        num = this.ViewPort.ToWorld(view);
      }
      int index = (int) num;
      if (num < 0.0)
        return this.ColorSet.First<Color>();
      return num >= (double) this.ColorSet.Count ? this.ColorSet.Last<Color>() : this.ColorSet[index];
    }

    public virtual double ColorToValue(double begin, double end, Color color)
    {
      double val1 = this.ValueMappingFunc(begin);
      double val2 = this.ValueMappingFunc(end);
      double begin1 = Math.Min(val1, val2);
      double end1 = Math.Max(val1, val2);
      for (int index = 0; index < this.ColorSet.Count; ++index)
      {
        Color color1 = this.ColorSet[index];
        if (color.Equals(color1))
        {
          lock (this.ViewPort)
          {
            this.ViewPort.World.Set(0.0, (double) this.ColorSet.Count);
            this.ViewPort.View.Set(begin1, end1);
            return this.ValueReverseMappingFunc(this.ViewPort.ToView((double) index));
          }
        }
      }
      return begin;
    }

    public virtual void SetLinearMap()
    {
      this.ValueMappingFunc = (Func<double, double>) (x => x);
      this.ValueReverseMappingFunc = (Func<double, double>) (x => x);
    }

    public virtual void SetLogMap()
    {
      this.ValueMappingFunc = (Func<double, double>) (x => Math.Log(x));
      this.ValueReverseMappingFunc = (Func<double, double>) (x => Math.Exp(x));
    }

    public virtual void SetLog10Map()
    {
      this.ValueMappingFunc = (Func<double, double>) (x => Math.Log10(x));
      this.ValueReverseMappingFunc = (Func<double, double>) (x => Math.Pow(10.0, x));
    }

    public virtual void SetExpMap()
    {
      this.ValueMappingFunc = (Func<double, double>) (x => Math.Exp(x));
      this.ValueReverseMappingFunc = (Func<double, double>) (x => Math.Log(x));
    }

    public virtual void SetPowerMap(double base_num = 10.0)
    {
      this.ValueMappingFunc = (Func<double, double>) (x => Math.Pow(base_num, x));
      this.ValueReverseMappingFunc = (Func<double, double>) (x => Math.Log(x, base_num));
    }

    public virtual void SetColors(IEnumerable<int> enumerable)
    {
      lock (this.ViewPort)
      {
        lock (this.ColorSet)
        {
          this.ColorSet.Clear();
          IEnumerator<int> enumerator = enumerable.GetEnumerator();
          while (enumerator.MoveNext())
          {
            int current = enumerator.Current;
            this.ColorSet.Add(Color.FromRgb((byte) (current >> 16 & (int) byte.MaxValue), (byte) (current >> 8 & (int) byte.MaxValue), (byte) (current & (int) byte.MaxValue)));
          }
        }
      }
    }

    public virtual void SetColors(IEnumerable<string> enumerable)
    {
      lock (this.ViewPort)
      {
        lock (this.ColorSet)
        {
          this.ColorSet.Clear();
          IEnumerator<string> enumerator = enumerable.GetEnumerator();
          while (enumerator.MoveNext())
          {
            string rrggbb = enumerator.Current.Trim();
            if (!string.IsNullOrWhiteSpace(rrggbb))
              this.ColorSet.Add(rrggbb.ToColor());
          }
        }
      }
    }

    public virtual void SetColors(IEnumerable<Color> colors)
    {
      lock (this.ViewPort)
      {
        lock (this.ColorSet)
        {
          this.ColorSet.Clear();
          this.ColorSet.AddRange(colors);
        }
      }
    }

    public virtual IEnumerable<Color> GetColors() => (IEnumerable<Color>) this.ColorSet;
  }
}
