using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pricing.Option.MonteCarlo
{
  public class MonteCarloSimulator
  {
    public List<SimulatedPrice> PriceSimList { get; private set; }
    private bool UseMultiCore = true;

    private double volatility { get; set; }
    private double riskFree { get; set; }
    private double assetPrice { get; set; }
    private double strike { get; set; }
    private double maturity { get; set; }
    private int steps { get; set; }
    private EPutCall putCall = EPutCall.Call;
    private int nSims { get; set; }

    private IDiscretizationScheme scheme;

    public MonteCarloSimulator(
      double assetPrice,
      double strike,
      double maturity,
      double volatility,
      double riskFree,
      EPutCall putCall,
      int steps,
      int nSims,
      IDiscretizationScheme scheme)
    {
      this.assetPrice = assetPrice;
      this.strike = strike;
      this.maturity = maturity;
      this.volatility = volatility;
      this.riskFree = riskFree;
      this.putCall = putCall;
      this.steps = steps;
      this.nSims = nSims;
      this.scheme = scheme;
    }

    public void RunSimulation()
    {
      PriceSimList = new List<SimulatedPrice>();
      for(var i=0; i<this.nSims; i++)
      {
        var simPrice = new SimulatedPrice(assetPrice, strike, maturity, volatility, riskFree, steps, scheme);
        PriceSimList.Add(simPrice);
      }

      if (UseMultiCore)
        MultiThread();
      else
        SingleThread();
    }

    private void SingleThread()
    {
      foreach (var e in PriceSimList)
        e.SimulatePrice();
    }

    private void MultiThread()
    {
      Task.WaitAll(PriceSimList.Select(c => c.RunSim()).ToArray());
    }

    public double CallValue()
    {
      return Math.Exp(-this.riskFree * this.steps * this.maturity / Const.kDaysInAYear) * this.PriceSimList.Select(p => Math.Max(p.simulatedPriceArray.Last() - this.strike, 0)).Average();
    }

    public double StandardError()
    {
      double[] values = this.PriceSimList.Select(p => p.simulatedPriceArray.Last()).ToArray();
      double avg = values.Average();
      double sum = values.Sum(v => Math.Pow(v - avg, 2));
      return Math.Sqrt(sum / Math.Pow(values.Length, 2));
    }

  }
}
