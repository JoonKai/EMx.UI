// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.SelectFilepathControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
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
  [InstanceContract(ClassID = "ad992ec3-541a-4fe2-9ee0-4f3bfe473983")]
  public partial class SelectFilepathControl : UserControl, IManagedType, IComponentConnector
  {
    public static readonly RoutedEvent FileSelectedEvent = EventManager.RegisterRoutedEvent("FileSelected", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SelectFilepathControl));
    public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(nameof (Filter), typeof (string), typeof (SelectFilepathControl));
    internal TextBox txtValue;
    private bool _contentLoaded;

    public event RoutedEventHandler FileSelected
    {
      add => this.AddHandler(SelectFilepathControl.FileSelectedEvent, (Delegate) value);
      remove => this.RemoveHandler(SelectFilepathControl.FileSelectedEvent, (Delegate) value);
    }

    public virtual string Filter
    {
      get => (string) this.GetValue(SelectFilepathControl.FilterProperty);
      set => this.SetValue(SelectFilepathControl.FilterProperty, (object) value);
    }

    public SelectFilepathControl()
    {
      this.InitializeComponent();
      this.Filter = "All Files(*.*)|*.*";
    }

    protected virtual void InvokeFileSelectedEvent() => this.RaiseEvent(new RoutedEventArgs(SelectFilepathControl.FileSelectedEvent));

    public virtual string GetValue() => !this.IsThreadSafe() ? this.Dispatcher.Invoke<string>(new Func<string>(this.GetValue)) : this.txtValue.Text.Trim();

    public virtual bool IsValid() => !string.IsNullOrWhiteSpace(this.txtValue.Text);

    private void btnSelectFile_Clicked(object sender, RoutedEventArgs e)
    {
      string path = this.txtValue.Text.Trim();
      OpenFileDialog openFileDialog = new OpenFileDialog();
      if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
        openFileDialog.FileName = path;
      openFileDialog.Filter = this.Filter;
      openFileDialog.RestoreDirectory = true;
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.txtValue.Text = openFileDialog.FileName;
      this.InvokeFileSelectedEvent();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/selectfilepathcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtValue = (TextBox) target;
          break;
        case 2:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.btnSelectFile_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
