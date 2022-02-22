
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
	public interface ICreatePdfService
	{
		Task CreatePdf(int customerID);
		Task CreateContract(int customerID, string pdfData);
	}
}