// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.WaitingRunnableDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using EMx.Processes;
using EMx.UI.Controls;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  public partial class WaitingRunnableDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    protected RunnableTimer UpdateUIThread;
    protected int PushStopButtonCount;
    internal Label TitleCtrl;
    internal RadialProgressControl ctrlProgress;
    internal TextBox txtLog;
    internal Button btnStop;
    private bool _contentLoaded;

    public Runnable RunnableObject { get; set; }

    public int TryCountToStop { get; set; }

    public int TimePeriod
    {
      get => this.UpdateUIThread.TimePeriod;
      set => this.UpdateUIThread.TimePeriod = value;
    }

    public WaitingRunnableDialog()
    {
      this.InitializeComponent();
      this.TryCountToStop = 2;
      this.UpdateUIThread = new RunnableTimer(new Action(this.DoUpdateUI), "update.ui");
      this.UpdateUIThread.TimePeriod = 66;
      this.UpdateUIThread.UseBackground = true;
      this.Title = "Waiting Dialog";
      this.DataContext = (object) this;
      this.Owner = Application.Current.MainWindow;
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void DoUpdateUI()
    {
      if (this.RunnableObject == null)
        return;
      this.Dispatcher.BeginAction(0, (Action) (() => this.ctrlProgress.Value = this.RunnableObject.Progress.Value));
      if (this.RunnableObject.IsRunning)
        return;
      this.AppendText("{0} was finished.", (object) this.RunnableObject.Name);
      this.Dispatcher.Invoke((Action) (() => this.DialogResult = new bool?(true)));
    }

    private void btnStop_Click(object sender, RoutedEventArgs e)
    {
      if (this.RunnableObject == null)
        this.Close();
      else if (this.RunnableObject.IsRun)
      {
        this.RunnableObject.RunFlag.Value = false;
        if (++this.PushStopButtonCount == this.TryCountToStop)
        {
          this.btnStop.Content = (object) "Abort";
        }
        else
        {
          if (this.PushStopButtonCount <= this.TryCountToStop)
            return;
          WaitingRunnableDialog.log.Warn("Abort Runnable[{0}] by user.", (object) this.RunnableObject.Name);
          this.RunnableObject.Stop();
          this.Close();
        }
      }
      else
        this.Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.RunnableObject == null)
      {
        this.AppendText("RunnableObject is null.");
      }
      else
      {
        this.RunnableObject.ProgressMessageEvent += new Action<Runnable, string>(this.OnProgressMessage);
        if (this.RunnableObject.IsStarted)
          this.AppendText("'{0}' has started at {1}", (object) this.RunnableObject.Name, (object) this.RunnableObject.StartTime.ToString("HH:mm:ss"));
        else
          this.AppendText("'{0}' will start.", (object) this.RunnableObject.Name);
        this.UpdateUIThread.Start();
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      this.UpdateUIThread.RunFlag.Value = false;
      if (this.RunnableObject == null)
        return;
      this.RunnableObject.ProgressMessageEvent -= new Action<Runnable, string>(this.OnProgressMessage);
    }

    private void OnProgressMessage(Runnable run, string message) => this.AppendText(message);

    private void AppendText(string fmt, params object[] args)
    {
      string msg = args != null ? string.Format(fmt, args) : fmt;
      WaitingRunnableDialog.log.Debug((Action<LoggingToken>) (x =>
      {
        x.Message = msg;
        x.StackHops = 1;
      }));
      this.txtLog.Dispatcher.BeginAction(0, (Action) (() =>
      {
        this.txtLog.AppendText(msg + Environment.NewLine);
        this.txtLog.ScrollToEnd();
      }));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/waitingrunnabledialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

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
          this.ctrlProgress = (RadialProgressControl) target;
          break;
        case 4:
          this.txtLog = (TextBox) target;
          break;
        case 5:
          this.btnStop = (Button) target;
          this.btnStop.Click += new RoutedEventHandler(this.btnStop_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
