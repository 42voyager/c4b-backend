using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
	public class CreditService : ICreditService
	{
		public double CalculateIncome(int Limit, int Installment)
		{
			double perc = 0.2;
      		double interest = 0.05;
			double Income = ((Limit + Limit * interest) / Installment) * (1 / perc);
			return(Income);
		}
	}
}