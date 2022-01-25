// Decompiled with JetBrains decompiler
// Type: EMx.UI.Windows.FullScreenHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace EMx.UI.Windows
{
  public class FullScreenHelper
  {
    public bool IsFullScreenMode { get; protected set; }

    private WindowState LastWindowState { get; set; }

    private Rect LastWindowPosition { get; set; }

    private Window SettedWindow { get; set; }

    private HwndSource LastUsedHWndSource { get; set; }

    public FullScreenHelper(Window wnd)
    {
      Trace.Assert(wnd != null);
      this.LastWindowPosition = new Rect();
      this.SettedWindow = wnd;
    }

    public virtual bool ToFullScreen()
    {
      Window settedWindow = this.SettedWindow;
      if (settedWindow == null || this.IsFullScreenMode)
        return false;
      this.LastWindowState = settedWindow.WindowState;
      this.LastWindowPosition = new Rect(settedWindow.Left, settedWindow.Top, settedWindow.Width, settedWindow.Height);
      IntPtr handle = new WindowInteropHelper(settedWindow).Handle;
      Screen screen = Screen.FromHandle(handle);
      settedWindow.WindowState = WindowState.Normal;
      settedWindow.WindowStyle = WindowStyle.None;
      settedWindow.ResizeMode = ResizeMode.NoResize;
      settedWindow.Topmost = true;
      settedWindow.Top = 0.0;
      settedWindow.Left = 0.0;
      settedWindow.Width = (double) screen.Bounds.Width;
      settedWindow.Height = (double) screen.Bounds.Height;
      if (this.LastUsedHWndSource == null)
      {
        this.LastUsedHWndSource = HwndSource.FromHwnd(handle);
        this.LastUsedHWndSource.AddHook(new HwndSourceHook(this.WndProc));
      }
      this.IsFullScreenMode = true;
      return true;
    }

    public virtual bool ToNormalScreen()
    {
      Window settedWindow = this.SettedWindow;
      if (settedWindow == null || !this.IsFullScreenMode)
        return false;
      settedWindow.WindowStyle = WindowStyle.SingleBorderWindow;
      settedWindow.WindowState = this.LastWindowState;
      settedWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
      settedWindow.Topmost = false;
      Window window1 = settedWindow;
      Rect lastWindowPosition = this.LastWindowPosition;
      double left = lastWindowPosition.Left;
      window1.Left = left;
      Window window2 = settedWindow;
      lastWindowPosition = this.LastWindowPosition;
      double top = lastWindowPosition.Top;
      window2.Top = top;
      Window window3 = settedWindow;
      lastWindowPosition = this.LastWindowPosition;
      double width = lastWindowPosition.Width;
      window3.Width = width;
      Window window4 = settedWindow;
      lastWindowPosition = this.LastWindowPosition;
      double height = lastWindowPosition.Height;
      window4.Height = height;
      if (this.LastUsedHWndSource != null)
      {
        this.LastUsedHWndSource.RemoveHook(new HwndSourceHook(this.WndProc));
        this.LastUsedHWndSource = (HwndSource) null;
      }
      this.IsFullScreenMode = false;
      return true;
    }

    public bool AutoChangeWindowState() => this.IsFullScreenMode ? this.ToNormalScreen() : this.ToFullScreen();

    private IntPtr WndProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      if (msg == 274 && (wParam.ToInt32() & 65520) == 61456)
        handled = true;
      return IntPtr.Zero;
    }
  }
}
