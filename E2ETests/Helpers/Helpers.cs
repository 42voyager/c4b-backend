using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace E2ETests.Helpers
{
    public static class Helpers
    {
        /// <summary>
		/// Take a screenshot of the current state of the page and save it in the root project folder
		/// Em modo Async.
		/// </summary>
		/// <param name="page"> Id do feedback desejado. </param>
        public static async Task ScreenshotAsync(IPage page)
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            var title = await page.TitleAsync();
            var path = $"../../../screenshots/{date}_{title}-.png";
            var so = new PageScreenshotOptions()
            {
                Path = path,
                FullPage = true,
            };
            await page.ScreenshotAsync(so);
        }
    }
}