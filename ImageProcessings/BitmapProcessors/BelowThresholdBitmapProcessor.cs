// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.BelowThresholdBitmapProcessor
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
  [InstanceContract(ClassID = "51efbf43-22d2-4908-8be0-35f1e5853547")]
  public class BelowThresholdBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Threshold { get; set; }

    public BelowThresholdBitmapProcessor() => this.Threshold = 100;

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      double width = bmp.Width;
      double height = bmp.Height;
      SimpleRGB black = new SimpleRGB((byte) 0, (byte) 0, (byte) 0);
      return bmp.SelectEx((Func<byte, byte, byte, SimpleRGB>) ((r, g, b) => (int) b > this.Threshold ? SimpleRGB.From(r, g, b) : black));
    }

    public override string ToString() => string.Format("BelowThreshold[{0}] : Threshold({1})", (object) this.Name, (object) this.Threshold);
  }
}
