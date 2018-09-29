
namespace Pricing.Option.MonteCarlo
{
  public interface IDiscretizationScheme
  {
    string Name { get; }
    double Increment(double value, double volatility, double maturity, double riskFree);
  }
}
