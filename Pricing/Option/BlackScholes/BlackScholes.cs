using System;

namespace Pricing.Option.BlackScholes
{
  public static class BlackScholes
  {
    public static double Calculate(double x, double s, double t, double r, double v)
    {
      double y = Math.Pow(v, 2);
      double d1 = (Math.Log(s / x) + (r + 0.5 * y) * t) / v * Math.Sqrt(t);
      double d2 = d1 - v * Math.Sqrt(t);
      double model = s * CND(d1) - x * Math.Exp(-r * t) * CND(d2);
      return model;
    }

    private static double CND(double x) 
    {
      double L, K, dCND;
      const double a1 = 0.31938153;
      const double a2 = -0.356563782;
      const double a3 = 1.781477937;
      const double a4 = -1.821255978;
      const double a5 = 1.330274429;

      L = Math.Abs(x);
      K = 1.0 / (1.0 + 0.2316419 * L);

      dCND = 1.0 - 1.0 / Math.Sqrt(2 * Math.PI) * Math.Exp(-L*L/2.0) * 
        (a1 * K 
        + a2 * Math.Pow(K, 2.0) 
        + a3 * Math.Pow(K, 3.0)
        + a4 * Math.Pow(K, 4.0)
        + a5 * Math.Pow(K, 5.0)
        );

      if (x < 0)
        return 1.0 - dCND;
      else
        return dCND;
    }
  }
}
