// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.ColorToSolidBrushConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EMx.UI.ValueConverters
{
  [InstanceContract(ClassID = "19258439-00b9-44f5-970e-23b1b06ee64d")]
  [ValueConversion(typeof (Color), typeof (SolidColorBrush))]
  public class ColorToSolidBrushConverter : IValueConverter, IManagedType
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value != null && value.GetType() == typeof (Color) ? (object) new SolidColorBrush((Color) value) : (object) Brushes.Transparent;
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is SolidColorBrush solidColorBrush ? (object) solidColorBrush.Color : (object) Colors.Transparent;
    }
  }
}
