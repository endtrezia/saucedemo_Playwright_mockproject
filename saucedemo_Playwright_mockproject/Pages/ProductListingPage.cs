using Microsoft.Playwright;
using saucedemo_Playwright_mockproject.Model;
using System.Globalization;

namespace saucedemo_Playwright_mockproject.Pages
{
     class ProductListingPage
    {

        private readonly IPage _page;
        public ProductListingPage(IPage page) => _page = page;
        public string InventoryPageUrl => _page.Url;
        //Init the locators
        private ILocator _pageTitle() => _page.Locator("[data-test='title']");
        private ILocator _inventoryList() => _page.Locator(".inventory_list");
        private ILocator _inventoryItems() => _page.Locator(".inventory_item");
        private ILocator _sortMenu() => _page.Locator("select.product_sort_container");
        private ILocator _sortMenuOptions() => _sortMenu().Locator("option");
        //unused locators, but can be used in the future if needed
        private ILocator _inventoryItemsName() => _page.Locator(".inventory_item_name");
        private ILocator _inventoryItemsPrice() => _page.Locator(".inventory_item_price");
        private ILocator _inventoryItemsDescription() => _page.Locator(".inventory_item_desc");
        //Init the Actions
        public async Task<bool> IsPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
        public async Task<bool> IsInventoryListVisibleAsync() => await _inventoryList().IsVisibleAsync();
        public async Task SortMenuClickAsync() => await _sortMenu().ClickAsync();
        public async Task<bool> IsPageTitleCorrectAsync(string expectedTitle)
        {
            var actualTitle = await _pageTitle().InnerTextAsync();
            return actualTitle.Equals(expectedTitle, StringComparison.OrdinalIgnoreCase);
        }
        public async Task selectSortOption(string option)
        {
            await SortMenuClickAsync();
            await _sortMenu().SelectOptionAsync(new SelectOptionValue { Label = option });
        }
        public async Task<List<object>> GetAllInventoryItemAsObjectAsync()
        {
            var inventoryItems = new List<object>();
            for ( int i = 0; i < await _inventoryItems().CountAsync(); i++)
            {
                //Get the nth inventory item
                var item = _inventoryItems().Nth(i);
                //Get the item name, price and description from the inventory items
                string itemName = await _inventoryItemsName().Nth(i).InnerTextAsync();
                string itemPriceText = await _inventoryItemsPrice().Nth(i).InnerTextAsync();
                //Remove $ sign and convert to decimal
                decimal itemPrice = Decimal.Parse(itemPriceText.ToString().Replace("$","").Trim(), CultureInfo.InvariantCulture);
                string itemDescription = await _inventoryItemsDescription().Nth(i).InnerTextAsync();
                //Add the Product object to the inventoryItems list
                inventoryItems.Add(new Product(itemPrice, itemName, itemDescription));
            }
            return inventoryItems;
        }
        private ILocator GetAncestorLocator(ILocator locator, int level)
        {
            //Go back level layer in DOM to get the Parrent
            var root = locator;
            for (int i = 0; i < level; i++)
            {
                root = root.Locator("..");
            }
            return root;
        }
        public async Task ClickAddToCartByProductNameAsync(string productName)
        {
            //Get the product name locator from the inventory items
            var productNameLocator = _inventoryItemsName().Filter(new() { HasTextString = productName });
            // Go back 3 layer in DOM to get the Parrent
            var parrentLocator = GetAncestorLocator(productNameLocator, 3); 
            // Narrow down again
            var addToCartButton = parrentLocator.Filter(new() { Has = _page.Locator("button.btn.btn_primary")});
            if(!await addToCartButton.IsVisibleAsync())
            {
                return;
            }
            await addToCartButton.ClickAsync();
        }
        public async Task ClickProductNameAsync(string productName)
        {
            //Get the product name locator from the inventory items
            var productNameLocator = _inventoryItemsName().Filter(new() { HasTextString = productName });
            if (!await productNameLocator.IsVisibleAsync())
            {
                return;
            }
            await productNameLocator.ClickAsync();
        }
    }
}
