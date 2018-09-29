using System;

namespace Pricing.Option.BinomialTree
{
  public class BinomialTree
  {
    private double assetPrice = 0.0;
    private double strike = 0.0;
    private double maturity = 0.0;
    private double volatility = 0.0;
    private double riskFreeRate = 0.0;
    private int steps = 0;

    private EPutCall putCall = EPutCall.Call;


    public BinomialTree() { }

    public BinomialTree(
      double assetPrice, 
      double strike, 
      double timeStep, 
      double volatility, 
      double riskFreeRate, 
      EPutCall putCall, 
      int steps)
    {
      this.assetPrice = assetPrice;
      this.strike = strike;
      this.maturity = timeStep;
      this.volatility = volatility;
      this.riskFreeRate = riskFreeRate;
      this.putCall = putCall;
      this.steps = steps;
    }

    private double BinomialCoefficient(int m, int n)
    {
      // factorial(n) / factorial(m) * factorial (n-m)
      var coef = 1.0;
      for(var i=0; i < m; i++)
      {
        coef *= (n - i);
        coef /= (m - i);
      }

      return coef;
    }

    private double BinomialNodeProbabilityValue(int m, int steps, double p)
    {
      return BinomialCoefficient(m, steps) * Math.Pow(p, (double)m) * Math.Pow(1 - p, (double)(steps - m));
    }

    public double OptionValue()
    {
      double totalValue = 0.0;
      double u = Math.Exp(volatility * Math.Sqrt(maturity / steps)); //1.01
      double d = 1/ u; //0.99

      double f = FutureValue(1.0, riskFreeRate, maturity / steps);
      double p = (f - d) / (u - d);

      double nodeValue = 0.0;
      double payoffValue = 0.0;

      for (int j = 0; j <= steps; j++)
      {
        double exercise_price = assetPrice * Math.Pow(u, (double)j) * Math.Pow(d, (double)(steps - j));
        payoffValue = Payoff(exercise_price, strike, putCall);
        nodeValue = BinomialNodeProbabilityValue(j, steps, p);
        totalValue += nodeValue * payoffValue;
      }

      return PresentValue(totalValue, riskFreeRate, maturity);
    }

    private double Payoff(double S, double X, EPutCall PutCall)
    {
      switch (PutCall)
      {
        case EPutCall.Call:
          return Math.Max(0.0, S - X);

        case EPutCall.Put:
          return Math.Max(0.0, X - S);

        default:
          return 0.0;
      }
    }

    private double FutureValue(double P, double r, double n)
    {
      return P * Math.Pow(1.0 + r, n);
    }

    private double PresentValue(double F, double r, double n)
    {
      return F / Math.Pow(1.0 + r, n);
    }
  }
}
