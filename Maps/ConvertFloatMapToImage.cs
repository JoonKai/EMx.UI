// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.ConvertFloatMapToImage
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.FloatMaps;
using EMx.Logging;
using EMx.Maths;
using EMx.UI.ColorConverters;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Maps
{
  public class ConvertFloatMapToImage
  {
    private static ILog log = LogManager.GetLogger();
    public Func<FloatMapItem<double>, double> Getter;

    public virtual string DisplayItemName { get; set; }

    public NumericSize ImageSize { get; set; }

    public virtual double MapSizeRatio { get; set; }

    public virtual System.Windows.Media.Color WaferOuterLineColor { get; set; }

    public virtual double WaferOuterLineThickness { get; set; }

    public virtual string DataFormat { get; set; }

    public virtual double FontSize { get; set; }

    protected virtual WriteableBitmap WBitmap { get; set; }

    protected virtual string LastSetDisplayItemName { get; set; }

    public PredefinedColorsGradientConverter ColorConveter { get; set; }

    public ConvertFloatMapToImage()
    {
      this.ColorConveter = new PredefinedColorsGradientConverter();
      this.ImageSize = new NumericSize(300.0, 300.0);
      this.WaferOuterLineColor = "87CEFA".ToColor();
      this.WaferOuterLineThickness = 1.0;
      this.DisplayItemName = "";
      this.Getter = (Func<FloatMapItem<double>, double>) null;
      this.MapSizeRatio = 0.8;
      this.DataFormat = "#,##0.00";
      this.FontSize = 10.0;
    }

    public virtual unsafe void UpdateImage(PersistFloatMap<double> map)
    {
      if (map == null)
        return;
      if (this.WBitmap == null || this.ImageSize.Width != (double) this.WBitmap.PixelWidth || this.ImageSize.Height != (double) this.WBitmap.PixelHeight)
        this.WBitmap = new WriteableBitmap((int) this.ImageSize.Width, (int) this.ImageSize.Height, 96.0, 96.0, PixelFormats.Bgr32, (BitmapPalette) null);
      if (this.WBitmap == null)
        return;
      lock (this.WBitmap)
      {
        bool flag = false;
        try
        {
          if (flag = this.WBitmap.TryLock((Duration) new TimeSpan(0, 0, 1)))
          {
            uint num1 = 16777215;
            uint* pointer = (uint*) this.WBitmap.BackBuffer.ToPointer();
            int num2 = this.WBitmap.PixelWidth * this.WBitmap.PixelHeight;
            for (int index = 0; index < num2; ++index)
              pointer[index] = num1;
            this.WBitmap.AddDirtyRect(new Int32Rect(0, 0, this.WBitmap.PixelWidth, this.WBitmap.PixelHeight));
          }
        }
        finally
        {
          if (flag)
            this.WBitmap.Unlock();
        }
      }
      if (this.LastSetDisplayItemName != this.DisplayItemName || this.Getter == null)
      {
        if (string.IsNullOrWhiteSpace(this.DisplayItemName))
        {
          this.Getter = (Func<FloatMapItem<double>, double>) (o => o.Value1);
        }
        else
        {
          int channel_idx = map.ChannelNames.FindIndex((Predicate<string>) (x => string.Equals(x, this.DisplayItemName, StringComparison.CurrentCultureIgnoreCase)));
          if (channel_idx != -1)
            this.Getter = (Func<FloatMapItem<double>, double>) (o => channel_idx >= o.Items.Length ? 0.0 : o.Items[channel_idx]);
        }
        if (this.Getter != null)
          this.LastSetDisplayItemName = this.DisplayItemName;
      }
      if (this.Getter == null)
        return;
      List<FloatMapItem<double>> source = new List<FloatMapItem<double>>((IEnumerable<FloatMapItem<double>>) map.Items);
      double end = 1.0;
      double begin = 0.0;
      if (source.Count > 0)
      {
        end = source.Max<FloatMapItem<double>>((Func<FloatMapItem<double>, double>) (x => this.Getter(x)));
        begin = source.Min<FloatMapItem<double>>((Func<FloatMapItem<double>, double>) (x => this.Getter(x)));
      }
      lock (this.WBitmap)
      {
        bool flag = false;
        try
        {
          int val1 = this.WBitmap.PixelWidth / 2;
          int val2 = this.WBitmap.PixelHeight / 2;
          double num3 = (double) Math.Min(val1, val2) * this.MapSizeRatio;
          double num4 = num3;
          string format1 = "{0:" + this.DataFormat + "}";
          lock (map)
          {
            WriteableBitmap wbitmap = this.WBitmap;
            if (!(flag = this.WBitmap.TryLock((Duration) new TimeSpan(0, 0, 1))))
              return;
            Font font = new Font("tahoma", (float) this.FontSize);
            StringFormat format2 = new StringFormat();
            format2.Alignment = StringAlignment.Center;
            format2.LineAlignment = StringAlignment.Center;
            using (Bitmap bitmap = new Bitmap(wbitmap.PixelWidth, wbitmap.PixelHeight, wbitmap.BackBufferStride, System.Drawing.Imaging.PixelFormat.Format32bppRgb, wbitmap.BackBuffer))
            {
              using (Graphics graphics = Graphics.FromImage((Image) bitmap))
              {
                System.Drawing.Pen pen = new System.Drawing.Pen(this.WaferOuterLineColor.ToDrawingColor(), (float) this.WaferOuterLineThickness);
                graphics.DrawEllipse(pen, (float) val1 - (float) num3, (float) val2 - (float) num4, (float) (num3 * 2.0), (float) (num4 * 2.0));
                for (int index = 0; index < map.Count; ++index)
                {
                  FloatMapItem<double> floatMapItem = map.Items[index];
                  double num5 = this.Getter(floatMapItem);
                  double num6 = (double) val1 + ((double) floatMapItem.X - (double) map.Header.CenterX) / map.Header.MaxRadius * num3;
                  double num7 = (double) val2 - ((double) floatMapItem.Y - (double) map.Header.CenterY) / map.Header.MaxRadius * num4;
                  string s = string.Format(format1, (object) num5);
                  SolidBrush solidBrush = new SolidBrush(this.ColorConveter.ValueToColor(begin, end, num5).ToDrawingColor());
                  graphics.DrawString(s, font, (System.Drawing.Brush) solidBrush, (float) num6, (float) num7, format2);
                }
              }
            }
            wbitmap.AddDirtyRect(new Int32Rect(0, 0, wbitmap.PixelWidth, wbitmap.PixelHeight));
          }
        }
        catch (Exception ex)
        {
          ConvertFloatMapToImage.log.Error(ex, ex.Message);
        }
        finally
        {
          if (flag)
            this.WBitmap.Unlock();
        }
      }
    }

    public virtual BitmapSource GetImageSource() => (BitmapSource) this.WBitmap;
  }
}
