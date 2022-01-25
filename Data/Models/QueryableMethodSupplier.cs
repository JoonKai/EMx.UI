// Decompiled with JetBrains decompiler
// Type: EMx.UI.Data.Models.QueryableMethodSupplier
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using EMx.Data.Models.Handlers;
using EMx.Engine;
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
  public class QueryableMethodSupplier : IGridViewExternalHandler
  {
    private static ILog log = LogManager.GetLogger();

    public virtual IPropertyGridHandler CreateGridHandler(
      object source,
      PropertyInfo property,
      string HandlerParameger)
    {
      if (source == null || property == (PropertyInfo) null)
        return (IPropertyGridHandler) null;
      InstanceSerializerCache cache = InstanceSerializerCache.Inst;
      StringComboPropItemHandler comboPropItemHandler = new StringComboPropItemHandler();
      object obj = source;
      if (!string.IsNullOrWhiteSpace(HandlerParameger))
      {
        PropertyInfo property1 = Helper.Assem.GetProperty(source, HandlerParameger);
        if (property1 != (PropertyInfo) null)
          obj = property1.GetValue(source);
        else
          QueryableMethodSupplier.log.Warn("Not found HandlerParameter : {0}, source[{1}], prop[{2}]", (object) HandlerParameger, source, (object) property);
      }
      if (obj == null)
        return (IPropertyGridHandler) null;
      comboPropItemHandler.Items = ((IEnumerable<MethodInfo>) obj.GetType().GetMethods()).OrderBy<MethodInfo, string>((Func<MethodInfo, string>) (x => x.Name)).Where<MethodInfo>((Func<MethodInfo, bool>) (x => cache.IsDefined<QueryableMemberAttribute>((MemberInfo) x))).Select<MethodInfo, string>((Func<MethodInfo, string>) (x => x.Name)).ToList<string>();
      return (IPropertyGridHandler) comboPropItemHandler;
    }
  }
}
