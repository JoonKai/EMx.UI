// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.SingleCirclePositioner
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.Data;
using EMx.Equipments.ProcessedData;
using EMx.IO.MxData;
using EMx.UI.Maps;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EMx.UI.Susceptors
{
  [Serializable]
  public class SingleCirclePositioner
  {
    public event WaferEventHandler WaferEvent;

    private void OnWaferEvent(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent(sender, type, wafername);
    }

    public int Index { get; set; }

    public double Radius { get; set; }

    public double ScaleRatio { get; set; }

    public double Angle { get; set; }

    public double Margin { get; set; }

    public bool DisplayCircle { get; set; }

    public Point CenterPoint { get; set; }

    public double DistaceOffset { get; set; }

    public double AngleOffset { get; set; }

    public bool IsSelected { get; set; }

    private WaferViewer Circle { get; set; }

    public Point Position { get; set; }

    public Point StackedPosition { get; set; }

    public SingleCirclePositionerType PositionerType { get; set; }

    public SingleCirclePositioner()
    {
      this.Radius = 0.0;
      this.ScaleRatio = 0.0;
      this.Angle = 0.0;
      this.Margin = 0.0;
      this.DisplayCircle = true;
      this.CenterPoint = new Point();
      this.DistaceOffset = 0.0;
      this.AngleOffset = 0.0;
      this.IsSelected = false;
      this.Circle = new WaferViewer();
      this.Circle.WaferEvent += new WaferEventHandler(this.Circle_WaferEvent);
      this.Position = new Point();
      this.StackedPosition = new Point();
      this.PositionerType = SingleCirclePositionerType.Wafer;
    }

    private void Circle_WaferEvent(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      this.OnWaferEvent(sender, type, wafername);
      if (type != SusceptorWaferViewerRaiseEventType.Click)
        return;
      this.Select();
    }

    internal void Draw(Canvas sender, double scaleratio)
    {
      this.ScaleRatio = scaleratio;
      if (!this.DisplayCircle)
        return;
      if (!sender.Children.Contains((UIElement) this.Circle))
        sender.Children.Add((UIElement) this.Circle);
      switch (this.PositionerType)
      {
        case SingleCirclePositionerType.Susceptor:
          this.Circle.ArcVisible = Visibility.Collapsed;
          this.Circle.IndexVisible = Visibility.Collapsed;
          break;
        case SingleCirclePositionerType.Wafer:
          this.Circle.ArcVisible = Visibility.Visible;
          this.Circle.IndexVisible = Visibility.Visible;
          break;
      }
      this.Circle.Width = this.Radius * this.ScaleRatio * 2.0;
      this.Circle.Height = this.Radius * this.ScaleRatio * 2.0;
      this.Circle.RenderTransform = (Transform) new RotateTransform()
      {
        CenterX = (this.Radius * this.ScaleRatio),
        CenterY = (this.Radius * this.ScaleRatio),
        Angle = this.Angle
      };
      this.Circle.Index = this.Index;
      Canvas.SetLeft((UIElement) this.Circle, (this.StackedPosition.X - this.Radius) * this.ScaleRatio);
      Canvas.SetTop((UIElement) this.Circle, (this.StackedPosition.Y - this.Radius) * this.ScaleRatio);
    }

    internal void ChangeDisplayRange(MapRange obj) => this.Circle.ChangeDisplayRange(obj);

    internal void SetAutoRange() => this.Circle.SetAutoRange();

    internal MapData<double> GetMapData() => this.Circle.CurrentMap != null ? this.Circle.CurrentMap : new MapData<double>();

    internal MxFile GetFile() => this.Circle.CurrentFile != null ? this.Circle.CurrentFile : (MxFile) null;

    internal void DisplayMeasurement(InspectionItem insp, string meas) => this.Circle.DisplayMeasurement(insp, meas);

    internal void CalculatePosition()
    {
      double x = this.DistaceOffset * Math.Cos(this.AngleOffset * Math.PI / 180.0);
      double y = this.DistaceOffset * Math.Sin(this.AngleOffset * Math.PI / 180.0);
      if (this.AngleOffset == 0.0)
        ;
      this.Position = new Point(x, y);
    }

    internal void Select() => this.Circle.WaferBorderColor = (Brush) Brushes.Fuchsia;

    internal void DeSelect() => this.Circle.WaferBorderColor = (Brush) Brushes.Black;
  }
}
