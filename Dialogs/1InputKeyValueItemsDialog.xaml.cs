// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.InputKeyValueItemsDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace EMx.UI.Dialogs
{
  public class InputKeyValueItemsDialog : Window, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (InputKeyValueItemsDialog));
    public static readonly DependencyProperty TableDataProperty = DependencyProperty.Register(nameof (TableData), typeof (List<InputKeyValueItem>), typeof (InputKeyValueItemsDialog));
    internal Label TitleCtrl;
    internal StackPanel ActiveButtons;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public List<string> KeyList { get; set; }

    public string Message
    {
      get => (string) this.GetValue(InputKeyValueItemsDialog.MessageProperty);
      set => this.SetValue(InputKeyValueItemsDialog.MessageProperty, (object) value);
    }

    public List<InputKeyValueItem> TableData
    {
      get => (List<InputKeyValueItem>) this.GetValue(InputKeyValueItemsDialog.TableDataProperty);
      set => this.SetValue(InputKeyValueItemsDialog.TableDataProperty, (object) value);
    }

    public InputKeyValueItemsDialog()
    {
      this.InitializeComponent();
      this.DataContext = (object) this;
      this.KeyList = new List<string>();
      this.TableData = new List<InputKeyValueItem>();
    }

    private void TitleCtrl_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void btnOK_Clicked(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(true);

    private void btnCancel_Clicked(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(false);

    public static Dictionary<string, string> ShowInputDialog(
      Window wnd,
      string title,
      string message,
      List<string> keys)
    {
      InputKeyValueItemsDialog valueItemsDialog = new InputKeyValueItemsDialog();
      valueItemsDialog.Owner = wnd;
      valueItemsDialog.Title = title;
      valueItemsDialog.Message = message;
      valueItemsDialog.KeyList = keys;
      bool? nullable = valueItemsDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return (Dictionary<string, string>) null;
      Dictionary<string, string> dic = new Dictionary<string, string>();
      valueItemsDialog.TableData.ForEach((Action<InputKeyValueItem>) (x => dic[x.Key] = x.Value));
      return dic;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.TableData.Clear();
      this.KeyList.ForEach((Action<string>) (x => this.TableData.Add(new InputKeyValueItem(x, ""))));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/dialogs/inputkeyvalueitemsdialog.xaml", UriKind.Relative));
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
          this.ActiveButtons = (StackPanel) target;
          break;
        case 4:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Clicked);
          break;
        case 5:
          this.btnCancel = (Button) target;
          this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Clicked);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
