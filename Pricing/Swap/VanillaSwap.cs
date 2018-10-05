using Pricing.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Swap
{
  public class VanillaSwap
  {
    double yield1y = 2.62;
    double yield2y = 2.85;
    double yield3y = 2.94;

    List<double> yield_curve;
    List<double> yield_curve_norm;

    List<PriceEntity> libor1m;
    List<PriceEntity> libor1m_norm;

    public VanillaSwap()
    {
      yield_curve = new List<double> { yield1y , yield2y, yield3y};
      yield_curve_norm = yield_curve.Select(t => 1 + t / 100).ToList();

      var libor1m_byDay = LIBORRate.Get1MonthLIBOR();
      libor1m = libor1m_byDay.GroupBy(t => t.date.ToString("yyyyMM"))
        .Select(g => new PriceEntity()
        {
          date = new DateTime(int.Parse(g.Key.Substring(0, 4)), int.Parse(g.Key.Substring(4, 2)), 1),
          price = g.Select(a => a.price).Average()
        }).ToList();

      libor1m_norm = normalize(libor1m);
    }

    public List<PriceEntity> generateSwapCurve()
    {
      var maturity = 2;
      var swap_curve = new List<PriceEntity>();

      var from_date = new DateTime(2008, 1, 1);
      var to_date = new DateTime(2009, 1, 1);

      var current_date = from_date;

      while (current_date < to_date)
      {
        var expected_floating_rate = libor1m
          .Where(t => t.date > current_date
            && t.date <= current_date.AddYears(maturity))
          .Select(t => t.price).ToList();
        
        var p = calcFairFixedRate(maturity, expected_floating_rate);

        swap_curve.Add(new PriceEntity() { date = current_date, price = p });
        current_date = current_date.AddMonths(1);
      }

      return swap_curve;
    }

    private List<PriceEntity> normalize(List<PriceEntity> rates)
    {
      var norm = new List<PriceEntity>();

      foreach (var r in rates)
      {
        norm.Add(new PriceEntity() { date = r.date, price = (1 + r.price / 100.0) });
      }

      return norm;
    }

    public double calcFairFixedRate(int maturity, List<double> expected_floating_rate)
    {
      var frequency = 12;
      var N = maturity * frequency;

      var last = expected_floating_rate[N - 1] / frequency;
      var fixed_rate = 100 - 100 / Math.Pow( 1 + last/100, N);

      var denominator = 0.0;
      for (var i = 0; i < N; i++)
      {
        var r = expected_floating_rate[i] / frequency;
        denominator += 1.0 / Math.Pow( 1 + r/100, i + 1);
      }

      fixed_rate = fixed_rate / denominator * frequency;
      return fixed_rate;

    }

    public double calcFairFixedRateOld()
    {
      var maturity = 3;
      var yield_last = yield_curve_norm[maturity - 1];
      var fixed_rate = 100 - 100 / Math.Pow(yield_last, maturity);

      var denominator = 0.0;
      for (var i=0; i<maturity; i++)
      {
        var r = yield_curve_norm[i];
        denominator += 1.0 / Math.Pow(r, i + 1);
      }

      fixed_rate = fixed_rate / denominator;
      return fixed_rate;
    }
  }
}
