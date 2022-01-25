// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.InputPasswordDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

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
  public partial class InputPasswordDialog : Window, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (InputPasswordDialog), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty InputMessageProperty = DependencyProperty.Register(nameof (InputMessage), typeof (string), typeof (InputPasswordDialog), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.Register(nameof (MessageType), typeof (eNotifyMessageType), typeof (InputPasswordDialog), new PropertyMetadata((object) eNotifyMessageType.Normal, new PropertyChangedCallback(InputPasswordDialog.OnMessageTypeChanged)));
    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof (Buttons), typeof (eDialogButtons), typeof (InputPasswordDialog), new PropertyMetadata((object) eDialogButtons.YesNo, new PropertyChangedCallback(InputPasswordDialog.OnMessageTypeChanged)));
    internal Label TitleCtrl;
    internal System.Windows.Controls.Image imgIcon;
    internal PasswordBox txtInput;
    internal StackPanel ActiveButtons;
    internal Button btnYes;
    internal Button btnNo;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public string Message
    {
      get => (string) this.GetValue(InputPasswordDialog.MessageProperty);
      set => this.SetValue(InputPasswordDialog.MessageProperty, (object) value);
    }

    public string InputMessage
    {
      get => (string) this.GetValue(InputPasswordDialog.InputMessageProperty);
      set => this.SetValue(InputPasswordDialog.InputMessageProperty, (object) value);
    }

    public eNotifyMessageType MessageType
    {
      get => (eNotifyMessageType) this.GetValue(InputPasswordDialog.MessageTypeProperty);
      set => this.SetValue(InputPasswordDialog.MessageTypeProperty, (object) value);
    }

    public eDialogButtons Buttons
    {
      get => (eDialogButtons) this.GetValue(InputPasswordDialog.ButtonsProperty);
      set => this.SetValue(InputPasswordDialog.ButtonsProperty, (object) value);
    }

    public eDialogButtons SelectedButton { get; protected set; }

    public char PasswordChar { get; set; }

    public InputPasswordDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.SelectedButton = eDialogButtons.None;
      this.UpdateIcon();
      this.PasswordChar = '*';
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
      if (!(src is InputPasswordDialog inputPasswordDialog))
        return;
      inputPasswordDialog.UpdateIcon();
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
      this.InputMessage = this.txtInput.Password;
      this.DialogResult = new bool?(true);
    }

    public static string Show(
      Window parent,
      string title,
      string message,
      string input = "",
      eNotifyMessageType mtype = eNotifyMessageType.Question,
      int width = -1,
      int height = -1)
    {
      InputPasswordDialog inputPasswordDialog = new InputPasswordDialog();
      inputPasswordDialog.Message = message;
      inputPasswordDialog.MessageType = mtype;
      inputPasswordDialog.Buttons = eDialogButtons.OkCancel;
      inputPasswordDialog.Owner = parent;
      inputPasswordDialog.InputMessage = input;
      inputPasswordDialog.Title = title;
      if (width != -1)
        inputPasswordDialog.Width = (double) width;
      if (height != -1)
        inputPasswordDialog.Height = (double) height;
      bool? nullable = inputPasswordDialog.ShowDialog();
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue && inputPasswordDialog.SelectedButton == eDialogButtons.OK ? inputPasswordDialog.InputMessage : (string) null;
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.txtInput.Focus();
      this.txtInput.SelectAll();
      this.txtInput.PasswordChar = this.PasswordChar;
      this.txtInput.Password = this.InputMessage;
    }

    private void txtInput_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.InputMessage = this.txtInput.Password;
      this.SelectedButton = eDialogButtons.OK;
      this.DialogResult = new bool?(true);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/inputpassworddialog.xaml", UriKind.Relative));
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
          this.txtInput = (PasswordBox) target;
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
