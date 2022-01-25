// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.PropertyGridPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Engine;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "0de8613d-644f-4029-812d-9bc4b727db8a")]
  public partial class PropertyGridPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IManagedType,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private object SelectedObject;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public PropertyGridPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type) => !(type == (Type) null);

    public virtual bool ValidateObject()
    {
      try
      {
        return true;
      }
      catch (Exception ex)
      {
        PropertyGridPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      this.SelectedObject = obj;
      this.ClearChangedState();
      return true;
    }

    public object GetObject() => this.SelectedObject;

    public void ClearChangedState()
    {
    }

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => false;

    private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.SelectedObject == null)
        return;
      PropertyGridDialog propertyGridDialog = new PropertyGridDialog();
      propertyGridDialog.Owner = Application.Current.MainWindow;
      propertyGridDialog.UseCancellingChanges = true;
      propertyGridDialog.SelectedObject = this.SelectedObject;
      propertyGridDialog.ShowDialog();
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/propertygridpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.Label_MouseDoubleClick);
      else
        this._contentLoaded = true;
    }
  }
}
