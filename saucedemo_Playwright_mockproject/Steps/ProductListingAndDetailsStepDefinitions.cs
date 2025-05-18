using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using Reqnroll;
using saucedemo_Playwright_mockproject.Driver;
using saucedemo_Playwright_mockproject.Model;
using saucedemo_Playwright_mockproject.Pages;
using saucedemo_Playwright_mockproject.Utils;
using System;

namespace saucedemo_Playwright_mockproject.Steps
{
    [Binding]
    public class ProductListingAndDetailsStepDefinitions
    {
        private readonly IPage _page;
        private readonly SimpleDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly ProductListingPage _inventoryPage;

        public ProductListingAndDetailsStepDefinitions(IPage page, SimpleDriver driver)
        {
            _page = page;
            _driver = driver;
            _loginPage = new LoginPage(_page);
            _inventoryPage = new ProductListingPage(_page);
        }

        [Given("User logged in as a {string} and {string}")]
        public async Task GivenUserLoggedInAsAStandard_User(string username, string password)
        {
            await _loginPage.LoginActionAsync(username,password);
        }

        [When("User view the product list")]
        public void WhenUserViewTheProductList()
        {
            throw new PendingStepException();
        }

        [Then("User should see all products with correct names, images, and prices")]
        public void ThenUserShouldSeeAllProductsWithCorrectNamesImagesAndPrices()
        {
            List<Product> producData;
            try
            {
                producData = DeserializeJson.LoadJsonObjectData("Resources/ProductData.json").Cast<Product>().ToList();
            }
            catch (InvalidCastException ex) { }
            

        }

        [Given("User am logged in as a standard user")]
        public void GivenUserAmLoggedInAsAStandardUser()
        {
            throw new PendingStepException();
        }

        [When("User sort products by {string}")]
        public void WhenUserSortProductsBy(string p0)
        {
            throw new PendingStepException();
        }

        [Then("Products should be sorted by price in ascending order")]
        public void ThenProductsShouldBeSortedByPriceInAscendingOrder()
        {
            throw new PendingStepException();
        }

        [Then("products should be sorted by price in descending order")]
        public void ThenProductsShouldBeSortedByPriceInDescendingOrder()
        {
            throw new PendingStepException();
        }

        [Then("products should be sorted alphabetically by name")]
        public void ThenProductsShouldBeSortedAlphabeticallyByName()
        {
            throw new PendingStepException();
        }

        [When("User click on the first product")]
        public void WhenUserClickOnTheFirstProduct()
        {
            throw new PendingStepException();
        }

        [Then("The product details page should display the correct product information")]
        public void ThenTheProductDetailsPageShouldDisplayTheCorrectProductInformation()
        {
            throw new PendingStepException();
        }
    }
}
