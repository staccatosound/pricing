using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Option
{
  public class BinomialTree
  {
    private double assetPrice = 0.0;
    private double strike = 0.0;
    private double timeStep = 0.0;
    private double volatility = 0.0;

    private EPutCall putCall = EPutCall.Call;

    private double riskFreeRate = 0.0;
    private int steps = 0;

    public double AssetPrice
    {
      get { return assetPrice; }
      set { assetPrice = value; }
    }

    public double Strike
    {
      get { return strike; }
      set { strike = value; }
    }

    public double TimeStep
    {
      get { return timeStep; }
      set { timeStep = value; }
    }

    public double Valatility
    {
      get { return volatility; }
      set { volatility = value; }
    }

    public EPutCall PutCall
    {
      get { return putCall; }
      set { putCall = value; }
    }

    public double RiskFreeRate
    {
      get { return riskFreeRate; }
      set { riskFreeRate = value; }
    }

    public int Steps
    {
      get { return steps; }
      set { steps = value; }
    }

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
      this.timeStep = timeStep;
      this.volatility = volatility;
      this.riskFreeRate = riskFreeRate;
      this.putCall = putCall;
      this.steps = steps;
    }

    private double BinomialCoefficient(int m, int n)
    {
      return Factorial(n) / (Factorial(m) * Factorial(n - m));
    }

    private double BinomialNodeProbabilityValue(int m, int steps, double p)
    {
      return BinomialCoefficient(m, steps) * Math.Pow(p, (double)m) * Math.Pow(1 - p, (double)(steps - m));
    }

    public double OptionValue()
    {
      double totalValue = 0.0;
      double u = OptionUp(timeStep, volatility, steps); //1.01
      double d = OptionDown(timeStep, volatility, steps); //0.99
      double p = Probability(timeStep, volatility, steps, riskFreeRate); //0.5

      double nodeValue = 0.0;
      double payoffValue = 0.0;

      for (int j = 0; j <= steps; j++)
      {
        double exercise_price = AssetPrice * Math.Pow(u, (double)j) * Math.Pow(d, (double)(steps - j));
        payoffValue = Payoff(exercise_price, strike, putCall);
        nodeValue = BinomialNodeProbabilityValue(j, steps, p);
        totalValue += nodeValue * payoffValue;
      }

      return PresentValue(totalValue, riskFreeRate, timeStep);
    }

    private double Payoff(double S, double X, EPutCall PutCall)
    {
      switch (PutCall)
      {
        case EPutCall.Call:
          return Call(S, X);

        case EPutCall.Put:
          return Put(S, X);

        default:
          return 0.0;
      }
    }

    private double Call(double S, double X)
    {
      return Math.Max(0.0, S-X);
    }

    private double Put(double S, double X)
    {
      return Math.Max(0.0, X-S);
    }

    private double OptionUp(double t, double s, int n)
    {
      return Math.Exp(s * Math.Sqrt(t / n));
    }

    private double OptionDown(double t, double s, int n)
    {
      return Math.Exp(-s * Math.Sqrt(t / n));
    }

    private double Probability(double t, double s, int n, double r)
    {
      double d1 = FutureValue(1.0, r, t/n);
      double d2 = OptionUp(t, s, n);
      double d3 = OptionDown(t, s, n);
      return (d1 - d3) / (d2 - d3);
    }

    private double FutureValue(double P, double r, double n)
    {
      return P * Math.Pow(1.0 + r, n);
    }

    private double PresentValue(double F, double r, double n)
    {
      return F / Math.Exp(r * n);
    }

    private double Factorial(int n)
    {
      double d = 1;
      for (int j = 1; j <= n; j++)
      {
        d *= j;
      }
      return d;
    }

  }
}
