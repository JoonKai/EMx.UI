// Decompiled with JetBrains decompiler
// Type: EMx.UI.Converters.ValueToColorConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;

namespace EMx.UI.Converters
{
  public class ValueToColorConverter
  {
    private static readonly int[] RANGE_COLOR_INT = new int[34]
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
      16711680,
      16711680,
      16711680
    };

    public static int ValueToColor(double value, double from, double to)
    {
      double num1 = 0.0;
      double num2 = (double) (ValueToColorConverter.RANGE_COLOR_INT.Length - 1);
      double num3 = to - from;
      double num4 = num2 - num1 + 1.0;
      if (num3 == 0.0)
        num3 = 0.01;
      double num5 = (value - from) * num4 / num3 + num1;
      int index = Math.Max(0, Math.Min((int) num2, (int) num5));
      return index < 0 || index >= ValueToColorConverter.RANGE_COLOR_INT.Length ? 16777215 : ValueToColorConverter.RANGE_COLOR_INT[index];
    }
  }
}
