// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.CropBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "6ada731d-8178-4a16-8ae1-7f33321b60e2")]
  public class CropBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual bool UseCenterCrop { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int X { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Y { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Width { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Height { get; set; }

    public CropBitmapProcessor()
    {
      this.UseCenterCrop = true;
      this.Width = 1000;
      this.Height = 1000;
    }

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      double width = bmp.Width;
      double height = bmp.Height;
      if (this.UseCenterCrop)
      {
        this.X = (int) Math.Max(0.0, (width - (double) this.Width) / 2.0);
        this.Y = (int) Math.Max(0.0, (height - (double) this.Height) / 2.0);
      }
      return bmp.Crop(this.X, this.Y, this.Width, this.Height);
    }

    public override string ToString()
    {
      if (this.UseCenterCrop)
        return string.Format("Crop[{0}] : Centerized W{1}, H{2}", (object) this.Name, (object) this.Width, (object) this.Height);
      return string.Format("Crop[{0}] : {1},{2},{3},{4}", (object) this.Name, (object) this.X, (object) this.Y, (object) this.Width, (object) this.Height);
    }
  }
}
