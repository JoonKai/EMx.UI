// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.InverseBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "cb0b0dea-de3a-46ba-a6c7-27a4be383b7a")]
  public class InverseBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    public override WriteableBitmap Processing(WriteableBitmap bmp) => bmp.SelectEx((Func<byte, byte, byte, SimpleRGB>) ((r, g, b) => SimpleRGB.From((int) byte.MaxValue - (int) r, (int) byte.MaxValue - (int) g, (int) byte.MaxValue - (int) b)));

    public override string ToString() => string.Format("Inverse[{0}]", (object) this.Name);
  }
}
