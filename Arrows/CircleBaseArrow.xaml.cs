// Decompiled with JetBrains decompiler
// Type: EMx.UI.Arrows.CircleBaseArrow
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Helpers;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMx.UI.Arrows
{
  [InstanceContract(ClassID = "6800a659-acce-4279-b04a-83b33464bf7c")]
  public partial class CircleBaseArrow : UserControl, IManagedType, IComponentConnector
  {
    internal RotateTransform ctrlRotateTr;
    internal Polygon ctrlArrow;
    private bool _contentLoaded;

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int ArrowHeight { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int ArrowHeaderLength { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int ArrowHeaderHeight { get; set; }

    public CircleBaseArrow()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.HighQuality);
      this.ArrowHeight = 10;
      this.ArrowHeaderLength = 20;
      this.ArrowHeaderHeight = 20;
    }

    public virtual void SetLocation(double x0, double y0, double x1, double y1)
    {
      double num1 = Helper.Math.Distance(x0, y0, x1, y1);
      double num2 = Helper.Math.CalcAngle(x0, y0, x1, y1);
      double num3 = num1 * Math.Sqrt(2.0);
      double radian1 = Helper.Math.AngleToRadian(num2 + 45.0);
      double radian2 = Helper.Math.AngleToRadian(num2 - 45.0);
      double num4 = Math.Max(num3 * Math.Abs(Math.Cos(radian1)), num3 * Math.Abs(Math.Cos(radian2)));
      Canvas.SetLeft((UIElement) this, x0 - num4);
      Canvas.SetTop((UIElement) this, y0 - num4);
      this.Width = num1 * 2.0;
      this.Height = num1 * 2.0;
      int num5 = 3;
      double x2 = num1 * 2.0 - (double) num5;
      double x3 = num1;
      double y = num1;
      int num6 = this.ArrowHeight / 2;
      int num7 = this.ArrowHeaderHeight / 2;
      this.ctrlArrow.Points.Clear();
      this.ctrlArrow.Points.Add(new Point(x3, y + (double) num6));
      this.ctrlArrow.Points.Add(new Point(x3, y - (double) num6));
      this.ctrlArrow.Points.Add(new Point(x2 - (double) this.ArrowHeaderLength, y - (double) num6));
      this.ctrlArrow.Points.Add(new Point(x2 - (double) this.ArrowHeaderLength, y - (double) num7));
      this.ctrlArrow.Points.Add(new Point(x2, y));
      this.ctrlArrow.Points.Add(new Point(x2 - (double) this.ArrowHeaderLength, y + (double) num7));
      this.ctrlArrow.Points.Add(new Point(x2 - (double) this.ArrowHeaderLength, y + (double) num6));
      this.ctrlRotateTr.CenterX = num1;
      this.ctrlRotateTr.CenterY = num1;
      this.ctrlRotateTr.Angle = num2;
    }

    
  }
}
