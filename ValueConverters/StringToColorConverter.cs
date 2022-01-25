// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.StringToColorConverter
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
  [InstanceContract(ClassID = "155aabc0-5ec7-45be-a3f3-b6070dbeb3fa")]
  [ValueConversion(typeof (string), typeof (Color))]
  public class StringToColorConverter : IValueConverter, IManagedType
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is string rrggbb ? (object) rrggbb.ToColor() : (object) null;
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) ((Color) value).ConvertToString();
    }
  }
}
