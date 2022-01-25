// Decompiled with JetBrains decompiler
// Type: EMx.UI.GridEx
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EMx.UI
{
  public class GridEx : Grid
  {
    public static readonly DependencyProperty ShowCustomGridLinesProperty = DependencyProperty.Register(nameof (ShowCustomGridLines), typeof (bool), typeof (GridEx), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(nameof (GridLineBrush), typeof (Brush), typeof (GridEx), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Black));
    public static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register(nameof (GridLineThickness), typeof (double), typeof (GridEx), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));

    public bool ShowCustomGridLines
    {
      get => (bool) this.GetValue(GridEx.ShowCustomGridLinesProperty);
      set => this.SetValue(GridEx.ShowCustomGridLinesProperty, (object) value);
    }

    public Brush GridLineBrush
    {
      get => (Brush) this.GetValue(GridEx.GridLineBrushProperty);
      set => this.SetValue(GridEx.GridLineBrushProperty, (object) value);
    }

    public double GridLineThickness
    {
      get => (double) this.GetValue(GridEx.GridLineThicknessProperty);
      set => this.SetValue(GridEx.GridLineThicknessProperty, (object) value);
    }

    public GridEx() => this.SnapsToDevicePixels = true;

    protected override void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      if (!this.ShowCustomGridLines)
        return;
      Pen pen = new Pen(this.GridLineBrush, this.GridLineThickness);
      foreach (RowDefinition rowDefinition in (IEnumerable<RowDefinition>) this.RowDefinitions)
        dc.DrawLine(pen, new Point(0.0, rowDefinition.Offset), new Point(this.ActualWidth, rowDefinition.Offset));
      foreach (ColumnDefinition columnDefinition in (IEnumerable<ColumnDefinition>) this.ColumnDefinitions)
        dc.DrawLine(pen, new Point(columnDefinition.Offset, 0.0), new Point(columnDefinition.Offset, this.ActualHeight));
      dc.DrawRectangle((Brush) Brushes.Transparent, pen, new Rect(0.0, 0.0, this.ActualWidth, this.ActualHeight));
    }

    static GridEx() => FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (GridEx), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (GridEx)));
  }
}
