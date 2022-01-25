// Decompiled with JetBrains decompiler
// Type: EMx.UI.Engine.PropertyGrids.Handlers.NumericPointPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Logging;
using EMx.Maths;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Engine.PropertyGrids.Handlers
{
  [PropertyGridDefaultHandler(typeof (NumericPoint))]
  public partial class NumericPointPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private string SettedText;
    private object SettedObject;
    internal TextBox TextCtrl;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public NumericPointPropItemHandler() => this.InitializeComponent();

    public virtual bool IsSupportedType(Type type) => type != (Type) null && type == typeof (NumericPoint);

    public virtual bool ValidateObject()
    {
      try
      {
        bool flag = true;
        List<string> list = ((IEnumerable<string>) this.TextCtrl.Text.Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())).Where<string>((Func<string, bool>) (x => x.Length > 0)).ToList<string>();
        if (list.Count != 2)
        {
          flag = false;
        }
        else
        {
          double result = 0.0;
          if (!double.TryParse(list[0], out result) || !double.TryParse(list[1], out result))
            flag = false;
        }
        this.Background = flag ? (Brush) Brushes.White : (Brush) Brushes.Red;
        return flag;
      }
      catch (Exception ex)
      {
        NumericPointPropItemHandler.log.Debug("Validation Failed : {0}", (object) ex.Message);
        return false;
      }
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null || !(obj is NumericPoint numericPoint))
        return false;
      this.SettedObject = obj;
      this.TextCtrl.Text = string.Format("{0}, {1}", (object) numericPoint.X, (object) numericPoint.Y);
      this.ClearChangedState();
      return true;
    }

    public object GetObject()
    {
      try
      {
        if (this.IsReadOnly)
          return this.SettedObject;
        List<string> list = ((IEnumerable<string>) this.TextCtrl.Text.Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())).Where<string>((Func<string, bool>) (x => x.Length > 0)).ToList<string>();
        if (list.Count == 2)
          return (object) new NumericPoint(Convert.ToDouble(list[0]), Convert.ToDouble(list[1]));
        NumericPointPropItemHandler.log.Warn("Failed to convert to target object.");
        return this.SettedObject;
      }
      catch (Exception ex)
      {
        NumericPointPropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
        return (object) null;
      }
    }

    public void ClearChangedState() => this.SettedText = this.TextCtrl.Text;

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !string.Equals(this.TextCtrl.Text, this.SettedText);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/numericpointpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.TextCtrl = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
