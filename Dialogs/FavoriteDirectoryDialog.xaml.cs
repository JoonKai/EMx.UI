// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.FavoriteDirectoryDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using EMx.UI.Extensions;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  [InstanceContract(ClassID = "05c983c2-8624-496b-b604-a1ac7ebd7cc7")]
  public partial class FavoriteDirectoryDialog : Window, IManagedType, IComponentConnector
  {
    internal DataGrid ctrlGrid;
    private bool _contentLoaded;

    public List<FavoriteDirectoryItem> FavoriteItems { get; set; }

    public FavoriteDirectoryItem SelectedItem { get; set; }

    public FavoriteDirectoryDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.FavoriteItems = new List<FavoriteDirectoryItem>();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => this.Ordering();

    private void btnOK_Clicked(object sender, RoutedEventArgs e)
    {
      int selectedIndex = this.ctrlGrid.SelectedIndex;
      if (selectedIndex == -1)
      {
        int num = (int) this.ShowWarningMessage("Manager", "Please select a item to use.");
      }
      else
      {
        if (selectedIndex <= -1 || selectedIndex >= this.FavoriteItems.Count)
          return;
        this.SelectedItem = this.FavoriteItems[selectedIndex];
        ++this.SelectedItem.HitCount;
        this.DialogResult = new bool?(true);
      }
    }

    private void btnCancel_Clicked(object sender, RoutedEventArgs e)
    {
      this.SelectedItem = (FavoriteDirectoryItem) null;
      this.DialogResult = new bool?(false);
    }

    private void mnuNewItem_Clicked(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.CheckFileExists = false;
      openFileDialog.CheckPathExists = false;
      openFileDialog.Multiselect = false;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.ValidateNames = false;
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      string fullName = Directory.GetParent(openFileDialog.FileName).FullName;
      this.FavoriteItems.Add(new FavoriteDirectoryItem()
      {
        DirectoryPath = fullName
      });
      this.Ordering();
    }

    private void mnuDeleteItem_Clicked(object sender, RoutedEventArgs e)
    {
      int selectedIndex = this.ctrlGrid.SelectedIndex;
      if (selectedIndex == -1)
        return;
      this.FavoriteItems.RemoveAt(selectedIndex);
      this.Ordering();
    }

    private void mnuClearItems_Clicked(object sender, RoutedEventArgs e)
    {
      this.FavoriteItems.Clear();
      this.Ordering();
    }

    private void mnuSelectPath_Clicked(object sender, RoutedEventArgs e)
    {
      int selectedIndex = this.ctrlGrid.SelectedIndex;
      if (selectedIndex == -1)
        return;
      FavoriteDirectoryItem favoriteItem = this.FavoriteItems[selectedIndex];
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.CheckFileExists = false;
      openFileDialog.CheckPathExists = false;
      openFileDialog.Multiselect = false;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.ValidateNames = false;
      openFileDialog.InitialDirectory = favoriteItem.DirectoryPath;
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      string fullName = Directory.GetParent(openFileDialog.FileName).FullName;
      favoriteItem.DirectoryPath = fullName;
      this.Ordering();
    }

    private void Ordering()
    {
      lock (this.FavoriteItems)
      {
        int n = 0;
        this.FavoriteItems = this.FavoriteItems.OrderByDescending<FavoriteDirectoryItem, int>((Func<FavoriteDirectoryItem, int>) (x => x.HitCount)).ToList<FavoriteDirectoryItem>();
        this.FavoriteItems.ForEach((Action<FavoriteDirectoryItem>) (x => x.No = ++n));
        this.ctrlGrid.ItemsSource = (IEnumerable) null;
        this.ctrlGrid.ItemsSource = (IEnumerable) this.FavoriteItems;
      }
    }

    private void ctrlGrid_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.btnOK_Clicked(sender, (RoutedEventArgs) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/favoritedirectorydialog.xaml", UriKind.Relative));
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
          this.ctrlGrid = (DataGrid) target;
          this.ctrlGrid.PreviewKeyDown += new KeyEventHandler(this.ctrlGrid_PreviewKeyDown);
          break;
        case 3:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuNewItem_Clicked);
          break;
        case 4:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuDeleteItem_Clicked);
          break;
        case 5:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuClearItems_Clicked);
          break;
        case 6:
          ((MenuItem) target).Click += new RoutedEventHandler(this.mnuSelectPath_Clicked);
          break;
        case 7:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnOK_Clicked);
          break;
        case 8:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnCancel_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
