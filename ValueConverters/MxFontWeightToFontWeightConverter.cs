// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.MxFontWeightToFontWeightConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.UI.MxControls.Props;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EMx.UI.ValueConverters
{
  [ValueConversion(typeof (string), typeof (Thickness))]
  public class MxFontWeightToFontWeightConverter : IValueConverter
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value == null || !(value.GetType() == typeof (eMxFontWeight)) ? (object) null : (object) FontWeight.FromOpenTypeWeight((int) value);
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value == null || !value.GetType().Equals(typeof (FontWeight)) ? (object) null : (object) (eMxFontWeight) ((FontWeight) value).ToOpenTypeWeight();
    }
  }
}
