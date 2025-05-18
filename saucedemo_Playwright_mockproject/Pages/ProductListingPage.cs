using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Pages
{
     class ProductListingPage
    {
        private readonly IPage _page;
        public ProductListingPage(IPage page) => _page = page;
        public string InventoryPageUrl => _page.Url;
        private ILocator _pageTitle() => _page.Locator("[data-test='title']");

        public async Task<bool> isPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
    }
}
