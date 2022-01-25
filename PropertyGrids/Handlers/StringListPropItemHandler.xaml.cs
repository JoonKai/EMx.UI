// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.StringListPropItemHandler
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "04a6da30-7fa0-48a6-8f35-7b218b97e68b")]
  public partial class StringListPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private int LastSelectedIndex;
    internal ComboBox ComboCtrl;
    private bool _contentLoaded;

    protected virtual ObservableCollection<string> OriginalSource { get; set; }

    protected virtual ObservableCollection<string> CurrentSource { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public string HandlerParameter { get; set; }

    public Type ValueType { get; set; }

    public StringListPropItemHandler()
    {
      this.InitializeComponent();
      this.OriginalSource = new ObservableCollection<string>();
      this.CurrentSource = new ObservableCollection<string>();
    }

    public virtual bool IsSupportedType(Type type) => type.Equals(typeof (List<string>));

    public virtual bool ValidateObject() => true;

    public virtual bool SetObject(object obj)
    {
      if (!(obj is List<string> list))
        return false;
      this.OriginalSource = new ObservableCollection<string>(list);
      this.CurrentSource = new ObservableCollection<string>(list);
      this.ComboCtrl.ItemsSource = (IEnumerable) this.CurrentSource;
      return true;
    }

    public object GetObject() => (object) this.CurrentSource.Select<string, string>((Func<string, string>) (x => x)).ToList<string>();

    public void ClearChangedState() => this.SetObject((object) this.OriginalSource);

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !this.OriginalSource.SequenceEqual<string>((IEnumerable<string>) this.CurrentSource);

    private void btnNewItem_Clicked(object sender, RoutedEventArgs e) => this.CurrentSource.Add("");

    private void btnDeleteItem_Clicked(object sender, RoutedEventArgs e)
    {
      int selectedIndex = this.ComboCtrl.SelectedIndex;
      if (selectedIndex <= -1 || selectedIndex >= this.CurrentSource.Count)
        return;
      this.CurrentSource.RemoveAt(selectedIndex);
    }

    private void ComboCtrl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboCtrl.SelectedIndex == -1)
        return;
      this.LastSelectedIndex = this.ComboCtrl.SelectedIndex;
    }

    private void ComboCtrl_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      if (this.LastSelectedIndex > -1 && this.LastSelectedIndex < this.CurrentSource.Count)
      {
        this.CurrentSource[this.LastSelectedIndex] = this.ComboCtrl.Text;
        this.LastSelectedIndex = -1;
      }
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/stringlistpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ComboCtrl = (ComboBox) target;
          this.ComboCtrl.KeyDown += new KeyEventHandler(this.ComboCtrl_KeyDown);
          this.ComboCtrl.SelectionChanged += new SelectionChangedEventHandler(this.ComboCtrl_SelectionChanged);
          break;
        case 2:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnNewItem_Clicked);
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnDeleteItem_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
