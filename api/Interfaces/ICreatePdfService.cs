
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
	/// <summary>
	/// Interface <c>ICreatePdfService</c> tem os serviços que auxiliam os controllers
	/// nas requisições.
	/// Para criar e salvar o contrato no banco de dados.
	/// </summary>
	public interface ICreatePdfService
	{
		/// <summary>
		/// Este metódo <c>CreatePdf</c> cria um PDF em string com a informação completa do usuário: crédito, parcelas,
		/// dados bancários, CNPJ e contato.
		/// </summary>
		/// <param name="customerID">ID do usuário do banco de dados</param>
		Task CreatePdf(int customerID);

		/// <summary>
		/// Este metódo <c>CreateContract</c> salva o contrato no banco de dados.
		/// </summary>
		/// <param name="customerID">Id do customer pertencente do contrato.</param>
		/// <param name="pdfData">O Pdf</param>
		/// <returns></returns>
		Task CreateContract(int customerID, string pdfData);
	}
}