using System;

namespace Pricing.Option.MonteCarlo
{
  public class MilsteinDiscretization
  {
    public string Name { get { return "Milstein Discretization"; } }

    public double Increment(double value) 
    {
      double rand = GaussianBoxMuller.NextDouble();
      return value
        + (SimulatedPrice.Drift * value * SimulatedPrice.Delta)
        + (SimulatedPrice.Volatility * value * Math.Sqrt(SimulatedPrice.Delta) * rand)
        + (0.5 * Math.Pow(SimulatedPrice.Volatility, 2) * SimulatedPrice.Delta * (Math.Pow(rand, 2) - 1));
    }
  }
}
