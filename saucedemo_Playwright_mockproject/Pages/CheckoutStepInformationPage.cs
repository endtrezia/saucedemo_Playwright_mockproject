using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Pages
{
    public class CheckoutStepInformationPage
    {
        private readonly IPage _page;
        public CheckoutStepInformationPage(IPage page) => _page = page;
        public string ProductDetailPageUrl => _page.Url;
        public string CheckoutStepInformationPageUrl => _page.Url;
        //Locators
        private ILocator _pageTitle() => _page.Locator(".title");
        private ILocator _firstNameInput() => _page.Locator("#first-name");
        private ILocator _lastNameInput() => _page.Locator("#last-name");
        private ILocator _postalCodeInput() => _page.Locator("#postal-code");
        private ILocator _continueButton() => _page.Locator("#continue");
        private ILocator _cancelButton() => _page.Locator("#cancel");
        public async Task FillInformationFormnAsync(string firstName, string lastName, string postalCode)
        {
            await _page.WaitForLoadStateAsync();
            await _firstNameInput().FillAsync(firstName);
            await _lastNameInput().FillAsync(lastName);
            await _postalCodeInput().FillAsync(postalCode);
        }
        public async Task ClickContinueButtonAsync() => await _continueButton().ClickAsync();
        public async Task ClickCancelButtonAsync() => await _cancelButton().ClickAsync();
        public async Task<bool> IsPageTitleVisibleAsync() => await _pageTitle().IsVisibleAsync();
        public async Task<bool> IsPageTitleCorrectAsync(string expectedTitle)
        {
            var actualTitle = await _pageTitle().InnerTextAsync();
            return actualTitle.Equals(expectedTitle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
