// Decompiled with JetBrains decompiler
// Type: EMx.UI.ImageProcessings.BitmapProcessors.MedianBitmapProcessor
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.ImageProcessings.BitmapProcessors;
using EMx.Logging;
using EMx.Serialization;
using System.Windows.Media.Imaging;

namespace EMx.UI.ImageProcessings.BitmapProcessors
{
  [InstanceContract(ClassID = "52078be6-37c4-4707-b201-b9c1bea8b919")]
  public class MedianBitmapProcessor : BitmapProcessorBase, IManagedType
  {
    private static ILog log = LogManager.GetLogger();

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int MedianSize { get; set; }

    public MedianBitmapProcessor() => this.MedianSize = 3;

    public override WriteableBitmap Processing(WriteableBitmap bmp)
    {
      double width = bmp.Width;
      double height = bmp.Height;
      return bmp;
    }

    public override string ToString() => string.Format("Median[{0}] : Size({1})", (object) this.Name, (object) this.MedianSize);
  }
}
