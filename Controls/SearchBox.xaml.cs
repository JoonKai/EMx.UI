// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.SearchBox
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Engine;
using EMx.Serialization;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EMx.UI.Controls
{
  [InstanceContract(ClassID = "88dc9d51-6d69-46ac-95fc-21bcbc35d069")]
  public partial class SearchBox : UserControl, IManagedType, IComponentConnector
  {
    internal TextBox MainText;
    internal Image SearchButton;
    private bool _contentLoaded;

    public SearchBox() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/searchbox.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MainText = (TextBox) target;
          break;
        case 2:
          this.SearchButton = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
