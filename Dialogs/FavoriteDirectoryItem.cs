// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.FavoriteDirectoryItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Serialization;

namespace EMx.UI.Dialogs
{
  [InstanceContract(ClassID = "2653ae20-8c5b-458f-93b7-a5a76f25efde")]
  public class FavoriteDirectoryItem : IManagedType
  {
    [InstanceMember]
    [GridViewItem(true)]
    public int HitCount { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public string DirectoryPath { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public int No { get; set; }

    public FavoriteDirectoryItem()
    {
      this.HitCount = 0;
      this.DirectoryPath = "";
      this.No = 0;
    }
  }
}
