// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.CurrentRingInfo
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

namespace EMx.UI.Susceptors
{
  public class CurrentRingInfo
  {
    public double WaferSize { get; set; }

    public double Distance { get; set; }

    public int WaferCount { get; set; }

    public double InitialAngle { get; set; }

    public bool IsCCW { get; set; }

    public bool IsIn { get; set; }

    public CurrentRingInfo()
    {
      this.WaferSize = 10.0;
      this.Distance = 20.0;
      this.WaferCount = 30;
      this.InitialAngle = 40.0;
      this.IsCCW = true;
      this.IsIn = true;
    }
  }
}
