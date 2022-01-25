// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectFieldsDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Texts;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Dialogs
{
  public partial class SelectFieldsDialog : Window, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (SelectFieldsDialog), new PropertyMetadata((object) ""));
    internal StackPanel FieldCtrls;
    internal Button btnOk;
    internal Button btnCancel;
    private bool _contentLoaded;

    public string Message
    {
      get => (string) this.GetValue(SelectFieldsDialog.MessageProperty);
      set => this.SetValue(SelectFieldsDialog.MessageProperty, (object) value);
    }

    public List<FieldSelectionItem> FieldItems { get; set; }

    public SelectFieldsDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.FieldItems = new List<FieldSelectionItem>();
    }

    private void btnOk_Click(object sender, RoutedEventArgs e)
    {
      if (this.FieldItems.Any<FieldSelectionItem>((Func<FieldSelectionItem, bool>) (x => x.SelectedIndex == -1)))
      {
        int num = (int) this.ShowWarningMessage("관리자", "모든 항목이 선택되어 있어야 합니다.");
      }
      else
        this.DialogResult = new bool?(true);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      foreach (FieldSelectionItem fieldItem in this.FieldItems)
      {
        SelectFieldControl selectFieldControl = new SelectFieldControl();
        selectFieldControl.BorderBrush = (Brush) Brushes.DarkGray;
        selectFieldControl.BorderThickness = new Thickness(1.0);
        selectFieldControl.FieldItem = fieldItem;
        selectFieldControl.Margin = new Thickness(2.0);
        this.FieldCtrls.Children.Add((UIElement) selectFieldControl);
      }
    }

    public static bool Show(Window wnd, string message, params FieldSelectionItem[] items)
    {
      SelectFieldsDialog selectFieldsDialog = new SelectFieldsDialog();
      selectFieldsDialog.Owner = wnd;
      selectFieldsDialog.Message = message;
      selectFieldsDialog.FieldItems = ((IEnumerable<FieldSelectionItem>) items).ToList<FieldSelectionItem>();
      bool? nullable = selectFieldsDialog.ShowDialog();
      bool flag = true;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue;
    }

    public static List<FieldSelectionItem> Show(
      Window wnd,
      string message,
      SeparatedText sv,
      int sample_row,
      params string[] names)
    {
      List<string> csamples = new List<string>();
      for (int col = 0; col < sv.Cols; ++col)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int row = 1; row <= sample_row && row < sv.Rows; ++row)
          stringBuilder.AppendLine(sv.GetValue(row, col) ?? "");
        csamples.Add(stringBuilder.ToString());
      }
      List<FieldSelectionItem> fieldSelectionItemList = new List<FieldSelectionItem>();
      foreach (string name in names)
      {
        FieldSelectionItem fieldSelectionItem = new FieldSelectionItem(name, sv.Names.Fields, csamples);
        fieldSelectionItemList.Add(fieldSelectionItem);
      }
      return SelectFieldsDialog.Show(wnd, message, fieldSelectionItemList.ToArray()) ? fieldSelectionItemList : (List<FieldSelectionItem>) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectfieldsdialog.xaml", UriKind.Relative));
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
          this.FieldCtrls = (StackPanel) target;
          break;
        case 3:
          this.btnOk = (Button) target;
          this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
          break;
        case 4:
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
