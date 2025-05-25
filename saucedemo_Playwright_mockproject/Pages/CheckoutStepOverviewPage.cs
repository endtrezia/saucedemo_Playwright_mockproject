using Microsoft.Playwright;
using saucedemo_Playwright_mockproject.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Pages
{
    public class CheckoutStepOverviewPage
    {
        private readonly IPage _page;
        public CheckoutStepOverviewPage(IPage page) => _page = page;
        public string CheckoutStepOverviewPageUrl => _page.Url;
        private ILocator _pageTitle() => _page.Locator(".title");
        private ILocator _FinishButton() => _page.Locator("#finish");
        private ILocator _checkoutCancelButton() => _page.Locator("#cancel");
        private ILocator _overviewItems() => _page.Locator(".cart_item");
        private ILocator _overviewItemsName() => _page.Locator(".inventory_item_name");
        private ILocator _overviewItemsPrice() => _page.Locator(".inventory_item_price");
        private ILocator _overviewItemsDescription() => _page.Locator(".inventory_item_desc");
        private ILocator _subtotalValue() => _page.Locator("[data-test='subtotal-label']");
        private ILocator _VATlValue() => _page.Locator("[data-test='tax-label']");
        private ILocator _TotalValue() => _page.Locator("[data-test='total-label']");

        public async Task<bool> IsPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
        public async Task<bool> IsPageTitleCorrectAsync(string expectedTitle)
        {
            var actualTitle = await _pageTitle().InnerTextAsync();
            return actualTitle.Equals(expectedTitle, StringComparison.OrdinalIgnoreCase);
        }
        public async Task ClickFinishButtonAsync() => await _FinishButton().ClickAsync();
        public async Task<List<object>> GetAllOverviewItemAsObjectAsync()
        {
            var overviewItem = new List<object>();
            for (int i = 0; i < await _overviewItems().CountAsync(); i++)
            {
                //Get the item name, price and description from the inventory items
                string itemName = await _overviewItemsName().Nth(i).InnerTextAsync();
                string itemPriceText = await _overviewItemsPrice().Nth(i).InnerTextAsync();
                //Remove $ sign and convert to decimal
                decimal itemPrice = Decimal.Parse(itemPriceText.ToString().Replace("$", "").Trim(), CultureInfo.InvariantCulture);
                string itemDescription = await _overviewItemsDescription().Nth(i).InnerTextAsync();
                //Add the Product object to the inventoryItems list
                overviewItem.Add(new Product(itemPrice, itemName, itemDescription));
            }
            return overviewItem;
        }
        public async Task<decimal> GetSubtotalValueAsync()
        {
            var subtotalText = await _subtotalValue().InnerTextAsync();
            //Get only the numeric part of the subtotal text using regex
            Match regrexFilteredString = Regex.Match(subtotalText, @"\d+(\.\d+)?");
            return Decimal.Parse(regrexFilteredString.ToString().Trim(), CultureInfo.InvariantCulture);
        }
    }
}
