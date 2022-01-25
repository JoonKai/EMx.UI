// Decompiled with JetBrains decompiler
// Type: EMx.UI.CanvasEx
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Engine.Designers;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EMx.UI
{
  [InstanceContract(ClassID = "AD3E7045-AE17-4211-BAF8-48D564AEFDCA")]
  public class CanvasEx : Canvas, IUniqueID, IManagedType
  {
    private static ILog log = LogManager.GetLogger();
    public static DependencyProperty MuidProperty = DependencyProperty.Register(nameof (MUID), typeof (string), typeof (CanvasEx), new PropertyMetadata((PropertyChangedCallback) null));
    private Color _BackgroundColor;

    [GridViewItem(true, true)]
    public string MUID
    {
      get => (string) this.GetValue(CanvasEx.MuidProperty);
      set => this.SetValue(CanvasEx.MuidProperty, (object) value);
    }

    [DesignableMember(true)]
    public IList Items
    {
      get => (IList) this.Children;
      set
      {
      }
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public virtual int CanvasMarginWidth { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public virtual int CanvasMarginHeight { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout.Fixed")]
    public bool UseFixedSize { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout.Fixed")]
    public int FixedWidth { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout.Fixed")]
    public int FixedHeight { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public Color BackgroundColor
    {
      get => this._BackgroundColor;
      set
      {
        this._BackgroundColor = value;
        this.Background = value.ToBrush();
      }
    }

    public CanvasEx()
    {
      this.CanvasMarginWidth = 500;
      this.CanvasMarginHeight = 500;
      this.UseFixedSize = false;
      this.FixedWidth = 100;
      this.FixedHeight = 100;
    }

    public override string ToString() => string.Format("{0}[{1}] {2}", (object) this.GetType().FullName, (object) this.Name, (object) this.MUID);

    protected override Size MeasureOverride(Size constraints)
    {
      if (Mouse.Captured != null)
        return new Size(this.ActualWidth, this.ActualHeight);
      Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);
      Rect rect = new Rect(size);
      if (this.InternalChildren.Count == 0)
        return new Size(0.0, 0.0);
      foreach (UIElement child in this.Children)
        child?.Measure(size);
      double val1_1 = double.MaxValue;
      double val1_2 = double.MaxValue;
      double num1 = double.MinValue;
      double num2 = double.MinValue;
      foreach (UIElement child in this.Children)
      {
        if (child is FrameworkElement frameworkElement1)
        {
          double val2_1 = Helper.Data.Valuate(Canvas.GetLeft((UIElement) frameworkElement1), 0.0);
          double val2_2 = Helper.Data.Valuate(Canvas.GetTop((UIElement) frameworkElement1), 0.0);
          val1_1 = Math.Min(val1_1, val2_1);
          val1_2 = Math.Min(val1_2, val2_2);
          num1 = Math.Max(num1, val2_1 + frameworkElement1.ActualWidth);
          num2 = Math.Max(num2, val2_2 + frameworkElement1.ActualHeight);
        }
      }
      double width = Helper.Data.Valuate(num1, 0.0) + (double) this.CanvasMarginWidth;
      double height = Helper.Data.Valuate(num2, 0.0) + (double) this.CanvasMarginHeight;
      return this.UseFixedSize ? new Size((double) this.FixedWidth, (double) this.FixedHeight) : new Size(width, height);
    }
  }
}
