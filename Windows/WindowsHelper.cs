// Decompiled with JetBrains decompiler
// Type: EMx.UI.Windows.WindowsHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using EMx.UI.Windows.Screens;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace EMx.UI.Windows
{
  public class WindowsHelper
  {
    private static ILog log = LogManager.GetLogger();

    public eWindowsTaskbarLocation GetWindowsTaskbarLocation(int index = 0)
    {
      Screen[] allScreens = Screen.AllScreens;
      if (index > -1 && index < allScreens.Length)
      {
        Screen screen = allScreens[index];
        if (screen.Bounds.Equals((object) screen.WorkingArea))
          return eWindowsTaskbarLocation.None;
        if (screen.Bounds.Left != screen.WorkingArea.Left)
          return eWindowsTaskbarLocation.Left;
        if (screen.Bounds.Top != screen.WorkingArea.Top)
          return eWindowsTaskbarLocation.Top;
        if (screen.Bounds.Width != screen.WorkingArea.Width)
          return eWindowsTaskbarLocation.Right;
        if (screen.Bounds.Height != screen.WorkingArea.Height)
          return eWindowsTaskbarLocation.Bottom;
      }
      return eWindowsTaskbarLocation.None;
    }

    public int GetScreenIndexOfWindow(int left, int top)
    {
      Screen[] allScreens = Screen.AllScreens;
      for (int index = 0; index < allScreens.Length; ++index)
      {
        if (allScreens[index].Bounds.Contains(left, top))
          return index;
      }
      return -1;
    }

    public Rect GetScreenArea(int index, eWindowDockingPosition pos)
    {
      Screen[] allScreens = Screen.AllScreens;
      if (index > -1 && index < allScreens.Length)
      {
        Rectangle workingArea = allScreens[index].WorkingArea;
        int num1 = (workingArea.Left + workingArea.Width) / 2;
        int num2 = (workingArea.Top + workingArea.Height) / 2;
        int num3 = workingArea.Width / 2;
        int num4 = workingArea.Height / 2;
        switch (pos)
        {
          case eWindowDockingPosition.Entire:
            return new Rect((double) workingArea.Left, (double) workingArea.Top, (double) workingArea.Width, (double) workingArea.Height);
          case eWindowDockingPosition.DualLeft:
            return new Rect((double) workingArea.Left, (double) workingArea.Top, (double) num3, (double) workingArea.Height);
          case eWindowDockingPosition.DualRight:
            return new Rect((double) (workingArea.Left + num3), (double) workingArea.Top, (double) num3, (double) workingArea.Height);
          case eWindowDockingPosition.QuadRightTop:
            return new Rect((double) (workingArea.Left + num3), (double) workingArea.Top, (double) num3, (double) num4);
          case eWindowDockingPosition.QuadLeftTop:
            return new Rect((double) workingArea.Left, (double) workingArea.Top, (double) num3, (double) num4);
          case eWindowDockingPosition.QuadLeftBottom:
            return new Rect((double) workingArea.Left, (double) (workingArea.Top + num4), (double) num3, (double) num4);
          case eWindowDockingPosition.QuadRightBottom:
            return new Rect((double) (workingArea.Left + num3), (double) (workingArea.Top + num4), (double) num3, (double) num4);
        }
      }
      return Rect.Empty;
    }

    public Rect GetScreenArea(int x, int y, eWindowDockingPosition pos) => this.GetScreenArea(this.GetScreenIndexOfWindow(x, y), pos);

    public Rect ChangeScreenArea(Window wnd, eWindowDockingPosition pos)
    {
      Rect screenArea = this.GetScreenArea((int) wnd.Left, (int) wnd.Top, pos);
      wnd.Left = screenArea.Left;
      wnd.Top = screenArea.Top;
      wnd.Width = screenArea.Width;
      wnd.Height = screenArea.Height;
      return screenArea;
    }
  }
}
