// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.ColorPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Logging;
using EMx.UI.Extensions;
using EMx.UI.Windows;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  public partial class ColorPropItemHandler : System.Windows.Controls.UserControl, IPropertyGridHandler, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    internal System.Windows.Controls.Label lblMain;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual object LastSetValue { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public ColorPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type) => type == typeof (Color) || type == typeof (string);

    public virtual bool ValidateObject() => this.lblMain.Background is SolidColorBrush;

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      Type type = obj.GetType();
      if (type == typeof (Color))
      {
        this.lblMain.Background = (Brush) new SolidColorBrush((Color) obj);
      }
      else
      {
        if (!(type == typeof (string)))
          return false;
        this.lblMain.Background = (Brush) new SolidColorBrush((obj as string).ToColor());
      }
      this.LastSetValue = obj;
      return true;
    }

    public object GetObject()
    {
      Color color = (this.lblMain.Background as SolidColorBrush).Color;
      if (this.ValueType == typeof (Color))
        return (object) color;
      return this.ValueType == typeof (string) ? (object) color.ConvertToString() : (object) null;
    }

    public void ClearChangedState()
    {
    }

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => this.lblMain.Background is SolidColorBrush && !object.Equals(this.LastSetValue, this.GetObject());

    private void lblMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      Win32WindowHandle win32Handle = System.Windows.Application.Current.MainWindow.GetWin32Handle();
      ColorDialog colorDialog = new ColorDialog();
      colorDialog.Color = (this.lblMain.Background as SolidColorBrush).Color.ToDrawingColor();
      colorDialog.FullOpen = true;
      if (colorDialog.ShowDialog((IWin32Window) win32Handle) != DialogResult.OK)
        return;
      this.lblMain.Background = (Brush) new SolidColorBrush(colorDialog.Color.ToMediaColor());
      this.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent, (object) this));
    }

    private void lblMain_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Released || e.RightButton != MouseButtonState.Pressed)
        return;
      e.Handled = true;
      string rrggbb = System.Windows.Application.Current.MainWindow.ShowInputMessage("Color", "Please type the color as #RRGGBB format.", this.lblMain.Background.GetSolidColor().ConvertToString());
      if (!string.IsNullOrWhiteSpace(rrggbb))
      {
        this.lblMain.Background = rrggbb.ToColor().ToBrush();
        this.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent, (object) this));
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/colorpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.lblMain = (System.Windows.Controls.Label) target;
        this.lblMain.MouseDoubleClick += new MouseButtonEventHandler(this.lblMain_MouseDoubleClick);
        this.lblMain.MouseDown += new MouseButtonEventHandler(this.lblMain_MouseDown);
      }
      else
        this._contentLoaded = true;
    }
  }
}
