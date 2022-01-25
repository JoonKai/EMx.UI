// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.SelectMultiItemsPropItemPopupHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Extensions;
using EMx.Logging;
using EMx.UI.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.PropertyGrids.Handlers
{
  public partial class SelectMultiItemsPropItemPopupHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static DependencyProperty PropInfoProperty = DependencyProperty.Register(nameof (PropInfo), typeof (string), typeof (SelectMultiItemsPropItemPopupHandler));
    public static DependencyProperty PropInfoTooltipProperty = DependencyProperty.Register(nameof (PropInfoTooltip), typeof (object), typeof (SelectMultiItemsPropItemPopupHandler));
    private bool _contentLoaded;

    public virtual List<SelectMultiItemsData> ItemsSource { get; set; }

    public string FieldName { get; set; }

    public string Title { get; set; }

    public virtual string PropInfo
    {
      get => (string) this.GetValue(SelectMultiItemsPropItemPopupHandler.PropInfoProperty);
      set => this.SetValue(SelectMultiItemsPropItemPopupHandler.PropInfoProperty, (object) value);
    }

    public virtual object PropInfoTooltip
    {
      get => this.GetValue(SelectMultiItemsPropItemPopupHandler.PropInfoTooltipProperty);
      set => this.SetValue(SelectMultiItemsPropItemPopupHandler.PropInfoTooltipProperty, value);
    }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public string HandlerParameter { get; set; }

    public Type ValueType { get; set; }

    protected List<string> OriginSource { get; set; }

    public SelectMultiItemsPropItemPopupHandler()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.FieldName = "";
      this.ItemsSource = new List<SelectMultiItemsData>();
      this.OriginSource = (List<string>) null;
    }

    public virtual bool IsSupportedType(Type type) => type.Equals(typeof (List<string>));

    public virtual bool ValidateObject() => true;

    public virtual bool SetObject(object obj)
    {
      if (!(obj is List<string> stringList))
        return false;
      this.OriginSource = stringList;
      foreach (SelectMultiItemsData selectMultiItemsData in this.ItemsSource)
        selectMultiItemsData.IsSelected = stringList.Contains(selectMultiItemsData.Message);
      this.UpdatePropInfo();
      return true;
    }

    public object GetObject() => (object) this.ItemsSource.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, string>((Func<SelectMultiItemsData, string>) (x => x.Message)).ToList<string>();

    public void ClearChangedState() => this.OriginSource = this.GetObject() as List<string>;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !string.Equals(this.OriginSource != null ? this.OriginSource.AggregateText<string>(",", (Func<string, string>) (x => x)) : "", this.ItemsSource.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, string>((Func<SelectMultiItemsData, string>) (x => x.Message)).AggregateText<string>(",", (Func<string, string>) (x => x)));

    private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (this.ItemsSource == null)
        return;
      List<SelectMultiItemsData> list = this.ItemsSource.Select<SelectMultiItemsData, SelectMultiItemsData>((Func<SelectMultiItemsData, SelectMultiItemsData>) (x => x.Clone() as SelectMultiItemsData)).ToList<SelectMultiItemsData>();
      SelectMultiItemsDialog multiItemsDialog = new SelectMultiItemsDialog();
      multiItemsDialog.Owner = Application.Current.MainWindow;
      multiItemsDialog.Items = list;
      multiItemsDialog.FieldName = this.FieldName;
      multiItemsDialog.Title = this.Title;
      bool? nullable = multiItemsDialog.ShowDialog();
      bool flag = false;
      if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
        return;
      for (int index = 0; index < list.Count; ++index)
      {
        SelectMultiItemsData selectMultiItemsData = list[index];
        this.ItemsSource[index].IsSelected = selectMultiItemsData.IsSelected;
      }
      e.Handled = true;
      this.UpdatePropInfo();
      this.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent, (object) this));
    }

    private void UpdatePropInfo()
    {
      if (this.ItemsSource == null)
        return;
      this.PropInfo = string.Format("SelectedItems: {0:#,##0}", (object) this.ItemsSource.Count<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)));
      this.PropInfoTooltip = (object) this.ItemsSource.Where<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)).Select<SelectMultiItemsData, string>((Func<SelectMultiItemsData, string>) (x => x.Message)).AggregateText<string>(Environment.NewLine, (Func<string, string>) (x => x));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/selectmultiitemspropitempopuphandler.xaml", UriKind.Relative));
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
