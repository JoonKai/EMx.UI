// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.ExportDiffImageBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Helpers;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "9a65c3da-1cb8-44a6-9dd8-c3e039d3ad30")]
  public class ExportDiffImageBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual string BaseDirectory { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ReturnDiffImage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool ExportImage { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Offset { get; set; }

    public ExportDiffImageBitmapProcessor()
    {
      this.BaseDirectory = "";
      this.ReturnDiffImage = false;
      this.ExportImage = false;
      this.Offset = 128;
    }

    public override WriteableBitmap Processing(WriteableBitmap bmp)
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
      WriteableBitmap bmp1 = bmp.ConvoluteEx(kernel, offset, 1f);
      try
      {
        DateTime now = DateTime.Now;
        string str = Helper.IO.CombinePath(this.BaseDirectory, string.Format("Output D{0}\\T{1}_df{2}", (object) now.ToString("yyMMdd"), (object) now.ToString("HHmmss"), (object) Helper.Text.MakeRandomCharacters(3)) + ".png");
        if (this.ExportImage)
        {
          Helper.IO.CreateParentDirectory(str);
          bmp1.SaveFileAsPng(str);
        }
      }
      catch (Exception ex)
      {
        ExportDiffImageBitmapProcessor.log.Error(ex, ex.Message);
      }
      return this.ReturnDiffImage ? bmp1 : bmp;
    }

    public override string ToString() => string.Format("Diff[{0}] : Offset({1}))", (object) this.Name, (object) this.Offset);
  }
}
