using Pricing.Option;
using Pricing.Option.BinomialTree;
using Pricing.Option.BlackScholes;
using Pricing.Option.MonteCarlo;
using System;
using System.Collections.Generic;

namespace Pricing
{
  class Program
  {
    static void Main(string[] args)
    {
      var time_start = DateTime.Now;
      WorkWithSteps();
      WorkBlackScholes();
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
      for (var step = 1; step < 170; step++)
      {
        BinomialTree tree = new BinomialTree(100, 95, 0.5, 0.3, 0.08, EPutCall.Call, step);
        double option_price = tree.OptionValue();
        Console.WriteLine("Step : {0}, Option Price : {1}", step, option_price);
      }
    }

    static void WorkSimulateMonteCarlo()
    {
      var sim = new Simulator();
      SimulatedPrice.DiscretizationScheme = new EulerDiscretization();
      SimulatedPrice.Drift = 0.1;
      SimulatedPrice.Volatility = 0.1;
      SimulatedPrice.SpotPrice = 50;
      SimulatedPrice.StrikePrice = 55;
      SimulatedPrice.Steps = 252;
      sim.nSims = 10000;
      sim.RunSimulation();
      var call_value = sim.CallValue();

      Console.WriteLine("Call Value: {0}", call_value);
    }

    static void WorkBlackScholes()
    {
      var option_price = BlackScholes.Calculate(95, 100, 0.5, 0.08, 0.3);
      Console.WriteLine("Option Price (Black-Sholes): {0}", option_price);
    }

  }
}
