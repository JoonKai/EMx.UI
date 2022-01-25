// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.OffsetBitmapProcessor
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
  [InstanceContract(ClassID = "aeb79c32-9210-4301-85a3-819e72760e29")]
  public class OffsetBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int Offset { get; set; }

    public OffsetBitmapProcessor() => this.Offset = 128;

    public override WriteableBitmap Processing(WriteableBitmap bmp) => bmp.SelectEx((Func<byte, byte, byte, SimpleRGB>) ((r, g, b) => SimpleRGB.From(this.Offset + (int) r, this.Offset + (int) g, this.Offset + (int) b)));

    public override string ToString() => string.Format("Offset[{0}] : Value({1})", (object) this.Name, (object) this.Offset);
  }
}
