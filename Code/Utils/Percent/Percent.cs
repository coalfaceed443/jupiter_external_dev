using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Helper methods for dealing with UK time
/// </summary>
/// 

namespace Utils
{
    public static class Percent
    {

        static public double GetPercent(int smaller, int larger, int dp = 0)
        {
            return GetPercent((decimal)smaller, (decimal)larger, dp);
        }

        static public Decimal GetPercentDecimal(decimal smaller, decimal larger, int dp = 0)
        {
            try{
                return (decimal)GetPercent(smaller, larger, dp);
            }catch{
                return 0;
            }
        }

        static public double GetPercent(decimal smaller, decimal larger, int dp = 0)
        {
            return GetPercent((double)smaller, (double)larger, dp);
        }

        static public double GetPercent(double smaller, double larger, int dp = 0)
        {
            double ToReturn = 0;

            if (larger == 0 && smaller < 0)
            { return (double)100; }

            if (!(larger == 0 && smaller == 0))
            {
                double fullPercent = (smaller / larger) * 100d;

                double roundedPercent = Math.Round(fullPercent, dp);

                ToReturn = (double)roundedPercent;
            }

            return ToReturn;
        }
    }
}
