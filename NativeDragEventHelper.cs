// Decompiled with JetBrains decompiler
// Type: EMx.UI.NativeDragEventHelper
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
  public class NativeDragEventHelper
  {
    private static ILog log = LogManager.GetLogger();
    protected double PreviousDeltaX;
    protected double PreviousDeltaY;

    public virtual NativeDragEventHelper.eDragDirection DragDirection { get; set; }

    public virtual Point MouseBeginPosition { get; protected set; }

    public virtual Point MouseCurrentPosition { get; protected set; }

    public virtual bool IsDragging { get; protected set; }

    public virtual bool IsDragged { get; protected set; }

    public virtual bool UseCaptureMode { get; set; }

    public virtual bool IsCanceled { get; set; }

    public virtual int DragSequenceNo { get; protected set; }

    public event Action<NativeDragEventHelper> DragStart;

    protected virtual void OnDragStart()
    {
      if (this.DragStart == null)
        return;
      this.DragStart(this);
    }

    public event Action<NativeDragEventHelper, Point> DragMoving;

    protected virtual void OnDragMoving(Point delta)
    {
      if (this.DragMoving == null)
        return;
      this.DragMoving(this, delta);
    }

    public event Action<NativeDragEventHelper> DragEnd;

    protected virtual void OnDragEnd()
    {
      if (this.DragEnd == null)
        return;
      this.DragEnd(this);
    }

    public virtual UIElement TargetElement { get; protected set; }

    public NativeDragEventHelper()
    {
      this.DragDirection = NativeDragEventHelper.eDragDirection.Both;
      this.DragSequenceNo = 0;
      this.MouseBeginPosition = new Point();
      this.MouseCurrentPosition = new Point();
      this.IsDragging = false;
      this.UseCaptureMode = true;
      this.IsDragged = false;
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
      element.PreviewKeyDown += new KeyEventHandler(this.KeyDown);
    }

    private void KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
        return;
      e.Handled = true;
      this.IsCanceled = true;
      this.IsDragging = false;
      this.MouseUp(sender, (MouseButtonEventArgs) null);
    }

    public virtual void Unregister()
    {
      if (this.TargetElement == null)
        return;
      this.TargetElement.PreviewMouseDown -= new MouseButtonEventHandler(this.MouseDown);
      this.TargetElement.PreviewMouseUp -= new MouseButtonEventHandler(this.MouseUp);
      this.TargetElement.PreviewMouseMove -= new MouseEventHandler(this.MouseMove);
      this.TargetElement.PreviewKeyDown -= new KeyEventHandler(this.KeyDown);
      this.TargetElement = (UIElement) null;
    }

    protected virtual void MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Pressed)
        return;
      this.MouseBeginPosition = e.GetPosition((IInputElement) null);
      if (Helper.Data.AnyInvalid(this.MouseBeginPosition.X))
        this.MouseBeginPosition = new Point(0.0, this.MouseBeginPosition.Y);
      if (Helper.Data.AnyInvalid(this.MouseBeginPosition.Y))
        this.MouseBeginPosition = new Point(this.MouseBeginPosition.X, 0.0);
      this.MouseCurrentPosition = this.MouseBeginPosition;
      if (!Helper.Data.AnyInvalid(this.MouseBeginPosition.X, this.MouseBeginPosition.Y))
        return;
      NativeDragEventHelper.log.Warn((Action<LoggingToken>) (x => x.SetMessage("Cancel StartDrag due to invalid position value. Begin Position({0},{1})", (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y)));
    }

    protected virtual void MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (!this.IsDragging || e.LeftButton == MouseButtonState.Pressed)
        return;
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
      if (e.LeftButton != MouseButtonState.Pressed)
        return;
      this.MouseCurrentPosition = e.GetPosition((IInputElement) null);
      if (!this.IsDragging && !this.MouseBeginPosition.Equals(this.MouseCurrentPosition))
      {
        this.IsDragging = true;
        this.IsDragged = true;
        this.OnDragStart();
        if (this.UseCaptureMode)
          Mouse.Capture((IInputElement) this.TargetElement, CaptureMode.Element);
      }
      if (!this.IsDragging)
        return;
      Point delta;
      ref Point local = ref delta;
      double x1 = this.MouseCurrentPosition.X;
      Point point = this.MouseBeginPosition;
      double x2 = point.X;
      double x3 = x1 - x2;
      point = this.MouseCurrentPosition;
      double y1 = point.Y;
      point = this.MouseBeginPosition;
      double y2 = point.Y;
      double y3 = y1 - y2;
      local = new Point(x3, y3);
      if (this.DragDirection == NativeDragEventHelper.eDragDirection.Both && (delta.X != this.PreviousDeltaX || delta.Y != this.PreviousDeltaY))
        this.OnDragMoving(delta);
      else if (this.DragDirection == NativeDragEventHelper.eDragDirection.Horizon && delta.X != this.PreviousDeltaX)
        this.OnDragMoving(delta);
      else if (this.DragDirection == NativeDragEventHelper.eDragDirection.Vertical && delta.Y != this.PreviousDeltaY)
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
