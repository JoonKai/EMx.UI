// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectFieldControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  public partial class SelectFieldControl : UserControl, IComponentConnector
  {
    internal Label lblFieldName;
    internal ComboBox cmbSamples;
    internal Label Sample;
    private bool _contentLoaded;

    public FieldSelectionItem FieldItem { get; set; }

    public SelectFieldControl()
    {
      this.InitializeComponent();
      this.FieldItem = new FieldSelectionItem();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      this.lblFieldName.Content = (object) this.FieldItem.FieldName;
      this.cmbSamples.ItemsSource = (IEnumerable) this.FieldItem.CandidateNames;
    }

    private void cmbSamples_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int selectedIndex = this.cmbSamples.SelectedIndex;
      this.FieldItem.SelectedIndex = selectedIndex;
      if (selectedIndex <= -1 || selectedIndex >= this.FieldItem.CandidateSamples.Count)
        return;
      this.Sample.Content = (object) this.FieldItem.CandidateSamples[selectedIndex];
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectfieldcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.UserControl_Loaded);
          break;
        case 2:
          this.lblFieldName = (Label) target;
          break;
        case 3:
          this.cmbSamples = (ComboBox) target;
          this.cmbSamples.SelectionChanged += new SelectionChangedEventHandler(this.cmbSamples_SelectionChanged);
          break;
        case 4:
          this.Sample = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
