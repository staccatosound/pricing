using System;

namespace Pricing.Option.MonteCarlo
{
  public class EulerDiscretization : IDiscretizationScheme
  {
    public string Name { get { return "Euler Discretization"; } }
    public double Increment(double value, double volatility, double maturity, double riskFree)
    {
      return value
        + (riskFree * value * maturity / Const.kDaysInAYear)
        + (volatility * value * Math.Sqrt(maturity / Const.kDaysInAYear) * GaussianBoxMuller.NextDouble());
    }
  }
}
