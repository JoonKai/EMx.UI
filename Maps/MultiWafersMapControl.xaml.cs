// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.MultiWafersMapControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Equipments.ProcessedData;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.UI.Extensions;
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

namespace EMx.UI.Maps
{
  public partial class MultiWafersMapControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty RangeBarWidthProperty = DependencyProperty.Register(nameof (RangeBarWidth), typeof (double), typeof (MultiWafersMapControl), new PropertyMetadata((object) 80.0));
    public int SelectedIndex;
    protected ConvertMapToImage Converter;
    protected List<MultiWaferItem> Items;
    protected List<double> RawValues;
    protected double BoxWidth = 200.0;
    internal ColumnDefinition colFirst;
    internal Grid ctrlMapGrid;
    internal ListBox ctrlItems;
    internal MapRangeControl ctrlRangeBar;
    private bool _contentLoaded;

    public double RangeBarWidth
    {
      get => (double) this.GetValue(MultiWafersMapControl.RangeBarWidthProperty);
      set => this.SetValue(MultiWafersMapControl.RangeBarWidthProperty, (object) value);
    }

    public event SelectionChangedEventHandler SelectionChanged;

    public MultiWafersMapControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.SelectedIndex = -1;
      this.Items = new List<MultiWaferItem>();
      this.RawValues = new List<double>();
      this.Converter = new ConvertMapToImage();
      if (this.Background is SolidColorBrush)
        this.Converter.BackgroundColor = (this.Background as SolidColorBrush).Color;
      this.ctrlRangeBar.ColorConveter = this.Converter.ColorConveter;
      this.ctrlRangeBar.Range = this.Converter.Range;
      this.ctrlRangeBar.Range.RangeChanged += new Action<MapRange>(this.OnGlobalRangeChanged);
    }

    public virtual void UpdateData(List<MultiWaferItem> items)
    {
      this.Items = new List<MultiWaferItem>((IEnumerable<MultiWaferItem>) items);
      this.Converter.Range.UseAutoRange = true;
      this.Converter.Range.UseDistribution = true;
      this.RawValues = this.Converter.UpdateRanges(this.Items.Select<MultiWaferItem, MapData<double>>((Func<MultiWaferItem, MapData<double>>) (x => x.Map)).ToList<MapData<double>>());
      this.UpdateImages();
    }

    protected virtual void UpdateImages()
    {
      this.ctrlItems.Items.Clear();
      this.Converter.Range.UseAutoRange = false;
      this.Converter.Range.UseDistribution = false;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        MultiWaferItem multiWaferItem = this.Items[index];
        this.Converter.EmptyImage();
        this.Converter.UpdateImage((IMapData) multiWaferItem.Map);
        MultiWaferItemUI multiWaferItemUi = new MultiWaferItemUI();
        multiWaferItemUi.Width = this.BoxWidth;
        multiWaferItemUi.Height = this.BoxWidth;
        multiWaferItemUi.UpdateImage(multiWaferItem, this.Converter.GetImageSource());
        this.ctrlItems.Items.Add((object) multiWaferItemUi);
      }
      this.Converter.Range.UseDistribution = true;
      this.Converter.UpdateRange(this.RawValues);
      this.ctrlRangeBar.InvalidateVisual();
    }

    private void OnGlobalRangeChanged(MapRange range) => this.UpdateImages();

    private void mnuChangeItemSize_Clicked(object sender, RoutedEventArgs e)
    {
      double val2 = Application.Current.MainWindow.ShowInputNumberDialog("Item Width", "Please type the new item width.", this.BoxWidth);
      if (val2.IsInvalid())
        return;
      this.BoxWidth = Math.Max(10.0, val2);
      this.UpdateImages();
    }

    private void ctrlItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.SelectedIndex = this.ctrlItems.SelectedIndex;
      SelectionChangedEventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged(sender, e);
    }

    private void ctrlItems_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (!Helper.Windows.IsKeyDown(ModifierKeys.Control))
        return;
      if (e.Delta > 0)
        this.BoxWidth += 10.0;
      else
        this.BoxWidth -= 10.0;
      e.Handled = true;
      this.UpdateImages();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/maps/multiwafersmapcontrol.xaml", UriKind.Relative));
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
          this.ctrlMapGrid = (Grid) target;
          break;
        case 3:
          this.ctrlItems = (ListBox) target;
          this.ctrlItems.SelectionChanged += new SelectionChangedEventHandler(this.ctrlItems_SelectionChanged);
          this.ctrlItems.PreviewMouseWheel += new MouseWheelEventHandler(this.ctrlItems_MouseWheel);
          break;
        case 4:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuChangeItemSize_Clicked);
          break;
        case 5:
          this.ctrlRangeBar = (MapRangeControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
