// Decompiled with JetBrains decompiler
// Type: EMx.UI.FloatMaps.WaferFloatMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.FloatMaps;
using EMx.Data.Models;
using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.FloatMaps
{
  [InstanceContract(ClassID = "070d6ab2-d103-4e0b-9572-f3bbfff1caef")]
  public partial class WaferFloatMapControl : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    internal Image ctrlMapImage;
    private bool _contentLoaded;

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string DataFormat { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ShowDataValue { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ShowWaferImage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string OuterLineColor { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual double OuterLineThickness { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual double DrawingFontSize { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual double FlatzoneHeightPercentage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual double MapSizeRatio { get; set; }

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public virtual PersistFloatMap<double> FloatMap { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string FloatMapFieldName { get; set; }

    protected ConvertFloatMapToImage Converter { get; set; }

    public WaferFloatMapControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.FloatMap = new PersistFloatMap<double>(1);
      this.OuterLineColor = "#202020";
      this.OuterLineThickness = 2.0;
      this.FlatzoneHeightPercentage = 10.0;
      this.MapSizeRatio = 0.9;
      this.ShowDataValue = false;
      this.DataFormat = "#,##0.0";
      this.DrawingFontSize = 11.0;
      this.Converter = new ConvertFloatMapToImage();
      this.OnPulled();
      this.FloatMapFieldName = "";
    }

    [InstanceTrigger(eInstanceTrigger.Always)]
    protected void OnPulled()
    {
      this.Converter.WaferOuterLineColor = this.OuterLineColor.ToColor();
      this.Converter.WaferOuterLineThickness = this.OuterLineThickness;
      this.Converter.FontSize = this.DrawingFontSize;
      this.Converter.DisplayItemName = this.FloatMapFieldName;
      this.Converter.MapSizeRatio = this.MapSizeRatio;
      this.Converter.DataFormat = this.DataFormat;
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (Helper.Data.AllValid(this.ActualWidth, this.ActualHeight))
      {
        this.Converter.ImageSize.Width = this.ActualWidth;
        this.Converter.ImageSize.Height = this.ActualHeight;
      }
      this.UpdateImages();
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
      this.Converter.UpdateImage(this.FloatMap);
      this.ctrlMapImage.Source = (ImageSource) this.Converter.GetImageSource();
      this.ctrlMapImage.InvalidateVisual();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/floatmaps/waferfloatmapcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
          break;
        case 2:
          this.ctrlMapImage = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
