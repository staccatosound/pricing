using System;

namespace Pricing.Option.MonteCarlo
{
  public class EulerDiscretization : IDiscretizationScheme
  {
    public string Name { get { return "Euler Discretization"; } }
    public double Increment(double value)
    {
      return value
        + (SimulatedPrice.Drift * value * SimulatedPrice.Delta)
        + (SimulatedPrice.Volatility * value * Math.Sqrt(SimulatedPrice.Delta) * GaussianBoxMuller.NextDouble());
    }
  }
}
