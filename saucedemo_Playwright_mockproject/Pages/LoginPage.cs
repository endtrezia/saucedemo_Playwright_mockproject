using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saucedemo_Playwright_mockproject.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        public LoginPage(IPage page) => _page = page;
        public string LoginPageUrl => _page.Url;
        //Init the locators
        private ILocator _txtUsername() => _page.Locator("#user-name");
        private ILocator _txtPassword() => _page.Locator("#password");
        private ILocator _btnLogin() => _page.Locator("#login-button");
        private ILocator _errMessage() => _page.Locator("[data-test='error']");
        private ILocator _errCloseButton() => _page.Locator("[data-test='error-button']");
        


        //Init the Actions
        public async Task ClickButtonLoginAsync() => await _btnLogin().ClickAsync();
        public async Task FillUsernameAsync(string username) => await _txtUsername().FillAsync(username);
        public async Task FillPasswordAsync(string password) => await _txtPassword().FillAsync(password);
        public async Task<string> GetErrorMessageAsync() => await _errMessage().IsVisibleAsync() ? await _errMessage().InnerTextAsync() : string.Empty; //If not visible, return empty string
        public async Task<bool> isErrorMessageVisibleAsync() => await _errMessage().IsVisibleAsync();
        public async Task CloseErrorMessageAsync() 
        {
            if (await _errCloseButton().IsVisibleAsync())
            {
                await _errCloseButton().ClickAsync();
            }
        }
        //Login method combines the actions
        public async Task LoginActionAsync(string username, string password)
        {
            //Wait for the page DOM to load before performing actions
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            //Fill the username and password fields and click the login button
            await FillUsernameAsync(username);
            await FillPasswordAsync(password);
            await ClickButtonLoginAsync();
        }

    }

}
