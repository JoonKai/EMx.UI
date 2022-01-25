// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectDateRangeDialog
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
  public partial class SelectDateRangeDialog : Window, IComponentConnector
  {
    public static DependencyProperty BeginTitleProperty = DependencyProperty.Register(nameof (BeginTitle), typeof (string), typeof (InputRangeDialog), new PropertyMetadata((object) nameof (Begin)));
    public static DependencyProperty EndTitleProperty = DependencyProperty.Register(nameof (EndTitle), typeof (string), typeof (InputRangeDialog), new PropertyMetadata((object) nameof (End)));
    internal Label TitleCtrl;
    internal DatePicker ctrlBeginDate;
    internal DatePicker ctrlEndDate;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual string BeginTitle
    {
      get => (string) this.GetValue(SelectDateRangeDialog.BeginTitleProperty);
      set => this.SetValue(SelectDateRangeDialog.BeginTitleProperty, (object) value);
    }

    public virtual string EndTitle
    {
      get => (string) this.GetValue(SelectDateRangeDialog.EndTitleProperty);
      set => this.SetValue(SelectDateRangeDialog.EndTitleProperty, (object) value);
    }

    public DateTime Begin { get; set; }

    public DateTime End { get; set; }

    public SelectDateRangeDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Title = "Select Date Dialog";
      this.BeginTitle = nameof (Begin);
      this.EndTitle = nameof (End);
      this.Begin = DateTime.MinValue;
      this.End = DateTime.MinValue;
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        DateTime? selectedDate = this.ctrlBeginDate.SelectedDate;
        this.Begin = selectedDate ?? DateTime.MinValue;
        selectedDate = this.ctrlEndDate.SelectedDate;
        this.End = selectedDate ?? DateTime.MinValue;
        this.DialogResult = new bool?(true);
      }
      catch (Exception ex)
      {
        int num = (int) this.ShowWarningMessage("Manager", "Error : " + ex.Message);
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ctrlBeginDate.SelectedDate = new DateTime?(this.Begin);
      this.ctrlEndDate.SelectedDate = new DateTime?(this.End);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectdaterangedialog.xaml", UriKind.Relative));
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
          this.ctrlBeginDate = (DatePicker) target;
          break;
        case 4:
          this.ctrlEndDate = (DatePicker) target;
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
