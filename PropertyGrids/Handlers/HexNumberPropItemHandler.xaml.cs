// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.HexNumberPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Helpers;
using EMx.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  public partial class HexNumberPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private string SettedText;
    internal TextBox TextCtrl;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public HexNumberPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type) => Helper.Data.IsNumber(type);

    public virtual bool ValidateObject()
    {
      bool flag = true;
      try
      {
        Helper.Text.FromHex(this.TextCtrl.Text.Trim());
      }
      catch (Exception ex)
      {
        HexNumberPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        flag = false;
      }
      this.Background = flag ? (Brush) Brushes.Red : (Brush) Brushes.White;
      return flag;
    }

    public virtual bool SetObject(object obj)
    {
      if (!Helper.Data.IsNumber(obj))
        return false;
      this.TextCtrl.Text = Helper.Text.ToHex(Convert.ToInt32(obj));
      this.ClearChangedState();
      return true;
    }

    public object GetObject()
    {
      try
      {
        return (object) Helper.Text.FromHex(this.TextCtrl.Text.Trim());
      }
      catch (Exception ex)
      {
        HexNumberPropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
        return (object) null;
      }
    }

    public void ClearChangedState() => this.SettedText = this.TextCtrl.Text;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !string.Equals(this.TextCtrl.Text, this.SettedText);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/hexnumberpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.TextCtrl = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
