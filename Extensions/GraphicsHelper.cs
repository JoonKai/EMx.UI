// Decompiled with JetBrains decompiler
// Type: EMx.UI.Extensions.GraphicsHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Extensions
{
  public class GraphicsHelper
  {
    private static ILog log = LogManager.GetLogger();

    public WriteableBitmap LoadWriteableBitmap(string path)
    {
      Stopwatch stopwatch = Stopwatch.StartNew();
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        WriteableBitmap writeableBitmap = BitmapFactory.New(1, 1).FromStream((Stream) fileStream);
        if (writeableBitmap.Format != PixelFormats.Bgra32)
          writeableBitmap = new WriteableBitmap((BitmapSource) new FormatConvertedBitmap((BitmapSource) writeableBitmap, PixelFormats.Bgra32, (BitmapPalette) null, 0.0));
        stopwatch.Stop();
        GraphicsHelper.log.Info("Load Image : Format({0}) - {1}, Elapsed {2:#,##0}ms", (object) writeableBitmap.Format, (object) path, (object) stopwatch.ElapsedMilliseconds);
        return writeableBitmap;
      }
    }
  }
}
