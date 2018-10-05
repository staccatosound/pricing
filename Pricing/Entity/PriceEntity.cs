using System;

namespace Pricing.Entity
{
  public class PriceEntity
  {
    public DateTime date { get; set; }
    public double price { get; set; }

    public static PriceEntity FromCsv(string csv)
    {
      string[] values = csv.Split(',');
      var d = DateTime.Parse(values[0]);
      var p = 0.0;
      double.TryParse(values[1], out p);

      return new PriceEntity(){date=d, price=p};
    }
  }
}
