// Decompiled with JetBrains decompiler
// Type: EMx.UI.WPFs.MouseWheelGesture
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.Windows.Input;

namespace EMx.UI.WPFs
{
  public class MouseWheelGesture : MouseGesture
  {
    public eMouseWheelEventType WheelEventType { get; set; }

    public MouseWheelGesture(eMouseWheelEventType wheel = eMouseWheelEventType.Click)
      : this(ModifierKeys.None, MouseAction.WheelClick, wheel)
    {
    }

    public MouseWheelGesture(
      ModifierKeys modifiers,
      MouseAction mouse_action,
      eMouseWheelEventType wheel = eMouseWheelEventType.Click)
      : base(MouseAction.WheelClick, modifiers)
    {
      this.WheelEventType = wheel;
    }

    public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
    {
      if (!base.Matches(targetElement, inputEventArgs) || !(inputEventArgs is MouseWheelEventArgs mouseWheelEventArgs))
        return false;
      switch (this.WheelEventType)
      {
        case eMouseWheelEventType.Up:
          return mouseWheelEventArgs.Delta > 0;
        case eMouseWheelEventType.Click:
          return mouseWheelEventArgs.Delta == 0;
        case eMouseWheelEventType.Down:
          return mouseWheelEventArgs.Delta < 0;
        default:
          return false;
      }
    }

    public static MouseWheelGesture WheelDown => new MouseWheelGesture(eMouseWheelEventType.Down);

    public static MouseWheelGesture WheelUp => new MouseWheelGesture(eMouseWheelEventType.Up);
  }
}
