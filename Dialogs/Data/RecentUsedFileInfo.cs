// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.Data.RecentUsedFileInfo
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;

namespace EMx.UI.Dialogs.Data
{
  public class RecentUsedFileInfo
  {
    public string FilePath { get; set; }

    public int Count { get; set; }

    public DateTime LastUsedTime { get; set; }

    public DateTime FirstUsedTime { get; set; }

    public RecentUsedFileInfo() => this.FirstUsedTime = DateTime.Now;
  }
}
