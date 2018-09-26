using System.Threading.Tasks;

namespace Pricing.Option.MonteCarlo
{
  public class SimulatedPrice
  {
    public static IDiscretizationScheme DiscretizationScheme { get; set; }

    public static double Volatility { get; set; }
    public static double Drift { get; set; }
    public static double SpotPrice { get; set; }
    public static double StrikePrice { get; set; }

    public static uint Steps { get; set; }

    public const double Delta = 1 / 252.0;

    public double[] SimulatedPriceArray { get; private set; }

    public SimulatedPrice()
    {
      this.SimulatedPriceArray = new double[SimulatedPrice.Steps];
      this.SimulatedPriceArray[0] = SimulatedPrice.SpotPrice;
    }

    public Task RunSim() 
    {
      return Task.Run(() => SimulatePrice());
    }

    public void SimulatePrice()
    {
      for (uint i = 1; i < SimulatedPrice.Steps; i++)
      {
        SimulatedPriceArray[i] = DiscretizationScheme.Increment(SimulatedPriceArray[i-1]);
      }
    }


  }
}
