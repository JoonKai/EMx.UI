// Decompiled with JetBrains decompiler
// Type: EMx.UI.Susceptors.SusceptorMaker
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

namespace EMx.UI.Susceptors
{
  public partial class SusceptorMaker : UserControl, IComponentConnector
  {
    internal MultiRingSusceptorMaker ctrlMultiSus;
    internal SusceptorLayoutViewer ctrlViewer;
    private bool _contentLoaded;

    public SusceptorMaker() => this.InitializeComponent();

    private void ctrlMultiSus_SusceptorChanged(object sender, EventArgs e) => this.ctrlViewer.SetData(this.ctrlMultiSus.CurrentSusceptor);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/susceptors/susceptormaker.xaml", UriKind.Relative));
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
          this.ctrlMultiSus = (MultiRingSusceptorMaker) target;
          break;
        case 2:
          this.ctrlViewer = (SusceptorLayoutViewer) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
