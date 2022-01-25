// Decompiled with JetBrains decompiler
// Type: EMx.UI.UIElementDragEventHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using EMx.Logging;
using System;
using System.Windows;
using System.Windows.Input;

namespace EMx.UI
{
  public class UIElementDragEventHelper
  {
    private static ILog log = LogManager.GetLogger();
    protected double PreviousDeltaX;
    protected double PreviousDeltaY;
    protected bool IsMouseDown;

    public virtual UIElementDragEventHelper.eDragDirection DragDirection { get; set; }

    public virtual Point MouseBeginPosition { get; protected set; }

    public virtual Point MouseCurrentPosition { get; protected set; }

    public virtual bool IsDragging { get; protected set; }

    public virtual bool UseCaptureMode { get; set; }

    public virtual int DragSequenceNo { get; protected set; }

    public virtual UIElement TargetElement { get; protected set; }

    public event Action<UIElementDragEventHelper, UIElementDragStartArgs> DragStart;

    protected virtual UIElementDragStartArgs OnDragStart()
    {
      UIElementDragStartArgs elementDragStartArgs = new UIElementDragStartArgs();
      if (this.DragStart != null)
        this.DragStart(this, elementDragStartArgs);
      return elementDragStartArgs;
    }

    public event Action<UIElementDragEventHelper, Point> DragMoving;

    protected virtual void OnDragMoving(Point delta)
    {
      if (this.DragMoving == null)
        return;
      this.DragMoving(this, delta);
    }

    public event Action<UIElementDragEventHelper> DragEnd;

    protected virtual void OnDragEnd()
    {
      if (this.DragEnd == null)
        return;
      this.DragEnd(this);
    }

    public UIElementDragEventHelper()
    {
      this.DragDirection = UIElementDragEventHelper.eDragDirection.Both;
      this.DragSequenceNo = 0;
      this.MouseBeginPosition = new Point();
      this.MouseCurrentPosition = new Point();
      this.IsDragging = false;
      this.UseCaptureMode = true;
      this.TargetElement = (UIElement) null;
    }

    public virtual void Register(UIElement element)
    {
      this.Unregister();
      if (element == null)
        return;
      this.TargetElement = element;
      element.PreviewMouseDown += new MouseButtonEventHandler(this.MouseDown);
      element.PreviewMouseUp += new MouseButtonEventHandler(this.MouseUp);
      element.PreviewMouseMove += new MouseEventHandler(this.MouseMove);
    }

    public virtual void Unregister()
    {
      if (this.TargetElement == null)
        return;
      this.TargetElement.PreviewMouseDown -= new MouseButtonEventHandler(this.MouseDown);
      this.TargetElement.PreviewMouseUp -= new MouseButtonEventHandler(this.MouseUp);
      this.TargetElement.PreviewMouseMove -= new MouseEventHandler(this.MouseMove);
      this.TargetElement = (UIElement) null;
    }

    protected virtual void MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.TargetElement == null || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.MouseBeginPosition = e.GetPosition((IInputElement) this.TargetElement);
      this.IsMouseDown = true;
      if (Helper.Data.AnyInvalid(this.MouseBeginPosition.X))
        this.MouseBeginPosition = new Point(0.0, this.MouseBeginPosition.Y);
      Point mouseBeginPosition;
      if (Helper.Data.AnyInvalid(this.MouseBeginPosition.Y))
      {
        mouseBeginPosition = this.MouseBeginPosition;
        this.MouseBeginPosition = new Point(mouseBeginPosition.X, 0.0);
      }
      this.MouseCurrentPosition = this.MouseBeginPosition;
      DataHelper data = Helper.Data;
      double[] numArray = new double[2];
      mouseBeginPosition = this.MouseBeginPosition;
      numArray[0] = mouseBeginPosition.X;
      mouseBeginPosition = this.MouseBeginPosition;
      numArray[1] = mouseBeginPosition.Y;
      if (!data.AnyInvalid(numArray))
        return;
      UIElementDragEventHelper.log.Warn((Action<LoggingToken>) (x => x.SetMessage("Cancel StartDrag due to invalid position value. Begin Position({0},{1})", (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y)));
    }

    protected virtual void MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (!this.IsDragging || this.TargetElement == null || e.LeftButton == MouseButtonState.Pressed)
        return;
      this.IsMouseDown = false;
      this.IsDragging = false;
      if (this.UseCaptureMode)
        Mouse.Capture((IInputElement) null);
      this.PreviousDeltaX = double.MinValue;
      this.PreviousDeltaY = double.MinValue;
      e.Handled = true;
      this.OnDragEnd();
    }

    protected virtual void MouseMove(object sender, MouseEventArgs e)
    {
      if (this.TargetElement == null || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.MouseCurrentPosition = e.GetPosition((IInputElement) this.TargetElement);
      Point point;
      int num;
      if (this.IsMouseDown && !this.IsDragging)
      {
        point = this.MouseBeginPosition;
        num = !point.Equals(this.MouseCurrentPosition) ? 1 : 0;
      }
      else
        num = 0;
      if (num != 0)
      {
        this.IsDragging = true;
        if (this.OnDragStart().IsAbortDrag)
        {
          this.IsDragging = false;
          return;
        }
        if (this.UseCaptureMode)
          Mouse.Capture((IInputElement) this.TargetElement, CaptureMode.Element);
      }
      if (!this.IsDragging)
        return;
      Point delta;
      ref Point local = ref delta;
      point = this.MouseCurrentPosition;
      double x1 = point.X;
      point = this.MouseBeginPosition;
      double x2 = point.X;
      double x3 = x1 - x2;
      point = this.MouseCurrentPosition;
      double y1 = point.Y;
      point = this.MouseBeginPosition;
      double y2 = point.Y;
      double y3 = y1 - y2;
      local = new Point(x3, y3);
      if (this.DragDirection == UIElementDragEventHelper.eDragDirection.Both && (delta.X != this.PreviousDeltaX || delta.Y != this.PreviousDeltaY))
        this.OnDragMoving(delta);
      else if (this.DragDirection == UIElementDragEventHelper.eDragDirection.Horizon && delta.X != this.PreviousDeltaX)
        this.OnDragMoving(delta);
      else if (this.DragDirection == UIElementDragEventHelper.eDragDirection.Vertical && delta.Y != this.PreviousDeltaY)
        this.OnDragMoving(delta);
      this.PreviousDeltaX = delta.X;
      this.PreviousDeltaY = delta.Y;
      e.Handled = true;
    }

    public enum eDragDirection
    {
      Horizon,
      Vertical,
      Both,
    }
  }
}
