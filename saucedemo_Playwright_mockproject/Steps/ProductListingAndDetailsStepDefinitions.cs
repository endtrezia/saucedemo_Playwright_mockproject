using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using Reqnroll;
using saucedemo_Playwright_mockproject.Driver;
using saucedemo_Playwright_mockproject.Enums;
using saucedemo_Playwright_mockproject.Model;
using saucedemo_Playwright_mockproject.Pages;
using saucedemo_Playwright_mockproject.Utils;
using System;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Steps
{
    [Binding]
    public class ProductListingAndDetailsStepDefinitions
    {
        private readonly IPage _page;
        private readonly SimpleDriver _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly ProductListingPage _inventoryPage;
        private readonly ProductDetailPage _productDetailPage;
        //Static Expected Product Data for using accross the step definitions
        private static List<Product> producData = BuildProductJSONData();

        public ProductListingAndDetailsStepDefinitions(IPage page, SimpleDriver driver, ScenarioContext scenarioContext)
        {
            _page = page;
            _driver = driver;
            _scenarioContext = scenarioContext;
            _loginPage = new LoginPage(_page);
            _inventoryPage = new ProductListingPage(_page);
            _productDetailPage = new ProductDetailPage(_page);
        }

        public static List<Product> BuildProductJSONData()
        {
            /*This whole code block is just to map Deserialized JSON data to Product object for expectation, 
            found out there is more simple way to do this by just directly deserializing the JSON to a list of Product objects.
            But "accidentally" create a Deserialize Ultity, so i just want use it.*/
            var expandoList = DeserializeJson.LoadJsonObjectData("Resources/ProductData.json");
            var data = expandoList.Select(
                item =>
                {
                    dynamic d = item;
                    return new Product
                    (
                        Convert.ToDecimal(d.Price),
                        d.Name.ToString(),
                        d.Description.ToString()
                    );
                }).ToList();
            //Get the inventory items from the page and map them to Product object
            return data;
        }

        [Given("User logged in as a {string} and {string}")]
        public async Task GivenUserLoggedInAsAStandard_User(string username, string password)
        {
            await _loginPage.LoginActionAsync(username,password);
        }

        [When("User view the product list")]
        public void WhenUserViewTheProductList()
        {
            _page.WaitForLoadStateAsync();
            if (!_page.Url.ToString().Equals(_inventoryPage.InventoryPageUrl) || !_inventoryPage.IsPageTitleVisibleAsync().Result || !_inventoryPage.IsPageTitleCorrectAsync("Products").Result)
            {
                return; // User is not on the inventory page, stop test.
            }
            // Check if the inventory list is visible
            Assert.That(_inventoryPage.IsInventoryListVisibleAsync().Result, Is.True, ValidatorMessage.NoInventory.RetunMessage());
        }

        [Then("User should see all products with correct names, description, and prices")]
        public async Task ThenUserShouldSeeAllProductsWithCorrectNamesDescriptionAndPrices()
        {
            //Reset sort order of Expected Product Data
            producData = producData.OrderBy(x => x.ProductName).ToList();
            var inventoryItem = await _inventoryPage.GetAllInventoryItemAsObjectAsync();
            //Check if the inventory items are not empty then check the count is equal to the expected product data
            if (producData.Any() && inventoryItem.Any())
            {
                Assert.That(producData.Count, Is.EqualTo(inventoryItem.Count));
            }
            else
            {
                Assert.Fail(ValidatorMessage.NoInventory.RetunMessage());
            }
            //Check each item in the inventory items list equal with the expected product data
            for (int i = 0; i < inventoryItem.Count; i++)
            {
                Assert.That(inventoryItem[i], Is.EqualTo(producData[i]), ValidatorMessage.InventoryMissing.ReturnMessageWithParam(producData[i].ProductName));
            }
        }

        [When("User sort products by {string}")]
        public async Task WhenUserSortProductsBy(string selection)
        {
            await _page.WaitForLoadStateAsync();
            //Click to open sort menu and select the sort option
            await _inventoryPage.SortMenuClickAsync();
            await _inventoryPage.selectSortOption(selection);   
            //Dynamically switch between sort options and update the expected list
            switch (selection)
            {
                case "Name (A to Z)":
                    producData =  producData.OrderBy(x => x.ProductName).ToList();
                    break;
                case "Name (Z to A)":
                    producData = producData.OrderByDescending(x => x.ProductName).ToList();
                    break;
                case "Price (low to high)":
                    producData = producData.OrderBy(x => x.Price).ToList();
                    break;
                case "Price (high to low)":
                    producData = producData.OrderByDescending(x => x.Price).ToList();
                    break;
                // do nothing
                default:
                    break;
            }
        }

        [Then("Products should be sorted by price in ascending order")]
        public async Task ThenProductsShouldBeSortedByPriceInAscendingOrder()
        {
            //Fetch the inventory items after sorting
            var inventoryItem = await _inventoryPage.GetAllInventoryItemAsObjectAsync();
            //Check if the inventory items are not empty
            if (!inventoryItem.Any())
            {
                Assert.Fail(ValidatorMessage.NoInventory.RetunMessage());
            }
            Assert.That(inventoryItem.First(), Is.EqualTo(producData.First()), ValidatorMessage.WrongSortOrder.RetunMessage());
        }

        [Then("products should be sorted by price in descending order")]
        public async Task ThenProductsShouldBeSortedByPriceInDescendingOrder()
        {
            //Fetch the inventory items after sorting
            var inventoryItem = await _inventoryPage.GetAllInventoryItemAsObjectAsync();
            //Check if the inventory items are not empty
            if (!inventoryItem.Any())
            {
                Assert.Fail(ValidatorMessage.NoInventory.RetunMessage());
            }
            Assert.That(inventoryItem.First(), Is.EqualTo(producData.First()), ValidatorMessage.WrongSortOrder.RetunMessage());
        }

        [Then("products should be sorted alphabetically by name")]
        public async Task ThenProductsShouldBeSortedAlphabeticallyByName()
        {
            //Fetch the inventory items after sorting
            var inventoryItem = await _inventoryPage.GetAllInventoryItemAsObjectAsync();
            //Check if the inventory items are not empty
            if (!inventoryItem.Any())
            {
                Assert.Fail(ValidatorMessage.NoInventory.RetunMessage());
            }
            Assert.That(inventoryItem.First(), Is.EqualTo(producData.First()), ValidatorMessage.WrongSortOrder.RetunMessage());
        }

        [When(@"User click on the (.*)")]
        public async Task WhenUserClickOnTheProduct(string product)
        {
            _scenarioContext["ProductName"] = product;
            //Reset sort order of Expected Product Data
            producData = producData.OrderBy(x => x.ProductName).ToList();
            //Click on the first product item
            await _inventoryPage.IsInventoryListVisibleAsync();
            await _inventoryPage.ClickProductNameAsync(product);
            

        }

        [Then("The product details page should display the correct product information")]
        public async Task ThenTheProductDetailsPageShouldDisplayTheCorrectProductInformation()
        {

            await _page.WaitForLoadStateAsync();
            //Check if standing at product detail page
            Assert.That(_page.Url, Does.Contain("inventory-item"), ValidatorMessage.UserNotAt.ReturnMessageWithParam("Detail Page"));
            //Get product detail on page
            Product productDetail = await _productDetailPage.GetInventoryItemAsObjectAsync();
            //Get the product name from the scenario context
            string productName = _scenarioContext["ProductName"].ToString();
            Product expectedProduct = producData.FirstOrDefault(x => x.ProductName.Equals(productName, StringComparison.OrdinalIgnoreCase));
            Assert.That(productDetail, Is.EqualTo(expectedProduct), ValidatorMessage.WrongProductDetail.ReturnMessageWithParam(expectedProduct.ProductName));
        }
    }
}
