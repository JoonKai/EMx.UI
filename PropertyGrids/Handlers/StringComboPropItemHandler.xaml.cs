// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.StringComboPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "ce761d1e-3374-4a1e-b15f-fa5562bf583e")]
  public partial class StringComboPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private string LastSelectedText;
    internal ComboBox ComboCtrl;
    private bool _contentLoaded;

    public virtual List<string> Items
    {
      get => this.ComboCtrl.ItemsSource as List<string>;
      set => this.ComboCtrl.ItemsSource = (IEnumerable) value;
    }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public string HandlerParameter { get; set; }

    public Type ValueType { get; set; }

    public StringComboPropItemHandler()
    {
      this.InitializeComponent();
      this.LastSelectedText = "";
    }

    public virtual bool IsSupportedType(Type type) => type.Equals(typeof (string));

    public virtual bool ValidateObject() => true;

    public virtual bool SetObject(object obj)
    {
      this.ComboCtrl.Text = (obj ?? (object) "").ToString();
      return true;
    }

    public object GetObject() => (object) this.ComboCtrl.Text;

    public void ClearChangedState() => this.LastSelectedText = this.ComboCtrl.Text;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !string.Equals(this.LastSelectedText, this.ComboCtrl.Text);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/stringcombopropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.ComboCtrl = (ComboBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
