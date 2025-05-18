using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace saucedemo_Playwright_mockproject.Driver
{
    public class SimpleDriver
    {
        public IBrowser? Browser { get; set; }
        public IBrowserContext? BrowserContext { get; set; }
        public IPage? Page { get; set; }
        public string? BaseUrl { get; set; }

        // Initialize Playwright and launch a Chromium browser instance with param headless and page URL
        public async Task InitPlaywrightAsync(bool headless, string baseUrl)
        {
            var playwright = await Playwright.CreateAsync();
            //Launch a new Chrome instance with the specified headless option
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = headless });
            BrowserContext = await Browser.NewContextAsync(new BrowserNewContextOptions());
            //Create a new page in the browser context
            Page = await BrowserContext.NewPageAsync();
            BaseUrl = baseUrl;
            ViewportSize viewportSize = new ViewportSize { Width = 1920, Height = 1080 }; //Set the viewport size for the page
            //Force navigation on initialization
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                await Page.GotoAsync(baseUrl);
            }
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                await Page.GotoAsync(baseUrl);
            }

        }

        // Close the browser instance
        public async Task ClosePlaywrightAsync()
        {
            //Check if the Page, BrowserContext, and Browser are not null before closing them
            try
            {
                //Close everything
                await Page.CloseAsync();
                await BrowserContext.CloseAsync();
                await Browser.CloseAsync();
            }
            catch (PlaywrightException pe)
            {
                Console.WriteLine($"Error closing Playwright: {pe.Message}");

            }
        }
    }
}
