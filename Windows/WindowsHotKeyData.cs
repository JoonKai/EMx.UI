// Decompiled with JetBrains decompiler
// Type: EMx.UI.Windows.WindowsHotKeyData
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.Windows.Input;

namespace EMx.UI.Windows
{
  public class WindowsHotKeyData
  {
    public Key Key { get; set; }

    public eHotKeyModifier ModifierKey { get; set; }

    public WindowsHotKeyHandler Handler { get; set; }

    public bool Enabled { get; set; }

    public int RegisteredID { get; set; }

    public WindowsHotKeyData() => this.Enabled = true;

    public WindowsHotKeyData(
      eHotKeyModifier modifier,
      Key key,
      WindowsHotKeyHandler handler,
      int id)
      : this()
    {
      this.Key = key;
      this.ModifierKey = modifier;
      this.Handler = handler;
      this.RegisteredID = id;
    }

    public virtual int ConvertToLParam()
    {
      int modifierKey = (int) this.ModifierKey;
      return KeyInterop.VirtualKeyFromKey(this.Key) << 16 | modifierKey;
    }

    public virtual int ConvertToVitualKey() => KeyInterop.VirtualKeyFromKey(this.Key);
  }
}
