// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.MapRangeControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Maths;
using EMx.Serialization;
using EMx.UI.ColorConverters;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Maps
{
  [InstanceContract(ClassID = "23933e32-765e-43f3-8029-9350b8d8d6c6")]
  public partial class MapRangeControl : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    protected double PreviousMovingDelta;
    private List<Tuple<Rect, long>> LastRangeBarCellPositions = new List<Tuple<Rect, long>>();
    private object LastTooltipedCell = (object) null;
    internal MenuItem chkAutoRange;
    internal MenuItem mnuSetRange;
    internal MenuItem chkEnableDistribution;
    internal MenuItem chkDisableDistribution;
    internal MenuItem chkUseOverlap;
    internal MenuItem chkUseLogScaleBar;
    internal MenuItem chkUseIgnoreOutOfRange;
    private bool _contentLoaded;

    [DesignableMember(true)]
    public IColorConverter ColorConveter { get; set; }

    [DesignableMember(true)]
    public virtual MapRange Range { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string DisplayFormat { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int ColorBarWidth { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool UseOverlapping { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool UseLogScaleBar { get; set; }

    protected UIElementDragEventHelper DragHandler { get; set; }

    protected NumericRange OriginalRange { get; set; }

    public MapRangeControl()
    {
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this, EdgeMode.Aliased);
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.NearestNeighbor);
      this.DataContext = (object) this;
      this.UseLogScaleBar = false;
      this.UseOverlapping = true;
      this.ColorBarWidth = 45;
      this.DisplayFormat = "Auto";
      this.Range = new MapRange();
      this.ColorConveter = (IColorConverter) new PredefinedColorsConverter();
      this.OriginalRange = new NumericRange();
      this.PreviousMovingDelta = double.MinValue;
      this.DragHandler = new UIElementDragEventHelper();
      this.DragHandler.DragDirection = UIElementDragEventHelper.eDragDirection.Vertical;
      this.DragHandler.Register((UIElement) this);
      this.DragHandler.DragStart += new Action<UIElementDragEventHelper, UIElementDragStartArgs>(this.OnDragStart);
      this.DragHandler.DragMoving += new Action<UIElementDragEventHelper, Point>(this.OnDragMoving);
      this.PreviewMouseDoubleClick += new MouseButtonEventHandler(this.OnMouseDoubleClicked);
    }

    private void OnDragMoving(UIElementDragEventHelper dh, Point delta)
    {
      if (Helper.Data.AnyInvalid(this.ActualHeight) || this.ActualHeight == 0.0)
        return;
      double num = -delta.Y / this.ActualHeight;
      NumericRange originalRange = this.OriginalRange;
      if (Math.Abs(delta.Y - this.PreviousMovingDelta) < 3.0)
        return;
      this.PreviousMovingDelta = delta.Y;
      this.Range.UseAutoRange = false;
      this.Range.Begin = originalRange.Begin + originalRange.Delta * num;
      this.Range.End = originalRange.End + originalRange.Delta * num;
      this.Range.InvokeRangeChanged();
      this.InvalidateVisual();
    }

    private void OnDragStart(UIElementDragEventHelper dh, UIElementDragStartArgs args)
    {
      this.OriginalRange.Begin = this.Range.Begin;
      this.OriginalRange.End = this.Range.End;
      this.PreviousMovingDelta = double.MinValue;
    }

    protected override void OnRender(DrawingContext dc)
    {
      base.OnRender(dc);
      if (Helper.Data.AnyInvalid(this.ActualWidth, this.ActualHeight) || this.ColorBarWidth == 0 || this.Range == null || this.ColorConveter == null)
        return;
      List<Color> list = this.ColorConveter.GetColors().ToList<Color>();
      if (list.Count == 0)
        return;
      this.DrawColorBar(dc, list);
      this.DrawNumberBar(dc, list);
    }

    protected void DrawColorBar(DrawingContext dc, List<Color> colors)
    {
      double height = this.ActualHeight / (double) colors.Count;
      if (height <= 1.0)
        return;
      double width1 = this.UseOverlapping ? this.ActualWidth : Math.Min((double) this.ColorBarWidth, this.ActualWidth / 2.0);
      Brush background = this.Background;
      if (background == null)
      {
        Control parent = this.FindParent<Control>();
        if (parent != null)
          background = parent.Background;
      }
      List<long> source = (List<long>) null;
      lock (this.Range)
        source = this.Range.Distributions.ToList<long>();
      if (source.Count == 0)
        return;
      List<Tuple<Rect, long>> tupleList = new List<Tuple<Rect, long>>();
      long val2 = source.Max();
      double num = Math.Log10((double) Math.Max(1L, val2));
      for (int index = 0; index < colors.Count; ++index)
      {
        Color color = colors[index];
        double y = height * (double) index;
        Rect rectangle = new Rect(0.0, y, this.ActualWidth, height);
        dc.DrawRectangle(background, (Pen) null, rectangle);
        if (this.Range.UseDistribution && colors.Count == source.Count)
        {
          double width2 = width1 * (double) source[index] / (double) val2;
          if (this.UseLogScaleBar)
            width2 = width1 * Math.Log10((double) Math.Max(1L, source[index])) / num;
          tupleList.Add(Tuple.Create<Rect, long>(rectangle, source[index]));
          dc.DrawRectangle((Brush) Brushes.White, (Pen) null, new Rect(0.0, y, width1, height - 1.0));
          dc.DrawRectangle((Brush) new SolidColorBrush(color), (Pen) null, new Rect(0.0, y, width2, height - 1.0));
        }
        else
          dc.DrawRectangle((Brush) new SolidColorBrush(color), (Pen) null, new Rect(0.0, y, width1, height - 1.0));
      }
      lock (this.LastRangeBarCellPositions)
        this.LastRangeBarCellPositions = tupleList;
    }

    protected void DrawNumberBar(DrawingContext dc, List<Color> colors)
    {
      if (this.Range.Delta <= 0.0)
        return;
      if (Helper.Data.AnyInvalid(this.ActualHeight, this.ActualWidth))
        return;
      CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-us");
      Typeface typeface = new Typeface("Verdana");
      int num1 = 10;
      double num2 = this.Range.Delta / (double) colors.Count;
      double num3 = this.ActualHeight / (double) colors.Count;
      double num4 = this.UseOverlapping ? 0.0 : Math.Min((double) this.ColorBarWidth, this.ActualWidth / 2.0);
      try
      {
        double num5 = 0.0;
        for (int index = 0; index < colors.Count; ++index)
        {
          double num6 = num3 * (double) index;
          FormattedText formattedText = new FormattedText((this.Range.Begin + num2 * ((double) index + 0.5)).ToFormattedString(this.DisplayFormat), cultureInfo, FlowDirection.LeftToRight, typeface, (double) num1, (Brush) Brushes.Black);
          double height = formattedText.Height;
          double x = num4 + (this.ActualWidth - num4 - formattedText.Width) / 2.0;
          double num7 = (num3 - height) / 2.0;
          double num8 = num6 + num7;
          if (num8 >= num5 && num8 + height <= this.ActualHeight)
          {
            num5 = num8 + height;
            dc.DrawText(formattedText, new Point(x, num6 + num7));
          }
        }
      }
      catch (Exception ex)
      {
        MapRangeControl.log.Error(ex, ex.Message);
      }
    }

    private void UserControl_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      double num = this.Range.Delta * (double) Math.Sign(e.Delta) / 20.0;
      this.Range.UseAutoRange = false;
      this.Range.Begin += num;
      this.Range.End -= num;
      this.Range.InvokeRangeChanged();
      e.Handled = true;
      this.InvalidateVisual();
    }

    private void chkAutoRange_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.Range.UseAutoRange = this.chkAutoRange.IsChecked;
      this.Range.InvokeRangeChanged();
    }

    private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      if (this.Range == null)
        return;
      this.chkAutoRange.IsChecked = this.Range.UseAutoRange;
      this.chkUseOverlap.IsChecked = this.UseOverlapping;
      this.chkUseLogScaleBar.IsChecked = this.UseLogScaleBar;
      this.chkUseIgnoreOutOfRange.IsChecked = this.Range.IgnoreOutOfRange;
    }

    private void chkEnableDistribution_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.Range.UseDistribution = true;
      this.Range.InvokeRangeChanged();
      this.InvalidateVisual();
    }

    private void chkDisableDistribution_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.Range.UseDistribution = false;
      this.InvalidateVisual();
    }

    private void chkUseOverlap_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.UseOverlapping = this.chkUseOverlap.IsChecked;
      this.InvalidateVisual();
    }

    private void chkUseLogScaleBar_Clicked(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.UseLogScaleBar = this.chkUseLogScaleBar.IsChecked;
      this.InvalidateVisual();
    }

    private void mnuSetRange_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      InputRangeDialog inputRangeDialog = new InputRangeDialog();
      inputRangeDialog.Owner = Application.Current.MainWindow;
      inputRangeDialog.Begin = this.Range.Begin;
      inputRangeDialog.End = this.Range.End;
      bool? nullable = inputRangeDialog.ShowDialog();
      bool flag = true;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
      {
        this.Range.UseAutoRange = false;
        this.Range.Begin = inputRangeDialog.Begin;
        this.Range.End = inputRangeDialog.End;
        this.Range.InvokeRangeChanged();
        this.InvalidateVisual();
      }
    }

    private void OnMouseDoubleClicked(object sender, MouseButtonEventArgs e) => this.mnuSetRange_Click(sender, (RoutedEventArgs) null);

    private void SelectComplementaryColor1_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      Colors.Blue.ToRRGGBBText(),
      Colors.Orange.ToRRGGBBText()
    });

    private void SelectComplementaryColor2_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "660099",
      Colors.Yellow.ToRRGGBBText()
    });

    private void SelectComplementaryColor3_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      Colors.Green.ToRRGGBBText(),
      Colors.Red.ToRRGGBBText()
    });

    private void SelectGrayColor_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "FFFFFF",
      "EEEEEE",
      "DDDDDD",
      "CCCCCC",
      "BBBBBB",
      "AAAAAA",
      "999999",
      "888888",
      "777777",
      "666666",
      "555555",
      "444444",
      "333333",
      "222222",
      "111111",
      "000000"
    });

    private void SelectBlueColor_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "DEFFFF",
      "CDEFFF",
      "BCDEFF",
      "ABCDEF",
      "9ABCDE",
      "89ABCD",
      "789ABC",
      "6789AB",
      "56789A",
      "456789",
      "345678",
      "234567",
      "123456"
    });

    private void SelectPalette1Color_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "FF00FF",
      "EF00FF",
      "CE00FF",
      "AD00FF",
      "8C04FF",
      "6B00FF",
      "5200FF",
      "2900E7",
      "0000FF",
      "0028E7",
      "0041BD",
      "005194",
      "006573",
      "007D52",
      "008E29",
      "00A210",
      "00B600",
      "00D300",
      "00EB00",
      "00FF08",
      "39FF00",
      "6BFF00",
      "A5FF00",
      "CEFF00",
      "FFFF00",
      "FFDB00",
      "FFC300",
      "FF9600",
      "FF8200",
      "FF5500",
      "FF4100",
      "FF0000"
    });

    private void SelectPalette2Color_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "000080",
      "010F84",
      "031F88",
      "05308C",
      "074190",
      "095294",
      "0B6498",
      "0E779C",
      "108AA1",
      "139DA5",
      "15A9A1",
      "1BB189",
      "21B971",
      "27C158",
      "2BC54B",
      "32CD32",
      "41D030",
      "50D32E",
      "61D52B",
      "73D829",
      "85DB27",
      "99DD24",
      "AEE022",
      "C4E21F",
      "DBE51D",
      "E8DD1A",
      "EAC918",
      "EDB415",
      "F09E12",
      "F2870F",
      "F56E0C",
      "F85409",
      "FA3906",
      "FD1D03",
      "FF0000",
      "F60606",
      "E11313",
      "CD1D1D",
      "B92424",
      "A52A2A"
    });

    private void SelectPalette3Color_Click(object sender, RoutedEventArgs e) => this.UpdateColors(new List<string>()
    {
      "C01ACE",
      "9B19E8",
      "6019E6",
      "221AD2",
      "1A45D3",
      "1A7AD3",
      "1AB0D4",
      "1AD5C2",
      "1AD58B",
      "1AD754",
      "1AD81C",
      "4BDA1A",
      "84DC1A",
      "BEDD19",
      "E1C61A",
      "E58E1A",
      "ED561A",
      "F71A1A"
    });

    private void SelectCustomPalette_Click(object sender, RoutedEventArgs e)
    {
      string text = InputLongMessageDialog.Show(Application.Current.MainWindow, "Custom Color Set", "Please type the list of color name.", this.ColorConveter.GetColors().Select<Color, string>((Func<Color, string>) (x => x.ToRRGGBBText())).Aggregate<string, string>("", (Func<string, string, string>) ((acc, x) => acc = acc + x + Environment.NewLine)));
      if (string.IsNullOrWhiteSpace(text))
        return;
      try
      {
        this.UpdateColors(Helper.Text.SplitIntoLines(text));
      }
      catch (Exception ex)
      {
        MapRangeControl.log.Error(ex, ex.Message);
        int num = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", ex.Message);
      }
    }

    private void UpdateColors(List<string> text_colors)
    {
      try
      {
        this.ColorConveter.SetColors((IEnumerable<Color>) text_colors.Select<string, Color>((Func<string, Color>) (x => x.ToColor())).ToList<Color>());
        this.Range.InvokeRangeChanged();
        this.InvalidateVisual();
      }
      catch (Exception ex)
      {
        MapRangeControl.log.Error(ex, ex.Message);
      }
    }

    private void OnDataFormat_Default_Clicked(object sender, RoutedEventArgs e)
    {
      this.DisplayFormat = "Auto";
      this.InvalidateVisual();
    }

    private void OnDataFormat_Clicked(object sender, RoutedEventArgs e)
    {
      string str = Application.Current.MainWindow.ShowInputMessage("Display Format", "Please type display format string.\r\nexamples.\r\n000.000\r\n#,###\r\n#,###.00\r\ng3\r\ng6\r\nAuto\r\n....", this.DisplayFormat);
      if (str == null)
        return;
      this.DisplayFormat = str;
      this.InvalidateVisual();
    }

    private void UserControl_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.LastRangeBarCellPositions == null)
        return;
      Point pt = e.GetPosition((IInputElement) this);
      lock (this.LastRangeBarCellPositions)
      {
        int index = this.LastRangeBarCellPositions.FindIndex((Predicate<Tuple<Rect, long>>) (x => x.Item1.Contains(pt)));
        if (index == -1)
          return;
        Tuple<Rect, long> rangeBarCellPosition = this.LastRangeBarCellPositions[index];
        if (this.LastTooltipedCell == rangeBarCellPosition)
          return;
        NumericRange histogramRange = this.Range.GetHistogramRange(index);
        this.LastTooltipedCell = (object) rangeBarCellPosition;
        if (histogramRange == null)
          return;
        ToolTip toolTip = new ToolTip();
        toolTip.Content = (object) string.Format("Range: {0} ~ {1}\r\nCount: {2:#,##0}", (object) histogramRange.Begin.ToFormattedString(this.DisplayFormat), (object) histogramRange.End.ToFormattedString(this.DisplayFormat), (object) rangeBarCellPosition.Item2);
        this.ToolTip = (object) null;
        this.ToolTip = (object) toolTip;
      }
    }

    private void chkUseIgnoreOutOfRange_Click(object sender, RoutedEventArgs e)
    {
      if (this.Range == null)
        return;
      this.Range.IgnoreOutOfRange = this.chkUseIgnoreOutOfRange.IsChecked;
      this.Range.InvokeRangeChanged();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/maps/maprangecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).MouseWheel += new MouseWheelEventHandler(this.UserControl_MouseWheel);
          ((FrameworkElement) target).ContextMenuOpening += new ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
          ((UIElement) target).MouseMove += new MouseEventHandler(this.UserControl_MouseMove);
          break;
        case 2:
          this.chkAutoRange = (MenuItem) target;
          this.chkAutoRange.Click += new RoutedEventHandler(this.chkAutoRange_Click);
          break;
        case 3:
          this.mnuSetRange = (MenuItem) target;
          this.mnuSetRange.Click += new RoutedEventHandler(this.mnuSetRange_Click);
          break;
        case 4:
          this.chkEnableDistribution = (MenuItem) target;
          this.chkEnableDistribution.Click += new RoutedEventHandler(this.chkEnableDistribution_Click);
          break;
        case 5:
          this.chkDisableDistribution = (MenuItem) target;
          this.chkDisableDistribution.Click += new RoutedEventHandler(this.chkDisableDistribution_Click);
          break;
        case 6:
          this.chkUseOverlap = (MenuItem) target;
          this.chkUseOverlap.Click += new RoutedEventHandler(this.chkUseOverlap_Click);
          break;
        case 7:
          this.chkUseLogScaleBar = (MenuItem) target;
          this.chkUseLogScaleBar.Click += new RoutedEventHandler(this.chkUseLogScaleBar_Clicked);
          break;
        case 8:
          this.chkUseIgnoreOutOfRange = (MenuItem) target;
          this.chkUseIgnoreOutOfRange.Click += new RoutedEventHandler(this.chkUseIgnoreOutOfRange_Click);
          break;
        case 9:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectGrayColor_Click);
          break;
        case 10:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectBlueColor_Click);
          break;
        case 11:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectComplementaryColor1_Click);
          break;
        case 12:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectComplementaryColor2_Click);
          break;
        case 13:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectComplementaryColor3_Click);
          break;
        case 14:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectPalette1Color_Click);
          break;
        case 15:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectPalette2Color_Click);
          break;
        case 16:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectPalette3Color_Click);
          break;
        case 17:
          ((MenuItem) target).Click += new RoutedEventHandler(this.SelectCustomPalette_Click);
          break;
        case 18:
          ((MenuItem) target).Click += new RoutedEventHandler(this.OnDataFormat_Default_Clicked);
          break;
        case 19:
          ((MenuItem) target).Click += new RoutedEventHandler(this.OnDataFormat_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
