using System;

namespace Pricing.Option.MonteCarlo
{
  public static class GaussianBoxMuller
  {
    public static double NextDouble()
    {
      double x, y, square;

      do
      {
        x = 2 * RandomProvider.GetThreadRandom().NextDouble() - 1;
        y = 2 * RandomProvider.GetThreadRandom().NextDouble() - 1;
        square = (x * x) + (y * y);
      }
      while (square >= 1);

      return x * Math.Sqrt(-2 * Math.Log(square) / square);
    }
  }
}
