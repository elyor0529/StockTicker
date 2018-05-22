using System;

namespace StockTicker.Lib.Common.Utils
{
    public static class RandomHelper
    {
        public static double GetNumber(double minimum = 1, double maximum = 1000)
        {
            var random = new Random();

            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
