// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.NotifyMessageDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Helpers;
using EMx.Logging;
using EMx.UI.Extensions;
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
  public partial class NotifyMessageDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (NotifyMessageDialog), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty MessageTypeProperty = DependencyProperty.Register(nameof (MessageType), typeof (eNotifyMessageType), typeof (NotifyMessageDialog), new PropertyMetadata((object) eNotifyMessageType.Normal, new PropertyChangedCallback(NotifyMessageDialog.OnMessageTypeChanged)));
    public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof (Buttons), typeof (eDialogButtons), typeof (NotifyMessageDialog), new PropertyMetadata((object) eDialogButtons.YesNo, new PropertyChangedCallback(NotifyMessageDialog.OnMessageTypeChanged)));
    private string DialogUID;
    private bool IsChoiceSelected;
    internal Label TitleCtrl;
    internal System.Windows.Controls.Image imgIcon;
    internal StackPanel ActiveButtons;
    internal Button btnYes;
    internal Button btnNo;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public string Message
    {
      get => (string) this.GetValue(NotifyMessageDialog.MessageProperty);
      set => this.SetValue(NotifyMessageDialog.MessageProperty, (object) value);
    }

    public eNotifyMessageType MessageType
    {
      get => (eNotifyMessageType) this.GetValue(NotifyMessageDialog.MessageTypeProperty);
      set => this.SetValue(NotifyMessageDialog.MessageTypeProperty, (object) value);
    }

    public eDialogButtons Buttons
    {
      get => (eDialogButtons) this.GetValue(NotifyMessageDialog.ButtonsProperty);
      set => this.SetValue(NotifyMessageDialog.ButtonsProperty, (object) value);
    }

    public eDialogButtons SelectedButton { get; protected set; }

    public NotifyMessageDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.DialogUID = Helper.Text.MakeRandomCharacters(8);
      this.SelectedButton = eDialogButtons.None;
      this.IsChoiceSelected = false;
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
      if (!(src is NotifyMessageDialog notifyMessageDialog))
        return;
      notifyMessageDialog.UpdateIcon();
    }

    private void ChoiseButton_Clicked(object sender, RoutedEventArgs e)
    {
      this.IsChoiceSelected = true;
      if (sender == this.btnYes)
        this.SelectedButton = eDialogButtons.Yes;
      else if (sender == this.btnNo)
        this.SelectedButton = eDialogButtons.No;
      else if (sender == this.btnOK)
        this.SelectedButton = eDialogButtons.OK;
      else if (sender == this.btnCancel)
        this.SelectedButton = eDialogButtons.Cancel;
      NotifyMessageDialog.log.Debug("Click NotifyDialog[{0}] : ID[{1}] Selected[{2}]", (object) this.MessageType, (object) this.DialogUID, (object) this.SelectedButton);
      if (!this.IsModal())
      {
        this.Close();
      }
      else
      {
        if ((uint) this.SelectedButton <= 0U)
          return;
        this.DialogResult = new bool?(true);
      }
    }

    public static eDialogButtons Show(
      Window parent,
      string title,
      string message,
      eNotifyMessageType mtype = eNotifyMessageType.Normal,
      eDialogButtons btns = eDialogButtons.YesNo,
      int width = -1,
      int height = -1,
      bool modal = true)
    {
      NotifyMessageDialog notifyMessageDialog = new NotifyMessageDialog();
      notifyMessageDialog.Message = message;
      notifyMessageDialog.MessageType = mtype;
      notifyMessageDialog.Buttons = btns;
      notifyMessageDialog.Owner = parent;
      notifyMessageDialog.Title = title;
      if (width != -1)
        notifyMessageDialog.Width = (double) width;
      if (height != -1)
        notifyMessageDialog.Height = (double) height;
      if (modal)
        notifyMessageDialog.ShowDialog();
      else
        notifyMessageDialog.Show();
      return notifyMessageDialog.SelectedButton;
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ActiveButtons.Focus();
      if (this.ActiveButtons.Children.Count > 0)
        this.ActiveButtons.Children[0].Focus();
      NotifyMessageDialog.log.Debug("Popup NotifyDialog[{0}] : ID[{1}] Title[{2}]\r\nMessage:{3}", (object) this.MessageType, (object) this.DialogUID, (object) this.Title, (object) this.Message);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (this.IsChoiceSelected)
        return;
      NotifyMessageDialog.log.Debug("Close NotifyDialog[{0}] with out choice : ID[{1}]", (object) this.MessageType, (object) this.DialogUID);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/notifymessagedialog.xaml", UriKind.Relative));
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
          this.imgIcon = (System.Windows.Controls.Image) target;
          break;
        case 4:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 5:
          this.btnYes = (Button) target;
          this.btnYes.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 6:
          this.btnNo = (Button) target;
          this.btnNo.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 7:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.ChoiseButton_Clicked);
          break;
        case 8:
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
