// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.ConvertMapToImage
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Equipments.ProcessedData;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.ColorConverters;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Maps
{
  [InstanceContract(ClassID = "5ea6b079-5125-48ee-aa64-2523b18d08d7")]
  public class ConvertMapToImage : IManagedType
  {
    private static ILog log = LogManager.GetLogger();
    protected int Cols;
    protected int Rows;
    protected int RadiusCount;
    public Func<object, double> Getter;
    protected IMapData LastSetMapData;

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int PixelWidth { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int PixelHeight { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string DisplayItemName { get; set; }

    [DesignableMember(true)]
    public IColorConverter ColorConveter { get; set; }

    [DesignableMember(true)]
    public virtual MapRange Range { get; set; }

    [InstanceMember]
    public virtual Color BackgroundColor { get; set; }

    protected virtual WriteableBitmap WBitmap { get; set; }

    protected virtual int LastSetPixelWidth { get; set; }

    protected virtual int LastSetPixelHeight { get; set; }

    protected virtual string LastSetDisplayItemName { get; set; }

    public ConvertMapToImage()
    {
      this.PixelWidth = 1;
      this.PixelHeight = 1;
      this.LastSetDisplayItemName = (string) null;
      this.DisplayItemName = (string) null;
      this.BackgroundColor = Colors.White;
      this.Getter = (Func<object, double>) null;
      this.Range = new MapRange();
      this.ColorConveter = (IColorConverter) new PredefinedColorsGradientConverter();
    }

    public void EmptyImage()
    {
      this.WBitmap = (WriteableBitmap) null;
      this.LastSetPixelWidth = 0;
      this.LastSetPixelHeight = 0;
    }

    public virtual void UpdateRange(List<MapItemData> map_items)
    {
      List<double> items = new List<double>();
      foreach (MapItemData mapItem in map_items)
      {
        double num = this.Getter(mapItem.Value);
        items.Add(num);
      }
      int distributions = this.ColorConveter.GetColors().Count<Color>();
      lock (this.Range)
        this.Range.Distribute(distributions, items);
    }

    public virtual List<double> UpdateRanges(List<MapData<double>> maps)
    {
      List<double> items = new List<double>();
      foreach (MapData<double> map in maps)
      {
        IEnumerable<double> collection = map.ToListT().Select<MapItemData<double>, double>((Func<MapItemData<double>, double>) (x => x.Value));
        items.AddRange(collection);
      }
      int distributions = this.ColorConveter.GetColors().Count<Color>();
      lock (this.Range)
        this.Range.Distribute(distributions, items);
      return items;
    }

    public virtual void UpdateRange(List<double> values)
    {
      int distributions = this.ColorConveter.GetColors().Count<Color>();
      lock (this.Range)
        this.Range.Distribute(distributions, values);
    }

    public virtual unsafe void UpdateImage(IMapData map)
    {
      if (map == null)
        return;
      if (this.LastSetPixelWidth != this.PixelWidth || this.LastSetPixelHeight != this.PixelHeight || this.Cols != map.ColCapacity || this.Rows != map.RowCapacity || this.WBitmap == null)
      {
        this.LastSetPixelWidth = this.PixelWidth;
        this.LastSetPixelHeight = this.PixelHeight;
        this.RadiusCount = Math.Max(map.RowCapacity, map.ColCapacity) / 2;
        this.Cols = map.ColCapacity;
        this.Rows = map.RowCapacity;
        if (this.Cols <= 0 || this.Rows <= 0)
        {
          this.WBitmap = new WriteableBitmap(1, 1, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
          this.WBitmap.SetPixel(0, 0, this.BackgroundColor);
          return;
        }
        this.WBitmap = new WriteableBitmap(this.Cols * this.PixelWidth, this.Rows * this.PixelHeight, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
      }
      if (this.WBitmap == null)
        return;
      if (this.LastSetDisplayItemName != this.DisplayItemName || this.Getter == null || this.LastSetMapData != map)
      {
        if (string.IsNullOrWhiteSpace(this.DisplayItemName))
        {
          this.Getter = (Func<object, double>) (o => (double) o);
        }
        else
        {
          PropertyInfo prop = map.ItemType.GetProperty(this.DisplayItemName);
          if (prop != (PropertyInfo) null)
            this.Getter = (Func<object, double>) (o => (double) prop.GetValue(o));
        }
        if (this.Getter != null)
        {
          this.LastSetMapData = map;
          this.LastSetDisplayItemName = this.DisplayItemName;
        }
      }
      if (this.Getter == null)
        return;
      if (this.WBitmap != null)
      {
        lock (this.WBitmap)
        {
          bool flag = false;
          try
          {
            if (flag = this.WBitmap.TryLock((Duration) new TimeSpan(0, 0, 1)))
            {
              uint uint32 = this.BackgroundColor.ToUInt32();
              uint* pointer = (uint*) this.WBitmap.BackBuffer.ToPointer();
              int num = this.Cols * this.PixelWidth * this.Rows * this.PixelHeight;
              for (int index = 0; index < num; ++index)
                pointer[index] = uint32;
              this.WBitmap.AddDirtyRect(new Int32Rect(0, 0, this.Cols * this.PixelWidth, this.Rows * this.PixelHeight));
            }
          }
          finally
          {
            if (flag)
              this.WBitmap.Unlock();
          }
        }
      }
      List<MapItemData> list = map.ToList();
      if (this.Range.UseAutoRange || this.Range.UseDistribution)
        this.UpdateRange(list);
      double begin = this.Range.Begin;
      double end = this.Range.End;
      lock (this.WBitmap)
      {
        bool flag = false;
        try
        {
          if (!(flag = this.WBitmap.TryLock((Duration) new TimeSpan(0, 0, 0, 1))))
            return;
          if (this.PixelWidth == 1 && this.PixelHeight == 1)
          {
            int* pointer = (int*) this.WBitmap.BackBuffer.ToPointer();
            foreach (MapItemData mapItemData in list)
            {
              double num1 = this.Getter(mapItemData.Value);
              Color color = this.ColorConveter.ValueToColor(begin, end, num1);
              int num2 = (int) color.A << 24 | (int) color.R << 16 | (int) color.G << 8 | (int) color.B;
              int index = this.RadiusCount + mapItemData.X;
              int num3 = this.RadiusCount - mapItemData.Y;
              (pointer + num3 * this.Cols)[index] = num2;
            }
          }
          else
          {
            foreach (MapItemData mapItemData in list)
            {
              double num = this.Getter(mapItemData.Value);
              Color color = this.ColorConveter.ValueToColor(begin, end, num);
              this.WBitmap.SetPixel(this.RadiusCount + mapItemData.X, this.RadiusCount - mapItemData.Y, color);
            }
          }
          this.WBitmap.AddDirtyRect(new Int32Rect(0, 0, this.Cols * this.PixelWidth, this.Rows * this.PixelHeight));
        }
        catch (Exception ex)
        {
          ConvertMapToImage.log.Error(ex, ex.Message);
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
