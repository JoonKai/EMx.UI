// Decompiled with JetBrains decompiler
// Type: EMx.UI.Data.Models.TargetPropertySupplier
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Helpers;
using EMx.Logging;
using EMx.Serialization;
using EMx.UI.Engine.PropertyGrids.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EMx.UI.Data.Models
{
  public class TargetPropertySupplier : IGridViewExternalHandler
  {
    private static ILog log = LogManager.GetLogger();

    public virtual IPropertyGridHandler CreateGridHandler(
      object source,
      PropertyInfo property,
      string HandlerParameger)
    {
      if (source == null || property == (PropertyInfo) null || !property.PropertyType.Equals(typeof (string)))
        return (IPropertyGridHandler) null;
      InstanceSerializerCache inst = InstanceSerializerCache.Inst;
      StringComboPropItemHandler comboPropItemHandler = new StringComboPropItemHandler();
      object obj = source;
      if (!string.IsNullOrWhiteSpace(HandlerParameger))
      {
        PropertyInfo property1 = Helper.Assem.GetProperty(source, HandlerParameger);
        if (property1 != (PropertyInfo) null)
          obj = property1.GetValue(source);
        else
          TargetPropertySupplier.log.Warn("Not found HandlerParameter : {0}, source[{1}], prop[{2}]", (object) HandlerParameger, source, (object) property);
      }
      if (obj == null)
        return (IPropertyGridHandler) null;
      Type type = obj.GetType();
      if (obj is Type)
        type = obj as Type;
      List<string> list = ((IEnumerable<PropertyInfo>) Helper.Assem.GetProperties(type)).Select<PropertyInfo, string>((Func<PropertyInfo, string>) (x => x.Name)).ToList<string>();
      comboPropItemHandler.Items = list;
      return (IPropertyGridHandler) comboPropItemHandler;
    }
  }
}
