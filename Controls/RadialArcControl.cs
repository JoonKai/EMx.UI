// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.RadialArcControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMx.UI.Controls
{
  public class RadialArcControl : Shape
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty BaseRadianProperty = DependencyProperty.Register(nameof (BaseRadian), typeof (double), typeof (RadialArcControl), new PropertyMetadata(new PropertyChangedCallback(RadialArcControl.OnPropertiesChanged)));
    public static readonly DependencyProperty LengthProperty = DependencyProperty.Register(nameof (Length), typeof (double), typeof (RadialArcControl), new PropertyMetadata(new PropertyChangedCallback(RadialArcControl.OnPropertiesChanged)));
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), typeof (SweepDirection), typeof (RadialArcControl), new PropertyMetadata(new PropertyChangedCallback(RadialArcControl.OnPropertiesChanged)));

    public double BaseRadian
    {
      get => (double) this.GetValue(RadialArcControl.BaseRadianProperty);
      set => this.SetValue(RadialArcControl.BaseRadianProperty, (object) value);
    }

    public double Length
    {
      get => (double) this.GetValue(RadialArcControl.LengthProperty);
      set => this.SetValue(RadialArcControl.LengthProperty, (object) value);
    }

    public SweepDirection Direction
    {
      get => (SweepDirection) this.GetValue(RadialArcControl.DirectionProperty);
      set => this.SetValue(RadialArcControl.DirectionProperty, (object) value);
    }

    public RadialArcControl()
    {
      this.Direction = SweepDirection.Clockwise;
      this.BaseRadian = -1.0 * Math.PI / 2.0;
      this.Length = 0.0;
    }

    protected override Geometry DefiningGeometry => this.GetGeometry();

    protected override void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      dc.DrawGeometry(this.Fill, new Pen(this.Stroke, this.StrokeThickness), this.GetGeometry());
    }

    protected static void OnPropertiesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is RadialArcControl radialArcControl))
        return;
      radialArcControl.InvalidateVisual();
    }

    private Geometry GetGeometry()
    {
      Size renderSize = this.RenderSize;
      double width = renderSize.Width;
      renderSize = this.RenderSize;
      double height = renderSize.Height;
      double num = Math.Abs(Math.Min(width, height) - this.StrokeThickness) / 2.0;
      double dx = this.RenderSize.Width / 2.0;
      double dy = this.RenderSize.Height / 2.0;
      NumericPoint numericPoint1 = Helper.Math.RadianToPoint(num, this.BaseRadian).Add(dx, dy);
      NumericPoint numericPoint2 = Helper.Math.RadianToPoint(num, this.BaseRadian + (this.Direction == SweepDirection.Clockwise ? this.Length : -this.Length)).Add(dx, dy);
      StreamGeometry streamGeometry = new StreamGeometry();
      using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
      {
        streamGeometryContext.BeginFigure(numericPoint1.ToPoint(), false, false);
        streamGeometryContext.ArcTo(numericPoint2.ToPoint(), new Size(num, num), 0.0, this.Length > Math.PI, this.Direction, true, false);
      }
      return (Geometry) streamGeometry;
    }
  }
}
