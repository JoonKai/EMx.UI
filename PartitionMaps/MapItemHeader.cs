// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.MapItemHeader
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Serialization;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "6fabda77-e6a5-4b85-a6d1-61fac9b40b23")]
  public class MapItemHeader
  {
    [InstanceMember]
    public int X;
    [InstanceMember]
    public int Y;
    [InstanceMember]
    public long BytesOffset;
    [InstanceMember]
    public int Length;
    [InstanceMember]
    public int Width;
    [InstanceMember]
    public int Height;
    [InstanceMember]
    public float RealX;
    [InstanceMember]
    public float RealY;
    [InstanceMember]
    public float RealW;
    [InstanceMember]
    public float RealH;
  }
}
