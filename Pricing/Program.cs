using Pricing.Option;
using Pricing.Option.BinomialTree;
using Pricing.Option.BlackScholes;
using Pricing.Option.MonteCarlo;
using Pricing.Swap;
using System;
using System.Collections.Generic;

namespace Pricing
{
  class Program
  {
    static void Main(string[] args)
    {
      var time_start = DateTime.Now;
      WorkVanilla();
      var exec_time = DateTime.Now - time_start;
      Console.WriteLine("Execute Time : {0}ms", exec_time.TotalMilliseconds);
      Console.Read();
    }

    static void Work()
    {
      List<int> StrikePrices = new List<int> { };
      for (var i = 60; i <= 140; i++)
      { StrikePrices.Add(i); }

      var maturity = 1; // 1 year
      var volatility = 0.3;
      var risk_free_rate = 0.05;

      foreach (var S in StrikePrices)
      {
        BinomialTree tree = new BinomialTree(100, S, maturity, volatility, risk_free_rate, EPutCall.Put, 12);

        double option_price = tree.OptionValue();
        Console.WriteLine("Strike : {0}, Option Price : {1}", S, option_price);
      }
    }

    static void WorkWithSteps()
    {
      for (var step = 1; step < 1000; step++)
      {
        BinomialTree tree = new BinomialTree(100, 95, 0.5, 0.3, 0.08, EPutCall.Call, step);
        double option_price = tree.OptionValue();
        Console.WriteLine("Step : {0}, Option Price : {1}", step, option_price);
      }
    }

    static void WorkSimulateMonteCarlo()
    {
      double riskFree = 0.08;
      double volatility = 0.3;
      double assetPrice = 100;
      double strike = 95;
      double maturity = 0.5;
      int steps = 252;
      int nSims = 10000;     
      IDiscretizationScheme scheme = new EulerDiscretization();

      var sim = new MonteCarloSimulator(assetPrice, strike, maturity, volatility, riskFree, EPutCall.Call, steps, nSims, scheme);
      sim.RunSimulation();
      var call_value = sim.CallValue();

      Console.WriteLine("Call Value: {0}", call_value);
    }

    static void WorkBlackScholes()
    {
      var option_price = BlackScholes.Calculate(95, 100, 0.5, 0.08, 0.3);
      Console.WriteLine("Option Price (Black-Sholes): {0}", option_price);
    }

    static void WorkVanilla()
    {
      var fixe_rate = new VanillaSwap().calcFairFixedRateOld();
      Console.WriteLine("Fair Fixed Rate: {0}", fixe_rate);

      Console.WriteLine("Swap Curve");

      var swap_curve = new VanillaSwap().generateSwapCurve();
      foreach (var swap_rate in swap_curve)
      {
        Console.WriteLine("{0}, {1}", swap_rate.date.ToString("yyyyMMdd"), swap_rate.price);
      }
    }
  }
}
