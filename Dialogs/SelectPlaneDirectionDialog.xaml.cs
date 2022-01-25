// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectPlaneDirectionDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data;
using EMx.Logging;
using EMx.UI.Extensions;
using EMx.UI.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Dialogs
{
  public partial class SelectPlaneDirectionDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static DependencyProperty IsFlipXProperty = DependencyProperty.Register(nameof (IsFlipX), typeof (bool), typeof (SelectPlaneDirectionDialog), new PropertyMetadata(new PropertyChangedCallback(SelectPlaneDirectionDialog.OnPropertyChanged)));
    public static DependencyProperty IsFlipYProperty = DependencyProperty.Register(nameof (IsFlipY), typeof (bool), typeof (SelectPlaneDirectionDialog), new PropertyMetadata(new PropertyChangedCallback(SelectPlaneDirectionDialog.OnPropertyChanged)));
    public static DependencyProperty IsRotatedProperty = DependencyProperty.Register(nameof (IsRotated), typeof (bool), typeof (SelectPlaneDirectionDialog), new PropertyMetadata(new PropertyChangedCallback(SelectPlaneDirectionDialog.OnPropertyChanged)));
    internal Label TitleCtrl;
    internal Image imgIcon;
    internal RotateTransform imgRotated;
    internal ScaleTransform imgScale;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public ePlaneDirection PlaneDirection { get; set; }

    public virtual bool IsFlipX
    {
      get => (bool) this.GetValue(SelectPlaneDirectionDialog.IsFlipXProperty);
      set => this.SetValue(SelectPlaneDirectionDialog.IsFlipXProperty, (object) value);
    }

    public virtual bool IsFlipY
    {
      get => (bool) this.GetValue(SelectPlaneDirectionDialog.IsFlipYProperty);
      set => this.SetValue(SelectPlaneDirectionDialog.IsFlipYProperty, (object) value);
    }

    public virtual bool IsRotated
    {
      get => (bool) this.GetValue(SelectPlaneDirectionDialog.IsRotatedProperty);
      set => this.SetValue(SelectPlaneDirectionDialog.IsRotatedProperty, (object) value);
    }

    public SelectPlaneDirectionDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.PlaneDirection = ePlaneDirection.None;
      this.imgIcon.Source = (ImageSource) Resources.read_direction.ToImageSource();
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ActiveButtons.Focus();
      if (this.ActiveButtons.Children.Count > 0)
        this.ActiveButtons.Children[0].Focus();
      SelectPlaneDirectionDialog.log.Debug("Popup SelectPlaneDirectionDialog : Title[{0}]", (object) this.Title);
    }

    private void Window_Closing(object sender, CancelEventArgs e) => SelectPlaneDirectionDialog.log.Debug("Close SelectPlaneDirectionDialog : Title[{0}]", (object) this.Title);

    private void btnOK_Clicked(object sender, RoutedEventArgs e)
    {
      this.PlaneDirection = (ePlaneDirection) ((this.IsRotated ? 1 : 0) | (this.IsFlipX ? 2 : 0) | (this.IsFlipY ? 4 : 0));
      SelectPlaneDirectionDialog.log.Debug("OK SelectPlaneDirectionDialog : Title[{0}], Direction[{1}]", (object) this.Title, (object) this.PlaneDirection);
      this.DialogResult = new bool?(true);
    }

    private void btnCancel_Clicked(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!(d is SelectPlaneDirectionDialog planeDirectionDialog))
        return;
      planeDirectionDialog.imgScale.ScaleX = planeDirectionDialog.IsFlipX ? -1.0 : 1.0;
      planeDirectionDialog.imgScale.ScaleY = planeDirectionDialog.IsFlipY ? -1.0 : 1.0;
      planeDirectionDialog.imgRotated.Angle = planeDirectionDialog.IsRotated ? 90.0 : 0.0;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectplanedirectiondialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.TitleCtrl = (Label) target;
          this.TitleCtrl.MouseDown += new MouseButtonEventHandler(this.TitleCtrl_MouseDown);
          break;
        case 3:
          this.imgIcon = (Image) target;
          break;
        case 4:
          this.imgRotated = (RotateTransform) target;
          break;
        case 5:
          this.imgScale = (ScaleTransform) target;
          break;
        case 6:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 7:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Clicked);
          break;
        case 8:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
