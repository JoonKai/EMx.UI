// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.SusceptorLayoutViewer
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.Data;
using EMx.Equipments.ProcessedData;
using EMx.IO.MxData;
using EMx.UI.Maps;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Susceptors
{
  public partial class SusceptorLayoutViewer : UserControl, IComponentConnector
  {
    internal Canvas cvsViewer;
    private bool _contentLoaded;

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

    private SusceptorBase CurrentData { get; set; }

    public SusceptorLayoutViewer()
    {
      this.InitializeComponent();
      this.CurrentData = new SusceptorBase();
    }

    public void SetData(SusceptorBase data)
    {
      this.cvsViewer.Children.Clear();
      this.CurrentData = data;
      this.CurrentData.GenerateDisplay(this.cvsViewer.RenderSize);
      this.CurrentData.WaferEvent += new WaferEventHandler(this.CurrentData_WaferDrop);
      this.DrawItems(this.cvsViewer);
    }

    public void ClearSelection() => this.CurrentData.WaferList.ForEach((Action<SingleCirclePositioner>) (x => x.DeSelect()));

    private void CurrentData_WaferDrop(
      object sender,
      SusceptorWaferViewerRaiseEventType type,
      string wafername)
    {
      this.OnWaferEvent(sender, type, wafername);
    }

    private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e) => this.DrawItems(this.cvsViewer);

    private void DrawItems(Canvas sender)
    {
      if (this.CurrentData.Tree.Root.RootNode.Value == null || double.IsNaN(this.cvsViewer.ActualWidth) || double.IsNaN(this.cvsViewer.ActualHeight))
        return;
      this.CurrentData.GenerateDisplay(new Size(this.cvsViewer.ActualWidth, this.cvsViewer.ActualHeight));
      double num = this.CurrentData.Tree.Root.RootNode.Value.Radius * 2.0;
      double scaleratio = Math.Min(sender.ActualWidth, sender.ActualHeight) / num;
      this.CurrentData.Tree.Root.RootNode.Value.Draw(sender, scaleratio);
      foreach (SingleCirclePositioner wafer in this.CurrentData.WaferList)
        wafer.Draw(sender, scaleratio);
    }

    public void ChangeRange(MapRange obj) => this.CurrentData.WaferList.ForEach((Action<SingleCirclePositioner>) (x => x.ChangeDisplayRange(obj)));

    public void SetAutoRange() => this.CurrentData.WaferList.ForEach((Action<SingleCirclePositioner>) (x => x.SetAutoRange()));

    public void DisplayInspection(InspectionItem insp, string meas) => this.CurrentData.WaferList.ForEach((Action<SingleCirclePositioner>) (x => x.DisplayMeasurement(insp, meas)));

    public List<MapData<double>> GetAllMapPoints()
    {
      List<MapData<double>> mapDataList = new List<MapData<double>>();
      foreach (SingleCirclePositioner wafer in this.CurrentData.WaferList)
        mapDataList.Add(wafer.GetMapData());
      return mapDataList;
    }

    public List<Tuple<int, MxFile>> GetAllMapFiles()
    {
      List<Tuple<int, MxFile>> tupleList = new List<Tuple<int, MxFile>>();
      foreach (SingleCirclePositioner wafer in this.CurrentData.WaferList)
      {
        if (wafer.GetFile() != null)
          tupleList.Add(new Tuple<int, MxFile>(wafer.Index, wafer.GetFile()));
      }
      return tupleList;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/susceptors/susceptorlayoutviewer.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.cvsViewer = (Canvas) target;
        this.cvsViewer.SizeChanged += new SizeChangedEventHandler(this.Canvas_SizeChanged);
      }
      else
        this._contentLoaded = true;
    }
  }
}
