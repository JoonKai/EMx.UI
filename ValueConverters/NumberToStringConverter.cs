// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.NumberToStringConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace EMx.UI.ValueConverters
{
  [ValueConversion(typeof (int), typeof (string))]
  public class NumberToStringConverter : IValueConverter, IManagedType
  {
    public Dictionary<int, string> Table { get; set; }

    public NumberToStringConverter() => this.Table = new Dictionary<int, string>();

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is int key && this.Table.ContainsKey(key) ? (object) this.Table[key] : (object) value.ToString();
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value is string b)
      {
        Dictionary<int, string>.Enumerator enumerator = this.Table.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if (string.Equals(enumerator.Current.Value, b))
            return (object) enumerator.Current.Key;
        }
      }
      return (object) 0;
    }
  }
}
