// Decompiled with JetBrains decompiler
// Type: EMx.UI.Charts.EMxChartAxis
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Logging;
using EMx.Maths;
using EMx.Maths.Viewport;
using EMx.UI.Dialogs;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Charts
{
  public partial class EMxChartAxis : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty AxisAliasProperty = DependencyProperty.Register(nameof (AxisAlias), typeof (string), typeof (EMxChartAxis), new PropertyMetadata((object) ""));
    public ZoomableRangeViewport AxisScale;
    private WriteableBitmap BackBuffer;
    protected bool IsOnDrag = false;
    protected System.Windows.Point DragStartPosition;
    protected System.Windows.Point DragCurrentPosition;
    protected System.Windows.Point DragStopPosition;
    protected double DragStartZoomOffset;
    protected double DragCurrentZoomOffset;
    private bool _contentLoaded;

    protected EMxChart Chart { get; set; }

    public string AxisAlias
    {
      get => (string) this.GetValue(EMxChartAxis.AxisAliasProperty);
      set => this.SetValue(EMxChartAxis.AxisAliasProperty, (object) value);
    }

    public bool IsAutoRange { get; set; }

    public List<double> MajorUnitPoints { get; set; }

    public virtual string DisplayFormat { get; set; }

    public eChartAxisDataType AxisDataType { get; set; }

    public eChartAxisPosition AxisPosition { get; set; }

    public bool UseZoomingControl { get; set; }

    public AxisUnitConverterType UnitConverterType { get; set; }

    public EMxChartAxis()
    {
      this.InitializeComponent();
      this.UnitConverterType = AxisUnitConverterType.None;
      this.AxisScale = new ZoomableRangeViewport();
      this.MajorUnitPoints = new List<double>();
      this.IsAutoRange = true;
      this.AxisDataType = eChartAxisDataType.Numeric;
      this.AxisPosition = eChartAxisPosition.Bottom;
      this.DisplayFormat = (string) null;
      this.UseZoomingControl = true;
      this.InitContextMenu();
    }

    protected void InitContextMenu()
    {
      ContextMenu mnu1 = new ContextMenu();
      MenuItem mnu2 = mnu1.AddSubMenu("Log Scale", (RoutedEventHandler) null);
      mnu2.AddSubMenu("Linear", new RoutedEventHandler(this.OnLinearScaleClicked));
      mnu2.AddSubMenu("Log10", new RoutedEventHandler(this.OnLog10ScaleClicked));
      mnu2.AddSubMenu("LogE", new RoutedEventHandler(this.OnLogEScaleClicked));
      MenuItem mnu3 = mnu1.AddSubMenu("Range", (RoutedEventHandler) null);
      mnu3.AddSubMenu("Auto", new RoutedEventHandler(this.OnRange_Auto_Clicked));
      mnu3.Items.Add((object) new Separator());
      mnu3.AddSubMenu("Fixed", new RoutedEventHandler(this.OnRange_Fixed_Clicked));
      mnu3.AddSubMenu("Set", new RoutedEventHandler(this.OnRange_Set_Clicked));
      MenuItem mnu4 = mnu1.AddSubMenu("Zoom", (RoutedEventHandler) null);
      mnu4.AddSubMenu("Reset", new RoutedEventHandler(this.OnZoom_Reset_Clicked));
      mnu4.AddSubMenu("200%", new RoutedEventHandler(this.OnZoom_200_Clicked));
      mnu4.AddSubMenu("500%", new RoutedEventHandler(this.OnZoom_500_Clicked));
      MenuItem mnu5 = mnu1.AddSubMenu("Unit Converter", (RoutedEventHandler) null);
      mnu5.AddSubMenu("None", new RoutedEventHandler(this.OnUnit_None_Clicked));
      mnu5.AddSubMenu("nm to eV", new RoutedEventHandler(this.OnUnit_Nano2eV_Clicked));
      MenuItem mnu6 = mnu1.AddSubMenu("Display Format", (RoutedEventHandler) null);
      mnu6.AddSubMenu("Default", new RoutedEventHandler(this.OnDataFormat_Default_Clicked));
      mnu6.AddSubMenu("Set", new RoutedEventHandler(this.OnDataFormat_Clicked));
      this.ContextMenu = mnu1;
    }

    private void OnLogEScaleClicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.SetLogNScale();
      if (this.Chart == null)
        return;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnLog10ScaleClicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.SetLog10Scale();
      if (this.Chart == null)
        return;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnLinearScaleClicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.SetLinearScale();
      if (this.Chart == null)
        return;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnRange_Set_Clicked(object sender, RoutedEventArgs e)
    {
      InputRangeDialog inputRangeDialog = new InputRangeDialog();
      inputRangeDialog.Owner = Application.Current.MainWindow;
      inputRangeDialog.Begin = this.AxisScale.World.Begin;
      inputRangeDialog.End = this.AxisScale.World.End;
      bool? nullable = inputRangeDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.AxisScale.World.Begin = inputRangeDialog.Begin;
      this.AxisScale.World.End = inputRangeDialog.End;
      this.IsAutoRange = false;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnRange_Fixed_Clicked(object sender, RoutedEventArgs e)
    {
      this.IsAutoRange = false;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnRange_Auto_Clicked(object sender, RoutedEventArgs e)
    {
      this.IsAutoRange = true;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnZoom_Reset_Clicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.ZoomOffset = 0.0;
      this.AxisScale.ZoomRatio = 1.0;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnZoom_500_Clicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.ZoomOffset = 0.0;
      this.AxisScale.ZoomRatio = 5.0;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnZoom_200_Clicked(object sender, RoutedEventArgs e)
    {
      this.AxisScale.ZoomOffset = 0.0;
      this.AxisScale.ZoomRatio = 2.0;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnDataFormat_Default_Clicked(object sender, RoutedEventArgs e)
    {
      this.DisplayFormat = (string) null;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnDataFormat_Clicked(object sender, RoutedEventArgs e)
    {
      string str = Application.Current.MainWindow.ShowInputMessage("Display Format", "Please type display format string.\r\nexamples.\r\n000.000\r\n#,###\r\n#,###.00\r\ng3\r\ng6\r\nAuto\r\n....", this.DisplayFormat);
      if (str == null)
        return;
      this.DisplayFormat = str;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnUnit_Nano2eV_Clicked(object sender, RoutedEventArgs e)
    {
      this.UnitConverterType = AxisUnitConverterType.NanometerToElectronvolt;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void OnUnit_None_Clicked(object sender, RoutedEventArgs e)
    {
      this.UnitConverterType = AxisUnitConverterType.None;
      this.Chart.ChartArea.InvalidateVisual();
    }

    private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
    {
      EMxChart parent = this.FindParent<EMxChart>();
      if (parent == null)
        return;
      this.Chart = parent;
    }

    protected override void OnRender(DrawingContext dc)
    {
      if (this.Chart == null || this.ActualWidth < 2.0 || this.ActualHeight < 2.0)
        return;
      if (this.BackBuffer == null || Math.Round(this.ActualWidth) != (double) this.BackBuffer.PixelWidth || Math.Round(this.ActualHeight) != (double) this.BackBuffer.PixelHeight)
        this.BackBuffer = new WriteableBitmap((int) this.ActualWidth, (int) this.ActualHeight, 72.0, 72.0, PixelFormats.Bgr24, (BitmapPalette) null);
      WriteableBitmap backBuffer = this.BackBuffer;
      bool flag = false;
      try
      {
        if (flag = backBuffer.TryLock((Duration) new TimeSpan(0, 0, 1)))
        {
          using (Bitmap bitmap = new Bitmap(backBuffer.PixelWidth, backBuffer.PixelHeight, backBuffer.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, backBuffer.BackBuffer))
          {
            using (Graphics g = Graphics.FromImage((System.Drawing.Image) bitmap))
              this.DrawChartAxis(g, backBuffer.PixelWidth, backBuffer.PixelHeight);
          }
          backBuffer.AddDirtyRect(new Int32Rect(0, 0, backBuffer.PixelWidth, backBuffer.PixelHeight));
        }
      }
      finally
      {
        if (flag)
          backBuffer.Unlock();
      }
      dc.DrawImage((ImageSource) backBuffer, new Rect(0.0, 0.0, (double) backBuffer.PixelWidth, (double) backBuffer.PixelHeight));
    }

    private bool IsIntegerUnits => this.MajorUnitPoints.Count != 0 && this.MajorUnitPoints.All<double>((Func<double, bool>) (x => x == (double) (long) x));

    private bool IsRealUnits => !this.IsIntegerUnits;

    public virtual string GetDisplayText(double org_val, string fmt)
    {
      if (fmt == null)
        fmt = this.DisplayFormat;
      double val = org_val;
      string str1 = "";
      if (this.UnitConverterType == AxisUnitConverterType.NanometerToElectronvolt && org_val > 1.0 && org_val < 3000.0)
      {
        val = 1240.0 / val;
        str1 = "eV";
      }
      string str2 = "";
      if (string.IsNullOrWhiteSpace(fmt) || fmt.Equals("Auto", StringComparison.CurrentCultureIgnoreCase))
      {
        if (this.IsIntegerUnits)
        {
          str2 = string.Format("{0:#,##0}", (object) val);
        }
        else
        {
          FloatingDecimal10 floatingDecimal10 = new FloatingDecimal10(val);
          int log10 = floatingDecimal10.GetLog10();
          str2 = log10 <= -1 || log10 >= 4 ? string.Format("{0:0.00}{1}", (object) floatingDecimal10.GetFraction(), (object) floatingDecimal10.GetPostfix()) : val.ToString().SafeSubstring(0, Math.Max(4, log10 + 4));
        }
      }
      else
      {
        try
        {
          str2 = !fmt.StartsWith("{0:") ? string.Format("{0:" + fmt + "}", (object) val) : string.Format(fmt, (object) val);
        }
        catch (Exception ex)
        {
          EMxChartAxis.log.Error(ex, ex.Message);
        }
      }
      return str2 + str1;
    }

    private void DrawChartAxis(Graphics g, int w, int h)
    {
      this.AutoMeasureAxisUnit();
      g.SmoothingMode = SmoothingMode.HighSpeed;
      SolidBrush solidBrush1 = new SolidBrush((this.Chart.Background as SolidColorBrush).Color.ToDrawingColor());
      SolidBrush solidBrush2 = new SolidBrush((this.Chart.Foreground as SolidColorBrush).Color.ToDrawingColor());
      g.FillRectangle((System.Drawing.Brush) solidBrush1, 0, 0, w, h);
      Font font = new Font("tahoma", 8.25f);
      System.Drawing.Pen pen = new System.Drawing.Pen(Colors.Black.ToDrawingColor(), 1f);
      System.Drawing.Point point1 = new System.Drawing.Point(w - 1, 0);
      System.Drawing.Point pt2 = new System.Drawing.Point(w - 1, h - 1);
      System.Drawing.Point pt1 = new System.Drawing.Point(0, 0);
      System.Drawing.Point point2 = new System.Drawing.Point(0, h - 1);
      StringFormat format = new StringFormat();
      format.Alignment = StringAlignment.Center;
      format.LineAlignment = StringAlignment.Center;
      switch (this.AxisPosition)
      {
        case eChartAxisPosition.Left:
          g.DrawLine(pen, point1, pt2);
          float num1 = (float) h;
          SizeF sizeF1 = g.MeasureString("T", font);
          for (int index = 0; index < this.MajorUnitPoints.Count; ++index)
          {
            double majorUnitPoint = this.MajorUnitPoints[index];
            bool success;
            int view = (int) this.AxisScale.ToView(majorUnitPoint, out success);
            if (success)
            {
              string displayText = this.GetDisplayText(majorUnitPoint, (string) null);
              if ((double) view + (double) sizeF1.Height / 2.0 < (double) num1 && (double) view - (double) sizeF1.Height / 2.0 > 0.0)
              {
                g.DrawString(displayText, font, (System.Drawing.Brush) solidBrush2, (float) (w / 2), (float) view, format);
                num1 = (float) view - sizeF1.Height / 2f;
              }
            }
          }
          break;
        case eChartAxisPosition.Bottom:
          float num2 = 0.0f;
          g.DrawLine(pen, pt1, point1);
          for (int index = 0; index < this.MajorUnitPoints.Count; ++index)
          {
            double majorUnitPoint = this.MajorUnitPoints[index];
            bool success;
            int view = (int) this.AxisScale.ToView(majorUnitPoint, out success);
            if (success)
            {
              string displayText = this.GetDisplayText(majorUnitPoint, (string) null);
              SizeF sizeF2 = g.MeasureString(displayText, font);
              if ((double) view - (double) sizeF2.Width / 2.0 > (double) num2 && (double) view + (double) sizeF2.Width < (double) w)
              {
                g.DrawString(displayText, font, (System.Drawing.Brush) solidBrush2, (float) view, (float) (h / 2), format);
                num2 = (float) view + sizeF2.Width / 2f;
              }
            }
          }
          break;
      }
    }

    public void AutoMeasureAxisUnit()
    {
      List<double> doubleList = new List<double>();
      if (this.AxisDataType == eChartAxisDataType.Numeric)
      {
        if (this.AxisScale.IsLogScale)
        {
          NumericRange scaledWorld = this.AxisScale.ScaledWorld;
          if (scaledWorld.Length > 0.0)
          {
            double num1 = Math.Pow(10.0, (double) ((int) Math.Round(Math.Log10(scaledWorld.Length), 0) - 1));
            if (num1 != 0.0)
            {
              double num2 = scaledWorld.Begin / num1;
              double num3 = scaledWorld.End / num1;
              int num4 = (int) num2;
              int num5 = (int) num3 + (num3 <= 0.0 || num3 % 1.0 == 0.0 ? 0 : 1);
              for (int index = num4; index <= num5; ++index)
              {
                double output;
                if (this.AxisScale.ToNonScaledWorld((double) index * num1, out output))
                  doubleList.Add(output);
              }
            }
          }
        }
        else
        {
          NumericRange world = this.AxisScale.World;
          if (world.Length > 0.0)
          {
            double num6 = Math.Pow(10.0, (double) ((int) Math.Round(Math.Log10(world.Length), 0) - 1));
            if (num6 != 0.0)
            {
              double num7 = world.Begin / num6;
              double num8 = world.End / num6;
              int num9 = (int) num7;
              int num10 = (int) num8 + (num8 <= 0.0 || num8 % 1.0 == 0.0 ? 0 : 1);
              for (int index = num9; index <= num10; ++index)
                doubleList.Add((double) index * num6);
            }
          }
        }
      }
      else if (this.AxisDataType == eChartAxisDataType.DateTime || this.AxisDataType != eChartAxisDataType.Custom)
        ;
      lock (this.MajorUnitPoints)
        this.MajorUnitPoints = doubleList;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      base.OnMouseDown(e);
      if (e.Handled || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.DragStartZoomOffset = this.AxisScale.ZoomOffset;
      this.DragCurrentZoomOffset = this.DragStartZoomOffset;
      this.DragStartPosition = e.GetPosition((IInputElement) this);
      this.DragCurrentPosition = this.DragStartPosition;
      Mouse.OverrideCursor = this.AxisPosition == eChartAxisPosition.Left || this.AxisPosition == eChartAxisPosition.Right ? Cursors.ScrollNS : Cursors.ScrollWE;
      Mouse.Capture((IInputElement) this);
      this.IsOnDrag = true;
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
      base.OnMouseUp(e);
      if (e.Handled || this.Chart == null || !this.IsOnDrag || e.LeftButton != MouseButtonState.Released)
        return;
      this.DragStopPosition = e.GetPosition((IInputElement) this);
      this.IsOnDrag = false;
      Mouse.OverrideCursor = (Cursor) null;
      Mouse.Capture((IInputElement) null);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (e.Handled || this.Chart == null || !this.IsOnDrag || !this.UseZoomingControl)
        return;
      this.DragCurrentPosition = e.GetPosition((IInputElement) this);
      Func<System.Windows.Point, double> func = (Func<System.Windows.Point, double>) (x => this.AxisPosition != eChartAxisPosition.Left && this.AxisPosition != eChartAxisPosition.Right ? x.X : x.Y);
      this.DragCurrentZoomOffset = this.DragStartZoomOffset + 2.0 * (func(this.DragCurrentPosition) - func(this.DragStartPosition)) / this.AxisScale.View.Length / Math.Max(1E-10, this.AxisScale.ZoomRatio);
      if (this.DragCurrentZoomOffset != this.AxisScale.ZoomOffset)
      {
        this.AxisScale.ZoomOffset = this.DragCurrentZoomOffset;
        this.Chart.ChartArea.InvalidateVisual();
      }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      base.OnMouseWheel(e);
      if (this.Chart == null || !this.UseZoomingControl || this.IsOnDrag)
        return;
      this.AxisScale.ZoomRatio += (double) Math.Sign(e.Delta) * this.AxisScale.ZoomRatio * 0.1;
      this.Chart.ChartArea.InvalidateVisual();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/charts/emxchartaxis.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded_1);
      else
        this._contentLoaded = true;
    }
  }
}
