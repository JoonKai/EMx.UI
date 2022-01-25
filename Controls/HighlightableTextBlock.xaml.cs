// Decompiled with JetBrains decompiler
// Type: EMx.UI.Controls.HighlightableTextBlock
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using EMx.Texts.Patterns;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace EMx.UI.Controls
{
  public partial class HighlightableTextBlock : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty PatternMatchDataProperty = DependencyProperty.Register(nameof (PatternMatchData), typeof (PatternMatchResult), typeof (HighlightableTextBlock), new PropertyMetadata((object) null, new PropertyChangedCallback(HighlightableTextBlock.OnPatternMatchDataChanged)));
    internal TextBlock txtMessage;
    private bool _contentLoaded;

    public virtual Color HighlightColor { get; set; }

    public virtual bool UseBoldWeight { get; set; }

    public virtual PatternMatchResult PatternMatchData
    {
      get => (PatternMatchResult) this.GetValue(HighlightableTextBlock.PatternMatchDataProperty);
      set => this.SetValue(HighlightableTextBlock.PatternMatchDataProperty, (object) value);
    }

    private static void OnPatternMatchDataChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is HighlightableTextBlock highlightableTextBlock))
        return;
      highlightableTextBlock.UpdateText(highlightableTextBlock.PatternMatchData);
    }

    public HighlightableTextBlock()
    {
      this.InitializeComponent();
      this.UseBoldWeight = true;
      this.HighlightColor = Colors.Red;
    }

    public virtual void UpdateText(PatternMatchResult result)
    {
      InlineCollection inlines = this.txtMessage.Inlines;
      inlines.Clear();
      if (result.IgnoredPrefixLength > 0)
      {
        string text = result.Text.Substring(0, result.IgnoredPrefixLength);
        InlineCollection inlineCollection = inlines;
        Run run = new Run(text);
        run.Foreground = (Brush) Brushes.DarkGray;
        inlineCollection.Add((Inline) run);
      }
      string text1 = result.Text;
      int start_index = result.IgnoredPrefixLength;
      for (int index = 0; index < result.Points.Count; ++index)
      {
        PatternMatchPoint point = result.Points[index];
        if (start_index < point.StartIndex)
        {
          string text2 = text1.SafeSubstring(start_index, point.StartIndex - start_index);
          inlines.Add(text2);
        }
        if (point.StartIndex < point.NextIndex)
        {
          string text3 = text1.SafeSubstring(point.StartIndex, point.Length);
          InlineCollection inlineCollection = inlines;
          Run run = new Run(text3);
          run.FontWeight = this.UseBoldWeight ? FontWeights.Bold : FontWeights.Normal;
          run.Foreground = (Brush) new SolidColorBrush(this.HighlightColor);
          inlineCollection.Add((Inline) run);
        }
        start_index = point.NextIndex;
      }
      if (start_index >= text1.Length)
        return;
      string text4 = text1.SafeSubstring(start_index);
      inlines.Add(text4);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/EMx.UI;component/controls/highlightabletextblock.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.txtMessage = (TextBlock) target;
      else
        this._contentLoaded = true;
    }
  }
}
