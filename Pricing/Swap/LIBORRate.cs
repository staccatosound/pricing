using Pricing.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pricing.Swap
{
  public static class LIBORRate
  {
    public static List<PriceEntity> Get1MonthLIBOR() 
    {
      var filepath = "Resource/USD1MTD156N.csv";

      List<PriceEntity> values = File.ReadAllLines(filepath)
                                 .Skip(1)
                                 .Select(v => PriceEntity.FromCsv(v))
                                 .ToList();

      return values;
    }
  }
}
