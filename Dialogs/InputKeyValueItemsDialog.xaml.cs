// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.InputKeyValueItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

namespace EMx.UI.Dialogs
{
  public partial class InputKeyValueItem
  {
    public string Key { get; set; }

    public string Value { get; set; }

    public InputKeyValueItem(string key, string value)
    {
      this.Key = key;
      this.Value = value;
    }
  }
}
