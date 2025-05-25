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
    public class CartPage
    {
        private readonly IPage _page;
        public CartPage(IPage page) => _page = page;
        public string CartPageUrl => _page.Url;
        private ILocator _cartItems() => _page.Locator(".cart_item");
        private ILocator _checkoutButton() => _page.Locator("#checkout");
        private ILocator _pageTitle() => _page.Locator(".title");
        public async Task ClickCheckoutButton() => await _checkoutButton().ClickAsync();
        public async Task<bool> IsPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
        public async Task<bool> IsPageTitleCorrectAsync(string expectedTitle)
        {
            var actualTitle = await _pageTitle().InnerTextAsync();
            return actualTitle.Equals(expectedTitle, StringComparison.OrdinalIgnoreCase);
        }
        public async Task<bool> VerifyItemInCart(List<string> productNames)
        {
            await _page.WaitForLoadStateAsync();
            bool productInCartName = false;
            //Check item name in cart
            foreach (var item in productNames)
            {
                //If item exist (visible) , set to True
                productInCartName = _cartItems().Filter(new() { HasText = item }).IsVisibleAsync().Result;
                //Else stop checking
                if(!productInCartName)
                {
                    break;
                }
            }
            return productInCartName;
        }
    }
}
