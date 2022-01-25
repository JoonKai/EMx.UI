// Decompiled with JetBrains decompiler
// Type: EMx.UI.SpectraProcessings.DBRCalculationWindow
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Maps;
using EMx.Data.Stats;
using EMx.Equipments.Data;
using EMx.Equipments.ProcessedData;
using EMx.Extensions;
using EMx.Helpers;
using EMx.IO.MxData;
using EMx.Logging;
using EMx.Maths;
using EMx.Spectroscopy.DBRs;
using EMx.UI.Charts;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using EMx.UI.PropertyGrids;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.SpectraProcessings
{
  public partial class DBRCalculationWindow : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    internal TextBox txtInfo;
    internal TextBox txtStat;
    internal ComboBox cmbDataType;
    internal WaferMapControl ctrlMapping;
    internal Label txtMappingMouseOverValue;
    internal EMxChart ctrlMappingChart;
    private bool _contentLoaded;

    public MxFile Wafer { get; set; }

    public InspectionList InspectionList { get; set; }

    public InspectionItem Inspection { get; set; }

    public PersistSpectraMapData SpectraMap { get; set; }

    public bool IgnoreInspectionData { get; set; }

    public EMx.Equipments.ProcessedData.MapData<double> OriginMapData { get; set; }

    public bool IsReqSave { get; set; }

    public string SaveInspectionName { get; set; }

    public List<PropertyInfo> SaveProperties { get; set; }

    public EMx.Equipments.ProcessedData.MapData<DBRCalcResult> DBRMap { get; set; }

    protected EMx.Equipments.ProcessedData.MapData<double> MapData { get; set; }

    protected DBRCalcRecipe Recipe { get; set; }

    protected DBRCalculator Calc { get; set; }

    protected List<PropertyInfo> DbrProperties { get; set; }

    protected int ThresholdCount { get; set; }

    public DBRCalculationWindow()
    {
      this.InitializeComponent();
      this.Calc = new DBRCalculator();
      this.DBRMap = new EMx.Equipments.ProcessedData.MapData<DBRCalcResult>();
      this.MapData = new EMx.Equipments.ProcessedData.MapData<double>();
      this.Recipe = new DBRCalcRecipe();
      this.ThresholdCount = 0;
      this.ctrlMapping.MapItemHovered += new Action<WaferMapControl, Point>(this.OnMapItemMouseOverred);
      this.ctrlMapping.ClearedSelectedPointsEvent += new Action<WaferMapControl>(this.OnMapLineProfileCleared);
      this.ctrlMapping.UpdatedSelectedPointsEvent += new Action<WaferMapControl, List<NumericPoint>>(this.OnMapLineProfileUpdated);
      this.ctrlMappingChart.ChartArea.XAxisDisplayFormat = "{0:0.00}";
      this.DbrProperties = ((IEnumerable<PropertyInfo>) Helper.Assem.GetProperties((object) new DBRCalcResult())).Where<PropertyInfo>((Func<PropertyInfo, bool>) (x => x.PropertyType.Equals(typeof (double)))).ToList<PropertyInfo>();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.OriginMapData != null)
        this.MapData = this.OriginMapData.DeepClone();
      if (this.SpectraMap == null)
      {
        int num = (int) this.ShowWarningMessage("Manager", "Spectra data is not set.");
        this.Close();
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (!this.IgnoreInspectionData)
        {
          if (this.Wafer == null)
          {
            int num = (int) this.ShowWarningMessage("Manager", "MxFile is not set.");
            this.Close();
            return;
          }
          if (this.Inspection == null || this.InspectionList == null)
          {
            int num = (int) this.ShowWarningMessage("Manager", "Inspection is not set.");
            this.Close();
            return;
          }
          stringBuilder.AppendFormat("File: {0}\r\n", (object) this.Wafer.Filename);
          stringBuilder.AppendFormat("Inspection: {0}\r\n", (object) this.Inspection.InspectionName);
          stringBuilder.AppendFormat("Inspection Type: {0}\r\n", (object) this.Inspection.InspectionType);
        }
        stringBuilder.AppendFormat("Number of wavelength: {0:#,##0}\r\n", (object) this.SpectraMap.Wavelengths.Length);
        stringBuilder.AppendFormat("Max Wavelength: {0:0.00}\r\n", (object) ((IEnumerable<PersistSpectraWavelength>) this.SpectraMap.Wavelengths).First<PersistSpectraWavelength>().Wavelength);
        stringBuilder.AppendFormat("Min Wavelength: {0:0.00}\r\n", (object) ((IEnumerable<PersistSpectraWavelength>) this.SpectraMap.Wavelengths).Last<PersistSpectraWavelength>().Wavelength);
        stringBuilder.AppendFormat("Map Width: {0:#,##0}\r\n", (object) this.SpectraMap.Header.Width);
        stringBuilder.AppendFormat("Map Height: {0:#,##0}", (object) this.SpectraMap.Header.Height);
        this.txtInfo.Text = stringBuilder.ToString();
        this.ctrlMapping.UseAutoRange = true;
        this.ctrlMapping.MapData = (IMapData) this.MapData;
        this.ctrlMapping.InvalidModels();
        this.cmbDataType.ItemsSource = (IEnumerable) this.DbrProperties;
      }
    }

    private void btnCalculate_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.SpectraMap == null)
        return;
      PropertyGridDialog propertyGridDialog = new PropertyGridDialog();
      propertyGridDialog.Height = 720.0;
      propertyGridDialog.Width = 450.0;
      propertyGridDialog.SelectedObject = (object) this.Recipe;
      propertyGridDialog.UseIndirectMode = true;
      propertyGridDialog.Owner = (Window) this;
      bool? nullable = propertyGridDialog.ShowDialog();
      bool flag1 = false;
      if (nullable.GetValueOrDefault() == flag1 & nullable.HasValue)
        return;
      this.DBRMap.ClearData();
      this.ThresholdCount = 0;
      DBRCalculationWindow.log.Info("DBR Calculation Start.");
      lock (this)
      {
        foreach (KeyValuePair<int, Dictionary<int, double>> keyValuePair1 in this.OriginMapData.GetRawData())
        {
          int key1 = keyValuePair1.Key;
          foreach (KeyValuePair<int, double> keyValuePair2 in keyValuePair1.Value)
          {
            int key2 = keyValuePair2.Key;
            double num = keyValuePair2.Value;
            if (this.SpectraMap.Has(key2, key1))
            {
              List<NumericPoint> processedSpectrum = this.SpectraMap.GetProcessedSpectrum(key2, key1);
              if (processedSpectrum != null)
              {
                DBRCalcResult data = this.Calc.Calculate((IList<NumericPoint>) processedSpectrum, this.Recipe);
                if (data == null || !data.HasStopband || data.LowThreshold)
                  ++this.ThresholdCount;
                else if (!data.HasDip && this.Recipe.SearchDIP)
                  ++this.ThresholdCount;
                else
                  this.DBRMap.Set(key1, key2, data);
              }
            }
          }
        }
      }
      DBRCalculationWindow.log.Info("DBR Calculation Finish.");
      bool flag2 = this.cmbDataType.SelectedIndex == -1;
      if (flag2)
        this.cmbDataType.SelectedIndex = 0;
      this.cmbDataType_SelectionChanged(sender, (SelectionChangedEventArgs) null);
      if (!flag2)
        return;
      this.cmbDataType.IsDropDownOpen = true;
    }

    private void btnSave_Clicked(object sender, RoutedEventArgs e)
    {
      string str = this.ShowInputMessage("Manager", "Please type new inspection name.");
      if (str.IsNullOrEmpty())
        return;
      this.SaveInspectionName = str;
      if (this.InspectionList.Has(str))
      {
        int num1 = (int) this.ShowWarningMessage("Manager", "Found duplicated inspection name. Please use different name.");
      }
      else
      {
        SelectMultiItemsDialog multiItemsDialog = new SelectMultiItemsDialog();
        multiItemsDialog.Owner = (Window) this;
        multiItemsDialog.Items = this.DbrProperties.Select<PropertyInfo, SelectMultiItemsData>((Func<PropertyInfo, SelectMultiItemsData>) (x => new SelectMultiItemsData(x.Name, (object) x, true))).ToList<SelectMultiItemsData>();
        multiItemsDialog.Title = "Select measurement items will be exported.";
        bool? nullable = multiItemsDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        this.SaveProperties = multiItemsDialog.Items.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, PropertyInfo>((Func<SelectMultiItemsData, PropertyInfo>) (x => (PropertyInfo) x.Object)).ToList<PropertyInfo>();
        if (this.SaveProperties.Count == 0)
        {
          int num2 = (int) this.ShowWarningMessage("Manager", "You need to select at least one to save them.");
        }
        else
        {
          this.IsReqSave = true;
          this.DialogResult = new bool?(true);
        }
      }
    }

    private void cmbDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      lock (this)
        this.MapData.ClearData();
      CommonStatistics commonStatistics = new CommonStatistics();
      PropertyInfo selectedItem = this.cmbDataType.SelectedItem as PropertyInfo;
      if (selectedItem != (PropertyInfo) null)
      {
        foreach (KeyValuePair<int, Dictionary<int, DBRCalcResult>> keyValuePair1 in this.DBRMap.GetRawData())
        {
          int key1 = keyValuePair1.Key;
          foreach (KeyValuePair<int, DBRCalcResult> keyValuePair2 in keyValuePair1.Value)
          {
            int key2 = keyValuePair2.Key;
            DBRCalcResult dbrCalcResult = keyValuePair2.Value;
            object data = selectedItem.GetValue((object) dbrCalcResult);
            this.MapData.Set(key1, key2, data);
            commonStatistics.AddVal((double) data);
          }
        }
      }
      commonStatistics.Calculate(true);
      this.txtStat.Text = string.Format("AVG: {0}\r\nSTD: {1}\r\nCV100: {2}\r\nMedianMean: {3}\r\nPoints: {4:#,##0}\r\nThreshold: {5:#,##0}", (object) commonStatistics.Average, (object) commonStatistics.StandDev, (object) commonStatistics.Pro, (object) commonStatistics.MedianMean, (object) commonStatistics.Count, (object) this.ThresholdCount);
      this.MapData.RowCapacity = this.OriginMapData.RowCapacity;
      this.MapData.ColCapacity = this.OriginMapData.ColCapacity;
      this.ctrlMapping.UseAutoRange = true;
      this.ctrlMapping.InvalidModels();
    }

    private void OnMapItemMouseOverred(WaferMapControl mc, Point pt)
    {
      int x = (int) Math.Floor(pt.X);
      int y = (int) Math.Floor(pt.Y);
      if (this.MapData.Has(y, x))
      {
        EMx.Equipments.ProcessedData.MapData<double> mapData = this.MapData;
        double num = mapData.Get(y, x);
        string str = "Value";
        this.txtMappingMouseOverValue.Content = (object) string.Format("Pos({0},{1}) Abs({2} mm, {3} mm)\r\n{4} : {5}", (object) x, (object) y, (object) ((double) x * mapData.CellWidth), (object) ((double) y * mapData.CellHeight), (object) str, (object) num);
      }
      if (mc.SelectedIndices.Count > 0)
        return;
      PersistSpectraMapData spectraMap = this.SpectraMap;
      if (spectraMap != null && spectraMap.Has(x, y))
      {
        List<NumericPoint> processedSpectrum = spectraMap.GetProcessedSpectrum(x, y);
        if (processedSpectrum == null)
          return;
        this.ctrlMappingChart.ChartArea.CreateOrGetScatterSeries(0).SetItemSource((object) processedSpectrum, "X", "Y");
        EMxChartScatterSeries getScatterSeries1 = this.ctrlMappingChart.ChartArea.CreateOrGetScatterSeries(1);
        EMxChartScatterSeries getScatterSeries2 = this.ctrlMappingChart.ChartArea.CreateOrGetScatterSeries(2);
        EMxChartScatterSeries getScatterSeries3 = this.ctrlMappingChart.ChartArea.CreateOrGetScatterSeries(3);
        EMxChartScatterSeries getScatterSeries4 = this.ctrlMappingChart.ChartArea.CreateOrGetScatterSeries(4);
        if (this.DBRMap.Has(y, x))
        {
          DBRCalcResult dbrCalcResult = this.DBRMap.Get(y, x);
          if (dbrCalcResult.HasStopband)
          {
            List<NumericPoint> numericPointList1 = new List<NumericPoint>()
            {
              new NumericPoint(dbrCalcResult.SBmin, dbrCalcResult.SBminY),
              new NumericPoint(dbrCalcResult.SBmax, dbrCalcResult.SBmaxY)
            };
            getScatterSeries1.SetItemSource((object) numericPointList1, "X", "Y");
            getScatterSeries1.LineThickness = 0.0f;
            getScatterSeries1.MarkerColor = Colors.Red;
            getScatterSeries1.MarkerSize = 6;
            getScatterSeries1.UseMarkerDrawing = true;
            List<NumericPoint> numericPointList2 = new List<NumericPoint>()
            {
              new NumericPoint(dbrCalcResult.SBBmin, dbrCalcResult.SBBminY),
              new NumericPoint(dbrCalcResult.SBBmax, dbrCalcResult.SBBmaxY)
            };
            getScatterSeries2.SetItemSource((object) numericPointList2, "X", "Y");
            getScatterSeries2.LineThickness = 0.0f;
            getScatterSeries2.MarkerColor = Colors.OrangeRed;
            getScatterSeries2.MarkerSize = 6;
            getScatterSeries2.UseMarkerDrawing = true;
            List<NumericPoint> numericPointList3 = new List<NumericPoint>()
            {
              new NumericPoint(dbrCalcResult.SBTmin, dbrCalcResult.SBTminY),
              new NumericPoint(dbrCalcResult.SBTmax, dbrCalcResult.SBTmaxY)
            };
            getScatterSeries3.SetItemSource((object) numericPointList3, "X", "Y");
            getScatterSeries3.LineThickness = 0.0f;
            getScatterSeries3.MarkerColor = Colors.OrangeRed;
            getScatterSeries3.MarkerSize = 6;
            getScatterSeries3.UseMarkerDrawing = true;
          }
          else
          {
            getScatterSeries1.DetachSource();
            getScatterSeries2.DetachSource();
            getScatterSeries3.DetachSource();
          }
          if (dbrCalcResult.HasDip)
          {
            List<NumericPoint> numericPointList = new List<NumericPoint>()
            {
              new NumericPoint(dbrCalcResult.DipX, dbrCalcResult.DipY),
              new NumericPoint(dbrCalcResult.DipX, dbrCalcResult.DipY)
            };
            getScatterSeries4.SetItemSource((object) numericPointList, "X", "Y");
            getScatterSeries4.LineThickness = 0.0f;
            getScatterSeries4.MarkerColor = Colors.Red;
            getScatterSeries4.MarkerSize = 5;
            getScatterSeries4.UseMarkerDrawing = true;
          }
          else
            getScatterSeries4.DetachSource();
        }
        else
        {
          getScatterSeries1.DetachSource();
          getScatterSeries2.DetachSource();
          getScatterSeries3.DetachSource();
          getScatterSeries4.DetachSource();
        }
        getScatterSeries1.DisableTracing = true;
        getScatterSeries2.DisableTracing = true;
        getScatterSeries3.DisableTracing = true;
        getScatterSeries4.DisableTracing = true;
        this.ctrlMappingChart.InvalidModels();
      }
      else if (this.ctrlMappingChart.ChartArea.Series.FirstOrDefault<IEMxChartSeries>() is EMxChartScatterSeries chartScatterSeries2)
      {
        chartScatterSeries2.DetachSource();
        this.ctrlMappingChart.InvalidModels();
      }
    }

    private void OnMapLineProfileUpdated(WaferMapControl map, List<NumericPoint> points)
    {
      this.ctrlMappingChart.XAxisAlias = "Index";
      if (!(this.ctrlMappingChart.ChartArea.Series.FirstOrDefault<IEMxChartSeries>() is EMxChartScatterSeries chartScatterSeries))
        return;
      chartScatterSeries.UseMarkerDrawing = true;
      List<NumericPoint> numericPointList = new List<NumericPoint>();
      EMx.Equipments.ProcessedData.MapData<double> mapData = this.MapData;
      if (mapData != null)
      {
        for (int index = 0; index < points.Count; ++index)
        {
          NumericPoint point = points[index];
          int x = (int) Math.Floor(point.X);
          int y1 = (int) Math.Floor(point.Y);
          double y2 = 0.0;
          if (mapData.Has(y1, x))
            y2 = mapData.Get(y1, x);
          numericPointList.Add(new NumericPoint((double) (index + 1), y2));
        }
      }
      chartScatterSeries.SetBaseItemSource((object) numericPointList, "X");
      chartScatterSeries.SetItemSource((object) numericPointList, "Y");
      this.ctrlMappingChart.InvalidModels();
    }

    private void OnMapLineProfileCleared(WaferMapControl map)
    {
      this.ctrlMappingChart.XAxisAlias = "Wavelength (nm)";
      if (!(this.ctrlMappingChart.ChartArea.Series.FirstOrDefault<IEMxChartSeries>() is EMxChartScatterSeries chartScatterSeries))
        return;
      chartScatterSeries.UseMarkerDrawing = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/spectraprocessings/dbrcalculationwindow.xaml", UriKind.Relative));
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
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.txtInfo = (TextBox) target;
          break;
        case 3:
          this.txtStat = (TextBox) target;
          break;
        case 4:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCalculate_Clicked);
          break;
        case 5:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSave_Clicked);
          break;
        case 6:
          this.cmbDataType = (ComboBox) target;
          this.cmbDataType.SelectionChanged += new SelectionChangedEventHandler(this.cmbDataType_SelectionChanged);
          break;
        case 7:
          this.ctrlMapping = (WaferMapControl) target;
          break;
        case 8:
          this.txtMappingMouseOverValue = (Label) target;
          break;
        case 9:
          this.ctrlMappingChart = (EMxChart) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
