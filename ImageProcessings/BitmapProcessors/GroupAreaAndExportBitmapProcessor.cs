// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.GroupAreaAndExportBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Helpers;
using EMx.ImageProcessings;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "5bf5359d-8998-4676-abeb-58a52cb44fe6")]
  public class GroupAreaAndExportBitmapProcessor : 
    BitmapProcessorBase,
    IManagedType,
    IPixelGroupInformation
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int RangeBegin { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int RangeEnd { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string BaseDirectory { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int MinimumPixels { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int MaximumPixels { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool FillArea { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool DrawArea { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ExportText { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ExportImage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ExportOriginalImage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ExportDiffImage { get; set; }

    public virtual List<PixelGroupInformation> GroupInfos { get; set; }

    public GroupAreaAndExportBitmapProcessor()
    {
      this.RangeBegin = 200;
      this.RangeEnd = (int) byte.MaxValue;
      this.BaseDirectory = "";
      this.MinimumPixels = 3;
      this.MaximumPixels = 99999999;
      this.DrawArea = true;
      this.ExportImage = true;
      this.ExportText = true;
      this.ExportOriginalImage = true;
      this.GroupInfos = new List<PixelGroupInformation>();
    }

    public virtual List<PixelGroupInformation> GetPixelGroups() => this.GroupInfos;

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      double width = bmp.Width;
      double height = bmp.Height;
      PixelGroupingProcess pixelGroupingProcess = new PixelGroupingProcess();
      int[,] mask = bmp.To2dArray<int>((Func<int, int>) (x => x & (int) byte.MaxValue));
      List<PixelGroupInformation> source = pixelGroupingProcess.Grouping(mask, this.RangeBegin, this.RangeEnd);
      this.GroupInfos.Clear();
      this.GroupInfos.AddRange((IEnumerable<PixelGroupInformation>) source);
      List<PixelGroupInformation> list = source.Where<PixelGroupInformation>((Func<PixelGroupInformation, bool>) (x =>
      {
        if (x.PixelCount < this.MinimumPixels)
          return false;
        return x.PixelCount <= this.MaximumPixels || x.PixelCount == -1;
      })).ToList<PixelGroupInformation>();
      int line_color = -65536;
      WriteableBitmap bmp1 = bmp.Clone();
      if (this.FillArea)
      {
        foreach (PixelGroupInformation groupInformation in list)
        {
          PixelGroupInformation area = groupInformation;
          bmp1.ForLoop(area.X, area.Y, area.Width, area.Height, (Func<int, int, int, int>) ((x, y, val) => mask[y, x] != area.Mask ? val : line_color));
        }
      }
      if (this.DrawArea)
      {
        foreach (PixelGroupInformation groupInformation in list)
          bmp1.DrawRectangle(groupInformation.X, groupInformation.Y, groupInformation.X + groupInformation.Width - 1, groupInformation.Y + groupInformation.Height - 1, line_color);
      }
      try
      {
        DateTime now = DateTime.Now;
        string str1 = string.Format("Output D{0}\\T{1}_{2}", (object) now.ToString("yyMMdd"), (object) now.ToString("HHmmss"), (object) Helper.Text.MakeRandomCharacters(5));
        string str2 = Helper.IO.CombinePath(this.BaseDirectory, str1 + ".png");
        string str3 = Helper.IO.CombinePath(this.BaseDirectory, str1 + "_origin.png");
        string path = Helper.IO.CombinePath(this.BaseDirectory, str1 + "_diff.png");
        string str4 = Helper.IO.CombinePath(this.BaseDirectory, str1 + ".txt");
        if (this.ExportText)
        {
          StringBuilder sb = new StringBuilder();
          sb.AppendLine(PixelGroupInformation.GetFieldHeaders());
          list.ForEach((Action<PixelGroupInformation>) (x => sb.AppendLine(x.ToString())));
          Helper.IO.CreateParentDirectory(str4);
          File.WriteAllText(str4, sb.ToString());
        }
        if (this.ExportImage)
        {
          Helper.IO.CreateParentDirectory(str2);
          bmp1.SaveFileAsPng(str2);
        }
        if (this.ExportOriginalImage)
        {
          Helper.IO.CreateParentDirectory(str3);
          bmp.SaveFileAsPng(str3);
        }
        if (this.ExportDiffImage)
        {
          int[,] kernel = new int[5, 5]
          {
            {
              0,
              0,
              0,
              0,
              0
            },
            {
              0,
              0,
              0,
              0,
              0
            },
            {
              -1,
              0,
              6,
              0,
              0
            },
            {
              -1,
              -1,
              0,
              0,
              0
            },
            {
              -1,
              -1,
              -1,
              0,
              0
            }
          };
          byte offset = 128;
          bmp.ConvoluteEx(kernel, offset, 1f).SaveFileAsPng(path);
        }
      }
      catch (Exception ex)
      {
        GroupAreaAndExportBitmapProcessor.log.Error(ex, ex.Message);
      }
      return bmp1;
    }

    public override string ToString() => string.Format("GroupAreaAndExport[{0}] : Range[{1} - {2}])", (object) this.Name, (object) this.RangeBegin, (object) this.RangeEnd);
  }
}
