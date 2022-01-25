// Decompiled with JetBrains decompiler
// Type: EMx.UI.UIElementSizeChangeDragger
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
  public class UIElementSizeChangeDragger
  {
    private static ILog log = LogManager.GetLogger();

    public Point MouseBeginPosition { get; set; }

    public Point OriginSize { get; set; }

    public bool IsDragging { get; protected set; }

    public bool UseCaptureMode { get; set; }

    public bool AllowHorizontalChange { get; set; }

    public bool AllowVerticalChanging { get; set; }

    public double MinWidth { get; set; }

    public double MinHeight { get; set; }

    protected FrameworkElement Parent { get; set; }

    protected UIElement Target { get; set; }

    public UIElementSizeChangeDragger()
    {
      this.MouseBeginPosition = new Point();
      this.OriginSize = new Point();
      this.IsDragging = false;
      this.UseCaptureMode = true;
      this.AllowHorizontalChange = true;
      this.AllowVerticalChanging = true;
      this.MinWidth = 10.0;
      this.MinHeight = 10.0;
    }

    public virtual bool StartDrag(FrameworkElement parent, UIElement target)
    {
      if (parent == null || this.IsDragging)
        return false;
      this.Parent = parent;
      this.Target = target;
      this.OriginSize = new Point(parent.ActualWidth, parent.ActualHeight);
      this.MouseBeginPosition = Mouse.GetPosition((IInputElement) Application.Current.MainWindow);
      UIElementSizeChangeDragger.log.Info((Action<LoggingToken>) (x => x.SetMessage("StartDrag UI.Size({0},{1}) Mouse.Pos({2},{3}) CaptureMode({4})", (object) this.OriginSize.X, (object) this.OriginSize.Y, (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y, (object) this.UseCaptureMode)));
      DataHelper data = Helper.Data;
      double[] numArray = new double[4]
      {
        this.MouseBeginPosition.X,
        0.0,
        0.0,
        0.0
      };
      Point point = this.MouseBeginPosition;
      numArray[1] = point.Y;
      point = this.OriginSize;
      numArray[2] = point.X;
      point = this.OriginSize;
      numArray[3] = point.Y;
      if (data.AnyInvalid(numArray))
      {
        UIElementSizeChangeDragger.log.Warn((Action<LoggingToken>) (x => x.SetMessage("Cancel StartDrag due to invalid position value. StartDrag UI.Pos({0},{1}) Mouse.Pos({2},{3}) CaptureMode({4})", (object) this.OriginSize.X, (object) this.OriginSize.Y, (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y, (object) this.UseCaptureMode)));
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
      Point cur = new Point(this.Parent.ActualWidth, this.Parent.ActualHeight);
      UIElementSizeChangeDragger.log.Info((Action<LoggingToken>) (x => x.SetMessage("StopDrag UI.OriginSize({0},{1}) UI.CurrSize({2},{3}) Mouse.Pos({4},{5}), CaptureMode({6})", (object) this.OriginSize.X, (object) this.OriginSize.Y, (object) cur.X, (object) cur.Y, (object) this.MouseBeginPosition.X, (object) this.MouseBeginPosition.Y, (object) this.UseCaptureMode)));
      if (this.UseCaptureMode)
        Mouse.Capture((IInputElement) null);
      return true;
    }

    public virtual void MouseMoved()
    {
      if (!this.IsDragging || this.Parent == null)
        return;
      Vector vector = Mouse.GetPosition((IInputElement) Application.Current.MainWindow) - this.MouseBeginPosition;
      Point point = this.OriginSize + vector;
      if (vector.X == 0.0 && vector.Y == 0.0)
        return;
      if (this.AllowHorizontalChange)
        this.Parent.Width = Math.Max(this.MinWidth, point.X);
      if (this.AllowVerticalChanging)
        this.Parent.Height = Math.Max(this.MinHeight, point.Y);
      this.Parent.InvalidateVisual();
    }
  }
}
