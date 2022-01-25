// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.FilePathLineControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace EMx.UI.Controls
{
  [InstanceContract(ClassID = "47de4014-9d61-43aa-a873-e36549cf0b32")]
  public partial class FilePathLineControl : UserControl, IManagedType, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof (Filter), typeof (string), typeof (FilePathLineControl));
    public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(nameof (FilePath), typeof (string), typeof (FilePathLineControl));
    public static readonly DependencyProperty NameLengthProperty = DependencyProperty.Register(nameof (NameLength), typeof (GridLength), typeof (FilePathLineControl));
    public static readonly DependencyProperty NameAliasProperty = DependencyProperty.Register(nameof (NameAlias), typeof (string), typeof (FilePathLineControl));
    private bool _contentLoaded;

    public virtual string Filter
    {
      get => (string) this.GetValue(FilePathLineControl.FilterProperty);
      set => this.SetValue(FilePathLineControl.FilterProperty, (object) value);
    }

    public virtual string FilePath
    {
      get => (string) this.GetValue(FilePathLineControl.FilePathProperty);
      set => this.SetValue(FilePathLineControl.FilePathProperty, (object) value);
    }

    public virtual GridLength NameLength
    {
      get => (GridLength) this.GetValue(FilePathLineControl.NameLengthProperty);
      set => this.SetValue(FilePathLineControl.NameLengthProperty, (object) value);
    }

    public virtual string NameAlias
    {
      get => (string) this.GetValue(FilePathLineControl.NameAliasProperty);
      set => this.SetValue(FilePathLineControl.NameAliasProperty, (object) value);
    }

    public event Action<FilePathLineControl> FilePathChangedEvent;

    public FilePathLineControl()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.NameLength = new GridLength(50.0);
      this.NameAlias = "Path";
      this.Filter = "All Files(*.*)|*.*";
      this.FilePath = "";
    }

    private void btnSelectFile_Clicked(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (!string.IsNullOrWhiteSpace(this.FilePath) && File.Exists(this.FilePath))
          openFileDialog.FileName = this.FilePath;
        openFileDialog.Filter = this.Filter;
        openFileDialog.RestoreDirectory = true;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
          this.FilePath = openFileDialog.FileName;
        if (this.FilePathChangedEvent == null)
          return;
        this.FilePathChangedEvent(this);
      }
      catch (Exception ex)
      {
        FilePathLineControl.log.Error(ex, ex.Message);
        int num = (int) Application.Current.MainWindow.ShowWarningMessage("Manager", "Error\r\n - " + ex.Message);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/filepathlinecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSelectFile_Clicked);
      else
        this._contentLoaded = true;
    }
  }
}
