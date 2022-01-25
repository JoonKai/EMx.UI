// Decompiled with JetBrains decompiler
// Type: EMx.UI.Windows.WindowsHotKeyArguments
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.Windows.Input;

namespace EMx.UI.Windows
{
  public class WindowsHotKeyArguments
  {
    public Key Key { get; protected set; }

    public eHotKeyModifier ModifierKey { get; protected set; }

    public bool IsHandled { get; set; }

    public WindowsHotKeyArguments(eHotKeyModifier modifier, Key key)
    {
      this.ModifierKey = modifier;
      this.Key = key;
      this.IsHandled = false;
    }
  }
}
