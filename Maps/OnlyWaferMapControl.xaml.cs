// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.OnlyWaferMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.ProcessedData;
using EMx.Helpers;
using EMx.Logging;
using EMx.UI.ColorConverters;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Maps
{
  public partial class OnlyWaferMapControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    internal Grid ctrlMapGrid;
    internal Image ctrlMapImage;
    private bool _contentLoaded;

    public virtual ConvertMapToImage Converter { get; set; }

    public double EdgeExlusion { get; set; }

    public double WaferSize { get; set; }

    public virtual IColorConverter ColorConveter => this.Converter.ColorConveter;

    public IMapData MapData { get; set; }

    public OnlyWaferMapControl()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
      this.Converter = new ConvertMapToImage();
      if (this.Background is SolidColorBrush)
        this.Converter.BackgroundColor = (this.Background as SolidColorBrush).Color;
      this.DataContext = (object) this;
      this.SizeChanged += new SizeChangedEventHandler(this.OnControlSizeChanged);
    }

    public virtual void InvalidModels()
    {
      if (this.Dispatcher.Thread != Thread.CurrentThread)
        this.Dispatcher.Invoke(new Action(this.UpdateImages));
      else
        this.UpdateImages();
    }

    private void UpdateImages()
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      this.Converter.UpdateImage(this.MapData);
      stopwatch.Stop();
      if (stopwatch.ElapsedMilliseconds >= 200L)
        OnlyWaferMapControl.log.Info("Process time to convert WaferMap to Image : {0}ms", (object) stopwatch.ElapsedMilliseconds);
      this.ctrlMapImage.Source = this.MapData.ColCount != 0 && this.MapData.RowCount != 0 ? (ImageSource) this.Converter.GetImageSource() : (ImageSource) null;
      this.OnControlSizeChanged((object) this, (SizeChangedEventArgs) null);
    }

    private void OnControlSizeChanged(object sender, SizeChangedEventArgs e)
    {
      double actualWidth = this.ActualWidth;
      double actualHeight = this.ActualHeight;
      if (Helper.Data.AnyInvalid(actualWidth, actualHeight))
        return;
      double num1 = 0.0;
      if (this.WaferSize != 0.0)
        num1 = this.EdgeExlusion / this.WaferSize * this.ActualWidth;
      double num2 = Math.Min(actualWidth, actualHeight) - num1;
      if (num2 < 0.0)
        return;
      this.ctrlMapImage.Width = num2;
      this.ctrlMapImage.Height = num2;
    }

    void ChangeDisplayRange(MapRange obj)
    {
      this.Converter.Range.Set(obj.Begin, obj.End);
      this.Converter.Range.UseAutoRange = false;
      this.Converter.UpdateImage(this.MapData);
    }

    void SetAutoRange()
    {
      this.Converter.Range.UseAutoRange = true;
      this.Converter.UpdateImage(this.MapData);
    }
  }
}
