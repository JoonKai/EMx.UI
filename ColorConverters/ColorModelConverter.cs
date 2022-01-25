// Decompiled with JetBrains decompiler
// Type: EMx.UI.ColorConverters.ColorModelConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Maths;
using System;
using System.Windows.Media;

namespace EMx.UI.ColorConverters
{
  public class ColorModelConverter
  {
    public static Color HSV2RGB(NumericVector hsv) => ColorModelConverter.HSV2RGB(hsv.X, hsv.Y, hsv.Z);

    public static Color HSV2RGB(double hue, double saturation, double lightness)
    {
      Color color = new Color();
      if (lightness <= 0.0)
      {
        color.ScR = 0.0f;
        color.ScG = 0.0f;
        color.ScB = 0.0f;
        return color;
      }
      int num = Convert.ToInt32(Math.Floor(hue / 60.0)) % 6;
      return color;
    }

    public static NumericVector RGB2HSV(Color clr) => (NumericVector) null;
  }
}
