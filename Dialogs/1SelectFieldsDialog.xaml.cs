// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.FieldSelectionItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.Collections.Generic;

namespace EMx.UI.Dialogs
{
  public class FieldSelectionItem
  {
    public string FieldName { get; set; }

    public int SelectedIndex { get; set; }

    public List<string> CandidateNames { get; set; }

    public List<string> CandidateSamples { get; set; }

    public FieldSelectionItem()
    {
      this.FieldName = "";
      this.SelectedIndex = -1;
      this.CandidateNames = new List<string>();
      this.CandidateSamples = new List<string>();
    }

    public FieldSelectionItem(string name, List<string> cnames, List<string> csamples)
    {
      this.FieldName = name;
      this.SelectedIndex = -1;
      this.CandidateNames = cnames;
      this.CandidateSamples = csamples;
    }
  }
}
