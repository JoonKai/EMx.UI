// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.InputRangeDialog
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
  public partial class InputRangeDialog : Window, IComponentConnector
  {
    public static DependencyProperty BeginTitleProperty = DependencyProperty.Register(nameof (BeginTitle), typeof (string), typeof (InputRangeDialog), new PropertyMetadata((object) nameof (Begin)));
    public static DependencyProperty EndTitleProperty = DependencyProperty.Register(nameof (EndTitle), typeof (string), typeof (InputRangeDialog), new PropertyMetadata((object) nameof (End)));
    internal Label TitleCtrl;
    internal TextBox txtBegin;
    internal TextBox txtEnd;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual double Begin { get; set; }

    public virtual double End { get; set; }

    public virtual string BeginTitle
    {
      get => (string) this.GetValue(InputRangeDialog.BeginTitleProperty);
      set => this.SetValue(InputRangeDialog.BeginTitleProperty, (object) value);
    }

    public virtual string EndTitle
    {
      get => (string) this.GetValue(InputRangeDialog.EndTitleProperty);
      set => this.SetValue(InputRangeDialog.EndTitleProperty, (object) value);
    }

    public InputRangeDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Title = "Input Range Dialog";
      this.BeginTitle = nameof (Begin);
      this.EndTitle = nameof (End);
      this.txtBegin.GotFocus += new RoutedEventHandler(this.OnBeginGotFocus);
      this.txtEnd.GotFocus += new RoutedEventHandler(this.OnEndGotFocus);
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.txtEnd.Text = this.End.ToString();
      this.txtBegin.Text = this.Begin.ToString();
      this.txtBegin.Focus();
      this.txtBegin.SelectAll();
    }

    private void OnEndGotFocus(object sender, RoutedEventArgs e) => this.txtEnd.SelectAll();

    private void OnBeginGotFocus(object sender, RoutedEventArgs e) => this.txtBegin.SelectAll();

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Begin = Convert.ToDouble(this.txtBegin.Text);
        this.End = Convert.ToDouble(this.txtEnd.Text);
        this.DialogResult = new bool?(true);
      }
      catch (Exception ex)
      {
        int num = (int) this.ShowWarningMessage("Manager", string.Format("Error on converting text type to double.\r\n{0}", (object) ex.Message));
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void txtBegin_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.txtEnd.Focus();
      this.txtEnd.SelectAll();
    }

    private void txtEnd_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.btnOK_Click(sender, (RoutedEventArgs) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/inputrangedialog.xaml", UriKind.Relative));
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
          this.txtBegin = (TextBox) target;
          this.txtBegin.KeyDown += new KeyEventHandler(this.txtBegin_KeyDown);
          break;
        case 4:
          this.txtEnd = (TextBox) target;
          this.txtEnd.KeyDown += new KeyEventHandler(this.txtEnd_KeyDown);
          break;
        case 5:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 6:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
          break;
        case 7:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
