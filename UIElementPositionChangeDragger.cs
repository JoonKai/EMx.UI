// Decompiled with JetBrains decompiler
// Type: EMx.UI.UIElementPositionChangeDragger
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using EMx.Logging;
using EMx.UI.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EMx.UI
{
  public class UIElementPositionChangeDragger
  {
    private static ILog log = LogManager.GetLogger();
    protected Func<bool> IsSimpleTargetMovable = (Func<bool>) null;

    public Point MouseBeginPosition { get; set; }

    public Point OriginPosition { get; set; }

    public bool IsDragging { get; protected set; }

    public bool UseCaptureMode { get; set; }

    protected UIElement Parent { get; set; }

    protected UIElement Target { get; set; }

    public bool AllowHorizontalChange { get; set; }

    public bool AllowVerticalChanging { get; set; }

    public UIElementPositionChangeDragger()
    {
      this.MouseBeginPosition = new Point();
      this.OriginPosition = new Point();
      this.IsDragging = false;
      this.UseCaptureMode = true;
      this.AllowHorizontalChange = true;
      this.AllowVerticalChanging = true;
    }

    public virtual bool StartDrag(UIElement parent, UIElement target)
    {
      if (parent == null || this.IsDragging)
        return false;
      Canvas parent1 = parent.FindParent<Canvas>();
      if (parent1 == null)
        return false;
      this.Parent = parent;
      this.Target = target;
      this.OriginPosition = new Point(Canvas.GetLeft(this.Parent), Canvas.GetTop(this.Parent));
      this.MouseBeginPosition = Mouse.GetPosition((IInputElement) parent1);
      DataHelper data1 = Helper.Data;
      double[] numArray1 = new double[1];
      Point point = this.OriginPosition;
      numArray1[0] = point.X;
      if (data1.AnyInvalid(numArray1))
      {
        point = this.OriginPosition;
        this.OriginPosition = new Point(0.0, point.Y);
      }
      DataHelper data2 = Helper.Data;
      double[] numArray2 = new double[1];
      point = this.OriginPosition;
      numArray2[0] = point.Y;
      if (data2.AnyInvalid(numArray2))
      {
        point = this.OriginPosition;
        this.OriginPosition = new Point(point.X, 0.0);
      }
      DataHelper data3 = Helper.Data;
      double[] numArray3 = new double[4];
      point = this.MouseBeginPosition;
      numArray3[0] = point.X;
      point = this.MouseBeginPosition;
      numArray3[1] = point.Y;
      point = this.OriginPosition;
      numArray3[2] = point.X;
      point = this.OriginPosition;
      numArray3[3] = point.Y;
      if (data3.AnyInvalid(numArray3))
      {
        UIElementPositionChangeDragger.log.Warn((Action<LoggingToken>) (x => x.SetMessage("Cancel StartDrag due to invalid position value. StartDrag UI.Pos({0},{1}) Mouse.Pos({2},{3}) CaptureMode({4})", (object) this.OriginPosition.X, (object) this.OriginPosition.Y, (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y, (object) this.UseCaptureMode)));
        return false;
      }
      this.IsDragging = true;
      if (this.UseCaptureMode)
        Mouse.Capture((IInputElement) target);
      return true;
    }

    public virtual bool StopDrag()
    {
      if (!this.IsDragging || this.Parent == null)
        return false;
      this.IsDragging = false;
      Point point = new Point(Canvas.GetLeft(this.Parent), Canvas.GetTop(this.Parent));
      if (this.UseCaptureMode)
        Mouse.Capture((IInputElement) null);
      return true;
    }

    public virtual void MouseMoved()
    {
      if (!this.IsDragging || this.Parent == null)
        return;
      Canvas parent = this.Parent.FindParent<Canvas>();
      if (parent == null)
        return;
      Vector vector = Mouse.GetPosition((IInputElement) parent) - this.MouseBeginPosition;
      Point point = this.OriginPosition + vector;
      if (vector.X == 0.0 && vector.Y == 0.0)
        return;
      if (this.AllowHorizontalChange)
        Canvas.SetLeft(this.Parent, point.X);
      if (this.AllowVerticalChanging)
        Canvas.SetTop(this.Parent, point.Y);
      this.Parent.InvalidateVisual();
    }

    public virtual void SimpleRegister(UIElement parent, UIElement target, Func<bool> available)
    {
      this.Parent = parent;
      this.IsSimpleTargetMovable = available;
      if (this.Target == target)
        return;
      if (this.Target != null)
      {
        this.Target.MouseDown -= new MouseButtonEventHandler(this.Target_MouseDown);
        this.Target.MouseMove -= new MouseEventHandler(this.Target_MouseMove);
        this.Target.MouseUp -= new MouseButtonEventHandler(this.Target_MouseUp);
      }
      this.Target = target;
      if (target == null)
        return;
      this.Target.MouseDown += new MouseButtonEventHandler(this.Target_MouseDown);
      this.Target.MouseMove += new MouseEventHandler(this.Target_MouseMove);
      this.Target.MouseUp += new MouseButtonEventHandler(this.Target_MouseUp);
    }

    private void Target_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.IsSimpleTargetMovable == null || !this.IsSimpleTargetMovable() || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.StartDrag(this.Parent, sender as UIElement);
    }

    private void Target_MouseMove(object sender, MouseEventArgs e) => this.MouseMoved();

    private void Target_MouseUp(object sender, MouseButtonEventArgs e) => this.StopDrag();
  }
}
