// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectMultiItemsDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  public partial class SelectMultiItemsDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public Func<List<SelectMultiItemsData>, string> UpdateStatusMessageHandler;
    internal Label TitleCtrl;
    internal DataGrid ctrlGrid;
    internal DataGridTextColumn txtFieldName;
    internal Label lblStatus;
    internal StackPanel ActiveButtons;
    internal Button btnClear;
    internal Button btnSelectAll;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual string FieldName
    {
      get => this.txtFieldName.Header.ToString();
      set => this.txtFieldName.Header = (object) value;
    }

    public virtual string StatusMessage
    {
      get => this.lblStatus.Content as string;
      set => this.lblStatus.Content = (object) value;
    }

    public List<SelectMultiItemsData> Items { get; set; }

    public SelectMultiItemsDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.Items = new List<SelectMultiItemsData>();
      this.UpdateStatusMessageHandler = (Func<List<SelectMultiItemsData>, string>) null;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if ((this.Items.Count == 0 || this.Items.Count<SelectMultiItemsData>((Func<SelectMultiItemsData, bool>) (x => x.IsSelected)) == 0) && this.ShowQuestionMessage("Manager", "There is no selected items. Continue?") != eDialogButtons.Yes)
        return;
      this.DialogResult = new bool?(true);
    }

    private void btnSelectAll_Click(object sender, RoutedEventArgs e)
    {
      foreach (SelectMultiItemsData selectMultiItemsData in this.Items)
        selectMultiItemsData.IsSelected = true;
      this.UpdateStatusMessage();
    }

    private void btnClear_Click(object sender, RoutedEventArgs e)
    {
      foreach (SelectMultiItemsData selectMultiItemsData in this.Items)
        selectMultiItemsData.IsSelected = false;
      this.UpdateStatusMessage();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ctrlGrid.ItemsSource = (IEnumerable) this.Items;
      this.ctrlGrid.SelectedIndex = this.Items.FindIndex((Predicate<SelectMultiItemsData>) (x => x.IsSelected));
      if (this.ctrlGrid.SelectedIndex == -1)
        this.ctrlGrid.SelectedIndex = 0;
      this.ctrlGrid.SmartFocus();
      this.UpdateStatusMessage();
    }

    private void ctrlGridItem_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        e.Handled = true;
        this.btnOK_Click(sender, (RoutedEventArgs) null);
      }
      else if (e.Key == Key.Escape)
      {
        e.Handled = true;
        this.DialogResult = new bool?(false);
      }
      else
      {
        if (e.Key != Key.Space)
          return;
        e.Handled = true;
        foreach (SelectMultiItemsData selectedItem in (IEnumerable) this.ctrlGrid.SelectedItems)
        {
          if (selectedItem != null)
            selectedItem.IsSelected = !selectedItem.IsSelected;
        }
        this.UpdateStatusMessage();
      }
    }

    private void ctrlGrid_MouseUp(object sender, MouseButtonEventArgs e) => this.UpdateStatusMessage();

    private void UpdateStatusMessage()
    {
      if (this.UpdateStatusMessageHandler == null)
        return;
      try
      {
        this.lblStatus.Content = (object) this.UpdateStatusMessageHandler(this.Items);
      }
      catch (Exception ex)
      {
        SelectMultiItemsDialog.log.Error(ex, ex.Message);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectmultiitemsdialog.xaml", UriKind.Relative));
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
          this.ctrlGrid = (DataGrid) target;
          this.ctrlGrid.PreviewKeyDown += new KeyEventHandler(this.ctrlGridItem_KeyDown);
          this.ctrlGrid.MouseUp += new MouseButtonEventHandler(this.ctrlGrid_MouseUp);
          break;
        case 4:
          this.txtFieldName = (DataGridTextColumn) target;
          break;
        case 5:
          this.lblStatus = (Label) target;
          break;
        case 6:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 7:
          this.btnClear = (Button) target;
          this.btnClear.Click += new RoutedEventHandler(this.btnClear_Click);
          break;
        case 8:
          this.btnSelectAll = (Button) target;
          this.btnSelectAll.Click += new RoutedEventHandler(this.btnSelectAll_Click);
          break;
        case 9:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
          break;
        case 10:
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
