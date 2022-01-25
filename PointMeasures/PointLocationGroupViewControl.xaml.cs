// Decompiled with JetBrains decompiler
// Type: EMx.UI.PointMeasures.PointLocationGroupViewControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Equipments.Measures.Points;
using EMx.Extensions;
using EMx.Maths;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PointMeasures
{
  [InstanceContract(ClassID = "63752473-601d-47f6-a8eb-43d789d0cba9")]
  public partial class PointLocationGroupViewControl : UserControl, IManagedType, IComponentConnector
  {
    private bool _contentLoaded;

    public PointMeasurementLocationGroup PointGroup { get; set; }

    [GridViewItem(true)]
    [InstanceMember]
    public double WaferSize { get; set; }

    [GridViewItem(true)]
    [InstanceMember]
    public double DrawingWaferRatio { get; set; }

    [GridViewItem(true)]
    [InstanceMember]
    public double PixelSize { get; set; }

    public PointLocationGroupViewControl()
    {
      this.InitializeComponent();
      this.WaferSize = 150.0;
      this.DrawingWaferRatio = 0.9;
      this.PixelSize = 5.0;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) => this.InvalidateVisual();

    protected override void OnRender(DrawingContext dc)
    {
      double actualWidth = this.ActualWidth;
      double actualHeight = this.ActualHeight;
      if (actualWidth.IsInvalid() || actualHeight.IsInvalid())
        return;
      double x1 = actualWidth / 2.0;
      double y1 = actualHeight / 2.0;
      if (this.WaferSize <= 0.0 || this.PointGroup == null)
        return;
      SolidColorBrush solidColorBrush1 = new SolidColorBrush("FB0102".ToColor());
      SolidColorBrush solidColorBrush2 = new SolidColorBrush("D3D3D3".ToColor());
      Pen pen = new Pen((Brush) Brushes.IndianRed, 1.0);
      int num1 = 30;
      int num2 = 3;
      int num3 = 5;
      double num4 = Math.Min(actualWidth, actualHeight) * this.DrawingWaferRatio / 2.0;
      dc.DrawEllipse((Brush) solidColorBrush2, pen, new Point(x1, y1), num4, num4);
      dc.DrawRectangle((Brush) Brushes.IndianRed, (Pen) null, new Rect(x1 - (double) (num1 / 2), y1 + num4 + (double) num3, (double) num1, (double) num2));
      Typeface typeface = new Typeface(this.FontFamily, this.FontStyle, FontWeights.Normal, FontStretches.Normal);
      double num5 = this.PixelSize / 2.0;
      PointMeasurementLocationGroup pointGroup = this.PointGroup;
      List<PointMeasurementLocationItem> measurementLocationItemList = new List<PointMeasurementLocationItem>((IEnumerable<PointMeasurementLocationItem>) pointGroup.Items);
      for (int index = 0; index < measurementLocationItemList.Count; ++index)
      {
        PointMeasurementLocationItem measurementLocationItem = measurementLocationItemList[index];
        NumericPoint miliMeter = pointGroup.ConvertToMiliMeter(this.WaferSize, measurementLocationItem);
        double x2 = x1 + 2.0 * num4 * miliMeter.X / this.WaferSize;
        double y2 = y1 - 2.0 * num4 * miliMeter.Y / this.WaferSize;
        dc.DrawEllipse((Brush) solidColorBrush1, (Pen) null, new Point(x2, y2), num5, num5);
        FormattedText formattedText = new FormattedText(string.Format("{0}", (object) measurementLocationItem.Sequence), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, 9.0, (Brush) Brushes.Black);
        dc.DrawText(formattedText, new Point(x2 - formattedText.Width / 2.0, y2 + num5));
      }
      base.OnRender(dc);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/pointmeasures/pointlocationgroupviewcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
      else
        this._contentLoaded = true;
    }
  }
}
