using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pricing.Option.MonteCarlo
{
  public class Simulator
  {
    public event Action SimulationComplete;

    public UInt32 nSims {get; set;}
    public List<SimulatedPrice> PriceSimList { get; private set; }
    public bool UseMultiCore = true;

    public void RunSimulation()
    {
      PriceSimList = Enumerable.Range(0, (int)this.nSims).Select(i => new SimulatedPrice()).ToList();

      if (UseMultiCore)
        MultiThread();
      else
        SingleThread();

      if (SimulationComplete != null)
        SimulationComplete();
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

    public double CallValule()
    {
      return Math.Exp(-SimulatedPrice.Drift * SimulatedPrice.Steps * SimulatedPrice.Delta) * this.PriceSimList.Select(p => Math.Max(p.SimulatedPriceArray.Last() - SimulatedPrice.StrikePrice, 0)).Average();
    }

    public double StandardError()
    {
      double[] values = this.PriceSimList.Select(p => p.SimulatedPriceArray.Last()).ToArray();
      double avg = values.Average();
      double sum = values.Sum(v => Math.Pow(v - avg, 2));
      return Math.Sqrt(sum / Math.Pow(values.Length, 2));
    }

  }
}
