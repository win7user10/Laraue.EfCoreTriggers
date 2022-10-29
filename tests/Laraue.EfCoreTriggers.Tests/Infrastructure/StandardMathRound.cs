using System;

namespace Laraue.EfCoreTriggers.Tests.Infrastructure
{
    internal class StandardMathRound
    {
        // This table is required for the Round function which can specify the number of digits to round to
        private static readonly double[] roundPower10Double = new double[] {
          1E0, 1E1, 1E2, 1E3, 1E4, 1E5, 1E6, 1E7, 1E8,
          1E9, 1E10, 1E11, 1E12, 1E13, 1E14, 1E15
        };
        private const double doubleRoundLimit = 1e16d;
        public static double RoundToNegativeInfinity(double value, int digits)
        {
            if (Math.Abs(value) < doubleRoundLimit)
            {
                double power10 = roundPower10Double[digits];

                value *= power10;

                value = (double)Floor((decimal)value);

                value /= power10;
            }

            return value;
        }
        public static decimal Floor(decimal d)
        {
            return decimal.Floor(d);
        }
    }
}
