// Decompiled with JetBrains decompiler
// Type: EMx.UI.ColorConverters.IColorConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using System.Collections.Generic;
using System.Windows.Media;

namespace EMx.UI.ColorConverters
{
  public interface IColorConverter : IManagedType
  {
    Color ValueToColor(double begin, double end, double value);

    double ColorToValue(double begin, double end, Color color);

    void SetColors(IEnumerable<Color> colors);

    IEnumerable<Color> GetColors();
  }
}
