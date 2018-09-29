using System.Threading.Tasks;

namespace Pricing.Option.MonteCarlo
{
  public class SimulatedPrice
  {
    public IDiscretizationScheme scheme { get; set; }

    private double volatility { get; set; }
    private double riskFree { get; set; }
    private double assetPrice { get; set; }
    private double strike { get; set; }
    private double maturity { get; set; }
    private int steps { get; set; }

    public double[] simulatedPriceArray { get; private set; }

    public SimulatedPrice(
      double assetPrice,
      double strike,
      double maturity,
      double volatility,
      double riskFree,
      int steps,
      IDiscretizationScheme scheme)
    {
      this.assetPrice = assetPrice;
      this.strike = strike;
      this.maturity = maturity;
      this.volatility = volatility;
      this.riskFree = riskFree;
      this.steps = steps;
      this.scheme = scheme;

      simulatedPriceArray = new double[steps];
      simulatedPriceArray[0] = assetPrice;
    }

    public Task RunSim() 
    {
      return Task.Run(() => SimulatePrice());
    }

    public void SimulatePrice()
    {
      for (uint i = 1; i < steps; i++)
      {
        simulatedPriceArray[i] = scheme.Increment(simulatedPriceArray[i-1], volatility, maturity, riskFree);
      }
    }


  }
}
