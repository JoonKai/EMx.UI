// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.FilePathPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Extensions;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "d11bad05-d043-4abd-89a2-7618e73e3cd4")]
  public partial class FilePathPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private string SettedText;
    private object SettedObject;
    private bool OnlyDisplay;
    internal TextBox TextCtrl;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value && !this.OnlyDisplay;
    }

    public FilePathPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type)
    {
      if (type != (Type) null && type.IsPrimitive || type == typeof (string))
        return true;
      if (!type.IsValueType)
        return false;
      this.IsReadOnly = true;
      this.OnlyDisplay = true;
      return true;
    }

    public virtual bool ValidateObject()
    {
      try
      {
        bool flag = Helper.Data.ConvertType((object) this.TextCtrl.Text, this.ValueType) != null;
        this.Background = flag ? (Brush) Brushes.White : (Brush) Brushes.Red;
        return flag;
      }
      catch (Exception ex)
      {
        FilePathPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      this.SettedObject = obj;
      this.TextCtrl.Text = obj.ToString();
      this.ClearChangedState();
      return true;
    }

    public object GetObject()
    {
      try
      {
        return this.OnlyDisplay ? this.SettedObject : Helper.Data.ConvertType((object) this.TextCtrl.Text, this.ValueType);
      }
      catch (Exception ex)
      {
        FilePathPropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
        return (object) null;
      }
    }

    public void ClearChangedState() => this.SettedText = this.TextCtrl.Text;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !string.Equals(this.TextCtrl.Text, this.SettedText);

    private void Label_MouseUp(object sender, MouseButtonEventArgs e)
    {
      string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string str = this.TextCtrl.Text.Trim();
      string path = Environment.CurrentDirectory;
      bool flag = str.Contains<char>(':') || str.Contains<char>('/');
      if (!str.IsNullOrEmpty())
        path = Helper.IO.AssemblyRelativeFilename(str);
      string filter = this.HandlerParameter.IsNullOrEmpty() ? "All Files(*.*)|*.*" : this.HandlerParameter;
      string text = Application.Current.MainWindow.OpenFileDialog(path, filter, (string) null);
      if (text.IsNullOrEmpty())
        return;
      if (!flag && text.StartsWith(baseDirectory))
        text = text.Substring(baseDirectory.Length);
      e.Handled = true;
      this.TextCtrl.Focus();
      this.TextCtrl.Text = text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/filepathpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextCtrl = (TextBox) target;
          break;
        case 2:
          ((UIElement) target).MouseUp += new MouseButtonEventHandler(this.Label_MouseUp);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
