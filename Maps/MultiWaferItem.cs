// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.MultiWaferItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.Data;
using EMx.Equipments.ProcessedData;
using EMx.IO.MxData;
using System.Collections.Generic;

namespace EMx.UI.Maps
{
  public class MultiWaferItem
  {
    public MxFile File { get; set; }

    public Dictionary<string, string> LotInfo { get; set; }

    public InspectionItem Inspection { get; set; }

    public MeasurementItem Measurement { get; set; }

    public MapData<double> Map { get; set; }

    public MultiWaferItem(
      MxFile file,
      Dictionary<string, string> lot,
      InspectionItem insp,
      MeasurementItem meas,
      MapData<double> map)
    {
      this.File = file;
      this.LotInfo = lot;
      this.Inspection = insp;
      this.Measurement = meas;
      this.Map = map;
    }
  }
}
