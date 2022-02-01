using System;
using System.Threading.Tasks;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using E2ETests.PageObjects;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace E2ETests.Features
{

	public class SliderTest : PageTest
	{
		private string _url;
		private CreditFormObjects _creditFormObjects;

		[SetUp]
		public void Setup()
		{
			 var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);
			var config = builder.Build();
			_url = config.GetSection("SiteUrl").Value;
			_creditFormObjects = new CreditFormObjects(Page);
		}
		[Test, Description("Deveria mudar o valor do credit slider quando clickamos ele numa posicao")]
		public async Task SelectValueCreditSlider()
		{
			await Page.GotoAsync(_url);
			var creditSlider = _creditFormObjects.GetCreditSlider();
			await _creditFormObjects.ClickSliderPositionXAsync(creditSlider, 0.3);
		}
		[Test, Description("Deveria mudar o valor do installments slider quando clickamos ele numa posicao")]
		public async Task SelectValueInstallmentsSlider()
		{
			await Page.GotoAsync(_url);
			var installmentsSlider = _creditFormObjects.GetInstallmentsSlider();
			await _creditFormObjects.ClickSliderPositionXAsync(installmentsSlider, 0.3);
		}
		[Test, Description("Deveria mudar o valor do input credit quando clickamos numa posicao do slider")]
		public async Task SelectValueCreditSliderAndCheckInput()
		{
			await Page.GotoAsync(_url);
			var creditSlider = _creditFormObjects.GetCreditSlider();
			await _creditFormObjects.ClickSliderPositionXAsync(creditSlider, 0.3);
			// await Helpers.Helpers.Screenshot(Page);
			var creditValue = await _creditFormObjects.GetCreditInputValue();
			Assert.GreaterOrEqual(Convert.ToInt32(creditValue), 10000);
		}
		[Test, Description("Deveria calcular o min income quando clickamos o valor para o credit e para o isntallments")]
		public async Task CheckMinIncomeCalculation()
		{
			await Page.GotoAsync(_url);
			// Mudar o valor do credit slider
			var creditSlider = _creditFormObjects.GetCreditSlider();
			await _creditFormObjects.ClickSliderPositionXAsync(creditSlider, 0.3);
			// Mudar o valor do installments slider
			var installmentsSlider = _creditFormObjects.GetInstallmentsSlider();
			await _creditFormObjects.ClickSliderPositionXAsync(installmentsSlider, 0.3);
			// Obter o valor do min income
			var minIncome = await _creditFormObjects.GetMinIncomeValue();
			Assert.AreNotEqual(minIncome, " R$ 0,00");
		}
	}
}