// Decompiled with JetBrains decompiler
// Type: EMx.UI.PartitionMaps.PartitionMapUIControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Extensions;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using EMx.Texts;
using EMx.UI.ColorConverters;
using EMx.UI.Extensions;
using EMx.UI.Maps;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PartitionMaps
{
  [InstanceContract(ClassID = "710dc707-6ff1-4908-88dc-ec7cb1927860")]
  public partial class PartitionMapUIControl : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty RangeBarWidthProperty = DependencyProperty.Register(nameof (RangeBarWidth), typeof (double), typeof (PartitionMapUIControl), new PropertyMetadata((object) 90.0));
    internal ColumnDefinition colFirst;
    internal PartitionMapControl ctrlPMap;
    internal MapRangeControl ctrlRangeBar;
    internal Label txtImageOffset;
    internal Label txtCenterPos;
    private bool _contentLoaded;

    public double RangeBarWidth
    {
      get => (double) this.GetValue(PartitionMapUIControl.RangeBarWidthProperty);
      set => this.SetValue(PartitionMapUIControl.RangeBarWidthProperty, (object) value);
    }

    [DesignableMember(true)]
    public PartitionMap PMap
    {
      get => this.ctrlPMap.PMap;
      set
      {
        this.ctrlPMap.PMap = value;
        this.ctrlPMap.ResetView();
        this.OnRangeBarChanged(this.ctrlRangeBar.Range);
      }
    }

    public PartitionMapUIControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.ctrlPMap.ImageOffsetChanged += new Action<PartitionMapControl, NumericPoint>(this.OnImageOffsetChanged);
      this.ctrlRangeBar.Range = this.ctrlPMap.Range;
      this.ctrlRangeBar.ColorConveter = (IColorConverter) this.ctrlPMap.ColorConverter;
      this.ctrlRangeBar.DisplayFormat = "#,##0";
      this.ctrlPMap.Range.RangeChanged += new Action<MapRange>(this.OnRangeBarChanged);
    }

    private void OnRangeBarChanged(MapRange range)
    {
      PartitionMap pmap = this.ctrlPMap.PMap;
      if (pmap == null)
        return;
      List<Color> list = this.ctrlRangeBar.ColorConveter.GetColors().ToList<Color>();
      if (list.Count < 1)
        return;
      List<long> histogram1 = pmap.Histogram;
      if (histogram1 == null || histogram1.Count == 0)
        return;
      List<long> histogram2 = new List<long>();
      double num1 = 0.0;
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        long num2 = 0;
        double num3 = (range.Begin + range.Delta * (double) (index1 + 1) / (double) list.Count) / 256.0;
        for (int index2 = 0; index2 < histogram1.Count; ++index2)
        {
          if (num1 <= (double) index2 && (double) index2 < num3)
            num2 += histogram1[index2];
        }
        num1 = num3;
        histogram2.Add(num2);
      }
      range.SetHistogram(histogram2);
      this.ctrlRangeBar.InvalidateVisual();
    }

    private void OnImageOffsetChanged(PartitionMapControl pmap, NumericPoint pt) => this.txtImageOffset.Content = (object) string.Format("Offset ({0:#,##0}, {1:#,##0})", (object) pt.X, (object) pt.Y);

    private void txtImageOffset_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      string phrase = Application.Current.MainWindow.ShowInputMessage("Go to offset", "Please type offset value.", string.Format("{0},{1}", (object) this.ctrlPMap.ImageOffset.X, (object) this.ctrlPMap.ImageOffset.Y));
      if (string.IsNullOrWhiteSpace(phrase))
        return;
      StringFormatParser stringFormatParser = StringFormatParser.DoParse("{0},{1}", phrase);
      if (stringFormatParser.SuccessBlockCount < 2)
      {
        int num = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", "Invalid format.\r\nPlease follow format {x},{y}");
      }
      else
      {
        this.ctrlPMap.ImageOffset.X = (double) stringFormatParser.Result[0].ToInt(0);
        this.ctrlPMap.ImageOffset.Y = (double) stringFormatParser.Result[1].ToInt(0);
        this.ctrlPMap.InvalidateVisual();
        this.OnImageOffsetChanged(this.ctrlPMap, this.ctrlPMap.ImageOffset);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/partitionmaps/partitionmapuicontrol.xaml", UriKind.Relative));
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
          this.colFirst = (ColumnDefinition) target;
          break;
        case 2:
          this.ctrlPMap = (PartitionMapControl) target;
          break;
        case 3:
          this.ctrlRangeBar = (MapRangeControl) target;
          break;
        case 4:
          this.txtImageOffset = (Label) target;
          this.txtImageOffset.MouseDoubleClick += new MouseButtonEventHandler(this.txtImageOffset_MouseDoubleClick);
          break;
        case 5:
          this.txtCenterPos = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
