// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.ComboPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  public partial class ComboPropItemHandler : UserControl, IPropertyGridHandler, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private int LastSelectedIndex;
    private bool IsItemsSourceInitialized;
    private Dictionary<string, object> ItemMap = new Dictionary<string, object>();
    internal ComboBox ComboCtrl;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public ComboPropItemHandler()
    {
      this.InitializeComponent();
      this.LastSelectedIndex = -1;
      this.IsItemsSourceInitialized = false;
    }

    public virtual bool IsSupportedType(Type type) => type != (Type) null && (type.IsEnum || type.IsPrimitive || type == typeof (string));

    public virtual bool ValidateObject()
    {
      try
      {
        string text = this.ComboCtrl.Text;
        if (!this.ItemMap.ContainsKey(text))
          return false;
        Helper.Data.ConvertType(this.ItemMap[text], this.ValueType);
        return true;
      }
      catch (Exception ex)
      {
        ComboPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      if (!this.IsItemsSourceInitialized)
      {
        this.ItemMap.Clear();
        if (this.ValueType.IsEnum)
        {
          InstanceSerializerCache inst = InstanceSerializerCache.Inst;
          Array values = Enum.GetValues(this.ValueType);
          List<string> stringList = new List<string>();
          foreach (object obj1 in values)
          {
            string key = obj1.ToString();
            string enumDescription = Helper.Data.GetEnumDescription(obj1);
            if (!string.IsNullOrWhiteSpace(enumDescription))
              key = enumDescription;
            this.ItemMap[key] = obj1;
          }
        }
        else
        {
          if (string.IsNullOrWhiteSpace(this.HandlerParameter))
            return false;
          List<string> list = ((IEnumerable<string>) this.HandlerParameter.Split(',')).Where<string>((Func<string, bool>) (x => !string.IsNullOrWhiteSpace(x))).ToList<string>();
          for (int index = 0; index < list.Count; ++index)
          {
            string[] strArray = list[index].Split(':');
            string key = strArray[0];
            string str = strArray.Length > 1 ? strArray[1] : (string) null;
            object obj2 = (object) null;
            if (Helper.Data.IsNumber(this.ValueType))
              obj2 = str == null ? (object) index : Helper.Data.ConvertType((object) str, this.ValueType);
            else if (Helper.Data.IsBool(this.ValueType))
              obj2 = str == null ? Helper.Data.ConvertType((object) key, this.ValueType) : Helper.Data.ConvertType((object) str, this.ValueType);
            this.ItemMap[key] = obj2 ?? (object) key;
          }
        }
        if (this.ItemMap.Count > 0)
        {
          this.IsItemsSourceInitialized = true;
          this.ComboCtrl.ItemsSource = (IEnumerable) this.ItemMap.Keys.ToList<string>();
        }
      }
      KeyValuePair<string, object> keyValuePair = this.ItemMap.FirstOrDefault<KeyValuePair<string, object>>((Func<KeyValuePair<string, object>, bool>) (p => p.Value.Equals(obj)));
      if (keyValuePair.Key == null)
        return false;
      if (this.ItemMap.ContainsKey(keyValuePair.Key))
        this.ComboCtrl.SelectedItem = (object) keyValuePair.Key;
      this.ClearChangedState();
      return true;
    }

    public object GetObject()
    {
      try
      {
        string text = this.ComboCtrl.Text;
        return !this.ItemMap.ContainsKey(text) ? (object) false : Helper.Data.ConvertType(this.ItemMap[text], this.ValueType);
      }
      catch (Exception ex)
      {
        ComboPropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
        return (object) null;
      }
    }

    public void ClearChangedState() => this.LastSelectedIndex = this.ComboCtrl.SelectedIndex;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => this.LastSelectedIndex != this.ComboCtrl.SelectedIndex;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/combopropitemhandler.xaml", UriKind.Relative));
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
