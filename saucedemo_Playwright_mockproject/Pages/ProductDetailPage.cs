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
    public class ProductDetailPage
    {
        private readonly IPage _page;
        public ProductDetailPage(IPage page) => _page = page;
        public string ProductDetailPageUrl => _page.Url;
        //Init the locators
        private ILocator _inventoryItemsName() => _page.Locator(".inventory_details_name");
        private ILocator _inventoryItemsPrice() => _page.Locator(".inventory_details_price");
        private ILocator _inventoryItemsDescription() => _page.Locator(".inventory_details_desc");
        private ILocator _addToCartButton() => _page.Locator("#add-to-cart");

        public async Task<string> GetInventoryItemsName() => await _inventoryItemsName().IsVisibleAsync() ? await _inventoryItemsName().InnerTextAsync() : string.Empty;
        public async Task<string> GetInventoryItemsDescription() => await _inventoryItemsDescription().IsVisibleAsync() ? await _inventoryItemsDescription().InnerTextAsync() : string.Empty;

        public async Task<Product> GetInventoryItemAsObjectAsync()
        {
            //Get the item name, price and description from the inventory items
            string itemName = await GetInventoryItemsName();
            string itemPriceText = await _inventoryItemsPrice().InnerTextAsync();
            //Remove $ sign and convert to decimal
            decimal itemPrice = Decimal.Parse(itemPriceText.ToString().Replace("$", "").Trim(), CultureInfo.InvariantCulture);
            string itemDescription = await GetInventoryItemsDescription();
            //Add the Product object to the inventoryItems list
            Product product = new Product(itemPrice, itemName, itemDescription);
            return product;
        }
    }
}
