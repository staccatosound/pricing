using System;

namespace Pricing.Option.MonteCarlo
{
  public class MilsteinDiscretization
  {
    public string Name { get { return "Milstein Discretization"; } }

    public double Increment(double value, double volatility, double maturity, double riskFree)
    {
      double rand = GaussianBoxMuller.NextDouble();
      return value
        + (riskFree * value * maturity / Const.kDaysInAYear)
        + (volatility * value * Math.Sqrt(maturity / Const.kDaysInAYear) * rand)
        + (0.5 * Math.Pow(volatility, 2) * maturity / Const.kDaysInAYear * (Math.Pow(rand, 2) - 1));
    }
  }
}
