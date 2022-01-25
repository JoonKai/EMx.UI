// Decompiled with JetBrains decompiler
// Type: EMx.UI.ColorControls.HueControl
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Logging;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.ColorControls
{
  public partial class HueControl : UserControl, IComponentConnector
  {
    private static ILog log = LogManager.GetLogger();
    public static readonly DependencyProperty IsVertialModeProperty = DependencyProperty.Register(nameof (IsVerticalMode), typeof (bool), typeof (HueControl));
    private bool _contentLoaded;

    public bool IsVerticalMode
    {
      get => (bool) this.GetValue(HueControl.IsVertialModeProperty);
      set => this.SetValue(HueControl.IsVertialModeProperty, (object) value);
    }

    public HueControl() => this.InitializeComponent();

    protected override void OnRender(DrawingContext dc)
    {
      double actualHeight = this.ActualHeight;
      double actualWidth = this.ActualWidth;
      if (!this.IsVerticalMode)
        ;
      base.OnRender(dc);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/colorcontrols/huecontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
