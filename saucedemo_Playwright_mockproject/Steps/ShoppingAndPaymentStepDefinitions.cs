using Microsoft.Playwright;
using Reqnroll;
using saucedemo_Playwright_mockproject.Driver;
using saucedemo_Playwright_mockproject.Enums;
using saucedemo_Playwright_mockproject.Model;
using saucedemo_Playwright_mockproject.Pages;
using System;
using System.Runtime.CompilerServices;

namespace saucedemo_Playwright_mockproject.Steps
{
    [Binding]
    public class ShoppingAndPaymentStepDefinitions
    {
        private readonly IPage _page;
        private readonly SimpleDriver _driver;
        private readonly ProductListingPage _inventoryPage;
        private readonly CartPage _cartPage;
        private readonly CheckoutStepInformationPage _checkoutInformationPage;
        private readonly CheckoutStepOverviewPage _checkoutStepOverviewPage;
        private readonly CheckoutCompletePage  _checkoutCompletePage;
        private readonly ScenarioContext _scenarioContext;

        public ShoppingAndPaymentStepDefinitions(IPage page, SimpleDriver driver, ScenarioContext scenarioContext)
        {
            _page = page;
            _driver = driver;
            _inventoryPage = new ProductListingPage(_page);
            _cartPage = new CartPage(_page);
            _checkoutInformationPage = new CheckoutStepInformationPage(_page);
            _checkoutStepOverviewPage = new CheckoutStepOverviewPage(_page);
            _checkoutCompletePage = new CheckoutCompletePage(_page);
            _scenarioContext = scenarioContext;
        }

        [When("User adds the following products to the cart:")]
        public async Task WhenUserAddsTheFollowingProductsToTheCart(DataTable dataTable)
        {
            List<string>? productList = [];
            await _page.WaitForLoadStateAsync();
            //Stop test if user not start and inventory page
            if (!_page.Url.ToString().Equals(_inventoryPage.InventoryPageUrl) || !_inventoryPage.IsPageTitleVisibleAsync().Result || !_inventoryPage.IsPageTitleCorrectAsync("Products").Result)
            {
                Assert.Fail(ValidatorMessage.UserNotAt.ReturnMessageWithParam("Inventory Page"));
            }
            //Check if cart was empty
            if (!await _inventoryPage.IsCartBadegeHiddenAsync())
            {
                Assert.Fail(ValidatorMessage.CartIsNotEmpty.RetunMessage());
            }
            //Loop through the data table rows and add product to cart
            foreach (var item in dataTable.Rows)
            {
                string productName = item["product"];
                productList.Add(productName);
                await _inventoryPage.ClickAddToCartByProductNameAsync(productName);
            }
            //Goto cart page
            await _inventoryPage.CartIconClickAsync();
            await _page.WaitForLoadStateAsync();
            if (_page.Url != _cartPage.CartPageUrl || !_cartPage.IsPageTitleVisibleAsync().Result || !_cartPage.IsPageTitleCorrectAsync("Your Cart").Result)
            {
                Assert.Fail(ValidatorMessage.UserNotAt.ReturnMessageWithParam("Cart Page"));
            }
            //Check if the cart contains added products
            if (!await _cartPage.VerifyItemInCart(productList))
            {
                Assert.Fail(ValidatorMessage.CartIsEmpty.RetunMessage());
            }
            //Assign this for using later in payment overview
            _scenarioContext["productListName"] = productList;
        }

        [When("User proceeds to payment steps")]
        public async Task WhenUserProceedsToPaymentSteps()
        {
            await _cartPage.ClickCheckoutButton();
            await _page.WaitForLoadStateAsync();
            //Stop test if user can't access the checkout information page
            if (!_page.Url.ToString().Equals(_checkoutInformationPage.CheckoutStepInformationPageUrl) || !_checkoutInformationPage.IsPageTitleVisibleAsync().Result || !_checkoutInformationPage.IsPageTitleCorrectAsync("Checkout: Your Information").Result)
            {
                Assert.Fail(ValidatorMessage.UserNotAt.ReturnMessageWithParam("Checkout: Information Page"));
            }
        }

        [When("User enters payment information:")]
        public async Task WhenUserEntersPaymentInformation(DataTable formData)
        {
            await _page.WaitForLoadStateAsync();
            //Using this instead of create Object because there is only 1 row.
            await _checkoutInformationPage.FillInformationFormnAsync(formData.Rows[0]["firstName"], formData.Rows[0]["lastName"], formData.Rows[0]["postalCode"]);
            await _checkoutInformationPage.ClickContinueButtonAsync();
        }

        [When("User continues and review their order checkout page")]
        public async Task WhenUserContinuesPaymentAndReviewOrder()
        {
            if (!_page.Url.ToString().Equals(_checkoutStepOverviewPage.CheckoutStepOverviewPageUrl) || !_checkoutStepOverviewPage.IsPageTitleVisibleAsync().Result || !_checkoutStepOverviewPage.IsPageTitleCorrectAsync("Checkout: Overview").Result)
            {
                Assert.Fail(ValidatorMessage.UserNotAt.ReturnMessageWithParam("Checkout: Overview Page"));
            }
            await _page.WaitForLoadStateAsync();
            var inventoryItem = await _checkoutStepOverviewPage.GetAllOverviewItemAsObjectAsync();
            //Get the product list from the scenario context
            var overviewProductListName = _scenarioContext["productListName"] as List<string>;
            decimal productTotalPrice = 0;
            //Loop through the product list and sum the price of all products
            foreach (var item in overviewProductListName)
            {
                var product = inventoryItem.Cast<Product>().FirstOrDefault(p => p.ProductName == item);
                if (product != null)
                {
                    productTotalPrice += product.Price; //Sum here
                }
                else
                {
                    Assert.Fail(ValidatorMessage.ProductNotFoundInOverview.ReturnMessageWithParam(item));
                } 
            }
            //Check if the subtotal value is equal to the sum of all product prices
            Assert.That(await _checkoutStepOverviewPage.GetSubtotalValueAsync(), Is.EqualTo(productTotalPrice).Within((decimal)0.01), ValidatorMessage.SubtotalValueNotCorrect.RetunMessage());
        }

        [When("User finishes the payment process")]
        public async Task WhenUserFinishesPayment()
        {
            await _page.WaitForLoadStateAsync();
            //Finish the payment process
            await _checkoutStepOverviewPage.ClickFinishButtonAsync();
        }

        [Then("The payment should be completed successfully")]
        public async Task ThenThePaymentShouldBeCompletedSuccessfully()
        {
            await _page.WaitForLoadStateAsync();
            //Check if user is on the checkout complete page
            var pageCheck = _page.Url.ToString().Equals(_checkoutCompletePage.CheckoutStepOverviewPageUrl) && _checkoutCompletePage.IsPageTitleVisibleAsync().Result && _checkoutCompletePage.IsPageTitleCorrectAsync("Checkout: Complete!").Result;
            Assert.That(pageCheck, Is.True, ValidatorMessage.UserNotAt.ReturnMessageWithParam("Overview Page"));
        }
    }
}
