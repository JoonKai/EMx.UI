// Decompiled with JetBrains decompiler
// Type: EMx.UI.CanvasExBoxItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models;
using EMx.Engine;
using EMx.Engine.Linkers;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI
{
  [InstanceContract(ClassID = "0a1eddeb-537b-41e6-8c0e-3b10a1332e29")]
  public partial class CanvasExBoxItem : UserControl, IManagedType, IComponentConnector
  {
    private int _PaddingValue;
    public static DependencyProperty HeaderHeightProperty = DependencyProperty.Register(nameof (HeaderHeight), typeof (double), typeof (CanvasExBoxItem), new PropertyMetadata((object) 25.0));
    public static DependencyProperty HeaderTitleProperty = DependencyProperty.Register(nameof (HeaderTitle), typeof (string), typeof (CanvasExBoxItem));
    public static DependencyProperty HeaderColorProperty = DependencyProperty.Register(nameof (HeaderColor), typeof (Color), typeof (CanvasExBoxItem));
    public static DependencyProperty ScaleTransform_ScaleXProperty = DependencyProperty.Register(nameof (ScaleTransform_ScaleX), typeof (double), typeof (CanvasExBoxItem), new PropertyMetadata((object) 1.0));
    public static DependencyProperty ScaleTransform_ScaleYProperty = DependencyProperty.Register(nameof (ScaleTransform_ScaleY), typeof (double), typeof (CanvasExBoxItem), new PropertyMetadata((object) 1.0));
    public static DependencyProperty RotateTransform_AngleProperty = DependencyProperty.Register(nameof (RotateTransform_Angle), typeof (double), typeof (CanvasExBoxItem), new PropertyMetadata((object) 0.0));
    protected UIElementPositionChangeDragger MovingControl = new UIElementPositionChangeDragger();
    internal Label TitleCtrl;
    internal Border ctrlBorder;
    internal ContentControl ctrlMain;
    private bool _contentLoaded;

    [InstanceMember]
    [GridViewItem(true)]
    public virtual int PaddingValue
    {
      get => this._PaddingValue;
      set
      {
        this._PaddingValue = value;
        this.ctrlBorder.Padding = new Thickness((double) value);
      }
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Header")]
    public double HeaderHeight
    {
      get => (double) this.GetValue(CanvasExBoxItem.HeaderHeightProperty);
      set => this.SetValue(CanvasExBoxItem.HeaderHeightProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Header")]
    public string HeaderTitle
    {
      get => (string) this.GetValue(CanvasExBoxItem.HeaderTitleProperty);
      set => this.SetValue(CanvasExBoxItem.HeaderTitleProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Header")]
    public Color HeaderColor
    {
      get => (Color) this.GetValue(CanvasExBoxItem.HeaderColorProperty);
      set => this.SetValue(CanvasExBoxItem.HeaderColorProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Layout.Transform")]
    public virtual double ScaleTransform_ScaleX
    {
      get => (double) this.GetValue(CanvasExBoxItem.ScaleTransform_ScaleXProperty);
      set => this.SetValue(CanvasExBoxItem.ScaleTransform_ScaleXProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Layout.Transform")]
    public virtual double ScaleTransform_ScaleY
    {
      get => (double) this.GetValue(CanvasExBoxItem.ScaleTransform_ScaleYProperty);
      set => this.SetValue(CanvasExBoxItem.ScaleTransform_ScaleYProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("Layout.Transform")]
    public virtual double RotateTransform_Angle
    {
      get => (double) this.GetValue(CanvasExBoxItem.RotateTransform_AngleProperty);
      set => this.SetValue(CanvasExBoxItem.RotateTransform_AngleProperty, (object) value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public double DesignedHeight
    {
      get => this.Height;
      set => this.Height = value;
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public double DesignedWidth
    {
      get => this.Width;
      set => this.Width = value;
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public double CanvasLeft
    {
      get => Canvas.GetLeft((UIElement) this);
      set => Canvas.SetLeft((UIElement) this, value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public double CanvasTop
    {
      get => Canvas.GetTop((UIElement) this);
      set => Canvas.SetTop((UIElement) this, value);
    }

    [InstanceMember]
    [GridViewItem(true)]
    [Category("UI.Layout")]
    public bool DisableMouseMoving { get; set; }

    [DesignableMember(true)]
    [DeclaredLinkedState(eDeclaredLinkedState.Target)]
    public virtual UIElement Element
    {
      get => this.ctrlMain.Content as UIElement;
      set => this.ctrlMain.Content = (object) value;
    }

    public CanvasExBoxItem()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
    }

    private void Title_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (this.DisableMouseMoving || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.MovingControl.StartDrag((UIElement) this, sender as UIElement);
    }

    private void Title_MouseMove(object sender, MouseEventArgs e)
    {
      if (this.DisableMouseMoving)
        return;
      this.MovingControl.MouseMoved();
    }

    private void Title_MouseUp(object sender, MouseButtonEventArgs e)
    {
      if (this.DisableMouseMoving)
        return;
      this.MovingControl.StopDrag();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/canvasexboxitem.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TitleCtrl = (Label) target;
          this.TitleCtrl.MouseMove += new MouseEventHandler(this.Title_MouseMove);
          this.TitleCtrl.MouseUp += new MouseButtonEventHandler(this.Title_MouseUp);
          this.TitleCtrl.MouseDown += new MouseButtonEventHandler(this.Title_MouseDown);
          break;
        case 2:
          this.ctrlBorder = (Border) target;
          break;
        case 3:
          this.ctrlMain = (ContentControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
