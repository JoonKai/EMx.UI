// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.ManagedTypePropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Engine;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "cd5f9da1-6727-4548-8d93-9c71ab75d511")]
  public partial class ManagedTypePropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IManagedType,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private Type CurrentType;
    private Type SelectedType;
    internal Label txtTypeName;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    private object ParamSource { get; set; }

    private PropertyInfo ParamProperty { get; set; }

    private string ParamHandlerParameter { get; set; }

    private List<Type> ParamTypes { get; set; }

    public ManagedTypePropItemHandler(
      object source,
      PropertyInfo property,
      string HandlerParameger)
    {
      this.InitializeComponent();
      this.ParamSource = source;
      this.ParamProperty = property;
      this.ParamHandlerParameter = HandlerParameger;
      InstanceSerializerCache inst = InstanceSerializerCache.Inst;
      List<Type> typeList = new List<Type>();
      if (!string.IsNullOrWhiteSpace(HandlerParameger))
      {
        PropertyInfo property1 = Helper.Assem.GetProperty(source, HandlerParameger);
        if (property1 != (PropertyInfo) null)
          typeList = property1.GetValue(source) as List<Type>;
        else
          ManagedTypePropItemHandler.log.Warn("Not found HandlerParameter : {0}, source[{1}], prop[{2}]", (object) HandlerParameger, source, (object) property);
      }
      else
        typeList = Helper.Runtime.GetSubClasses<IManagedType>(true);
      this.ParamTypes = typeList;
    }

    public virtual bool IsSupportedType(Type type) => !(type == (Type) null) && this.ParamSource != null && !(this.ParamProperty == (PropertyInfo) null) && (this.ParamProperty.PropertyType.Equals(typeof (Type)) || this.ParamProperty.PropertyType.Equals(typeof (string)));

    public virtual bool ValidateObject()
    {
      try
      {
        return true;
      }
      catch (Exception ex)
      {
        ManagedTypePropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      switch (obj)
      {
        case null:
          this.SelectedType = (Type) null;
          break;
        case string _:
          this.SelectedType = Helper.Assem.GetTypeInCurrentDomainWithFullName(obj as string);
          break;
        case Type _:
          this.SelectedType = obj as Type;
          break;
      }
      this.CurrentType = this.SelectedType;
      this.ClearChangedState();
      return true;
    }

    public object GetObject() => (object) this.CurrentType;

    public void ClearChangedState()
    {
      this.SelectedType = this.CurrentType;
      this.txtTypeName.Content = this.CurrentType != (Type) null ? (object) this.CurrentType.Name : (object) "null";
    }

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !object.Equals((object) this.CurrentType, (object) this.SelectedType);

    private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      SelectListItemSearchableDialog searchableDialog = new SelectListItemSearchableDialog();
      searchableDialog.Width = 700.0;
      searchableDialog.Title = "Please select a class type.";
      searchableDialog.Owner = Application.Current.MainWindow;
      searchableDialog.ItemSource = (IEnumerable) this.ParamTypes;
      searchableDialog.NameGetter = (Func<object, string>) (x => Helper.Assem.GetGenericTypeName(x as Type));
      searchableDialog.InitialName = this.CurrentType != (Type) null ? this.CurrentType.Name : "";
      bool? nullable = searchableDialog.ShowDialog();
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        return;
      Type selectedObject = searchableDialog.SelectedObject as Type;
      if (selectedObject == (Type) null)
        return;
      this.CurrentType = selectedObject;
      this.txtTypeName.Content = this.CurrentType != (Type) null ? (object) this.CurrentType.Name : (object) "null";
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/managedtypepropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtTypeName = (Label) target;
          break;
        case 2:
          ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.Label_MouseDoubleClick);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
