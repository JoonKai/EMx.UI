// Decompiled with JetBrains decompiler
// Type: EMx.UI.Extensions.SimpleRGB
// Assembly: EMx.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=2364f9ab963233d5
// MVID: 2693350C-00C5-44BA-A8C4-80C592805269
// Assembly location: D:\07_etamax\2DInterpolationSimulator_190729\2DInterpolationSimulator_190729\EMx.UI.dll

namespace EMx.UI.Extensions
{
  public class SimpleRGB
  {
    public byte R { get; set; }

    public byte G { get; set; }

    public byte B { get; set; }

    public byte A { get; set; }

    public SimpleRGB() => this.A = byte.MaxValue;

    public SimpleRGB(byte r, byte g, byte b)
    {
      this.R = r;
      this.G = g;
      this.B = b;
    }

    public SimpleRGB(byte a, byte r, byte g, byte b)
      : this(r, g, b)
    {
      this.A = a;
    }

    public static SimpleRGB From(byte r, byte g, byte b) => new SimpleRGB(r, g, b);

    public static SimpleRGB From(byte a, byte r, byte g, byte b) => new SimpleRGB(a, r, g, b);

    public static SimpleRGB From(int r, int g, int b) => new SimpleRGB((byte) r, (byte) g, (byte) b);
  }
}
