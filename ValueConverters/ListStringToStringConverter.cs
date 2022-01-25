// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.ListStringToStringConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EMx.UI.ValueConverters
{
  [InstanceContract(ClassID = "a65fe062-6f17-4844-8140-d819e7cc628a")]
  [ValueConversion(typeof (List<string>), typeof (string))]
  public class ListStringToStringConverter : IValueConverter
  {
    public string LineDelimiter { get; set; }

    public ListStringToStringConverter()
      : this("\r\n")
    {
    }

    public ListStringToStringConverter(string line_delimiter) => this.LineDelimiter = line_delimiter;

    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value != null && value.GetType() == typeof (List<string>) ? (object) (value as List<string>).AggregateText<string>(this.LineDelimiter, (Func<string, string>) (x => x)) : (object) "";
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is string text ? (object) ((IEnumerable<string>) text.Split(this.LineDelimiter)).ToList<string>() : (object) new List<string>();
    }
  }
}
