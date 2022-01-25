// Decompiled with JetBrains decompiler
// Type: EMx.UI.Dialogs.SelectMultiItemsData
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Extensions;
using System;
using System.ComponentModel;

namespace EMx.UI.Dialogs
{
  public class SelectMultiItemsData : INotifyPropertyChanged, ICloneable
  {
    public bool _IsSelected;

    public object Object { get; set; }

    public string Message { get; set; }

    public bool IsSelected
    {
      get => this._IsSelected;
      set
      {
        if (this._IsSelected == value)
          return;
        this._IsSelected = value;
        this.InvokePropertyChanged(nameof (IsSelected));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void InvokePropertyChanged(string property)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(property));
    }

    public SelectMultiItemsData() => this.Message = "";

    public SelectMultiItemsData(string message, object obj, bool selected)
    {
      this.Object = obj;
      this.Message = message;
      this.IsSelected = selected;
    }

    public virtual object Clone() => this.MemberwiseClone();

    public override string ToString() => string.Format("[{0}] {1}", (object) this.IsSelected.ToOX(), (object) this.Message);
  }
}
