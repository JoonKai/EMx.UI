// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.WaferViewer
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Maps;
using EMx.Equipments.Configurations;
using EMx.Equipments.Data;
using EMx.Equipments.ProcessedData;
using EMx.Extensions;
using EMx.IO.MxData;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Maps;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMx.UI.Susceptors
{
  public partial class WaferViewer : UserControl, INotifyPropertyChanged, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private Brush waferBorderColor;
    private Visibility _ArcVisible;
    private int _Index;
    private Visibility _IndexVisible;
    internal MenuItem mnuDelete;
    internal MenuItem mnuLoad;
    internal Grid grd;
    internal OnlyWaferMapControl ctrlMap;
    internal Ellipse ellp;
    internal Arc arcThis;
    internal Label lbl;
    private bool _contentLoaded;

    public MxFile CurrentFile { get; set; }

    public MapData<double> CurrentMap { get; set; }

    public event WaferEventHandler WaferEvent;

    private void OnWaferDropped(string wafername)
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent((object) this, SusceptorWaferViewerRaiseEventType.WaferDrop, wafername);
    }

    private void OnWaferDeleted(string wafername)
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent((object) this, SusceptorWaferViewerRaiseEventType.Delete, wafername);
    }

    private void OnWaferRequest()
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent((object) this, SusceptorWaferViewerRaiseEventType.WarerRequest, "");
    }

    private void OnWaferClick()
    {
      WaferEventHandler waferEvent = this.WaferEvent;
      if (waferEvent == null)
        return;
      waferEvent((object) this, SusceptorWaferViewerRaiseEventType.Click, "");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public Brush WaferBorderColor
    {
      get => this.waferBorderColor;
      set
      {
        this.waferBorderColor = value;
        this.OnPropertyChanged(nameof (WaferBorderColor));
      }
    }

    public Visibility ArcVisible
    {
      get => this._ArcVisible;
      set
      {
        this._ArcVisible = value;
        this.OnPropertyChanged(nameof (ArcVisible));
      }
    }

    public int Index
    {
      get => this._Index;
      set
      {
        this._Index = value;
        this.OnPropertyChanged(nameof (Index));
      }
    }

    public Visibility IndexVisible
    {
      get => this._IndexVisible;
      set
      {
        this._IndexVisible = value;
        this.OnPropertyChanged(nameof (IndexVisible));
      }
    }

    public WaferViewer()
    {
      this.InitializeComponent();
      this.ArcVisible = Visibility.Visible;
      this.IndexVisible = Visibility.Collapsed;
      this.Index = 0;
      this.WaferBorderColor = (Brush) Brushes.Black;
      this.DataContext = (object) this;
    }

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (double.IsNaN(this.Width) || double.IsNaN(this.Height))
        return;
      this.arcThis.Center = new Point(this.Width / 2.0, this.Height / 2.0);
      this.arcThis.Radius = this.Height / 2.0;
      this.lbl.FontSize = 8.0 + this.Height / 30.0;
    }

    public void DisplayMeasurement(InspectionItem insp, string meas)
    {
      MapData<double> map_data = new MapData<double>();
      if (this.CurrentFile != null)
      {
        InspectionList inspectionList = InspectionList.GetInspectionList(this.CurrentFile);
        if (inspectionList != null && inspectionList.Has(insp.InspectionName))
        {
          InspectionItem inspectionItem = inspectionList.Get(insp.InspectionName);
          if (inspectionItem.Has(meas))
          {
            this.LoadMappingData(this.CurrentFile, inspectionItem.Get(meas), map_data);
            ConfigData configData = this.LoadRecipe(this.CurrentFile);
            ConfigDataItem configDataItem = configData.Items.FirstOrDefault<ConfigDataItem>((Func<ConfigDataItem, bool>) (x => x.ItemPath.Contains(insp.InspectionName) && x.ItemPath.Contains("EdgeExclusion")));
            double num = (double) configData.GetItemByName("", "WaferSize").Value;
            if (configDataItem != null)
            {
              this.ctrlMap.EdgeExlusion = (double) configDataItem.Value;
              this.ctrlMap.WaferSize = num;
            }
          }
        }
      }
      this.CurrentMap = map_data;
      this.SetMap((IMapData) map_data);
    }

    private ConfigData LoadRecipe(MxFile mxf)
    {
      ConfigData configData = new ConfigData();
      if (mxf != null)
      {
        string name = "#recipe_file";
        if (mxf.HasData(name))
        {
          Tuple<object, List<string>> tuple = InstanceSerializer.ReadFromMemory(mxf.GetData(name));
          if (tuple.Item2.Count > 0)
          {
            WaferViewer.log.Warn("Error on load recipe: {0}", (object) mxf.FilePath);
            foreach (string fmt in tuple.Item2)
              WaferViewer.log.Warn(fmt);
          }
          if (tuple.Item1 != null && tuple.Item1 is ConfigData)
            configData = tuple.Item1 as ConfigData;
        }
      }
      return configData;
    }

    internal void SetAutoRange() => this.ctrlMap.SetAutoRange();

    internal void ChangeDisplayRange(MapRange obj) => this.ctrlMap.ChangeDisplayRange(obj);

    public bool LoadMappingData(MxFile file, MeasurementItem meas, MapData<double> map_data)
    {
      if (meas != null && file != null && map_data != null)
      {
        string str = meas.ItemPaths.SafeGet<string, string>(nameof (map_data));
        if (str != null && file.HasData(str))
        {
          if (WaferViewer.LoadPMapDataToMap(file, str, (IMapData) map_data))
            return true;
          WaferViewer.log.Warn("Fail to load map data : {0}", (object) str);
        }
      }
      map_data.ClearData();
      return false;
    }

    public static bool LoadPMapDataToMap(MxFile mxf, string path, IMapData target_map)
    {
      if (mxf == null || !mxf.HasData(path) || target_map == null)
        return false;
      bool flag = false;
      PersistMapData<float> float32 = PersistMapFactory.CreateFloat32(0, 0, 0, 0);
      using (Stream dataStream = mxf.GetDataStream(path))
        flag = float32.Load(dataStream);
      if (flag)
      {
        target_map.ClearData();
        int num1 = Math.Max(Math.Abs(float32.Header.X1), Math.Abs(float32.Header.X0));
        int num2 = Math.Max(Math.Abs(float32.Header.Y1), Math.Abs(float32.Header.Y0));
        for (int y = -num2; y <= num2; ++y)
        {
          for (int x = -num1; x <= num1; ++x)
          {
            if (float32.Has(x, y))
              target_map.Set(y, x, (object) (double) float32.Get(x, y));
          }
        }
        target_map.CellWidth = (double) float32.Header.CellWidth;
        target_map.CellHeight = (double) float32.Header.CellHeight;
        target_map.RowCapacity = num2 * 2 + 1;
        target_map.ColCapacity = num1 * 2 + 1;
      }
      return flag;
    }

    private void UserControl_Drop(object sender, DragEventArgs e) => this.OnWaferDropped(e.Data.GetData(typeof (string)).ToString());

    public void SetMap(IMapData map)
    {
      this.ctrlMap.MapData = map;
      this.ctrlMap.InvalidModels();
    }

    private void mnuDelete_Click(object sender, RoutedEventArgs e)
    {
      if (this.CurrentFile == null)
        return;
      this.CurrentFile = (MxFile) null;
      this.CurrentMap.ClearData();
      this.SetMap((IMapData) this.CurrentMap);
      this.OnWaferDeleted("");
    }

    private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.CurrentFile == null)
        return;
      this.OnWaferClick();
    }

    private void mnuLoad_Click(object sender, RoutedEventArgs e) => this.OnWaferRequest();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/susceptors/waferviewer.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).SizeChanged += new SizeChangedEventHandler(this.UserControl_SizeChanged);
          ((UIElement) target).Drop += new DragEventHandler(this.UserControl_Drop);
          ((UIElement) target).MouseLeftButtonUp += new MouseButtonEventHandler(this.UserControl_MouseLeftButtonUp);
          break;
        case 2:
          this.mnuDelete = (MenuItem) target;
          this.mnuDelete.Click += new RoutedEventHandler(this.mnuDelete_Click);
          break;
        case 3:
          this.mnuLoad = (MenuItem) target;
          this.mnuLoad.Click += new RoutedEventHandler(this.mnuLoad_Click);
          break;
        case 4:
          this.grd = (Grid) target;
          break;
        case 5:
          this.ctrlMap = (OnlyWaferMapControl) target;
          break;
        case 6:
          this.ellp = (Ellipse) target;
          break;
        case 7:
          this.arcThis = (Arc) target;
          break;
        case 8:
          this.lbl = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
