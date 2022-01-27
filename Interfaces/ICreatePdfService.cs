
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
	public interface ICreatePdfService
	{
		Task<string> CreatePdf(int customerID);
	}
}