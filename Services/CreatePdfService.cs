using System;
using System.IO;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Security;

namespace backend.Services
{
    public class CreatePdfService : ICreatePdfService
    {
		private readonly ICustomerService _customerService;
		private readonly IBankInfoService _bankInfoService;
		private readonly IContractService _contractService;

		public CreatePdfService(ICustomerService customerService, IBankInfoService bankInfoService, IContractService contractService)
		{
			_customerService = customerService;
			_bankInfoService = bankInfoService;
			_contractService = contractService;
		}
		/// <summary>
		/// Cria um PDF com a informação completa do usuário: crédito, parcelas,
		/// dados bancários, CNPJ e contato
		/// </summary>
		/// <param name="customerID">ID do usuário do banco de dados</param>
		public async Task CreatePdf(int customerID)
        {
			var customer = await _customerService.GetAsync(customerID);
			var	bankinfo = await _bankInfoService.GetAsync(customerID);
			var	currentDate = DateTime.Now;
            ///Create a new PDF document
            PdfDocument document = new PdfDocument();

            ///Add a page to the document
            PdfPage page = document.Pages.Add();

            ///Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            ///Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 14);

			PdfStringFormat format = new PdfStringFormat();

			///Document security.
			PdfSecurity security = document.Security;
			///Specifies key size and encryption algorithm.
			security.KeySize = PdfEncryptionKeySize.Key128Bit;
			security.Algorithm = PdfEncryptionAlgorithm.RC4;
			security.OwnerPassword = "voyager";

			//It allows printing and accessibility copy content
			security.Permissions = PdfPermissionsFlags.Print | PdfPermissionsFlags.AccessibilityCopyContent;
			security.UserPassword = customer.Cnpj;

			string contractTitle = string.Format("Contrato");
			///String for Contract PDF
			string contractText = string.Format(
				@$"
				O crédito outorgado a empresa {customer.Company} com CNPJ {customer.Cnpj}
				pela quantidade de R${customer.Limit},00 dividida em {customer.Installment} parcelas será
				depositado na seguinte conta:

				{bankinfo.BankName}
				Agência: {bankinfo.Branch}
				Conta: {bankinfo.CheckingAccount}

				São Paulo, {currentDate.ToString()}



				Assinaturas




				Responsável Crédito					Representante da empresa
						Caique									{customer.Name}
				"
			);
			format.Alignment = PdfTextAlignment.Left;
			///Measure string
			SizeF sizeTitle = font.MeasureString(contractTitle);
			SizeF sizeText = font.MeasureString(contractText);
            graphics.DrawString(contractTitle, font, PdfBrushes.Black, new PointF(250, 0));
			graphics.DrawString(contractText, font, PdfBrushes.Black, new Point(0, 50), format);

            ///Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);
            stream.Position = 0;
			document.Close(true);
            byte[] bytes = stream.ToArray();
			var pdfData = "data:application/pdf;base64," + Convert.ToBase64String(bytes);
			await CreateContract(customerID, pdfData);
        }
		public async Task CreateContract(int customerID, string pdfData) {
			var contract = new Contract();
			contract.CustomerID = customerID;
			contract.ContractPdf = pdfData;
			await _contractService.AddAsync(contract);
		}
    }
}