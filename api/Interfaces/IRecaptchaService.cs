using System.Threading.Tasks;

namespace backend.Interfaces
{
	public interface IRecaptchaService
	{
		Task<bool> ValidateRecaptchaScore(string token);
	}
}