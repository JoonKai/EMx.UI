// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.ThicknessTypePropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PropertyGrids.Handlers
{
  [PropertyGridDefaultHandler(typeof (Thickness))]
  [InstanceContract(ClassID = "1e52de0c-b5d4-4a2c-beaa-6e75635cb144")]
  public partial class ThicknessTypePropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private string SettedText;
    private object SettedObject;
    internal TextBox TextCtrl;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    protected ThicknessConverter Conv { get; set; }

    public ThicknessTypePropItemHandler()
    {
      this.InitializeComponent();
      this.Conv = new ThicknessConverter();
    }

    public virtual bool IsSupportedType(Type type) => type != (Type) null && (type.Equals(typeof (Thickness)) || type.Equals(typeof (string)));

    public virtual bool ValidateObject()
    {
      try
      {
        Thickness thickness = (Thickness) this.Conv.ConvertFromString(this.TextCtrl.Text);
        this.Background = (Brush) Brushes.White;
        return true;
      }
      catch (Exception ex)
      {
        ThicknessTypePropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        this.Background = (Brush) Brushes.Red;
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      this.SettedObject = obj;
      this.TextCtrl.Text = obj.ToString();
      this.ClearChangedState();
      return true;
    }

    public object GetObject()
    {
      try
      {
        string text = this.TextCtrl.Text;
        if (this.ValueType.Equals(typeof (Thickness)))
          return this.Conv.ConvertFromString(text);
        if (this.ValueType.Equals(typeof (string)))
          return (object) text;
        ThicknessTypePropItemHandler.log.Error("Not impleted type : {0}", (object) Helper.Assem.GetGenericTypeName(this.ValueType));
        return this.SettedObject;
      }
      catch (Exception ex)
      {
        ThicknessTypePropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
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
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/thicknesstypepropitemhandler.xaml", UriKind.Relative));
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
