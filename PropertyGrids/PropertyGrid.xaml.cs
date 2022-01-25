// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.PropertyGrid
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Data.Models.Handlers;
using EMx.Engine;
using EMx.Engine.Designers;
using EMx.Engine.Designers.Plugins;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Data.Models;
using EMx.UI.Engine.PropertyGrids;
using EMx.UI.Engine.PropertyGrids.Handlers;
using EMx.UI.Extensions;
using EMx.UI.Properties;
using EMx.UI.PropertyGrids.Handlers;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.PropertyGrids
{
  public partial class PropertyGrid : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register(nameof (SelectedObject), typeof (object), typeof (PropertyGrid), new PropertyMetadata((object) null, new PropertyChangedCallback(PropertyGrid.OnSelectedObjectChanged)));
    public static readonly DependencyProperty KeyColumnWidthProperty = DependencyProperty.Register(nameof (KeyColumnWidth), typeof (object), typeof (GridLength));
    public static readonly DependencyProperty ValueColumnWidthProperty = DependencyProperty.Register(nameof (ValueColumnWidth), typeof (object), typeof (GridLength));
    private static readonly BitmapSource OptionIcon = Resources.indi_options.ToImageSource();
    private static readonly BitmapSource DisabledIcon = Resources.forbidden.ToImageSource();
    protected Brush BrushForActivedLabel = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#6ADF21"));
    internal Label Title;
    internal StackPanel Toolar;
    internal Label DropPoint;
    internal Button btnAbcOrder;
    internal Button btnCategoryOrder;
    internal TextBox txtSearchPattern;
    internal Label lblHide;
    internal Grid GridCtrl;
    internal Label txtDetails;
    private bool _contentLoaded;

    public object SelectedObject
    {
      get => this.GetValue(PropertyGrid.SelectedObjectProperty);
      set => this.SetValue(PropertyGrid.SelectedObjectProperty, value);
    }

    public bool UseIndirectMode { get; set; }

    public bool UseCancellingChanges { get; set; }

    public object MetaObject { get; set; }

    public bool UseCategory { get; set; }

    public string Filter { get; set; }

    public GridLength KeyColumnWidth
    {
      get => (GridLength) this.GetValue(PropertyGrid.KeyColumnWidthProperty);
      set => this.SetValue(PropertyGrid.KeyColumnWidthProperty, (object) value);
    }

    public GridLength ValueColumnWidth
    {
      get => (GridLength) this.GetValue(PropertyGrid.ValueColumnWidthProperty);
      set => this.SetValue(PropertyGrid.ValueColumnWidthProperty, (object) value);
    }

    public List<IPropertyGridPlugin> Plugins { get; set; }

    public bool IndicatorMode { get; set; }

    protected Dictionary<Tuple<object, PropertyInfo>, object> BackupValues { get; set; }

    protected List<Tuple<object, PropertyInfo>> BackupItems { get; set; }

    public event Action<PropertyGrid, object, PropertyInfo> PropertyChanged;

    private void InvokePropertyChanged(PropertyGridItem item)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged(this, item.Source, item.Property);
    }

    private List<PropertyGridItem> CreatedPropertyControls { get; set; }

    private PropertyGridItem LastSelectedPropertyGridItem { get; set; }

    public PropertyGrid()
    {
      this.InitializeComponent();
      RenderOptions.SetBitmapScalingMode((DependencyObject) this, BitmapScalingMode.HighQuality);
      this.IndicatorMode = false;
      this.UseCancellingChanges = false;
      this.LastSelectedPropertyGridItem = (PropertyGridItem) null;
      this.UseCategory = true;
      this.Filter = (string) null;
      this.Plugins = new List<IPropertyGridPlugin>();
      this.KeyColumnWidth = new GridLength(1.0, GridUnitType.Star);
      this.ValueColumnWidth = new GridLength(1.0, GridUnitType.Star);
      this.DataContext = (object) this;
      this.UseIndirectMode = false;
      this.CreatedPropertyControls = new List<PropertyGridItem>();
      this.BackupItems = new List<Tuple<object, PropertyInfo>>();
      this.BackupValues = new Dictionary<Tuple<object, PropertyInfo>, object>();
    }

    protected void SetPropertyValue(object src, PropertyInfo p, object value)
    {
      if (this.UseIndirectMode)
        this.BackupValues[Tuple.Create<object, PropertyInfo>(src, p)] = value;
      else
        Helper.Data.SetValue(src, p, value);
    }

    protected object GetPropertyValue(object src, PropertyInfo p)
    {
      if (!this.UseIndirectMode)
        return Helper.Data.GetValue(src, p);
      Tuple<object, PropertyInfo> key = Tuple.Create<object, PropertyInfo>(src, p);
      if (this.BackupValues.ContainsKey(key))
        return this.BackupValues[key];
      PropertyGrid.log.Warn("Not found proerpty {0} from {1}", (object) p.Name, src);
      return (object) null;
    }

    protected void BackupSelectedObjects()
    {
      this.BackupValues.Clear();
      for (int index = 0; index < this.BackupItems.Count; ++index)
      {
        Tuple<object, PropertyInfo> backupItem = this.BackupItems[index];
        object o = backupItem.Item1;
        PropertyInfo p = backupItem.Item2;
        if (o != null && p != (PropertyInfo) null)
        {
          object obj = Helper.Data.GetValue(o, p);
          this.BackupValues.Add(backupItem, obj);
        }
      }
    }

    protected void RestoreSelectedObjects()
    {
      for (int index = 0; index < this.BackupItems.Count; ++index)
      {
        Tuple<object, PropertyInfo> backupItem = this.BackupItems[index];
        object o = backupItem.Item1;
        PropertyInfo p = backupItem.Item2;
        if (o != null && p != (PropertyInfo) null && this.BackupValues.ContainsKey(backupItem))
        {
          object backupValue = this.BackupValues[backupItem];
          Helper.Data.SetValue(o, p, backupValue);
        }
      }
    }

    public virtual void Commit()
    {
      if (!this.UseIndirectMode)
        return;
      this.RestoreSelectedObjects();
    }

    public virtual void Rollback()
    {
      if (!this.UseCancellingChanges)
        return;
      this.RestoreSelectedObjects();
    }

    public void RefreshSelectedObject()
    {
      foreach (PropertyGridItem createdPropertyControl in this.CreatedPropertyControls)
      {
        if (createdPropertyControl.Source != null && createdPropertyControl.Value != null)
          this.OnValueCtrlLostFocus((object) createdPropertyControl.Value, (RoutedEventArgs) null);
      }
      this.GridCtrl.Children.Clear();
      this.GridCtrl.RowDefinitions.Clear();
      this.CreatedPropertyControls.Clear();
      if (this.SelectedObject == null)
        return;
      List<object> objectList = new List<object>();
      if (this.SelectedObject is IEnumerable)
      {
        foreach (object obj in this.SelectedObject as IEnumerable)
          objectList.Add(obj);
      }
      else
        objectList.Add(this.SelectedObject);
      InstanceSerializerCache cache = InstanceSerializerCache.Inst;
      Func<PropertyInfo, string> get_disp_name = (Func<PropertyInfo, string>) (p =>
      {
        GridViewItemAttribute customAttribute1 = cache.GetCustomAttribute<GridViewItemAttribute>((MemberInfo) p);
        if (customAttribute1 != null && !customAttribute1.Alias.IsNullOrEmpty())
          return customAttribute1.Alias;
        DisplayNameAttribute customAttribute2 = cache.GetCustomAttribute<DisplayNameAttribute>((MemberInfo) p);
        return customAttribute2 != null ? customAttribute2.DisplayName : p.Name;
      });
      Func<PropertyInfo, string> get_sort_name = (Func<PropertyInfo, string>) (p =>
      {
        GridViewItemAttribute customAttribute3 = cache.GetCustomAttribute<GridViewItemAttribute>((MemberInfo) p);
        if (customAttribute3 != null)
        {
          if (!customAttribute3.AliasForSort.IsNullOrEmpty())
            return customAttribute3.AliasForSort;
          if (!customAttribute3.Alias.IsNullOrEmpty())
            return customAttribute3.Alias;
        }
        DisplayNameAttribute customAttribute4 = cache.GetCustomAttribute<DisplayNameAttribute>((MemberInfo) p);
        return customAttribute4 != null ? customAttribute4.DisplayName : p.Name;
      });
      Func<PropertyInfo, string> get_category_name = (Func<PropertyInfo, string>) (p =>
      {
        CategoryAttribute customAttribute = cache.GetCustomAttribute<CategoryAttribute>((MemberInfo) p);
        return customAttribute != null ? customAttribute.Category : "Default";
      });
      int hide_count = 0;
      List<Tuple<object, PropertyInfo>> total_props = new List<Tuple<object, PropertyInfo>>();
      foreach (object obj in objectList)
      {
        object val = obj;
        if (val != null)
          ((IEnumerable<PropertyInfo>) val.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).Where<PropertyInfo>(closure_0 ?? (closure_0 = (Func<PropertyInfo, bool>) (x =>
          {
            GridViewItemAttribute customAttribute = cache.GetCustomAttribute<GridViewItemAttribute>((MemberInfo) x);
            if (customAttribute == null || !customAttribute.Show)
            {
              ++hide_count;
              return false;
            }
            if (customAttribute == null && !typeof (IManagedVariable).IsAssignableFrom(x.PropertyType))
            {
              ++hide_count;
              return false;
            }
            string str = get_disp_name(x);
            if (string.IsNullOrWhiteSpace(this.Filter))
              return true;
            bool flag = str.IndexOf(this.Filter, StringComparison.CurrentCultureIgnoreCase) > -1;
            if (!flag)
              ++hide_count;
            return flag;
          }))).ToList<PropertyInfo>().ForEach((Action<PropertyInfo>) (x => total_props.Add(Tuple.Create<object, PropertyInfo>(val, x))));
      }
      if (this.UseCategory)
        total_props = total_props.OrderBy<Tuple<object, PropertyInfo>, string>((Func<Tuple<object, PropertyInfo>, string>) (x => get_category_name(x.Item2))).ThenBy<Tuple<object, PropertyInfo>, string>((Func<Tuple<object, PropertyInfo>, string>) (x => get_sort_name(x.Item2))).ToList<Tuple<object, PropertyInfo>>();
      else
        total_props.SortExt<Tuple<object, PropertyInfo>>((Comparison<Tuple<object, PropertyInfo>>) ((a, b) => get_disp_name(a.Item2).CompareTo(get_sort_name(b.Item2))));
      int num1 = total_props.Count + hide_count;
      if (hide_count > 0)
        this.lblHide.Content = (object) string.Format("Hide({0})", (object) hide_count);
      else
        this.lblHide.Content = (object) "";
      this.BackupItems = total_props;
      if (this.UseIndirectMode || this.UseCancellingChanges)
        this.BackupSelectedObjects();
      int num2 = 0;
      string a1 = (string) null;
      for (int index1 = 0; index1 < total_props.Count; ++index1)
      {
        object obj = total_props[index1].Item1;
        PropertyInfo propertyInfo = total_props[index1].Item2;
        Type dataType = Helper.Data.GetDataType(obj, propertyInfo);
        object propertyValue = this.GetPropertyValue(obj, propertyInfo);
        string b = get_category_name(propertyInfo);
        string str1 = get_disp_name(propertyInfo);
        List<ePropertyIndicatorType> propertyIndicatorTypeList = new List<ePropertyIndicatorType>();
        for (int index2 = 0; index2 < this.Plugins.Count; ++index2)
        {
          IPropertyGridPlugin plugin = this.Plugins[index2];
          propertyIndicatorTypeList.AddRange((IEnumerable<ePropertyIndicatorType>) plugin.GetIndicators(this.MetaObject, this.SelectedObject, (MemberInfo) propertyInfo));
        }
        if (!this.IndicatorMode || propertyIndicatorTypeList.Count != 0)
        {
          if (!string.Equals(a1, b) && this.UseCategory)
          {
            a1 = b;
            SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb((byte) 107, (byte) 141, (byte) 187));
            Label label1 = new Label();
            label1.Content = (object) b;
            label1.Background = (Brush) solidColorBrush;
            label1.VerticalContentAlignment = VerticalAlignment.Center;
            Grid.SetRow((UIElement) label1, num2);
            Grid.SetColumn((UIElement) label1, 1);
            label1.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
            label1.Padding = new Thickness(0.0);
            this.GridCtrl.Children.Add((UIElement) label1);
            Label label2 = new Label();
            label2.Content = (object) "";
            label2.Background = (Brush) solidColorBrush;
            Grid.SetRow((UIElement) label2, num2);
            Grid.SetColumn((UIElement) label2, 2);
            label2.Margin = new Thickness(0.0, 0.0, 1.0, 0.0);
            label2.Padding = new Thickness(0.0);
            this.GridCtrl.Children.Add((UIElement) label2);
            Label label3 = new Label();
            label3.Content = (object) "";
            label3.Background = (Brush) solidColorBrush;
            label3.Margin = new Thickness(1.0, 0.0, 0.0, 0.0);
            label3.Padding = new Thickness(0.0);
            Grid.SetRow((UIElement) label3, num2);
            Grid.SetColumn((UIElement) label3, 0);
            this.GridCtrl.Children.Add((UIElement) label3);
            ++num2;
          }
          GridViewItemAttribute customAttribute5 = propertyInfo.GetCustomAttribute<GridViewItemAttribute>();
          bool flag = false;
          eGridViewItemHandler gridViewItemHandler = eGridViewItemHandler.Default;
          Type type1 = (Type) null;
          string str2 = "";
          if (customAttribute5 != null)
          {
            flag = customAttribute5.ReadOnly;
            gridViewItemHandler = customAttribute5.Handler;
            type1 = customAttribute5.ExternalHandler;
            str2 = customAttribute5.HandlerParam;
          }
          Label label = new Label();
          label.Content = (object) str1;
          label.VerticalContentAlignment = VerticalAlignment.Center;
          label.Margin = new Thickness(1.0);
          label.Padding = new Thickness(2.0, 0.0, 0.0, 0.0);
          label.ToolTip = (object) str1;
          StackPanel stackPanel = new StackPanel();
          stackPanel.MouseLeftButtonDown += new MouseButtonEventHandler(this.OnNameMouseDown);
          stackPanel.ContextMenuOpening += new ContextMenuEventHandler(this.OnNameContextMenuOpening);
          stackPanel.Orientation = Orientation.Horizontal;
          for (int index3 = 0; index3 < propertyIndicatorTypeList.Count; ++index3)
          {
            switch (propertyIndicatorTypeList[index3])
            {
              case ePropertyIndicatorType.Disabled:
                UIElementCollection children1 = stackPanel.Children;
                Image image1 = new Image();
                image1.Source = (ImageSource) PropertyGrid.DisabledIcon;
                image1.Margin = new Thickness(1.0);
                children1.Add((UIElement) image1);
                break;
              case ePropertyIndicatorType.Option:
                UIElementCollection children2 = stackPanel.Children;
                Image image2 = new Image();
                image2.Source = (ImageSource) PropertyGrid.OptionIcon;
                image2.Margin = new Thickness(1.0);
                children2.Add((UIElement) image2);
                break;
            }
          }
          stackPanel.Children.Add((UIElement) label);
          Grid.SetRow((UIElement) stackPanel, num2);
          Grid.SetColumn((UIElement) stackPanel, 1);
          IPropertyGridHandler propertyGridHandler = (IPropertyGridHandler) null;
          if (gridViewItemHandler == eGridViewItemHandler.Default)
          {
            if (dataType.IsEnum)
              gridViewItemHandler = eGridViewItemHandler.Enumeration;
            else if (dataType == typeof (bool))
            {
              gridViewItemHandler = eGridViewItemHandler.Enumeration;
              str2 = "False,True";
            }
            else if (dataType == typeof (Color))
              gridViewItemHandler = eGridViewItemHandler.Color;
            else if (dataType == typeof (TimeSpan))
              gridViewItemHandler = eGridViewItemHandler.TimeSpan;
            else if (dataType.IsPrimitive || dataType == typeof (string) || dataType == typeof (DateTime))
            {
              gridViewItemHandler = eGridViewItemHandler.String;
            }
            else
            {
              gridViewItemHandler = eGridViewItemHandler.String;
              foreach (Type type2 in Helper.Assem.FindAllSubclass(typeof (IPropertyGridHandler)))
              {
                PropertyGridDefaultHandlerAttribute customAttribute6 = type2.GetCustomAttribute<PropertyGridDefaultHandlerAttribute>(false);
                if (customAttribute6 != null && dataType == customAttribute6.AvailableType)
                {
                  gridViewItemHandler = eGridViewItemHandler.Default;
                  propertyGridHandler = (IPropertyGridHandler) Helper.Assem.CreateObjectByType(type2);
                  propertyGridHandler.IsReadOnly = customAttribute6.ReadOnly;
                  break;
                }
              }
            }
          }
          PropertyGridItem propertyGridItem = new PropertyGridItem();
          switch (gridViewItemHandler)
          {
            case eGridViewItemHandler.Default:
              propertyGridItem.Value = propertyGridHandler;
              break;
            case eGridViewItemHandler.Boolean:
              propertyGridItem.Value = (IPropertyGridHandler) new ComboPropItemHandler();
              break;
            case eGridViewItemHandler.String:
              propertyGridItem.Value = (IPropertyGridHandler) new TextPropItemHandler();
              break;
            case eGridViewItemHandler.HexString:
              propertyGridItem.Value = (IPropertyGridHandler) new HexStringPropItemHandler();
              break;
            case eGridViewItemHandler.Enumeration:
              propertyGridItem.Value = (IPropertyGridHandler) new ComboPropItemHandler();
              break;
            case eGridViewItemHandler.Color:
              propertyGridItem.Value = (IPropertyGridHandler) new ColorPropItemHandler();
              break;
            case eGridViewItemHandler.IntListItem:
              propertyGridItem.Value = (IPropertyGridHandler) new IntListPropItemHandler();
              break;
            case eGridViewItemHandler.DoubleListItem:
              propertyGridItem.Value = (IPropertyGridHandler) new DoubleListPropItemHandler();
              break;
            case eGridViewItemHandler.StringListItem:
              propertyGridItem.Value = (IPropertyGridHandler) new StringListPropItemHandler();
              break;
            case eGridViewItemHandler.HexNumber:
              propertyGridItem.Value = (IPropertyGridHandler) new HexNumberPropItemHandler();
              break;
            case eGridViewItemHandler.StringItemWithListSource:
              StringComboPropItemHandler comboPropItemHandler = new StringComboPropItemHandler();
              if (!string.IsNullOrWhiteSpace(customAttribute5.HandlerParam))
              {
                PropertyInfo property = Helper.Assem.GetProperty(obj, customAttribute5.HandlerParam);
                if (property != (PropertyInfo) null && property.GetValue(obj) is List<string> stringList7)
                {
                  comboPropItemHandler.Items = stringList7;
                  propertyGridItem.Value = (IPropertyGridHandler) comboPropItemHandler;
                }
                break;
              }
              break;
            case eGridViewItemHandler.External:
              if (Helper.Assem.CreateObjectByType(customAttribute5.ExternalHandler) is IGridViewExternalHandler objectByType5)
              {
                propertyGridItem.Value = objectByType5.CreateGridHandler(obj, propertyInfo, customAttribute5.HandlerParam);
                break;
              }
              break;
            case eGridViewItemHandler.SerialPorts:
              propertyGridItem.Value = (IPropertyGridHandler) new StringComboPropItemHandler()
              {
                Items = ((IEnumerable<string>) SerialPort.GetPortNames()).ToList<string>()
              };
              break;
            case eGridViewItemHandler.TimeSpan:
              propertyGridItem.Value = (IPropertyGridHandler) new TimeSpanPropItemHandler();
              break;
            case eGridViewItemHandler.FilePath:
            case eGridViewItemHandler.DirPath:
              propertyGridItem.Value = (IPropertyGridHandler) new FilePathPropItemHandler();
              break;
            case eGridViewItemHandler.PropertyGrid:
              propertyGridItem.Value = (IPropertyGridHandler) new PropertyGridPropItemHandler();
              break;
            case eGridViewItemHandler.PropertyGridItems:
              propertyGridItem.Value = (IPropertyGridHandler) new PropertyGridItemsPropItemHandler();
              break;
            case eGridViewItemHandler.QueryableMethod:
              propertyGridItem.Value = new QueryableMethodSupplier().CreateGridHandler(obj, propertyInfo, customAttribute5.HandlerParam);
              break;
            case eGridViewItemHandler.TargetProperty:
              propertyGridItem.Value = new TargetPropertySupplier().CreateGridHandler(obj, propertyInfo, customAttribute5.HandlerParam);
              break;
            case eGridViewItemHandler.TargetProperties:
              propertyGridItem.Value = new TargetPropertiesSupplier().CreateGridHandler(obj, propertyInfo, customAttribute5.HandlerParam);
              break;
            case eGridViewItemHandler.LiteralString:
              propertyGridItem.Value = (IPropertyGridHandler) new EncodingLiteralStringPropItemHandler();
              break;
            case eGridViewItemHandler.ManagedType:
              propertyGridItem.Value = (IPropertyGridHandler) new ManagedTypePropItemHandler(obj, propertyInfo, customAttribute5.HandlerParam);
              break;
          }
          if (propertyGridItem.Value == null)
          {
            PropertyGrid.log.Warn("Failed to create property handler : Name({0}) Type({1}) Handler({2}) External({3})", (object) str1, (object) Helper.Assem.GetGenericTypeName(dataType), (object) gridViewItemHandler, (object) type1);
          }
          else
          {
            propertyGridItem.Value.IsReadOnly = flag;
            propertyGridItem.Value.ValueType = dataType;
            propertyGridItem.Value.HandlerParameter = str2;
            if (!propertyGridItem.Value.IsSupportedType(dataType))
            {
              PropertyGrid.log.Warn("Is not supported type({0}) by {1}", (object) Helper.Assem.GetGenericTypeName(dataType), (object) Helper.Assem.GetGenericTypeName(propertyGridItem.Value.GetType()));
            }
            else
            {
              propertyGridItem.Value.SetObject(propertyValue);
              propertyGridItem.Value.ClearChangedState();
              UIElement uiElement = propertyGridItem.Value.GetUIElement();
              if (uiElement == null)
              {
                PropertyGrid.log.Error("Failed to get UIElement from {0}", (object) Helper.Assem.GetGenericTypeName(propertyGridItem.Value.GetType()));
              }
              else
              {
                uiElement.GotFocus += new RoutedEventHandler(this.OnValueCtrlGotFocus);
                uiElement.LostFocus += new RoutedEventHandler(this.OnValueCtrlLostFocus);
                Grid.SetRow(uiElement, num2);
                Grid.SetColumn(uiElement, 2);
                this.GridCtrl.Children.Add((UIElement) stackPanel);
                this.GridCtrl.Children.Add(propertyGridItem.Value.GetUIElement());
                propertyGridItem.Source = obj;
                propertyGridItem.Property = propertyInfo;
                propertyGridItem.Name = stackPanel;
                this.CreatedPropertyControls.Add(propertyGridItem);
                ++num2;
              }
            }
          }
        }
      }
      for (int index = 0; index < num2; ++index)
        this.GridCtrl.RowDefinitions.Add(new RowDefinition()
        {
          Height = new GridLength(23.0, GridUnitType.Pixel)
        });
    }

    public virtual void RefreshControlText()
    {
      if (this.UseIndirectMode)
        return;
      foreach (PropertyGridItem createdPropertyControl in this.CreatedPropertyControls)
      {
        Type dataType = Helper.Data.GetDataType(createdPropertyControl.Source, createdPropertyControl.Property);
        object obj = Helper.Data.GetValue(createdPropertyControl.Source, createdPropertyControl.Property);
        if (createdPropertyControl.Value.IsSupportedType(dataType) && !createdPropertyControl.Value.SetObject(obj))
          PropertyGrid.log.Warn("Failed to update control({0}) value with value({1}).", (object) Helper.Assem.GetGenericTypeName(createdPropertyControl.Value.GetType()), obj);
      }
    }

    private void OnValueCtrlLostFocus(object sender, RoutedEventArgs e)
    {
      IPropertyGridHandler value = sender as IPropertyGridHandler;
      PropertyGridItem propertyGridItem = this.CreatedPropertyControls.FirstOrDefault<PropertyGridItem>((Func<PropertyGridItem, bool>) (x => x.Value == value));
      if (value == null || propertyGridItem == null)
        return;
      if (value.GetUIElement() == null)
        return;
      try
      {
        if (!value.IsValueChanged || !value.ValidateObject())
          return;
        this.SetPropertyValue(propertyGridItem.Source, propertyGridItem.Property, value.GetObject());
        value.ClearChangedState();
        this.InvokePropertyChanged(propertyGridItem);
      }
      catch (Exception ex)
      {
        PropertyGrid.log.Error(ex, "Error on lost focusing.");
      }
    }

    private void OnValueCtrlGotFocus(object sender, RoutedEventArgs e)
    {
      IPropertyGridHandler value = sender as IPropertyGridHandler;
      PropertyGridItem propertyGridItem = this.CreatedPropertyControls.FirstOrDefault<PropertyGridItem>((Func<PropertyGridItem, bool>) (x => x.Value == value));
      if (value == null || propertyGridItem == null)
        return;
      this.CreatedPropertyControls.ForEach((Action<PropertyGridItem>) (x =>
      {
        if (x.Value == value)
        {
          x.Name.Background = this.BrushForActivedLabel;
          this.LastSelectedPropertyGridItem = x;
        }
        else
          x.Name.Background = (Brush) Brushes.White;
        DescriptionAttribute customAttribute = InstanceSerializerCache.Inst.GetCustomAttribute<DescriptionAttribute>((MemberInfo) x.Property);
        if (customAttribute != null)
          this.txtDetails.Content = (object) ("Description\r\n" + customAttribute.Description);
        else
          this.txtDetails.Content = (object) "";
      }));
    }

    private void OnNameMouseDown(object sender, MouseButtonEventArgs e)
    {
      StackPanel panel = sender as StackPanel;
      if (panel != null)
      {
        this.CreatedPropertyControls.ForEach((Action<PropertyGridItem>) (x => x.Name.Background = (Brush) Brushes.White));
        panel.Background = (Brush) Brushes.LightSkyBlue;
        this.LastSelectedPropertyGridItem = this.CreatedPropertyControls.FirstOrDefault<PropertyGridItem>((Func<PropertyGridItem, bool>) (x => x.Name == panel));
        this.txtDetails.Content = (object) "";
        PropertyGridItem propertyGridItem = this.CreatedPropertyControls.Find((Predicate<PropertyGridItem>) (x => x.Name == panel));
        if (propertyGridItem != null)
        {
          DescriptionAttribute customAttribute = propertyGridItem.Property.GetCustomAttribute<DescriptionAttribute>();
          if (customAttribute != null)
            this.txtDetails.Content = (object) ("Description\r\n" + customAttribute.Description);
        }
      }
      e.Handled = true;
    }

    private void OnNameContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      FrameworkElement source = e.Source as FrameworkElement;
      ContextMenu contextMenu = new ContextMenu();
      for (int index = 0; index < this.Plugins.Count; ++index)
      {
        foreach (PropertyGridContextMenuItem contextCommand in this.Plugins[index].GetContextCommands())
        {
          MenuItem menuItem1 = new MenuItem();
          menuItem1.Header = (object) contextCommand.Text;
          menuItem1.Tag = (object) contextCommand;
          MenuItem menuItem2 = menuItem1;
          menuItem2.Click += new RoutedEventHandler(this.OnContextMenItemClicked);
          contextMenu.Items.Add((object) menuItem2);
        }
      }
      source.ContextMenu = contextMenu;
    }

    private void OnContextMenItemClicked(object sender, RoutedEventArgs e)
    {
      if (!(sender is MenuItem menuItem) || !(menuItem.Tag is PropertyGridContextMenuItem tag))
        return;
      IPropertyGridPlugin plugin = tag.Plugin;
      if (plugin == null)
        return;
      if (this.LastSelectedPropertyGridItem != null)
        plugin.ExecuteCommand(tag, this.MetaObject, this.SelectedObject, (MemberInfo) this.LastSelectedPropertyGridItem.Property);
      e.Handled = true;
    }

    protected void LinkNotifyEvent(object src)
    {
      if (src == null)
        return;
      List<object> objectList = new List<object>();
      if (src is IEnumerable)
      {
        foreach (object obj in src as IEnumerable)
          objectList.Add(obj);
      }
      else
        objectList.Add(src);
      foreach (object obj in objectList)
      {
        if (obj is INotifyPropertyChanged notifyPropertyChanged1)
          notifyPropertyChanged1.PropertyChanged += new PropertyChangedEventHandler(this.OnSourcePropertyItemsChanged);
      }
    }

    protected void DelinkNotifyEvent(object src)
    {
      if (src == null)
        return;
      List<object> objectList = new List<object>();
      if (src is IEnumerable)
      {
        foreach (object obj in src as IEnumerable)
          objectList.Add(obj);
      }
      else
        objectList.Add(src);
      foreach (object obj in objectList)
      {
        if (obj is INotifyPropertyChanged notifyPropertyChanged1)
          notifyPropertyChanged1.PropertyChanged -= new PropertyChangedEventHandler(this.OnSourcePropertyItemsChanged);
      }
    }

    private void OnSourcePropertyItemsChanged(object sender, PropertyChangedEventArgs e) => this.RefreshControlText();

    private static void OnSelectedObjectChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PropertyGrid propertyGrid = d as PropertyGrid;
      if (e.NewValue == e.OldValue)
        return;
      propertyGrid.DelinkNotifyEvent(e.OldValue);
      propertyGrid.RefreshSelectedObject();
      propertyGrid.LinkNotifyEvent(e.NewValue);
    }

    private void btnAbcOrder_Click(object sender, RoutedEventArgs e)
    {
      this.UseCategory = false;
      this.RefreshSelectedObject();
    }

    private void btnCategoryOrder_Click(object sender, RoutedEventArgs e)
    {
      this.UseCategory = true;
      this.RefreshSelectedObject();
    }

    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      this.Filter = this.txtSearchPattern.Text;
      this.RefreshSelectedObject();
    }

    private void txtSearchPattern_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.btnSearch_Click(sender, (RoutedEventArgs) null);
    }

    private void DropPoint_DragEnter(object sender, DragEventArgs e)
    {
      DesignInstanceItem data1 = e.Data.GetData("DesignItem") as DesignInstanceItem;
      PropertyInfo data2 = e.Data.GetData("PropertyInfo") as PropertyInfo;
      if (data1 == null || data1.Instance == null || data2 == (PropertyInfo) null)
        return;
      if (data2.PropertyType.IsPrimitive)
      {
        this.DropPoint.Background = (Brush) Brushes.Red;
        e.Effects = DragDropEffects.None;
      }
      else
        this.DropPoint.Background = (Brush) Brushes.LawnGreen;
    }

    private void DropPoint_DragLeave(object sender, DragEventArgs e)
    {
      if (e.Data.GetData("DesignItem") == null)
        return;
      this.DropPoint.Background = (Brush) Brushes.LightGray;
    }

    private void DropPoint_Drop(object sender, DragEventArgs e)
    {
      this.DropPoint.Background = (Brush) Brushes.LightGray;
      DesignInstanceItem data1 = e.Data.GetData("DesignItem") as DesignInstanceItem;
      PropertyInfo data2 = e.Data.GetData("PropertyInfo") as PropertyInfo;
      if (data1 == null || data2 == (PropertyInfo) null || data1 == this.SelectedObject || data1.Instance == null)
        return;
      object obj = data2.GetValue((object) data1.Instance);
      if (obj == null || obj.GetType().IsPrimitive)
        return;
      if (typeof (Var).IsAssignableFrom(obj.GetType()) && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
        this.SelectedObject = (obj as Var).GetObject();
      else
        this.SelectedObject = obj;
    }

    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
      PropertyGrid.log.Debug("Unloaded PropertyGrid.");
      this.DelinkNotifyEvent(this.SelectedObject);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/propertygrid.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Unloaded += new RoutedEventHandler(this.UserControl_Unloaded);
          break;
        case 2:
          this.Title = (Label) target;
          break;
        case 3:
          this.Toolar = (StackPanel) target;
          break;
        case 4:
          this.DropPoint = (Label) target;
          this.DropPoint.DragEnter += new DragEventHandler(this.DropPoint_DragEnter);
          this.DropPoint.DragLeave += new DragEventHandler(this.DropPoint_DragLeave);
          this.DropPoint.Drop += new DragEventHandler(this.DropPoint_Drop);
          break;
        case 5:
          this.btnAbcOrder = (Button) target;
          this.btnAbcOrder.Click += new RoutedEventHandler(this.btnAbcOrder_Click);
          break;
        case 6:
          this.btnCategoryOrder = (Button) target;
          this.btnCategoryOrder.Click += new RoutedEventHandler(this.btnCategoryOrder_Click);
          break;
        case 7:
          this.txtSearchPattern = (TextBox) target;
          this.txtSearchPattern.KeyDown += new KeyEventHandler(this.txtSearchPattern_KeyDown);
          break;
        case 8:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSearch_Click);
          break;
        case 9:
          this.lblHide = (Label) target;
          break;
        case 10:
          this.GridCtrl = (Grid) target;
          break;
        case 11:
          this.txtDetails = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
