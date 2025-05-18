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
            Assert.That(_page.Url.ToString(), Is.EqualTo(_loginPage.LoginPageUrl), "User is not at Login Page");
        }

        [When("User enters valid username {string} and password {string}")]
        public async Task WhenUserEntersCredentials(string username, string password)
        {
            await _loginPage.LoginActionAsync(username,password);
        }

        [Then("User should be redirected to the home page")]
        public void ThenUserShouldBeRedirectedToTheHomePage()
        {
            _page.WaitForLoadStateAsync();
            Assert.Multiple(() =>
            {
                Assert.That(_page.Url.ToString(), Is.EqualTo(_inventoryPage.InventoryPageUrl));
                Assert.That(_inventoryPage.isPageTitleVisibleAsync, Is.True, "User is not at Inventory Page");
            });
        }

        [When("User enters invalid username {string} and password {string}")]
        public async Task WhenUserEntersInvalidUsernameAndPassword(string username, string password)
        {
            await _loginPage.LoginActionAsync(username, password);
        }

        [Then("Error message related to Invalid_User will be prompted")]
        public void ThenErrorMessageRelatedToInvalid_UserWillBePrompted()
        {
            Assert.That(_loginPage.GetErrorMessageAsync, Does.Contain(ErrorMessage.InvalidUser));
        }

        [Then("User should not be redirected to the inventory page")]
        public void ThenUserShouldNotBeRedirectedToTheHomePage()
        {
            _page.WaitForLoadStateAsync();
            Assert.Multiple(() =>
            {
                Assert.That(_page.Url.ToString(), Is.EqualTo(_loginPage.LoginPageUrl), "User is not at Login Page");
                Assert.That(_loginPage.isErrorMessageVisibleAsync, Is.True);
            });
        }

        [When("User enters locked out username {string} and password {string}")]
        public async Task WhenUserEntersLockedOutUsernameAndPassword(string username, string password)
        {
            await _loginPage.LoginActionAsync(username, password);
        }

        [Then("Error message related to Locked_Out state will be prompted")]
        public void ThenErrorMessageRelatedToLocked_OutStateWillBePrompted()
        {
            Assert.That(_loginPage.GetErrorMessageAsync, Does.Contain(ErrorMessage.LockedOutUser));
        }
    }
}
