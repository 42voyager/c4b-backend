using System;
using backend.Interfaces;

namespace backend.Services
{
	public class CreditService : ICreditService
	{
		public double CalculateIncome(int Limit, int Installment)
		{
			double perc = 0.2;
			double interest = 0.05;
			double Income = ((Limit + Limit * interest) / Installment) * (1 / perc);

			if (Income < 20000) return AproximateToRange(Income, 500);
			if (Income < 50000) return AproximateToRange(Income, 1000);
			if (Income < 100000) return AproximateToRange(Income, 5000);
			if (Income < 500000) return AproximateToRange(Income, 10000);
			return AproximateToRange(Income, 50000);
		}

		private double AproximateToRange(double input, double range)
		{
			if (input < range) return range;

			double inputAsDecimal = input / range;
			double inputAsDecimalRounded = Math.Round(inputAsDecimal);
			double inputToNearestRange = inputAsDecimalRounded * range;
			return inputToNearestRange;
		}
	}
}