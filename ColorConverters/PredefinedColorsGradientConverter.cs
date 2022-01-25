// Decompiled with JetBrains decompiler
// Type: EMx.UI.ColorConverters.PredefinedColorsGradientConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Extensions;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Linq;
using System.Windows.Media;

namespace EMx.UI.ColorConverters
{
  [InstanceContract(ClassID = "7e77c69e-5a40-4b27-8555-93b1fcdea0d8")]
  public class PredefinedColorsGradientConverter : PredefinedColorsConverter, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    public override Color ValueToColor(double begin, double end, double value)
    {
      if (value.IsInvalid())
        return Colors.Transparent;
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
        num = this.ViewPort.ToWorld(view) - 0.5;
      }
      if (num < 0.0)
        return this.ColorSet.First<Color>();
      if (num + 1.0 >= (double) this.ColorSet.Count)
        return this.ColorSet.Last<Color>();
      if (this.ColorSet.Count == 0)
        return Colors.White;
      int index1 = (int) num;
      if (index1 < 0 || index1 >= this.ColorSet.Count)
        return Colors.Transparent;
      double inter = num % 1.0;
      if (inter != 0.0)
      {
        int index2 = index1 + 1;
        if (index2 < this.ColorSet.Count)
        {
          Color color1 = this.ColorSet[index1];
          Color color2 = this.ColorSet[index2];
          return color1.Interpolate(inter, color2);
        }
      }
      return this.ColorSet[index1];
    }

    public override double ColorToValue(double begin, double end, Color color)
    {
      double val1 = this.ValueMappingFunc(begin);
      double val2 = this.ValueMappingFunc(end);
      double begin1 = Math.Min(val1, val2);
      double end1 = Math.Max(val1, val2);
      for (int index = 1; index < this.ColorSet.Count; ++index)
      {
        Color color1 = this.ColorSet[index - 1];
        Color color2 = this.ColorSet[index];
        if (color.IsBetween(color1, color2))
        {
          lock (this.ViewPort)
          {
            this.ViewPort.World.Set(0.0, (double) this.ColorSet.Count);
            this.ViewPort.View.Set(begin1, end1);
            double view1 = this.ViewPort.ToView((double) (index - 1));
            double view2 = this.ViewPort.ToView((double) index);
            double maxRatio = color.GetMaxRatio(color1, color2);
            double num = 1.0 - maxRatio;
            return this.ValueReverseMappingFunc(view1 * num + view2 * maxRatio);
          }
        }
      }
      return begin;
    }
  }
}
