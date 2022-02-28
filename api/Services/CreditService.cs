using System;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{

	/// <summary>
	/// Class <c>CreditService</c> implementa <c>ICreditService</c> interface.
	/// </summary>
	public class CreditService : ICreditService
	{

		/// <inheritdoc />
		public double CalculateIncome(Credit credit)
		{
			double perc = 0.2;
			double interest = 0.05;
			double Income = ((credit.Limit + credit.Limit * interest) / credit.Installment) 
				* (1 / perc);

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
			double inputAsDecimalRounded = Math.Ceiling(inputAsDecimal);
			double inputToNearestRange = inputAsDecimalRounded * range;
			return inputToNearestRange;
		}
	}
}