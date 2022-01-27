using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.Interfaces;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;

namespace backend.Services
{
    public class CreatePdfService : ICreatePdfService
    {
		private readonly ICustomerService _customerService;
		private readonly IBankInfoService _bankInfoService;

		public CreatePdfService(ICustomerService customerService, IBankInfoService bankInfoService)
		{
			_customerService = customerService;
			_bankInfoService = bankInfoService;
		}
        public async Task<string> CreatePdf(int customerID)
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
			return "data:application/pdf;base64," + Convert.ToBase64String(bytes);
        }
    }
}