// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.ImageAndTextButton
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
using System.Windows.Media;

namespace EMx.UI.Controls
{
  public partial class ImageAndTextButton : UserControl, INotifyPropertyChanged, IComponentConnector
  {
    private string _ImageURI;
    private string _Text;
    private Thickness _ImageMargin;
    private Thickness _TextPadding;
    private GridLength _TextRowHeight;
    internal Image image;
    private bool _contentLoaded;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string info)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(info));
    }

    public string ImageURI
    {
      get => this._ImageURI;
      set
      {
        this._ImageURI = value;
        this.OnPropertyChanged(nameof (ImageURI));
      }
    }

    public string Text
    {
      get => this._Text;
      set
      {
        this._Text = value;
        this.OnPropertyChanged(nameof (Text));
      }
    }

    public Thickness ImageMargin
    {
      get => this._ImageMargin;
      set
      {
        this._ImageMargin = value;
        this.OnPropertyChanged(nameof (ImageMargin));
      }
    }

    public Thickness TextPadding
    {
      get => this._TextPadding;
      set
      {
        this._TextPadding = value;
        this.OnPropertyChanged(nameof (TextPadding));
      }
    }

    public GridLength TextRowHeight
    {
      get => this._TextRowHeight;
      set
      {
        this._TextRowHeight = value;
        this.OnPropertyChanged(nameof (TextRowHeight));
      }
    }

    public ImageAndTextButton()
    {
      this.ImageMargin = new Thickness();
      this.TextPadding = new Thickness();
      this.TextRowHeight = new GridLength();
      this.DataContext = (object) this;
      this.InitializeComponent();
      RenderOptions.SetEdgeMode((DependencyObject) this.image, EdgeMode.Aliased);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/imageandtextbutton.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.image = (Image) target;
      else
        this._contentLoaded = true;
    }
  }
}
