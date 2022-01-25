// Decompiled with JetBrains decompiler
// Type: XamlGeneratedNamespace.GeneratedInternalTypeHelper
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows.Markup;

namespace XamlGeneratedNamespace
{
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public sealed class GeneratedInternalTypeHelper : InternalTypeHelper
  {
    protected override object CreateInstance(Type type, CultureInfo culture) => Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, (Binder) null, (object[]) null, culture);

    protected override object GetPropertyValue(
      PropertyInfo propertyInfo,
      object target,
      CultureInfo culture)
    {
      return propertyInfo.GetValue(target, BindingFlags.Default, (Binder) null, (object[]) null, culture);
    }

    protected override void SetPropertyValue(
      PropertyInfo propertyInfo,
      object target,
      object value,
      CultureInfo culture)
    {
      propertyInfo.SetValue(target, value, BindingFlags.Default, (Binder) null, (object[]) null, culture);
    }

    protected override Delegate CreateDelegate(
      Type delegateType,
      object target,
      string handler)
    {
      return (Delegate) target.GetType().InvokeMember("_CreateDelegate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder) null, target, new object[2]
      {
        (object) delegateType,
        (object) handler
      }, (CultureInfo) null);
    }

    protected override void AddEventHandler(EventInfo eventInfo, object target, Delegate handler) => eventInfo.AddEventHandler(target, handler);
  }
}
