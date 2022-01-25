// Decompiled with JetBrains decompiler
// Type: EMx.UI.Windows.WindowsHotKeyHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace EMx.UI.Windows
{
  public class WindowsHotKeyHelper : IDisposable
  {
    protected const int WM_HOTKEY = 786;
    protected Window WindowHandle;
    protected WindowInteropHelper WindowInterop;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    protected static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    protected List<WindowsHotKeyData> HotKeyData { get; set; }

    protected static int RegisterID { get; set; }

    public WindowsHotKeyHelper(Window wnd = null)
    {
      if (wnd == null)
        wnd = Application.Current.MainWindow;
      this.HotKeyData = new List<WindowsHotKeyData>();
      this.WindowHandle = wnd;
      this.WindowInterop = new WindowInteropHelper(this.WindowHandle);
      this.WindowInterop.EnsureHandle();
      ComponentDispatcher.ThreadPreprocessMessage += new ThreadMessageEventHandler(this.OnThreadPreprocessMessage);
    }

    protected virtual void OnThreadPreprocessMessage(ref MSG msg, ref bool handled)
    {
      if (msg.message != 786)
        return;
      lock (this)
      {
        foreach (WindowsHotKeyData windowsHotKeyData in this.HotKeyData)
        {
          if (windowsHotKeyData.ConvertToLParam() == msg.lParam.ToInt32() && windowsHotKeyData.Enabled && windowsHotKeyData.Handler != null)
          {
            WindowsHotKeyArguments args = new WindowsHotKeyArguments(windowsHotKeyData.ModifierKey, windowsHotKeyData.Key);
            windowsHotKeyData.Handler(this, args);
            if (args.IsHandled)
            {
              handled = true;
              break;
            }
          }
        }
      }
    }

    public virtual void Register(eHotKeyModifier modifier, Key key, WindowsHotKeyHandler handler)
    {
      if (handler == null)
        return;
      lock (this)
      {
        WindowsHotKeyData hkey = new WindowsHotKeyData(modifier, key, handler, ++WindowsHotKeyHelper.RegisterID);
        this.InternalRegisterHotKey(hkey);
        this.HotKeyData.Add(hkey);
      }
    }

    public virtual void UnregisterAll() => this.Dispose();

    protected virtual void InternalRegisterHotKey(WindowsHotKeyData hkey) => WindowsHotKeyHelper.RegisterHotKey(this.WindowInterop.Handle, hkey.RegisteredID, (int) hkey.ModifierKey, hkey.ConvertToVitualKey());

    protected virtual void InternalUnregisterHotKey(WindowsHotKeyData hkey) => WindowsHotKeyHelper.UnregisterHotKey(this.WindowInterop.Handle, hkey.RegisteredID);

    public virtual void Dispose() => this.ClearAllRegisteredHotKeys();

    public virtual void ClearAllRegisteredHotKeys()
    {
      lock (this)
      {
        for (int index = 0; index < this.HotKeyData.Count; ++index)
          this.InternalUnregisterHotKey(this.HotKeyData[index]);
        this.HotKeyData.Clear();
      }
    }
  }
}
