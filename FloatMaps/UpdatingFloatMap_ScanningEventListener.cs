// Decompiled with JetBrains decompiler
// Type: EMx.UI.FloatMaps.UpdatingFloatMap_ScanningEventListener
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.FloatMaps;
using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Equipments.Data;
using EMx.Equipments.WaferProcesses;
using EMx.Equipments.WaferProcesses.Scanner;
using EMx.Serialization;
using System.Collections.Generic;

namespace EMx.UI.FloatMaps
{
  [InstanceContract(ClassID = "4d585d59-84ef-4f7c-8e39-04bdd246336b")]
  public class UpdatingFloatMap_ScanningEventListener : ScannerEventListenerBase, IManagedType
  {
    [DesignableMember(true)]
    public List<WaferFloatMapControl> FloatMapControls { get; set; }

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public PersistFloatMap<double> Data { get; set; }

    public UpdatingFloatMap_ScanningEventListener()
    {
      this.FloatMapControls = new List<WaferFloatMapControl>();
      this.Data = (PersistFloatMap<double>) null;
    }

    public override bool ClearScanning(IWaferProcess sender, WaferData wafer)
    {
      this.UpdateFloatMapControl();
      return true;
    }

    public override bool PrepareScanning(IWaferProcess sender, WaferData wafer)
    {
      this.UpdateFloatMapControl();
      return true;
    }

    public override bool BeforeScanning(IWaferProcess sender, WaferData wafer)
    {
      this.UpdateFloatMapControl();
      return true;
    }

    public override bool DuringScanning(
      IWaferProcess sender,
      WaferData wafer,
      int curr_stage,
      int total_stage)
    {
      this.UpdateFloatMapControl();
      return true;
    }

    public override bool AfterScanning(IWaferProcess sender, WaferData wafer)
    {
      this.UpdateFloatMapControl();
      return true;
    }

    public virtual void UpdateFloatMapControl()
    {
      if (this.Data == null)
        return;
      for (int index = 0; index < this.FloatMapControls.Count; ++index)
      {
        WaferFloatMapControl floatMapControl = this.FloatMapControls[index];
        if (floatMapControl != null)
        {
          floatMapControl.FloatMap = this.Data;
          floatMapControl.InvalidModels();
        }
      }
    }
  }
}
