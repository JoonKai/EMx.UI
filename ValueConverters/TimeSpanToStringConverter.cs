// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.TimeSpanToStringConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EMx.UI.ValueConverters
{
  [InstanceContract(ClassID = "3775dc48-eab4-468e-9ea9-062ecbfc9669")]
  [ValueConversion(typeof (TimeSpan), typeof (string))]
  public class TimeSpanToStringConverter : IValueConverter, IManagedType
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value == null || value.GetType() != typeof (TimeSpan))
        return (object) null;
      TimeSpan timeSpan = (TimeSpan) value;
      return (object) string.Format("{0:00}:{1:00}:{2:00}", (object) (int) timeSpan.TotalHours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (!(value is string str))
        return (object) null;
      char[] chArray = new char[1]{ ':' };
      List<string> list = ((IEnumerable<string>) str.Split(chArray)).ToList<string>();
      if (list.Count != 3)
        return (object) null;
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      bool flag1 = int.TryParse(list[0], out result1);
      bool flag2 = int.TryParse(list[1], out result2);
      bool flag3 = int.TryParse(list[2], out result3);
      return !flag1 || !flag2 || !flag3 ? (object) null : (object) new TimeSpan(result1, result2, result3);
    }
  }
}
