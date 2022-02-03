using System;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace E2ETests.PageObjects
{
	class CreditFormObjects
	{
		private readonly IPage _page;

		public CreditFormObjects(IPage page)
		{
			_page = page;
		}

        /// <summary>
		/// Get the locator object of the creditSlider input element
		/// </summary>
		public ILocator GetCreditSlider()
		{
			return _page.Locator("#credit-slider");
		}	
        /// <summary>
		/// Get the locator object of the Installments Slider input element
		/// </summary>
		public ILocator GetInstallmentsSlider()
		{
			return _page.Locator("#installments-slider");
		}
        /// <summary>
		/// Get the current value of the credit slider input
		/// </summary>
		public async Task<string> GetCreditInputValue()
		{
			var text = await _page.InputValueAsync("#input-credit-slider");
			return text;
		}
        /// <summary>
		/// Click an slider in the given position
		/// </summary>
		/// <param name="slider"> Locator Slider object </param>
		/// <param name="position"> Proportional Position being 0.0 most left and 1.0 most right of the element </param>
		public async Task ClickSliderPositionXAsync(ILocator slider, double position = 0.5)
		{
			var creditSliderInputBox = await slider.BoundingBoxAsync();
			int xPositionCredit = (int) Math.Floor(creditSliderInputBox.Width * position);
			await slider.ClickAsync(new LocatorClickOptions { Position = new Position { X = xPositionCredit, Y = 0 } });
		}
        /// <summary>
		/// Get the min income value that is displayed
		/// </summary>
		/// <returns> the income value</returns>
		public async Task<string> GetMinIncomeValue()
		{
			var span = await _page.WaitForSelectorAsync("xpath=//*[@id=\"form-request\"]/div[1]/div[4]/p/b", new PageWaitForSelectorOptions 
			{ 
				State = WaitForSelectorState.Visible,
				Timeout = 5000
			});
			var incomeBox = _page.Locator("xpath=//*[@id=\"form-request\"]/div[1]/div[4]/p/b");
			var textIncome = await incomeBox.TextContentAsync();
			return textIncome;
		}
	}
}