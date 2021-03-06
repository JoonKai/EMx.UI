// Decompiled with JetBrains decompiler
// Type: EMx.UI.UIHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using EMx.UI.Extensions;
using EMx.UI.Windows;

namespace EMx.UI
{
  public static class UIHelper
  {
    private static ILog log = LogManager.GetLogger();

    public static WindowsHelper Windows { get; private set; }

    public static GraphicsHelper Graphics { get; private set; }

    static UIHelper()
    {
      UIHelper.Windows = new WindowsHelper();
      UIHelper.Graphics = new GraphicsHelper();
    }
  }
}
