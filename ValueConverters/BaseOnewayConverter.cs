// Decompiled with JetBrains decompiler
// Type: EMx.UI.ValueConverters.BaseOnewayConverter
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EMx.UI.ValueConverters
{
  public abstract class BaseOnewayConverter : MarkupExtension, IValueConverter
  {
    public abstract object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture);

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider) => (object) this;
  }
}
