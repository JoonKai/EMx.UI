// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectRecentUsedFileDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.UI.Dialogs.Data;
using EMx.UI.Extensions;
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
  public partial class SelectRecentUsedFileDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public Func<object, string> TextConverter;
    public Func<object, string> PathConverter;
    private List<RecentUsedFileInfo> RecentList = (List<RecentUsedFileInfo>) null;
    internal Label TitleCtrl;
    internal ListBox lstItem;
    internal TextBox txtFilepath;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public virtual IList ItemsSource { get; set; }

    public int SelectedIndex { get; set; }

    public bool IsOpenMode { get; set; }

    public string FileFilter { get; set; }

    public string FileExt { get; set; }

    public string FilePath { get; set; }

    public bool IsInternalListMode { get; set; }

    public string InternalID { get; protected set; }

    public SelectRecentUsedFileDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.IsOpenMode = true;
      this.FileExt = "";
      this.FileFilter = "All Files(*.*)|*.*";
      this.InternalID = "";
    }

    private void lstItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      int selectedIndex = this.lstItem.SelectedIndex;
      if (selectedIndex <= -1 || selectedIndex >= this.ItemsSource.Count)
        return;
      this.txtFilepath.Text = this.PathConverter(this.ItemsSource[selectedIndex]);
    }

    private void btnSelectFile_Clicked(object sender, RoutedEventArgs e)
    {
      try
      {
        string str = !this.IsOpenMode ? this.SaveFileDialog(this.txtFilepath.Text, this.FileFilter, this.FileExt) : this.OpenFileDialog(this.txtFilepath.Text, this.FileFilter, this.FileExt);
        if (string.IsNullOrWhiteSpace(str))
          return;
        this.txtFilepath.Text = str;
      }
      catch (Exception ex)
      {
      }
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.txtFilepath.Text))
      {
        int num = (int) this.ShowWarningMessage("Manager", "Please select a file.");
      }
      else
      {
        this.FilePath = this.txtFilepath.Text;
        this.UpdateInternalList(this.FilePath);
        this.DialogResult = new bool?(true);
      }
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.Owner == null)
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      List<string> stringList = new List<string>();
      foreach (object obj in (IEnumerable) this.ItemsSource)
        stringList.Add(this.TextConverter(obj));
      this.txtFilepath.Text = this.FilePath;
      this.lstItem.ItemsSource = (IEnumerable) stringList;
      this.lstItem.SelectedIndex = this.SelectedIndex;
      this.lstItem.SmartFocus();
    }

    public void SetInternalListID(string id)
    {
      this.InternalID = id;
      if (string.IsNullOrWhiteSpace(id))
      {
        this.IsInternalListMode = false;
      }
      else
      {
        this.IsInternalListMode = true;
        this.RecentList = new List<RecentUsedFileInfo>();
        string str = Helper.IO.AppDataFolderRelativeFilename("etamax\\recent_used_paths\\" + id);
        if (File.Exists(str))
        {
          List<RecentUsedFileInfo> recentUsedFileInfoList = Helper.IO.LoadObjectFromXml<List<RecentUsedFileInfo>>(str);
          if (recentUsedFileInfoList != null)
            this.RecentList = recentUsedFileInfoList;
        }
        this.RecentList.SortExt<RecentUsedFileInfo>((Comparison<RecentUsedFileInfo>) ((a, b) => a.Count - b.Count));
        this.ItemsSource = (IList) this.RecentList;
        this.TextConverter = (Func<object, string>) (o => o is RecentUsedFileInfo recentUsedFileInfo2 ? string.Format("[{0}] {1}", (object) recentUsedFileInfo2.Count, (object) recentUsedFileInfo2.FilePath) : "");
        this.PathConverter = (Func<object, string>) (o => (o as RecentUsedFileInfo).FilePath);
      }
    }

    private void UpdateInternalList(string path)
    {
      if (!this.IsInternalListMode)
        return;
      RecentUsedFileInfo recentUsedFileInfo = this.RecentList.FirstOrDefault<RecentUsedFileInfo>((Func<RecentUsedFileInfo, bool>) (x => x.FilePath.Equals(path)));
      if (recentUsedFileInfo == null)
        this.RecentList.Add(new RecentUsedFileInfo()
        {
          Count = 1,
          FilePath = path,
          LastUsedTime = DateTime.Now
        });
      else
        ++recentUsedFileInfo.Count;
      Helper.IO.SaveObjectToXml((object) this.RecentList, Helper.IO.AppDataFolderRelativeFilename("etamax\\recent_used_paths\\" + this.InternalID));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/selectrecentusedfiledialog.xaml", UriKind.Relative));
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
          this.lstItem.SelectionChanged += new SelectionChangedEventHandler(this.lstItem_SelectionChanged);
          break;
        case 4:
          this.txtFilepath = (TextBox) target;
          break;
        case 5:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSelectFile_Clicked);
          break;
        case 6:
          this.ActiveButtons = (StackPanel) target;
          break;
        case 7:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
          break;
        case 8:
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
