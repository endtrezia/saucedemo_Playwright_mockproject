using Microsoft.Playwright;
using NUnit.Framework;
using Reqnroll;
using saucedemo_Playwright_mockproject.Driver;
using saucedemo_Playwright_mockproject.Enums;
using saucedemo_Playwright_mockproject.Pages;


namespace saucedemo_Playwright_mockproject.Steps
{
    [Binding]
    public class LoginStepDefinitions
    {
        private readonly IPage _page;
        private readonly SimpleDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly ProductListingPage _inventoryPage;

        //Injecting the pages and driver instances into the constructor
        public LoginStepDefinitions(IPage page, SimpleDriver driver)
        {
            _page = page;
            _driver = driver;
            _loginPage = new LoginPage(_page);
            _inventoryPage = new ProductListingPage(_page);
        }

        [Given("Annonymous user is on the login page")]
        public void GivenAnnonymousUserIsOnTheLoginPage()
        {
            //Check if the user is on the login page using URL
            Assert.That(_page.Url.ToString(), Is.EqualTo(_loginPage.LoginPageUrl), ValidatorMessage.UserNotAt.ReturnMessageWithParam("Login Page"));
        }

        [When("User enters valid username {string} and password {string}")]
        public async Task WhenUserEntersCredentials(string username, string password)
        {
            //Try Login with the provided credentials
            await _loginPage.LoginActionAsync(username,password);
        }

        [Then("User should be redirected to the home page")]
        public void ThenUserShouldBeRedirectedToTheHomePage()
        {
            //Wait for the page to load and check if the user is on the inventory page using URL
            _page.WaitForLoadStateAsync();
            Assert.Multiple(() =>
            {
                Assert.That(_page.Url.ToString(), Is.EqualTo(_inventoryPage.InventoryPageUrl));
                //Check if the page title is visible and correct
                Assert.That(_inventoryPage.IsPageTitleVisibleAsync().Result && _inventoryPage.IsPageTitleCorrectAsync("Products").Result, Is.True, ValidatorMessage.UserNotAt.ReturnMessageWithParam("Inventory Page"));
            });
        }

        [When("User enters invalid username {string} and password {string}")]
        public async Task WhenUserEntersInvalidUsernameAndPassword(string username, string password)
        {
            //Try Login with the provided credentials
            await _loginPage.LoginActionAsync(username, password);
        }

        [Then("Error message related to Invalid_User will be prompted")]
        public void ThenErrorMessageRelatedToInvalid_UserWillBePrompted()
        {
            //Check if the error message is displayed and contains the expected text
            Assert.That(_loginPage.GetErrorMessageAsync, Does.Contain(ErrorMessage.InvalidUser));
        }

        [Then("User should not be redirected to the inventory page")]
        public void ThenUserShouldNotBeRedirectedToTheHomePage()
        {
            //Wait for the page to load
            _page.WaitForLoadStateAsync();
            //And check if the user is still on the login page
            Assert.Multiple(() =>
            {
                Assert.That(_page.Url.ToString(), Is.EqualTo(_loginPage.LoginPageUrl), ValidatorMessage.UserNotAt.ReturnMessageWithParam("Login Page"));
                Assert.That(_loginPage.isErrorMessageVisibleAsync, Is.True);
            });
        }

        [When("User enters locked out username {string} and password {string}")]
        public async Task WhenUserEntersLockedOutUsernameAndPassword(string username, string password)
        {
            //Try Login with the provided credentials
            await _loginPage.LoginActionAsync(username, password);
        }

        [Then("Error message related to Locked_Out state will be prompted")]
        public void ThenErrorMessageRelatedToLocked_OutStateWillBePrompted()
        {
            //Check if the error message is displayed and contains the expected text
            Assert.That(_loginPage.GetErrorMessageAsync, Does.Contain(ErrorMessage.LockedOutUser));
        }
    }
}
