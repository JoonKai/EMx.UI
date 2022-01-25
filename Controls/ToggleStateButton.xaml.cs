// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.ToggleStateButton
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Controls
{
  [InstanceContract(ClassID = "4f4d548a-3ddd-416a-90c8-d48bb2ecd97f")]
  public partial class ToggleStateButton : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ToggleStateButton), new PropertyMetadata((object) "This"));
    public static DependencyProperty TextSizeProperty = DependencyProperty.Register(nameof (TextSize), typeof (double), typeof (ToggleStateButton), new PropertyMetadata((object) 12.0));
    internal Label button;
    private bool _contentLoaded;

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Color")]
    public Color SelectedColor { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Color")]
    public Color UnSelectedColor { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Color")]
    public Color TextColor { get; set; }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Canvas")]
    public double DesignedHeight
    {
      get => this.Height;
      set => this.Height = value;
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Canvas")]
    public double CanvasLeft
    {
      get => Canvas.GetLeft((UIElement) this);
      set => Canvas.SetLeft((UIElement) this, value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Canvas")]
    public double CanvasTop
    {
      get => Canvas.GetTop((UIElement) this);
      set => Canvas.SetTop((UIElement) this, value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Canvas")]
    public int CanvasZIndex
    {
      get => Panel.GetZIndex((UIElement) this);
      set => Panel.SetZIndex((UIElement) this, value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Canvas")]
    public double DesignedWidth
    {
      get => this.Width;
      set => this.Width = value;
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI")]
    public string Text
    {
      get => (string) this.GetValue(ToggleStateButton.TextProperty);
      set => this.SetValue(ToggleStateButton.TextProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI")]
    public double TextSize
    {
      get => (double) this.GetValue(ToggleStateButton.TextSizeProperty);
      set => this.SetValue(ToggleStateButton.TextSizeProperty, (object) value);
    }

    [InstanceMember]
    [DesignableMember(true)]
    public Var<bool> IsChecked { get; set; }

    public ToggleStateButton()
    {
      try
      {
        this.DataContext = (object) this;
        this.InitializeComponent();
        this.IsChecked = new Var<bool>();
        this.UnSelectedColor = Colors.LightGray;
        this.SelectedColor = "#B3D9B3".ToColor();
        this.TextColor = Colors.Black;
      }
      catch (Exception ex)
      {
        ToggleStateButton.log.Error(ex.ToString());
      }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e) => this.RefreshLayout();

    private void RefreshLayout()
    {
      if (this.IsChecked == null)
        return;
      if (this.IsChecked.Value)
        this.button.Background = (Brush) new SolidColorBrush(this.SelectedColor);
      else
        this.button.Background = (Brush) new SolidColorBrush(this.UnSelectedColor);
      this.button.Foreground = (Brush) new SolidColorBrush(this.TextColor);
    }

    private void button_Click(object sender, RoutedEventArgs e)
    {
      this.IsChecked.Value = !this.IsChecked.Value;
      this.RefreshLayout();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/togglestatebutton.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
          break;
        case 2:
          this.button = (Label) target;
          this.button.MouseLeftButtonUp += new MouseButtonEventHandler(this.button_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
