// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.Arc
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMx.UI.Susceptors
{
  public sealed class Arc : Shape
  {
    public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof (Center), typeof (Point), typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Point(0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), typeof (double), typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), typeof (double), typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) (Math.PI / 2.0), FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof (Radius), typeof (double), typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) 10.0, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty SmallAngleProperty = DependencyProperty.Register(nameof (SmallAngle), typeof (bool), typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));

    public Point Center
    {
      get => (Point) this.GetValue(Arc.CenterProperty);
      set => this.SetValue(Arc.CenterProperty, (object) value);
    }

    public double StartAngle
    {
      get => (double) this.GetValue(Arc.StartAngleProperty);
      set => this.SetValue(Arc.StartAngleProperty, (object) value);
    }

    public double EndAngle
    {
      get => (double) this.GetValue(Arc.EndAngleProperty);
      set => this.SetValue(Arc.EndAngleProperty, (object) value);
    }

    public double Radius
    {
      get => (double) this.GetValue(Arc.RadiusProperty);
      set => this.SetValue(Arc.RadiusProperty, (object) value);
    }

    public bool SmallAngle
    {
      get => (bool) this.GetValue(Arc.SmallAngleProperty);
      set => this.SetValue(Arc.SmallAngleProperty, (object) value);
    }

    static Arc() => FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Arc), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Arc)));

    protected override Geometry DefiningGeometry
    {
      get
      {
        double num1 = this.StartAngle < 0.0 ? this.StartAngle / 180.0 * Math.PI + 2.0 * Math.PI : this.StartAngle / 180.0 * Math.PI;
        double num2 = this.EndAngle < 0.0 ? this.EndAngle / 180.0 * Math.PI + 2.0 * Math.PI : this.EndAngle / 180.0 * Math.PI;
        if (num2 < num1)
          num2 += 2.0 * Math.PI;
        SweepDirection sweepDirection = SweepDirection.Counterclockwise;
        bool isLargeArc;
        if (this.SmallAngle)
        {
          isLargeArc = false;
          sweepDirection = num2 - num1 <= Math.PI ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
        }
        else
          isLargeArc = Math.Abs(num2 - num1) < Math.PI;
        return (Geometry) new PathGeometry((IEnumerable<PathFigure>) new List<PathFigure>(1)
        {
          new PathFigure(this.Center + new Vector(Math.Cos(num1), Math.Sin(num1)) * this.Radius, (IEnumerable<PathSegment>) new List<PathSegment>(1)
          {
            (PathSegment) new ArcSegment(this.Center + new Vector(Math.Cos(num2), Math.Sin(num2)) * this.Radius, new Size(this.Radius, this.Radius), 0.0, isLargeArc, sweepDirection, true)
          }, true)
          {
            IsClosed = false
          }
        }, FillRule.EvenOdd, (Transform) null);
      }
    }
  }
}
