// Decompiled with JetBrains decompiler
// Type: EMx.UI.PropertyGrids.Handlers.TimeSpanPropItemHandler
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Logging;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.PropertyGrids.Handlers
{
  [InstanceContract(ClassID = "c4204feb-c0e9-41f4-b5a4-2fe52a23ca28")]
  public partial class TimeSpanPropItemHandler : 
    UserControl,
    IPropertyGridHandler,
    IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    private TimeSpan SettedTime;
    internal TextBox txtHours;
    internal Label txtCol1;
    internal TextBox txtMinutes;
    internal Label txtCol2;
    internal TextBox txtSecons;
    private bool _contentLoaded;

    public virtual string HandlerParameter { get; set; }

    public virtual Type ValueType { get; set; }

    public virtual bool IsReadOnly
    {
      get => !this.IsEnabled;
      set => this.IsEnabled = !value;
    }

    public TimeSpanPropItemHandler()
    {
      this.InitializeComponent();
      this.SettedTime = new TimeSpan();
    }

    public virtual bool IsSupportedType(Type type) => type == typeof (TimeSpan);

    public virtual bool ValidateObject()
    {
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      bool flag1 = int.TryParse(this.txtHours.Text.Trim(), out result1);
      bool flag2 = int.TryParse(this.txtMinutes.Text.Trim(), out result2);
      bool flag3 = int.TryParse(this.txtSecons.Text.Trim(), out result3);
      bool flag4 = flag1 && result1 >= 0;
      bool flag5 = flag2 && result2 >= 0 && result2 < 60;
      bool flag6 = flag3 && result3 >= 0 && result3 < 60;
      bool flag7 = flag4 & flag5 & flag6;
      this.txtHours.Background = flag4 ? (Brush) Brushes.White : (Brush) Brushes.Red;
      this.txtMinutes.Background = flag5 ? (Brush) Brushes.White : (Brush) Brushes.Red;
      this.txtSecons.Background = flag6 ? (Brush) Brushes.White : (Brush) Brushes.Red;
      return flag7;
    }

    protected TimeSpan GetTime()
    {
      int result1 = 0;
      int result2 = 0;
      int result3 = 0;
      bool flag1 = int.TryParse(this.txtHours.Text.Trim(), out result1);
      bool flag2 = int.TryParse(this.txtMinutes.Text.Trim(), out result2);
      bool flag3 = int.TryParse(this.txtSecons.Text.Trim(), out result3);
      return (flag1 && result1 >= 0) & (flag2 && result2 >= 0 && result2 < 60) & (flag3 && result3 >= 0 && result3 < 60) ? new TimeSpan(result1, result2, result3) : TimeSpan.Zero;
    }

    public virtual bool SetObject(object obj)
    {
      if (obj == null)
        return false;
      try
      {
        TimeSpan timeSpan = (TimeSpan) obj;
        this.txtHours.Text = string.Format("{0:00}", (object) (int) timeSpan.TotalHours);
        this.txtMinutes.Text = string.Format("{0:00}", (object) timeSpan.Minutes);
        this.txtSecons.Text = string.Format("{0:00}", (object) timeSpan.Seconds);
        this.ClearChangedState();
        return true;
      }
      catch (Exception ex)
      {
        TimeSpanPropItemHandler.log.Error(ex, ex.Message);
        return false;
      }
    }

    public object GetObject()
    {
      try
      {
        return !this.ValidateObject() ? (object) null : (object) this.GetTime();
      }
      catch (Exception ex)
      {
        TimeSpanPropItemHandler.log.Warn("Validation Failed : {0}", (object) ex.Message);
        return (object) null;
      }
    }

    public void ClearChangedState() => this.SettedTime = this.GetTime();

    public UIElement GetUIElement() => (UIElement) this;

    public virtual bool IsValueChanged => !object.Equals((object) this.SettedTime, (object) this.GetTime());

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/propertygrids/handlers/timespanpropitemhandler.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.txtHours = (TextBox) target;
          break;
        case 2:
          this.txtCol1 = (Label) target;
          break;
        case 3:
          this.txtMinutes = (TextBox) target;
          break;
        case 4:
          this.txtCol2 = (Label) target;
          break;
        case 5:
          this.txtSecons = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
