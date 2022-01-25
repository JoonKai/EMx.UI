// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.BinarizationBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "1dc7e09f-54ca-4d13-add2-3bd45e7eb2c5")]
  public class BinarizationBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Threshold { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int UpperValue { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int LowerValue { get; set; }

    public BinarizationBitmapProcessor()
    {
      this.Threshold = 128;
      this.UpperValue = (int) byte.MaxValue;
      this.LowerValue = 0;
    }

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      SimpleRGB upper = SimpleRGB.From(this.UpperValue, this.UpperValue, this.UpperValue);
      SimpleRGB lower = SimpleRGB.From(this.LowerValue, this.LowerValue, this.LowerValue);
      return bmp.SelectEx((Func<byte, byte, byte, SimpleRGB>) ((r, g, b) => (int) b < this.Threshold ? lower : upper));
    }

    public override string ToString() => string.Format("Binarization[{0}] : Threshold({1:0.00})", (object) this.Name, (object) this.Threshold);
  }
}
