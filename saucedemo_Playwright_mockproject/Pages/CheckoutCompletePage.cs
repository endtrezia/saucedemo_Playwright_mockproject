using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Pages
{
    public class CheckoutCompletePage
    {
        private readonly IPage _page;
        public CheckoutCompletePage(IPage page) => _page = page;
        public string CheckoutStepOverviewPageUrl => _page.Url;
        private ILocator _pageTitle() => _page.Locator(".title");
        private ILocator _thankYouMessage() => _page.Locator(".complete-header");
        private ILocator _backHomeButton() => _page.Locator("#back-to-products");
        public async Task<bool> IsPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
        public async Task<bool> IsThankYouMessageVisibleAsync() => await _thankYouMessage().IsVisibleAsync();
        public async Task ClickBackHomeButtonAsync() => await _backHomeButton().ClickAsync();
        public async Task IsBackHomeButtonVisibleAsync() => await _backHomeButton().IsVisibleAsync();
        public async Task<bool> IsPageTitleCorrectAsync(string expectedTitle)
        {
            var actualTitle = await _pageTitle().InnerTextAsync();
            return actualTitle.Equals(expectedTitle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
