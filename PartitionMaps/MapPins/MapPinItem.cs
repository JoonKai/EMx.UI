// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.MapPins.MapPinItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Serialization;
using System;
using System.ComponentModel;

namespace EMx.UI.PartitionMaps.MapPins
{
  [InstanceContract(ClassID = "25e3cda8-b314-454f-8255-bdf5dc95287c")]
  public class MapPinItem
  {
    [InstanceMember]
    [GridViewItem(true)]
    [Category("Map Pin")]
    public double LocationX { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Map Pin")]
    public double LocationY { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Map Pin")]
    public string PinName { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Map Pin")]
    public DateTime CreateTime { get; set; }

    public MapPinItem()
    {
      this.PinName = "";
      this.CreateTime = DateTime.Now;
    }
  }
}
