// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.MapPins.MapPinGroup
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Maths;
using EMx.Serialization;
using System;
using System.Collections.Generic;

namespace EMx.UI.PartitionMaps.MapPins
{
  [InstanceContract(ClassID = "5a73132f-b81f-4ed2-965f-58237d27dc81")]
  public class MapPinGroup
  {
    [InstanceMember]
    public List<MapPinItem> Items { get; set; }

    public MapPinGroup() => this.Items = new List<MapPinItem>();

    public virtual MapPinItem FindNearest(NumericPoint pt, Func<MapPinItem, bool> filter = null)
    {
      if (this.Items.Count == 0 || pt == null)
        return (MapPinItem) null;
      MapPinItem mapPinItem1 = (MapPinItem) null;
      double num1 = double.MaxValue;
      lock (this)
      {
        for (int index = 0; index < this.Items.Count; ++index)
        {
          MapPinItem mapPinItem2 = this.Items[index];
          if (filter == null || filter(mapPinItem2))
          {
            double num2 = mapPinItem2.LocationX - pt.X;
            double num3 = mapPinItem2.LocationY - pt.Y;
            double num4 = num2 * num2 + num3 * num3;
            if (num4 < num1)
            {
              num1 = num4;
              mapPinItem1 = mapPinItem2;
            }
          }
        }
      }
      return mapPinItem1;
    }
  }
}
