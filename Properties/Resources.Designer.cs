// Decompiled with JetBrains decompiler
// Type: EMx.UI.Properties.Resources
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace EMx.UI.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (EMx.UI.Properties.Resources.resourceMan == null)
          EMx.UI.Properties.Resources.resourceMan = new ResourceManager("EMx.UI.Properties.Resources", typeof (EMx.UI.Properties.Resources).Assembly);
        return EMx.UI.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => EMx.UI.Properties.Resources.resourceCulture;
      set => EMx.UI.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap cancel_button => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (cancel_button), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap error => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (error), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap forbidden => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (forbidden), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap indi_options => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (indi_options), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap map_pin30 => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (map_pin30), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap message => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (message), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap question => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (question), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap read_direction => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (read_direction), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap search_button => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (search_button), EMx.UI.Properties.Resources.resourceCulture);

    internal static Bitmap warning => (Bitmap) EMx.UI.Properties.Resources.ResourceManager.GetObject(nameof (warning), EMx.UI.Properties.Resources.resourceCulture);
  }
}
