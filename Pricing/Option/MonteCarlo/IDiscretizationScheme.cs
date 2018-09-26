
namespace Pricing.Option.MonteCarlo
{
  public interface IDiscretizationScheme
  {
    string Name { get; }
    double Increment(double valule);
  }
}
