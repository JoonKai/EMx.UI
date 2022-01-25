// Decompiled with JetBrains decompiler
// Type: EMx.UI.Maps.ContextMenuRegistrationItem
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.Windows;

namespace EMx.UI.Maps
{
  public class ContextMenuRegistrationItem
  {
    public string Path { get; set; }

    public RoutedEventHandler Handler { get; set; }

    public Func<object, bool> Activation { get; set; }

    public object Sender { get; set; }

    public ContextMenuRegistrationItem()
    {
    }

    public ContextMenuRegistrationItem(
      string path,
      object sender,
      RoutedEventHandler handler,
      Func<object, bool> activation)
    {
      this.Path = path;
      this.Sender = sender;
      this.Handler = handler;
      this.Activation = activation;
    }
  }
}
