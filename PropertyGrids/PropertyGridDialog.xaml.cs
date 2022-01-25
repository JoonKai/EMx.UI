// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.PropertyGridDialog
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.PropertyGrids
{
  public partial class PropertyGridDialog : Window, IComponentConnector
  {
    internal PropertyGrid PropertiesControl;
    internal Button btnOK;
    internal Button btnCancel;
    private bool _contentLoaded;

    public PropertyGridDialog() => this.InitializeComponent();

    public virtual PropertyGrid Control => this.PropertiesControl;

    public GridLength KeyColumnWidth
    {
      get => this.PropertiesControl.KeyColumnWidth;
      set => this.PropertiesControl.KeyColumnWidth = value;
    }

    public GridLength ValueColumnWidth
    {
      get => this.PropertiesControl.ValueColumnWidth;
      set => this.PropertiesControl.ValueColumnWidth = value;
    }

    public virtual object SelectedObject
    {
      get => this.PropertiesControl.SelectedObject;
      set => this.PropertiesControl.SelectedObject = value;
    }

    public bool UseIndirectMode
    {
      get => this.PropertiesControl.UseIndirectMode;
      set => this.PropertiesControl.UseIndirectMode = value;
    }

    public bool UseCancellingChanges
    {
      get => this.PropertiesControl.UseCancellingChanges;
      set => this.PropertiesControl.UseCancellingChanges = value;
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (this.UseIndirectMode)
        this.PropertiesControl.Commit();
      this.DialogResult = new bool?(true);
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      if (this.UseCancellingChanges)
        this.PropertiesControl.Rollback();
      this.DialogResult = new bool?(false);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/propertygriddialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.PropertiesControl = (PropertyGrid) target;
          break;
        case 2:
          this.btnOK = (Button) target;
          this.btnOK.Click += new RoutedEventHandler(this.btnOK_Click);
          break;
        case 3:
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
