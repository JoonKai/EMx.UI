// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectListItemDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.UI.Extensions;
using EMx.UI.MxControls.Props;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  public partial class SelectListItemDialog : Window, IComponentConnector
  {
    internal Label TitleCtrl;
    internal ListBox lstItem;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual int SelectedIndex
    {
      get => this.lstItem.SelectedIndex;
      set => this.lstItem.SelectedIndex = value;
    }

    public virtual IEnumerable ItemSource
    {
      get => this.lstItem.ItemsSource;
      set => this.lstItem.ItemsSource = value;
    }

    public SelectListItemDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
    }

    public double ItemFontSize
    {
      get => this.lstItem.FontSize;
      set => this.lstItem.FontSize = value;
    }

    public eMxFontWeight ItemFontWeight
    {
      get => (eMxFontWeight) this.lstItem.FontWeight.ToOpenTypeWeight();
      set => this.lstItem.FontWeight = FontWeight.FromOpenTypeWeight((int) value);
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (this.SelectedIndex == -1)
      {
        int num = (int) this.ShowWarningMessage("Manager", "Please select a item.");
      }
      else
        this.DialogResult = new bool?(true);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.Owner == null)
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      this.lstItem.SmartFocus();
    }

    private void lstItem_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        e.Handled = true;
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
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectlistitemdialog.xaml", UriKind.Relative));
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
          this.lstItem = (ListBox) target;
          this.lstItem.KeyDown += new KeyEventHandler(this.lstItem_KeyDown);
          break;
        case 4:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 5:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
          break;
        case 6:
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
