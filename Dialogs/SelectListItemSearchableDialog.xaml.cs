// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectListItemSearchableDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Texts.Patterns;
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
  public partial class SelectListItemSearchableDialog : Window, IComponentConnector
  {
    public Func<object, string> NameGetter = (Func<object, string>) (x => (x ?? (object) "").ToString());
    internal TextBox txtTypeKeys;
    internal ListBox lstItem;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual object SelectedObject => !(this.lstItem.SelectedItem is PatternMatchResult selectedItem) ? (object) null : selectedItem.Source;

    public virtual IEnumerable ItemSource { get; set; }

    public bool HideNoPointedItems { get; set; }

    public string InitialName { get; set; }

    public SelectListItemSearchableDialog()
    {
      this.InitializeComponent();
      this.InitialName = "";
      this.DataContext = (object) this;
      this.HideNoPointedItems = true;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (this.lstItem.SelectedIndex == -1)
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
      this.txtTypeKeys.Focus();
      this.txtTypeKeys.Text = this.InitialName;
      this.SmartSearch(this.InitialName);
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

    private void txtTypeKeys_TextChanged(object sender, TextChangedEventArgs e) => this.SmartSearch(this.txtTypeKeys.Text);

    private void txtTypeKeys_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
      {
        if (this.lstItem.SelectedIndex <= -1)
          return;
        e.Handled = true;
        this.DialogResult = new bool?(true);
      }
      else
      {
        if (e.Key != Key.Escape)
          return;
        if (this.txtTypeKeys.Text.Length > 0)
          this.txtTypeKeys.Clear();
        else
          this.DialogResult = new bool?(false);
      }
    }

    private void SmartSearch(string m)
    {
      List<object> list1 = this.ItemSource.GetEnumerator().ToList();
      List<string> list2 = list1.Select<object, string>((Func<object, string>) (x => this.NameGetter(x))).ToList<string>();
      string text = "";
      if (list2.Count > 1)
      {
        text = list2[0];
        for (int index = 1; index < list2.Count; ++index)
        {
          int length = text.CompareCount(0, list2[index], 0, text.Length);
          if (length == 0)
          {
            text = "";
            break;
          }
          text = text.Substring(0, length);
        }
      }
      int length1 = text.Length;
      TextPatternMatcher textPatternMatcher = new TextPatternMatcher();
      List<PatternMatchResult> patternMatchResultList = new List<PatternMatchResult>();
      for (int index = 0; index < list1.Count; ++index)
      {
        PatternMatchResult patternMatchResult = textPatternMatcher.Match(m, list2[index], length1);
        patternMatchResult.Source = list1[index];
        patternMatchResult.IgnoredPrefixLength = length1;
        patternMatchResultList.Add(patternMatchResult);
      }
      patternMatchResultList.SortExt<PatternMatchResult>((Comparison<PatternMatchResult>) ((a, b) => b.CachedTotalPoints.CompareTo(a.CachedTotalPoints)));
      if (!string.IsNullOrWhiteSpace(m) && patternMatchResultList.Count > 0 && patternMatchResultList.First<PatternMatchResult>().CachedTotalPoints > 0.0)
        patternMatchResultList = patternMatchResultList.Where<PatternMatchResult>((Func<PatternMatchResult, bool>) (x => x.CachedTotalPoints > 0.0)).ToList<PatternMatchResult>();
      this.lstItem.ItemsSource = (IEnumerable) patternMatchResultList;
      if (patternMatchResultList.Count > 0)
        this.lstItem.SelectedIndex = 0;
      else
        this.lstItem.SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectlistitemsearchabledialog.xaml", UriKind.Relative));
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
          ((UIElement) target).MouseDown += new MouseButtonEventHandler(this.TitleCtrl_MouseDown);
          break;
        case 3:
          this.txtTypeKeys = (TextBox) target;
          this.txtTypeKeys.TextChanged += new TextChangedEventHandler(this.txtTypeKeys_TextChanged);
          this.txtTypeKeys.KeyDown += new KeyEventHandler(this.txtTypeKeys_KeyDown);
          break;
        case 4:
          this.lstItem = (ListBox) target;
          this.lstItem.KeyDown += new KeyEventHandler(this.lstItem_KeyDown);
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
