// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.PropertyGridItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using System.Reflection;
using System.Windows.Controls;

namespace EMx.UI.Engine.PropertyGrids
{
  internal class PropertyGridItem
  {
    public StackPanel Name { get; set; }

    public IPropertyGridHandler Value { get; set; }

    public PropertyInfo Property { get; set; }

    public object Source { get; set; }

    public static PropertyGridItem Create(
      object src,
      PropertyInfo prop,
      StackPanel name,
      IPropertyGridHandler value)
    {
      return new PropertyGridItem()
      {
        Name = name,
        Value = value,
        Property = prop,
        Source = src
      };
    }
  }
}
