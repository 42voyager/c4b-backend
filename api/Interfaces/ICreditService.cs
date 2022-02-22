using backend.Models;
namespace backend.Interfaces
{
	public interface ICreditService
	{
		double CalculateIncome(Credit credit);
	}
}