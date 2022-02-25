using System;
using System.IO;
using System.Threading.Tasks;
using backend.Interfaces;
using backend.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Security;
using Microsoft.Extensions.Configuration;

namespace backend.Services
{

	/// <summary>
	/// Class <c>CreatePdfService</c> implementa <c>ICreatePdfService</c> interface.
	/// </summary>
    public class CreatePdfService : ICreatePdfService
    {
		private readonly ICustomerService _customerService;
		private readonly IBankInfoService _bankInfoService;
		private readonly IContractService _contractService;

		private readonly IConfiguration _configuration;

		/// <summary>
		/// Este construtor inicializa o <paramref name="customerService"/>, <paramref name="banInfoService"/> e
		/// <paramref name="contractService"/> que são os serviços a serem utilizados.
		/// </summary>
		/// <param name="customerService">Instância do ICustomerService</param>
		/// <param name="bankInfoService">Instância do IBanInfoService</param>
		/// <param name="contractService">Instância do IContractService</param>
		public CreatePdfService(
			ICustomerService customerService, 
			IBankInfoService bankInfoService, 
			IContractService contractService, 
			IConfiguration configuration)
		{
			_customerService = customerService;
			_bankInfoService = bankInfoService;
			_contractService = contractService;
			_configuration = configuration;
		}

		/// <inheritdoc />
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
			security.OwnerPassword = string.Format(_configuration.GetSection("Contract:OwnerPassword").Value);

			//It allows printing and accessibility copy content
			security.Permissions = PdfPermissionsFlags.Print | PdfPermissionsFlags.AccessibilityCopyContent;
			security.UserPassword = customer.Cnpj;

			string contractTitle = string.Format(_configuration.GetSection("Contract:Title").Value);
			///String for Contract PDF
			string[] pdfBody =  _configuration.GetSection("Contract:Template").Get<string[]>();
			string contractText = string.Format(
				String.Join("\n", pdfBody),
				customer.Company,
				customer.Cnpj,
				customer.Limit,
				customer.Installment,
				bankinfo.BankName,
				bankinfo.Branch,
				bankinfo.CheckingAccount,
				currentDate.ToString(),
				customer.Name
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

		/// <inheritdoc />
		public async Task CreateContract(int customerID, string pdfData) {
			var contract = new Contract();
			contract.CustomerID = customerID;
			contract.ContractPdf = pdfData;
			await _contractService.AddAsync(contract);
		}
    }
}