// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.WaitingBarDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

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
  public partial class WaitingBarDialog : Window, IComponentConnector
  {
    internal Label TitleCtrl;
    internal ColumnDefinition CurrentField;
    internal ColumnDefinition RemainField;
    internal Label CurrentProcessCtrl;
    internal TextBox txtLog;
    private bool _contentLoaded;

    public WaitingBarDialog() => this.InitializeComponent();

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    public virtual void UpdateProcessRate(double process_rate)
    {
      double num = Math.Max(0.0, Math.Min(1.0, process_rate)) * 100.0;
      this.CurrentField.Width = new GridLength(num, GridUnitType.Star);
      this.RemainField.Width = new GridLength(100.0 - num, GridUnitType.Star);
      this.CurrentProcessCtrl.Content = (object) string.Format("{0:0.0}%", (object) num);
      Application.Current.DoEvents();
    }

    public virtual void AppendMessage(string message)
    {
      TextBox txtLog = this.txtLog;
      txtLog.Text = txtLog.Text + message + Environment.NewLine;
      this.txtLog.ScrollToEnd();
      Application.Current.DoEvents();
    }

    public virtual void ClearMessage() => this.txtLog.Text = "";

    public virtual void RequestClose() => this.Dispatcher.Invoke(new Action(((Window) this).Close));

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/waitingbardialog.xaml", UriKind.Relative));
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
          this.TitleCtrl.MouseDown += new MouseButtonEventHandler(this.TitleCtrl_MouseDown);
          break;
        case 2:
          this.CurrentField = (ColumnDefinition) target;
          break;
        case 3:
          this.RemainField = (ColumnDefinition) target;
          break;
        case 4:
          this.CurrentProcessCtrl = (Label) target;
          break;
        case 5:
          this.txtLog = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
