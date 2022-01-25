// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.MultiWaferItemUI
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EMx.UI.Maps
{
  public partial class MultiWaferItemUI : UserControl, IComponentConnector
  {
    internal Label lblWaferNo;
    internal Label lblWaferName;
    internal Label lblStat;
    internal Image ctrlImage;
    private bool _contentLoaded;

    public MultiWaferItemUI() => this.InitializeComponent();

    public void UpdateImage(MultiWaferItem item, BitmapSource bmp)
    {
      string text = item.LotInfo.SafeGet<string, string>("Wafer Name", "");
      if (text.IsNullOrEmpty())
        text = item.LotInfo.SafeGet<string, string>("WaferName", "");
      string str = item.LotInfo.SafeGet<string, string>("Slot No", "0");
      if (item.LotInfo.ContainsKey("Wafer No"))
        str = item.LotInfo["Wafer No"];
      this.lblWaferNo.Content = (object) str;
      this.lblWaferName.Content = (object) text;
      this.lblStat.Content = (object) string.Format("{0} ({1})", (object) item.Measurement.Stat.Average.ToFormattedString(), (object) item.Measurement.Stat.StandDev.ToFormattedString());
      this.ctrlImage.Source = (ImageSource) bmp;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/maps/multiwaferitemui.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.lblWaferNo = (Label) target;
          break;
        case 2:
          this.lblWaferName = (Label) target;
          break;
        case 3:
          this.lblStat = (Label) target;
          break;
        case 4:
          this.ctrlImage = (Image) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
