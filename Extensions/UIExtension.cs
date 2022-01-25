// Decompiled with JetBrains decompiler
// Type: EMx.UI.Extensions.UIExtension
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.UI.Charts;
using EMx.UI.Dialogs;
using EMx.UI.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace EMx.UI.Extensions
{
  public static class UIExtension
  {
    private static ILog log = LogManager.GetLogger();
    private static CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-us");
    private static Action EmptyDelegate = (Action) (() => { });
    private const string FileDialogRegisterGroup = "ComDlgMRU";

    public static DrawingContext DrawHoriLine(
      this DrawingContext dc,
      System.Windows.Media.Pen pen,
      double x0,
      double x1,
      double y)
    {
      if (dc == null)
        return (DrawingContext) null;
      double y1 = Math.Round(y) + 0.5;
      double x2 = Math.Floor(Math.Min(x0, x1));
      double x3 = Math.Ceiling(Math.Max(x0, x1));
      dc.DrawLine(pen, new System.Windows.Point(x2, y1), new System.Windows.Point(x3, y1));
      return dc;
    }

    public static DrawingContext DrawVertLine(
      this DrawingContext dc,
      System.Windows.Media.Pen pen,
      double x,
      double y0,
      double y1)
    {
      if (dc == null)
        return (DrawingContext) null;
      double x1 = Math.Round(x) + 0.5;
      double y2 = Math.Floor(Math.Min(y0, y1));
      double y3 = Math.Ceiling(Math.Max(y0, y1));
      dc.DrawLine(pen, new System.Windows.Point(x1, y2), new System.Windows.Point(x1, y3));
      return dc;
    }

    public static DrawingContext DrawText(
      this DrawingContext dc,
      string text,
      System.Windows.Point pos,
      Typeface font,
      double font_size,
      System.Windows.Media.Brush font_brush,
      int max_width = -1,
      HorizontalAlignment hori_align = HorizontalAlignment.Center,
      VerticalAlignment vert_align = VerticalAlignment.Center)
    {
      if (text.IsNullOrEmpty())
        return dc;
      FormattedText formattedText = new FormattedText(text, UIExtension.DefaultCulture, FlowDirection.LeftToRight, font, font_size, font_brush);
      if (max_width > 0)
        formattedText.MaxTextWidth = (double) max_width;
      double height = formattedText.Height;
      double width = formattedText.Width;
      double x = pos.X;
      double y = pos.Y;
      switch (hori_align)
      {
        case HorizontalAlignment.Center:
          x = pos.X - width / 2.0;
          break;
        case HorizontalAlignment.Right:
          x = pos.X - width;
          break;
      }
      switch (vert_align)
      {
        case VerticalAlignment.Center:
          y = pos.Y - height / 2.0;
          break;
        case VerticalAlignment.Bottom:
          y = pos.Y - height;
          break;
      }
      dc.DrawText(formattedText, new System.Windows.Point(x, y));
      return dc;
    }

    public static GridLength ConvertToGridLength(this string s) => (GridLength) new GridLengthConverter().ConvertFromString(s);

    public static Thickness ConvertToThickness(this string s) => (Thickness) new ThicknessConverter().ConvertFromString(s);

    public static string ConvertToString(this Thickness th) => new ThicknessConverter().ConvertToString((object) th);

    public static System.Windows.Media.Color GetSolidColor(this System.Windows.Media.Brush brush) => brush is SolidColorBrush solidColorBrush ? solidColorBrush.Color : Colors.White;

    public static Win32WindowHandle GetWin32Handle(this Window wnd) => new Win32WindowHandle(wnd);

    public static bool IsModal(this Window window) => window != null && (bool) typeof (Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) window);

    public static EMxChartScatterSeries CreateOrGetScatterSeries(
      this EMxChartArea chart_area,
      int index)
    {
      chart_area.Series.Resize<IEMxChartSeries>(index + 1, (Func<IEMxChartSeries>) (() => (IEMxChartSeries) new EMxChartScatterSeries()));
      if (!(chart_area.Series[index] is EMxChartScatterSeries chartScatterSeries))
      {
        UIExtension.log.Warn("Replace Series to ScatterSeries.");
        chart_area.Series[index] = (IEMxChartSeries) (chartScatterSeries = new EMxChartScatterSeries());
      }
      return chartScatterSeries;
    }

    public static BitmapSource ToCapturedImage(
      this FrameworkElement elem,
      int width = -1,
      int height = -1)
    {
      if (elem == null)
        return (BitmapSource) null;
      PresentationSource presentationSource = PresentationSource.FromVisual((Visual) elem);
      double dpiX = 96.0;
      double dpiY = 96.0;
      if (presentationSource != null)
      {
        Matrix transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        dpiX = 96.0 * transformToDevice.M11;
        transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        dpiY = 96.0 * transformToDevice.M22;
      }
      width = width != -1 ? (int) Math.Min(elem.ActualWidth, (double) width) : (int) elem.ActualWidth;
      height = height != -1 ? (int) Math.Min(elem.ActualHeight, (double) height) : (int) elem.ActualHeight;
      if (width == 0 || height == 0)
        return (BitmapSource) null;
      RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) ((double) width * dpiX / 96.0), (int) ((double) height * dpiY / 96.0), dpiX, dpiY, PixelFormats.Pbgra32);
      DrawingVisual drawingVisual = new DrawingVisual();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen())
      {
        VisualBrush visualBrush = new VisualBrush((Visual) elem);
        drawingContext.DrawRectangle((System.Windows.Media.Brush) visualBrush, (System.Windows.Media.Pen) null, new Rect(0.0, 0.0, (double) width, (double) height));
      }
      renderTargetBitmap.Render((Visual) drawingVisual);
      return (BitmapSource) renderTargetBitmap;
    }

    public static void CaptureImage(this FrameworkElement elem, int width = -1, int height = -1)
    {
      if (elem == null)
        return;
      string str = Application.Current.MainWindow.SaveFileDialog("", "PNG Image File(*.png)|*.png", "png");
      if (str.IsNullOrEmpty())
        return;
      PresentationSource presentationSource = PresentationSource.FromVisual((Visual) elem);
      double dpiX = 96.0;
      double dpiY = 96.0;
      if (presentationSource != null)
      {
        Matrix transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        dpiX = 96.0 * transformToDevice.M11;
        transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        dpiY = 96.0 * transformToDevice.M22;
      }
      width = width != -1 ? (int) Math.Min(elem.ActualWidth, (double) width) : (int) elem.ActualWidth;
      height = height != -1 ? (int) Math.Min(elem.ActualHeight, (double) height) : (int) elem.ActualHeight;
      if (width == 0 || height == 0)
        return;
      RenderTargetBitmap bmp = new RenderTargetBitmap((int) ((double) width * dpiX / 96.0), (int) ((double) height * dpiY / 96.0), dpiX, dpiY, PixelFormats.Pbgra32);
      DrawingVisual drawingVisual = new DrawingVisual();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen())
      {
        VisualBrush visualBrush = new VisualBrush((Visual) elem);
        drawingContext.DrawRectangle((System.Windows.Media.Brush) visualBrush, (System.Windows.Media.Pen) null, new Rect(0.0, 0.0, (double) width, (double) height));
      }
      bmp.Render((Visual) drawingVisual);
      bmp.SaveFileAsPng(str);
      if (Application.Current.MainWindow.ShowQuestionMessage("Export Manager", "Would you like to open a exported file?\r\n - {0}".At((object) str)) == eDialogButtons.Yes)
      {
        try
        {
          Process.Start(str);
        }
        catch (Exception ex)
        {
          UIExtension.log.Error(ex, ex.Message);
        }
      }
    }

    public static void SetWindowSize(this Window wnd, int width, int height)
    {
      if (wnd == null)
        return;
      wnd.Width = (double) width;
      wnd.Height = (double) height;
    }

    public static char ConvertToChar(this Key key) => new KeyConverter().ConvertToString((object) key).FirstOrDefault<char>();

    public static MenuItem AddSubMenu(
      this MenuItem mnu,
      string header,
      RoutedEventHandler handler)
    {
      MenuItem menuItem = new MenuItem();
      menuItem.Header = (object) header;
      if (handler != null)
        menuItem.Click += handler;
      mnu.Items.Add((object) menuItem);
      return menuItem;
    }

    public static MenuItem AddSubMenu(
      this ContextMenu mnu,
      string header,
      RoutedEventHandler handler)
    {
      MenuItem menuItem = new MenuItem();
      menuItem.Header = (object) header;
      if (handler != null)
        menuItem.Click += handler;
      mnu.Items.Add((object) menuItem);
      return menuItem;
    }

    public static void BeginAction(this Dispatcher dispatcher, int delay, Action action) => dispatcher.BeginInvoke((Delegate) (() =>
    {
      Thread.Sleep(delay);
      if (action == null)
        return;
      action();
    }));

    public static void SaveFileAsPng(this BitmapSource bmp, string path)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
      {
        PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
        pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));
        pngBitmapEncoder.Save((Stream) fileStream);
      }
    }

    public static void WriteAsPng(this BitmapSource bmp, Stream stream)
    {
      PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
      pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));
      pngBitmapEncoder.Save(stream);
    }

    public static unsafe void ClearColor(this WriteableBitmap bmp, int color)
    {
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadWrite))
      {
        int num = bitmapContext.Width * bitmapContext.Height;
        int* pixels = bitmapContext.Pixels;
        for (int index = 0; index < num; ++index)
          pixels[index] = color;
      }
    }

    public static unsafe void ForLoop(
      this WriteableBitmap bmp,
      int xpos,
      int ypos,
      int w,
      int h,
      Func<int, int, int, int> selector)
    {
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadWrite))
      {
        int width = bitmapContext.Width;
        int height = bitmapContext.Height;
        int num1 = width * height;
        int* pixels = bitmapContext.Pixels;
        int num2 = xpos + w;
        int num3 = ypos + h;
        for (int index1 = ypos; index1 < num3; ++index1)
        {
          for (int index2 = xpos; index2 < num2; ++index2)
          {
            int index3 = index1 * width + index2;
            pixels[index3] = selector(index2, index1, pixels[index3]);
          }
        }
      }
    }

    public static unsafe WriteableBitmap AccumulateEx(
      this WriteableBitmap bmp_src,
      bool x_dir,
      bool y_dir,
      byte base_value)
    {
      if (!x_dir && !y_dir)
      {
        UIExtension.log.Warn("Both X and Y directions are disabled.");
        return (WriteableBitmap) null;
      }
      using (BitmapContext bitmapContext1 = bmp_src.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int* pixels1 = bitmapContext1.Pixels;
        int width = bitmapContext1.Width;
        int height = bitmapContext1.Height;
        WriteableBitmap bmp = BitmapFactory.New(width, height);
        int[,] numArray = new int[height, width];
        if (x_dir)
        {
          for (int index1 = 0; index1 < height; ++index1)
          {
            int num1 = (int) base_value;
            int num2 = (int) base_value;
            int num3 = (int) base_value;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            for (int index2 = 0; index2 < width; ++index2)
            {
              int index3 = index1 * width + index2;
              int num8 = pixels1[index3];
              int num9 = num8 >> 16 & (int) byte.MaxValue;
              int num10 = num8 >> 8 & (int) byte.MaxValue;
              int num11 = num8 & (int) byte.MaxValue;
              int num12 = num9 - num1;
              int num13 = num10 - num2;
              int num14 = num11 - num3;
              int num15 = num12 * num4;
              int num16 = num13 * num5;
              int num17 = num14 * num6;
              if (num15 > 0)
              {
                numArray[index1, index2] += num7;
                num7 = num12;
              }
              else
                num7 = 0;
              num4 = num12;
              num1 = num9;
            }
            for (int index4 = width - 1; index4 > 0; --index4)
            {
              int index5 = index1 * width + index4;
              int num18 = pixels1[index5];
              int num19 = num18 >> 16 & (int) byte.MaxValue;
              int num20 = num18 >> 8 & (int) byte.MaxValue;
              int num21 = num18 & (int) byte.MaxValue;
              int num22 = num19 - num1;
              int num23 = num20 - num2;
              int num24 = num21 - num3;
              int num25 = num22 * num4;
              int num26 = num23 * num5;
              int num27 = num24 * num6;
              if (num25 > 0)
              {
                numArray[index1, index4] += num7;
                num7 = num22;
              }
              else
                num7 = 0;
              num4 = num22;
              num1 = num19;
            }
          }
        }
        if (y_dir)
        {
          for (int index6 = 0; index6 < width; ++index6)
          {
            int num28 = (int) base_value;
            int num29 = (int) base_value;
            int num30 = (int) base_value;
            int num31 = 0;
            int num32 = 0;
            int num33 = 0;
            int num34 = 0;
            for (int index7 = 0; index7 < height; ++index7)
            {
              int index8 = index7 * width + index6;
              int num35 = pixels1[index8];
              int num36 = num35 >> 16 & (int) byte.MaxValue;
              int num37 = num35 >> 8 & (int) byte.MaxValue;
              int num38 = num35 & (int) byte.MaxValue;
              int num39 = num36 - num28;
              int num40 = num37 - num29;
              int num41 = num38 - num30;
              int num42 = num39 * num31;
              int num43 = num40 * num32;
              int num44 = num41 * num33;
              if (num42 > 0)
              {
                numArray[index7, index6] += num34;
                num34 = num39;
              }
              else
                num34 = 0;
              num31 = num39;
              num28 = num36;
            }
            for (int index9 = height - 1; index9 > 0; --index9)
            {
              int index10 = index9 * width + index6;
              int num45 = pixels1[index10];
              int num46 = num45 >> 16 & (int) byte.MaxValue;
              int num47 = num45 >> 8 & (int) byte.MaxValue;
              int num48 = num45 & (int) byte.MaxValue;
              int num49 = num46 - num28;
              int num50 = num47 - num29;
              int num51 = num48 - num30;
              int num52 = num49 * num31;
              int num53 = num50 * num32;
              int num54 = num51 * num33;
              if (num52 > 0)
              {
                numArray[index9, index6] += num34;
                num34 = num49;
              }
              else
                num34 = 0;
              num31 = num49;
              num28 = num46;
            }
          }
        }
        double num55 = double.MinValue;
        double num56 = double.MaxValue;
        for (int index11 = 0; index11 < height; ++index11)
        {
          for (int index12 = 0; index12 < width; ++index12)
          {
            int num57 = numArray[index11, index12];
            if ((double) num57 < num56)
              num56 = (double) num57;
            if ((double) num57 > num55)
              num55 = (double) num57;
          }
        }
        double num58 = num55 - num56;
        if (num58 == 0.0)
        {
          UIExtension.log.Warn("Invalid Image Data. Max.Value[{0}] equals to Min.Value[{1}].", (object) num55, (object) num56);
          return bmp;
        }
        using (BitmapContext bitmapContext2 = bmp.GetBitmapContext())
        {
          int* pixels2 = bitmapContext2.Pixels;
          for (int index13 = 0; index13 < height; ++index13)
          {
            for (int index14 = 0; index14 < width; ++index14)
            {
              int index15 = index13 * width + index14;
              byte num59 = (byte) (256.0 * ((double) numArray[index13, index14] - num56) / num58);
              pixels2[index15] = -16777216 | (int) num59 << 16 | (int) num59 << 8 | (int) num59;
            }
          }
        }
        return bmp;
      }
    }

    public static unsafe List<byte> ToByteList(this WriteableBitmap bmp)
    {
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int capacity = bitmapContext.Width * bitmapContext.Height;
        List<byte> byteList = new List<byte>(capacity);
        int* pixels = bitmapContext.Pixels;
        for (int index = 0; index < capacity; ++index)
        {
          int num1 = pixels[index];
          byte num2 = (byte) (((num1 >> 16) + (num1 >> 8) + num1) / 3);
          byteList.Add(num2);
        }
        return byteList;
      }
    }

    public static unsafe List<TValue> ToList<TValue>(
      this WriteableBitmap bmp,
      Func<int, TValue> selector)
    {
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int capacity = bitmapContext.Width * bitmapContext.Height;
        List<TValue> objList = new List<TValue>(capacity);
        int* pixels = bitmapContext.Pixels;
        for (int index = 0; index < capacity; ++index)
          objList.Add(selector(pixels[index]));
        return objList;
      }
    }

    public static unsafe TValue[,] To2dArray<TValue>(
      this WriteableBitmap bmp,
      Func<int, TValue> selector)
    {
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int width = bitmapContext.Width;
        int height = bitmapContext.Height;
        int num = width * height;
        TValue[,] objArray = new TValue[height, width];
        int* pixels = bitmapContext.Pixels;
        for (int index1 = 0; index1 < height; ++index1)
        {
          for (int index2 = 0; index2 < width; ++index2)
            objArray[index1, index2] = selector(pixels[index1 * width + index2]);
        }
        return objArray;
      }
    }

    public static unsafe List<int> HistogrammingRGB(this WriteableBitmap bmp)
    {
      int capacity = 256;
      List<int> intList = new List<int>(capacity);
      for (int index = 0; index < capacity; ++index)
        intList.Add(0);
      using (BitmapContext bitmapContext = bmp.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int num1 = bitmapContext.Width * bitmapContext.Height;
        int* pixels = bitmapContext.Pixels;
        for (int index = 0; index < num1; ++index)
        {
          int num2 = pixels[index];
          byte num3 = (byte) (((num2 >> 16) + (num2 >> 8) + num2) / 3);
          ++intList[(int) num3];
        }
      }
      return intList;
    }

    public static unsafe void WriteFrom(
      this WriteableBitmap bmp_src,
      int src_offset,
      byte[] buffer,
      int start_at,
      int size)
    {
      if (size == -1)
        size = buffer.Length - start_at;
      using (BitmapContext bitmapContext = bmp_src.GetBitmapContext(ReadWriteMode.ReadWrite))
      {
        int width = bitmapContext.Width;
        int height = bitmapContext.Height;
        int* pixels = bitmapContext.Pixels;
        int num1 = Math.Min(Math.Min(buffer.Length - start_at, size), width * height);
        for (int index = 0; index < num1; ++index)
        {
          int num2 = (int) buffer[index + start_at] & (int) byte.MaxValue;
          pixels[index + src_offset] = -16777216 | num2 << 16 | num2 << 8 | num2;
        }
      }
    }

    public static unsafe WriteableBitmap SelectEx(
      this WriteableBitmap bmp_src,
      Func<byte, byte, byte, SimpleRGB> selector)
    {
      using (BitmapContext bitmapContext1 = bmp_src.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int width = bitmapContext1.Width;
        int height = bitmapContext1.Height;
        WriteableBitmap bmp = BitmapFactory.New(width, height);
        using (BitmapContext bitmapContext2 = bmp.GetBitmapContext())
        {
          int* pixels1 = bitmapContext1.Pixels;
          int* pixels2 = bitmapContext2.Pixels;
          int num1 = width * height;
          for (int index = 0; index < num1; ++index)
          {
            int num2 = pixels1[index];
            SimpleRGB simpleRgb = selector((byte) (num2 >> 16), (byte) (num2 >> 8), (byte) num2);
            pixels2[index] = -16777216 | (int) simpleRgb.R << 16 | (int) simpleRgb.G << 8 | (int) simpleRgb.B;
          }
          return bmp;
        }
      }
    }

    public static unsafe WriteableBitmap SelectExAlpha(
      this WriteableBitmap bmp_src,
      Func<byte, byte, byte, byte, SimpleRGB> selector)
    {
      using (BitmapContext bitmapContext1 = bmp_src.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int width = bitmapContext1.Width;
        int height = bitmapContext1.Height;
        WriteableBitmap bmp = BitmapFactory.New(width, height);
        using (BitmapContext bitmapContext2 = bmp.GetBitmapContext())
        {
          int* pixels1 = bitmapContext1.Pixels;
          int* pixels2 = bitmapContext2.Pixels;
          int num1 = width * height;
          for (int index = 0; index < num1; ++index)
          {
            int num2 = pixels1[index];
            SimpleRGB simpleRgb = selector((byte) (num2 >> 24), (byte) (num2 >> 16), (byte) (num2 >> 8), (byte) num2);
            int num3 = (int) simpleRgb.A << 24 | (int) simpleRgb.R << 16 | (int) simpleRgb.G << 8 | (int) simpleRgb.B;
            pixels2[index] = num3;
          }
          return bmp;
        }
      }
    }

    public static unsafe WriteableBitmap ConvoluteEx(
      this WriteableBitmap bmp_src,
      int[,] kernel,
      byte offset,
      float factor)
    {
      int num1 = kernel.GetUpperBound(0) + 1;
      int num2 = kernel.GetUpperBound(1) + 1;
      if ((num2 & 1) == 0 || (num1 & 1) == 0)
        throw new ArgumentException("Kernel width and height should be odd");
      int num3 = 0;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          num3 += kernel[index1, index2];
      }
      if (num3 == 0)
        num3 = 1;
      float nor_factor = factor / (float) num3;
      Func<int, byte> func = (Func<int, byte>) (x => (byte) Math.Max(0.0f, Math.Min((float) byte.MaxValue, (float) (x + (int) offset) * nor_factor)));
      using (BitmapContext bitmapContext1 = bmp_src.GetBitmapContext(ReadWriteMode.ReadOnly))
      {
        int width = bitmapContext1.Width;
        int height = bitmapContext1.Height;
        WriteableBitmap bmp = BitmapFactory.New(width, height);
        using (BitmapContext bitmapContext2 = bmp.GetBitmapContext())
        {
          int* pixels1 = bitmapContext1.Pixels;
          int* pixels2 = bitmapContext2.Pixels;
          int num4 = num2 >> 1;
          int num5 = num1 >> 1;
          for (int index3 = 0; index3 < height; ++index3)
          {
            for (int index4 = 0; index4 < width; ++index4)
            {
              int num6 = 0;
              int num7 = 0;
              int num8 = 0;
              for (int index5 = -num4; index5 <= num4; ++index5)
              {
                int num9 = Math.Max(0, Math.Min(width - 1, index5 + index4));
                for (int index6 = -num5; index6 <= num5; ++index6)
                {
                  int num10 = Math.Max(0, Math.Min(height - 1, index6 + index3));
                  int num11 = pixels1[num10 * width + num9];
                  int num12 = kernel[index6 + num5, index5 + num4];
                  if ((uint) num12 > 0U)
                  {
                    num6 += (num11 >> 16 & (int) byte.MaxValue) * num12;
                    num7 += (num11 >> 8 & (int) byte.MaxValue) * num12;
                    num8 += (num11 & (int) byte.MaxValue) * num12;
                  }
                }
              }
              byte num13 = func(num6);
              byte num14 = func(num7);
              byte num15 = func(num8);
              pixels2[index3 * width + index4] = -16777216 | (int) num13 << 16 | (int) num14 << 8 | (int) num15;
            }
          }
          return bmp;
        }
      }
    }

    public static void SmartFocus(this ListBox ctrl)
    {
      if (!(ctrl.SelectedItem is ListBoxItem listBoxItem) && ctrl.SelectedIndex > -1)
        listBoxItem = ctrl.ItemContainerGenerator.ContainerFromIndex(ctrl.SelectedIndex) as ListBoxItem;
      listBoxItem?.Focus();
    }

    public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
    {
      int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
      for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
      {
        DependencyObject child = VisualTreeHelper.GetChild(obj, childIndex);
        if (child != null && child is T)
          return (T) child;
        T visualChild = UIExtension.FindVisualChild<T>(child);
        if ((object) visualChild != null)
          return visualChild;
      }
      return default (T);
    }

    public static void SmartFocus(this DataGrid ctrl, int column_index = 0)
    {
      if (ctrl.SelectedIndex == -1)
        return;
      if (!(ctrl.SelectedItem is DataGridRow dataGridRow) && ctrl.SelectedIndex > -1)
      {
        ctrl.ScrollIntoView(ctrl.SelectedItem);
        dataGridRow = ctrl.ItemContainerGenerator.ContainerFromIndex(ctrl.SelectedIndex) as DataGridRow;
      }
      if (dataGridRow != null)
      {
        dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        ctrl.Focus();
      }
      else
        ctrl.Focus();
      if (ctrl.SelectedCells == null || ctrl.SelectedCells.Count <= column_index)
        return;
      DataGridCellInfo selectedCell = ctrl.SelectedCells[column_index];
      FrameworkElement cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);
      if (cellContent != null)
        Keyboard.Focus((IInputElement) (cellContent.Parent as DataGridCell));
    }

    public static void ScrollToEnd(this DataGrid ctrl)
    {
      if (ctrl == null || ctrl.Items.Count <= 0 || !(VisualTreeHelper.GetChild((DependencyObject) ctrl, 0) is Decorator child1) || !(child1.Child is ScrollViewer child2))
        return;
      child2.ScrollToEnd();
    }

    public static int PasteClipboardData(this DataGrid grid)
    {
      int num1 = 0;
      List<string[]> clipboardDataAsTable = Helper.Data.GetClipboardDataAsTable();
      if (clipboardDataAsTable.Count <= 1)
        return -1;
      try
      {
        int num2 = Math.Max(0, grid.Items.IndexOf(grid.CurrentItem));
        int num3 = grid.Items.Count - 1;
        int num4 = grid.SelectionUnit == DataGridSelectionUnit.FullRow ? 0 : Math.Max(0, grid.Columns.IndexOf(grid.CurrentColumn));
        int num5 = grid.Columns.Count - 1;
        for (int index1 = 0; index1 < clipboardDataAsTable.Count; ++index1)
        {
          string[] strArray = clipboardDataAsTable[index1];
          int index2 = num2 + index1;
          if (index1 <= num3)
          {
            for (int displayIndex = num4; displayIndex <= num5; ++displayIndex)
            {
              DataGridColumn dataGridColumn = grid.ColumnFromDisplayIndex(displayIndex);
              if (dataGridColumn != null)
              {
                int index3 = displayIndex - num4;
                if (index3 < strArray.Length)
                  dataGridColumn.OnPastingCellClipboardContent(grid.Items[index2], (object) strArray[index3]);
              }
            }
          }
          else if (!grid.CanUserAddRows)
          {
            UIExtension.log.Warn("Stop pasting clipboard items due to limited rows. Clipboard({0}/{1}), DataGrid({2}/{3})", (object) (index1 + 1), (object) clipboardDataAsTable.Count, (object) (index2 + 1), (object) (num3 + 1));
            break;
          }
        }
      }
      catch (Exception ex)
      {
        UIExtension.log.Error(ex, ex.Message);
        ++num1;
      }
      return num1;
    }

    public static void DoEvents(this Application app)
    {
      DispatcherFrame frame = new DispatcherFrame();
      Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new DispatcherOperationCallback(UIExtension.DoEventsExitFrame), (object) frame);
      Dispatcher.PushFrame(frame);
    }

    private static object DoEventsExitFrame(object obj)
    {
      ((DispatcherFrame) obj).Continue = false;
      return (object) null;
    }

    public static bool IsThreadSafe(this DispatcherObject dispatcher) => dispatcher.Dispatcher.Thread == Thread.CurrentThread;

    public static bool IsNotThreadSafe(this DispatcherObject dispather) => !dispather.IsThreadSafe();

    public static void ThreadSafeCall(this DispatcherObject dispatcher, Action action)
    {
      try
      {
        dispatcher.Dispatcher.Invoke(action);
      }
      catch (Exception ex)
      {
        UIExtension.log.Error(ex, ex.Message);
        throw ex;
      }
    }

    public static void ThreadSafeCall<TValue>(
      this DispatcherObject dispatcher,
      Action<TValue> action,
      TValue p1)
    {
      try
      {
        dispatcher.Dispatcher.Invoke((Delegate) action, (object) p1);
      }
      catch (Exception ex)
      {
        UIExtension.log.Error(ex, ex.Message);
        throw ex;
      }
    }

    public static TReturn ThreadSafeCallFunc<TReturn>(
      this DispatcherObject dispatcher,
      Func<TReturn> func)
    {
      return dispatcher.Dispatcher.Invoke<TReturn>(func);
    }

    public static System.Windows.Media.Color ToColor(this string rrggbb)
    {
      if (string.IsNullOrWhiteSpace(rrggbb))
      {
        UIExtension.log.Warn("Invalid color(null or white).");
        return Colors.White;
      }
      string str = rrggbb.Replace("#", "");
      int num = 16711935;
      try
      {
        num = Convert.ToInt32(str, 16);
      }
      catch (Exception ex)
      {
      }
      System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) (num >> 16 & (int) byte.MaxValue), (byte) (num >> 8 & (int) byte.MaxValue), (byte) (num & (int) byte.MaxValue));
      if (str.Length > 6)
        color.A = (byte) (num >> 24 & (int) byte.MaxValue);
      return color;
    }

    public static System.Windows.Media.Color ToColor(this uint rgb)
    {
      uint num = rgb;
      return System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) (num >> 16 & (uint) byte.MaxValue), (byte) (num >> 8 & (uint) byte.MaxValue), (byte) (num & (uint) byte.MaxValue));
    }

    public static System.Windows.Media.Color ToColor(this uint rgb, int alpha)
    {
      System.Windows.Media.Color color = rgb.ToColor();
      color.A = (byte) alpha;
      return color;
    }

    public static System.Windows.Media.Brush ToBrush(this uint rgb) => (System.Windows.Media.Brush) new SolidColorBrush(rgb.ToColor());

    public static string ConvertToString(this System.Windows.Media.Color clr, string prefix = "#") => clr.A != byte.MaxValue ? string.Format("{0}{1:X2}{2:X2}{3:X2}{4:X2}", (object) prefix, (object) clr.A, (object) clr.R, (object) clr.G, (object) clr.B) : string.Format("{0}{1:X2}{2:X2}{3:X2}", (object) prefix, (object) clr.R, (object) clr.G, (object) clr.B);

    public static System.Windows.Media.Color ToComplementaryColor(this System.Windows.Media.Color clr) => System.Windows.Media.Color.FromArgb(clr.A, (byte) ((uint) byte.MaxValue - (uint) clr.R), (byte) ((uint) byte.MaxValue - (uint) clr.G), (byte) ((uint) byte.MaxValue - (uint) clr.B));

    public static System.Windows.Media.Brush ToBrush(this string rrggbb) => rrggbb.IsNullOrEmpty() ? (System.Windows.Media.Brush) null : (System.Windows.Media.Brush) new SolidColorBrush(rrggbb.ToColor());

    public static System.Windows.Media.Brush ToBrush(this System.Windows.Media.Color clr) => (System.Windows.Media.Brush) new SolidColorBrush(clr);

    public static int ToInt(this System.Windows.Media.Color color) => ((int) color.R << 16) - 16777216 + ((int) color.G << 8) + (int) color.B;

    public static uint ToUInt(this System.Windows.Media.Color color) => (uint) (((int) color.R << 16) - 16777216 + ((int) color.G << 8)) + (uint) color.B;

    public static double GetMaxRatio(this System.Windows.Media.Color curr, System.Windows.Media.Color prev, System.Windows.Media.Color next)
    {
      float num1 = (double) next.ScR - (double) prev.ScR == 0.0 ? 0.0f : (float) (((double) curr.ScR - (double) prev.ScR) / ((double) next.ScR - (double) prev.ScR));
      float num2 = (double) next.ScG - (double) prev.ScG == 0.0 ? 0.0f : (float) (((double) curr.ScG - (double) prev.ScG) / ((double) next.ScG - (double) prev.ScG));
      float num3 = (double) next.ScB - (double) prev.ScB == 0.0 ? 0.0f : (float) (((double) curr.ScB - (double) prev.ScB) / ((double) next.ScB - (double) prev.ScB));
      return (double) Math.Min(1f, Math.Max(Math.Abs(num1), Math.Max(Math.Abs(num2), Math.Abs(num3))));
    }

    public static System.Windows.Media.Color Interpolate(
      this System.Windows.Media.Color clr,
      double inter,
      System.Windows.Media.Color next)
    {
      float num1 = (float) Math.Min(1.0, Math.Max(0.0, inter));
      float num2 = 1f - num1;
      return System.Windows.Media.Color.FromScRgb((float) ((double) clr.ScA * (double) num2 + (double) next.ScA * (double) num1), (float) ((double) clr.ScR * (double) num2 + (double) next.ScR * (double) num1), (float) ((double) clr.ScG * (double) num2 + (double) next.ScG * (double) num1), (float) ((double) clr.ScB * (double) num2 + (double) next.ScB * (double) num1));
    }

    public static bool IsBetween(this System.Windows.Media.Color curr, System.Windows.Media.Color begin, System.Windows.Media.Color end) => (int) begin.R <= (int) curr.R && (int) curr.R <= (int) end.R && (int) begin.G <= (int) curr.G && (int) curr.G <= (int) end.G && (int) begin.B <= (int) curr.B && (int) curr.B <= (int) end.B;

    public static System.Windows.Media.Color GetPixelColor(
      this BitmapSource bitmap,
      int x,
      int y)
    {
      int stride = (bitmap.Format.BitsPerPixel + 7) / 8;
      byte[] numArray = new byte[stride];
      Int32Rect sourceRect = new Int32Rect(x, y, 1, 1);
      bitmap.CopyPixels(sourceRect, (Array) numArray, stride, 0);
      System.Windows.Media.Color color;
      if (bitmap.Format == PixelFormats.Pbgra32)
        color = System.Windows.Media.Color.FromArgb(numArray[3], numArray[2], numArray[1], numArray[0]);
      else if (bitmap.Format == PixelFormats.Bgr32)
        color = System.Windows.Media.Color.FromArgb(byte.MaxValue, numArray[2], numArray[1], numArray[0]);
      else if (bitmap.Format == PixelFormats.Bgra32)
        color = System.Windows.Media.Color.FromArgb(numArray[3], numArray[2], numArray[1], numArray[0]);
      else if (bitmap.Format == PixelFormats.Gray8)
      {
        color = System.Windows.Media.Color.FromArgb(byte.MaxValue, numArray[0], numArray[0], numArray[0]);
      }
      else
      {
        switch (stride)
        {
          case 1:
            color = System.Windows.Media.Color.FromArgb(byte.MaxValue, numArray[0], numArray[0], numArray[0]);
            break;
          case 2:
            color = System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) ((int) numArray[1] >> 3 & 31), (byte) ((int) numArray[1] & 56 | (int) numArray[0] >> 5), (byte) ((uint) numArray[0] & 31U));
            break;
          case 3:
            color = System.Windows.Media.Color.FromArgb(byte.MaxValue, numArray[2], numArray[1], numArray[0]);
            break;
          default:
            color = Colors.Black;
            break;
        }
      }
      return color;
    }

    public static void SetPixelColor(this WriteableBitmap bitmap, int x, int y, System.Windows.Media.Color color)
    {
      int stride = (bitmap.Format.BitsPerPixel + 7) / 8;
      byte[] numArray = new byte[stride];
      Int32Rect sourceRect = new Int32Rect(x, y, 1, 1);
      if (bitmap.Format == PixelFormats.Pbgra32)
      {
        numArray[0] = color.B;
        numArray[1] = color.G;
        numArray[2] = color.R;
        numArray[3] = color.A;
      }
      else if (bitmap.Format == PixelFormats.Bgr32)
      {
        numArray[0] = color.B;
        numArray[1] = color.G;
        numArray[2] = color.R;
        numArray[3] = byte.MaxValue;
      }
      bitmap.WritePixels(sourceRect, (Array) numArray, stride, 0);
    }

    public static T FindParent<T>(this UIElement child) where T : class
    {
      if (child == null)
        return default (T);
      DependencyObject parent = VisualTreeHelper.GetParent((DependencyObject) child);
      if (parent == null)
        return default (T);
      return parent is T obj ? obj : (parent as UIElement).FindParent<T>();
    }

    public static eDialogButtons ShowNormalMessage(
      this Window wnd,
      string title,
      string message,
      eDialogButtons btns = eDialogButtons.OK,
      int width = -1,
      int height = -1)
    {
      return wnd.IsThreadSafe() ? NotifyMessageDialog.Show(wnd, title, message, btns: btns, width: width, height: height) : wnd.ThreadSafeCallFunc<eDialogButtons>((Func<eDialogButtons>) (() => NotifyMessageDialog.Show(wnd, title, message, btns: btns, width: width, height: height)));
    }

    public static eDialogButtons ShowQuestionMessage(
      this Window wnd,
      string title,
      string message,
      eDialogButtons btns = eDialogButtons.YesNo,
      int width = -1,
      int height = -1)
    {
      return wnd.IsThreadSafe() ? NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Question, btns, width, height) : wnd.ThreadSafeCallFunc<eDialogButtons>((Func<eDialogButtons>) (() => NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Question, btns, width, height)));
    }

    public static eDialogButtons ShowWarningMessage(
      this Window wnd,
      string title,
      string message,
      eDialogButtons btns = eDialogButtons.OK,
      int width = -1,
      int height = -1)
    {
      return wnd.IsThreadSafe() ? NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Warning, btns, width, height) : wnd.ThreadSafeCallFunc<eDialogButtons>((Func<eDialogButtons>) (() => NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Warning, btns, width, height)));
    }

    public static eDialogButtons ShowErrorMessage(
      this Window wnd,
      string title,
      string message,
      eDialogButtons btns = eDialogButtons.OK,
      int width = -1,
      int height = -1)
    {
      return wnd.IsThreadSafe() ? NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Error, btns, width, height) : wnd.ThreadSafeCallFunc<eDialogButtons>((Func<eDialogButtons>) (() => NotifyMessageDialog.Show(wnd, title, message, eNotifyMessageType.Error, btns, width, height)));
    }

    public static string ShowInputLongMessage(
      this Window wnd,
      string title,
      string message,
      string input,
      int width = -1,
      int height = -1,
      eNotifyMessageType qtype = eNotifyMessageType.Question)
    {
      return InputLongMessageDialog.Show(wnd, title, message, input, qtype, width, height);
    }

    public static string ShowInputMessage(
      this Window wnd,
      string title,
      string message,
      string input = "",
      int width = -1,
      int height = -1)
    {
      return InputMessageDialog.Show(wnd, title, message, input, width: width, height: height);
    }

    public static string ShowPasswordInputMessage(
      this Window wnd,
      string title,
      string message,
      string input = "",
      int width = -1,
      int height = -1)
    {
      return InputPasswordDialog.Show(wnd, title, message, input, width: width, height: height);
    }

    public static double ShowInputNumberDialog(
      this Window wnd,
      string title,
      string message,
      double input = 0.0,
      int width = -1,
      int height = -1)
    {
      string s = InputMessageDialog.Show(wnd, title, message, input.ToString(), width: width, height: height);
      if (s != null)
      {
        double result = 0.0;
        if (double.TryParse(s, out result))
          return result;
        int num = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", "Invalid number format : " + s);
      }
      return double.NaN;
    }

    public static void Refresh(this UIElement ui) => ui.Dispatcher.Invoke(DispatcherPriority.Render, (Delegate) UIExtension.EmptyDelegate);

    public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color) => System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

    public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color color) => System.Drawing.Color.FromArgb((int) color.A, (int) color.R, (int) color.G, (int) color.B);

    public static uint ToUInt32(this System.Windows.Media.Color clr) => (uint) ((int) clr.A << 24 | (int) clr.R << 16 | (int) clr.G << 8) | (uint) clr.B;

    public static int ToInt32(this System.Windows.Media.Color clr) => (int) clr.A << 24 | (int) clr.R << 16 | (int) clr.G << 8 | (int) clr.B;

    public static string ToRRGGBBText(this System.Windows.Media.Color color, string prefix = "#") => string.Format("{0}{1:X2}{2:X2}{3:X2}", (object) prefix, (object) color.R, (object) color.G, (object) color.B);

    public static BitmapSource ToImageSource(this Icon icon) => System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

    public static BitmapSource ToImageSource(this Bitmap bmp) => System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

    public static BitmapImage ToBitmapImage(this Bitmap bmp)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        bmp.Save((Stream) memoryStream, ImageFormat.Png);
        memoryStream.Position = 0L;
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = (Stream) memoryStream;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
      }
    }

    public static BitmapSource SubImage(
      this BitmapSource src,
      int x,
      int y,
      int w,
      int h)
    {
      return (BitmapSource) new WriteableBitmap(src).Crop(x, y, w, h);
    }

    public static Bitmap ToBitmap(this BitmapSource bitmap_source)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmap_source));
        bmpBitmapEncoder.Save((Stream) memoryStream);
        return new Bitmap((Stream) memoryStream);
      }
    }

    public static Icon ToIcon(this Bitmap bmp) => Icon.FromHandle(bmp.GetHicon());

    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    private static extern IntPtr __GetActiveWindow();

    public static Window GetActiveWindow(this Application app)
    {
      if (app == null)
        return (Window) null;
      IntPtr handle = UIExtension.__GetActiveWindow();
      return app.Windows.OfType<Window>().FirstOrDefault<Window>((Func<Window, bool>) (x => new WindowInteropHelper(x).Handle == handle));
    }

    public static string OpenFileDialog(this Window wnd, string path, string filter, string ext)
    {
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.RestoreDirectory = true;
      if (!string.IsNullOrWhiteSpace(path))
      {
        if (Directory.Exists(path))
          openFileDialog.InitialDirectory = path;
        else if (File.Exists(path))
        {
          openFileDialog.InitialDirectory = Helper.IO.GetParentPath(path);
          openFileDialog.FileName = path;
        }
      }
      if (!ext.IsNullOrEmpty())
        openFileDialog.DefaultExt = ext;
      if (!filter.IsNullOrEmpty())
        openFileDialog.Filter = filter;
      bool? nullable = openFileDialog.ShowDialog(wnd);
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue ? openFileDialog.FileName : (string) null;
    }

    public static string OpenFileDialog(
      this UserControl wnd,
      string path,
      string filter,
      string ext)
    {
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.RestoreDirectory = true;
      if (!string.IsNullOrWhiteSpace(path))
      {
        if (Directory.Exists(path))
          openFileDialog.InitialDirectory = path;
        else if (File.Exists(path))
        {
          openFileDialog.InitialDirectory = Helper.IO.GetParentPath(path);
          openFileDialog.FileName = path;
        }
      }
      if (!ext.IsNullOrEmpty())
        openFileDialog.DefaultExt = ext;
      if (!filter.IsNullOrEmpty())
        openFileDialog.Filter = filter;
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue ? openFileDialog.FileName : (string) null;
    }

    public static string OpenFileDialog(
      this Window wnd,
      int depth,
      string default_path,
      string filter,
      string ext)
    {
      string str = "";
      if (depth >= 0)
      {
        StackFrame stackFrame = new StackFrame(depth + 1, true);
        if (stackFrame != null)
          str = string.Format("{0}:{1}_{2}", (object) stackFrame.GetFileName(), (object) stackFrame.GetFileLineNumber(), (object) ext);
      }
      else
        str = string.Format("ID[{0}].{1}", (object) depth, (object) ext);
      string path = str.IsNullOrEmpty() ? default_path : Helper.Windows.GetUserRegistryKey("ComDlgMRU", str, default_path);
      string text = wnd.OpenFileDialog(path, filter, ext);
      if (!text.IsNullOrEmpty() && !str.IsNullOrEmpty())
        Helper.Windows.SetUserRegistryKey("ComDlgMRU", str, text);
      return text;
    }

    public static bool OpenFileDialog(
      this Window wnd,
      TextBox filepath,
      string filter,
      string ext)
    {
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.RestoreDirectory = true;
      string path = filepath.Text.Trim();
      if (File.Exists(path))
        openFileDialog.FileName = path;
      if (ext != null)
        openFileDialog.DefaultExt = ext;
      if (filter != null)
        openFileDialog.Filter = filter;
      bool? nullable = openFileDialog.ShowDialog(wnd);
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return false;
      filepath.Text = openFileDialog.FileName;
      return true;
    }

    public static string SaveFileDialog(
      this Window wnd,
      string dir_path,
      string filter,
      string ext)
    {
      Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
      saveFileDialog.RestoreDirectory = true;
      if (!string.IsNullOrWhiteSpace(dir_path))
      {
        if (Directory.Exists(dir_path))
        {
          saveFileDialog.InitialDirectory = dir_path;
        }
        else
        {
          DirectoryInfo parent = Directory.GetParent(dir_path);
          if (parent.Exists)
          {
            saveFileDialog.InitialDirectory = parent.FullName;
            saveFileDialog.FileName = dir_path;
          }
        }
        if (dir_path != null && !dir_path.Contains<char>(':') && !dir_path.Contains<char>('/') && !dir_path.Contains<char>('\\'))
          saveFileDialog.FileName = dir_path;
      }
      if (!string.IsNullOrWhiteSpace(ext))
        saveFileDialog.DefaultExt = ext;
      if (!string.IsNullOrWhiteSpace(filter))
        saveFileDialog.Filter = filter;
      bool? nullable = saveFileDialog.ShowDialog(wnd);
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue ? saveFileDialog.FileName : (string) null;
    }

    public static string SaveFileDialog(this Window wnd, int depth, string filter, string ext)
    {
      string dir_path = "";
      string str = "";
      if (depth >= 0)
      {
        StackFrame stackFrame = new StackFrame(depth + 1, true);
        if (stackFrame != null)
          str = string.Format("{0}:{1}_{2}", (object) stackFrame.GetFileName(), (object) stackFrame.GetFileLineNumber(), (object) ext);
      }
      else
        str = string.Format("ID[{0}].{1}", (object) depth, (object) ext);
      if (!str.IsNullOrEmpty())
        dir_path = Helper.Windows.GetUserRegistryKey("ComDlgMRU", str, "");
      string text = wnd.SaveFileDialog(dir_path, filter, ext);
      if (!text.IsNullOrEmpty() && !str.IsNullOrEmpty())
        Helper.Windows.SetUserRegistryKey("ComDlgMRU", str, dir_path);
      return text;
    }
  }
}
