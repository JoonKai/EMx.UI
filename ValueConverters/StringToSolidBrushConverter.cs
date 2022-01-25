// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.StringToSolidBrushConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EMx.UI.ValueConverters
{
  [InstanceContract(ClassID = "39f4805f-c02b-4b01-ba78-6e6ab762c9c4")]
  [ValueConversion(typeof (string), typeof (SolidColorBrush))]
  public class StringToSolidBrushConverter : IValueConverter, IManagedType
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is string rrggbb ? (object) new SolidColorBrush(rrggbb.ToColor()) : (object) null;
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is SolidColorBrush solidColorBrush ? (object) solidColorBrush.Color.ConvertToString() : (object) null;
    }
  }
}
