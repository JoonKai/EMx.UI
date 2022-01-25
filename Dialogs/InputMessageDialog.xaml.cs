// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.InputMessageDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using EMx.UI.Properties;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Dialogs
{
  [InstanceContract(ClassID = "53631ab3-13bb-4db9-9216-30aadbcf5d92")]
  public partial class InputMessageDialog : Window, IManagedType, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (InputMessageDialog), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty InputMessageProperty = DependencyProperty.Register(nameof (InputMessage), typeof (string), typeof (InputMessageDialog), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.Register(nameof (MessageType), typeof (eNotifyMessageType), typeof (InputMessageDialog), new PropertyMetadata((object) eNotifyMessageType.Normal, new PropertyChangedCallback(InputMessageDialog.OnMessageTypeChanged)));
    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof (Buttons), typeof (eDialogButtons), typeof (InputMessageDialog), new PropertyMetadata((object) eDialogButtons.YesNo, new PropertyChangedCallback(InputMessageDialog.OnMessageTypeChanged)));
    internal Label TitleCtrl;
    internal System.Windows.Controls.Image imgIcon;
    internal TextBox txtInput;
    internal StackPanel ActiveButtons;
    internal Button btnYes;
    internal Button btnNo;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public string Message
    {
      get => (string) this.GetValue(InputMessageDialog.MessageProperty);
      set => this.SetValue(InputMessageDialog.MessageProperty, (object) value);
    }

    public string InputMessage
    {
      get => (string) this.GetValue(InputMessageDialog.InputMessageProperty);
      set => this.SetValue(InputMessageDialog.InputMessageProperty, (object) value);
    }

    public eNotifyMessageType MessageType
    {
      get => (eNotifyMessageType) this.GetValue(InputMessageDialog.MessageTypeProperty);
      set => this.SetValue(InputMessageDialog.MessageTypeProperty, (object) value);
    }

    public eDialogButtons Buttons
    {
      get => (eDialogButtons) this.GetValue(InputMessageDialog.ButtonsProperty);
      set => this.SetValue(InputMessageDialog.ButtonsProperty, (object) value);
    }

    public eDialogButtons SelectedButton { get; protected set; }

    public InputMessageDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.SelectedButton = eDialogButtons.None;
      this.UpdateIcon();
    }

    public void UpdateIcon()
    {
      Bitmap bitmap = Resources.message;
      switch (this.MessageType)
      {
        case eNotifyMessageType.Normal:
          this.TitleCtrl.Background = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 51, (byte) 101, (byte) 152));
          bitmap = Resources.message;
          break;
        case eNotifyMessageType.Question:
          this.TitleCtrl.Background = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Green;
          bitmap = Resources.question;
          break;
        case eNotifyMessageType.Warning:
          this.TitleCtrl.Background = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Orange;
          bitmap = Resources.warning;
          break;
        case eNotifyMessageType.Error:
          this.TitleCtrl.Background = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte) 254, (byte) 0, (byte) 0));
          bitmap = Resources.error;
          break;
      }
      this.imgIcon.Source = (ImageSource) System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
      this.btnYes.Visibility = (this.Buttons & eDialogButtons.Yes) != eDialogButtons.None ? Visibility.Visible : Visibility.Hidden;
      this.btnNo.Visibility = (this.Buttons & eDialogButtons.No) != eDialogButtons.None ? Visibility.Visible : Visibility.Hidden;
      this.btnOK.Visibility = (this.Buttons & eDialogButtons.OK) != eDialogButtons.None ? Visibility.Visible : Visibility.Hidden;
      this.btnCancel.Visibility = (this.Buttons & eDialogButtons.Cancel) != eDialogButtons.None ? Visibility.Visible : Visibility.Hidden;
      List<Button> buttonList = new List<Button>()
      {
        this.btnYes,
        this.btnNo,
        this.btnOK,
        this.btnCancel
      };
      this.ActiveButtons.Children.Clear();
      buttonList.ForEach((Action<Button>) (x =>
      {
        if (x.Visibility != Visibility.Visible)
          return;
        this.ActiveButtons.Children.Add((UIElement) x);
      }));
    }

    private static void OnMessageTypeChanged(
      DependencyObject src,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(src is InputMessageDialog inputMessageDialog))
        return;
      inputMessageDialog.UpdateIcon();
    }

    private void ChoiseButton_Clicked(object sender, RoutedEventArgs e)
    {
      if (sender == this.btnYes)
        this.SelectedButton = eDialogButtons.Yes;
      else if (sender == this.btnNo)
        this.SelectedButton = eDialogButtons.No;
      else if (sender == this.btnOK)
        this.SelectedButton = eDialogButtons.OK;
      else if (sender == this.btnCancel)
        this.SelectedButton = eDialogButtons.Cancel;
      if ((uint) this.SelectedButton <= 0U)
        return;
      this.DialogResult = new bool?(true);
    }

    public static string Show(
      Window parent,
      string title,
      string message,
      string input,
      eNotifyMessageType mtype = eNotifyMessageType.Question,
      int width = -1,
      int height = -1)
    {
      InputMessageDialog inputMessageDialog = new InputMessageDialog();
      inputMessageDialog.Message = message;
      inputMessageDialog.MessageType = mtype;
      inputMessageDialog.Buttons = eDialogButtons.OkCancel;
      inputMessageDialog.Owner = parent;
      inputMessageDialog.InputMessage = input;
      inputMessageDialog.Title = title;
      if (width != -1)
        inputMessageDialog.Width = (double) width;
      if (height != -1)
        inputMessageDialog.Height = (double) height;
      bool? nullable = inputMessageDialog.ShowDialog();
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue && inputMessageDialog.SelectedButton == eDialogButtons.OK ? inputMessageDialog.InputMessage : (string) null;
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.txtInput.Focus();
      this.txtInput.SelectAll();
    }

    private void txtInput_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        e.Handled = true;
        this.SelectedButton = eDialogButtons.OK;
        this.DialogResult = new bool?(true);
      }
      else
      {
        if (e.Key != Key.Escape)
          return;
        e.Handled = true;
        this.DialogResult = new bool?(false);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/inputmessagedialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.TitleCtrl = (Label) target;
          this.TitleCtrl.MouseDown += new MouseButtonEventHandler(this.TitleCtrl_MouseDown);
          break;
        case 3:
          this.imgIcon = (System.Windows.Controls.Image) target;
          break;
        case 4:
          this.txtInput = (TextBox) target;
          this.txtInput.KeyDown += new KeyEventHandler(this.txtInput_KeyDown);
          break;
        case 5:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 6:
          this.btnYes = (Button) target;
          this.btnYes.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 7:
          this.btnNo = (Button) target;
          this.btnNo.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 8:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 9:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
