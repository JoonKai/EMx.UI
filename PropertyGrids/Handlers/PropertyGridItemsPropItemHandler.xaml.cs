// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.PropertyGridItemsPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Engine;
using EMx.Extensions;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "4781ba76-937b-44d3-9a1b-3b2fb72a0fbc")]
  public partial class PropertyGridItemsPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IManagedType,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private IEnumerable SelectedItems;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public PropertyGridItemsPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type) => typeof (IEnumerable).IsAssignableFrom(type);

    public virtual bool ValidateObject()
    {
      try
      {
        return true;
      }
      catch (Exception ex)
      {
        PropertyGridItemsPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      this.SelectedItems = obj as IEnumerable;
      if (this.SelectedItems == null)
        return false;
      this.ClearChangedState();
      return true;
    }

    public object GetObject() => (object) this.SelectedItems;

    public void ClearChangedState()
    {
    }

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => false;

    private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.SelectedItems == null)
        return;
      List<object> list = this.SelectedItems.GetEnumerator().ToList();
      SelectListItemDialog selectListItemDialog = new SelectListItemDialog();
      selectListItemDialog.Owner = Application.Current.MainWindow;
      selectListItemDialog.ItemSource = (IEnumerable) list;
      bool? nullable = selectListItemDialog.ShowDialog();
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue || selectListItemDialog.SelectedIndex == -1)
        return;
      object obj = list[selectListItemDialog.SelectedIndex];
      PropertyGridDialog propertyGridDialog = new PropertyGridDialog();
      propertyGridDialog.Owner = Application.Current.MainWindow;
      propertyGridDialog.UseCancellingChanges = true;
      propertyGridDialog.SelectedObject = obj;
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
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/propertygriditemspropitemhandler.xaml", UriKind.Relative));
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
