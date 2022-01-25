// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.WaitingDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Helpers;
using EMx.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace EMx.UI.Dialogs
{
  public partial class WaitingDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (WaitingDialog));
    protected DispatcherTimer Timer;
    protected int Count;
    internal Label TitleCtrl;
    internal StackPanel ActiveButtons;
    private bool _contentLoaded;

    public string Message
    {
      get => (string) this.GetValue(WaitingDialog.MessageProperty);
      set => this.SetValue(WaitingDialog.MessageProperty, (object) value);
    }

    [DesignableMember(true)]
    public Var<bool> ReqClose { get; set; }

    public WaitingDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Timer = new DispatcherTimer();
      this.ReqClose = new Var<bool>();
      this.Timer.Tick += new EventHandler(this.OnUpdateStatus);
      this.Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
    }

    private void OnUpdateStatus(object sender, EventArgs e)
    {
      this.Message = Helper.Text.MultiplyText(".", 3 + this.Count++ / 3 % 8);
      if (this.ReqClose == null || !this.ReqClose.Value)
        return;
      this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.OnUpdateStatus(sender, (EventArgs) null);
      this.Timer.Start();
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.Timer.Stop();

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    public static WaitingDialog ShowWaitingDialog(
      Window wnd,
      Var<bool> flag,
      bool async)
    {
      WaitingDialog waitingDialog = new WaitingDialog();
      if (flag != null)
        waitingDialog.ReqClose = flag;
      if (wnd != null)
        waitingDialog.Owner = wnd;
      else
        waitingDialog.Owner = Application.Current.MainWindow;
      waitingDialog.Message = "...";
      if (async)
        waitingDialog.Show();
      else
        waitingDialog.ShowDialog();
      return waitingDialog;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/waitingdialog.xaml", UriKind.Relative));
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
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.TitleCtrl = (Label) target;
          this.TitleCtrl.MouseDown += new MouseButtonEventHandler(this.TitleCtrl_MouseDown);
          break;
        case 3:
          this.ActiveButtons = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
