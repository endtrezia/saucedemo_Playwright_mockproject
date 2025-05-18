using Microsoft.Extensions.Configuration;
using Reqnroll;
using Reqnroll.BoDi;
using saucedemo_Playwright_mockproject.Driver;
using System.ComponentModel;

namespace saucedemo_Playwright_mockproject.Hooks
{
    [Binding]
    public class SimpleHook
    {
        private SimpleDriver? _driver;
        private IConfiguration? _configuration;
        IObjectContainer _container;

        public SimpleHook(IObjectContainer container)
        {
            _container = container;
        }

        public IConfiguration GetConfiguration()
        {
            return _configuration!;
        }

        //To initialize the Playwright driver before each scenario
        [BeforeScenario]
        public async Task BeforeScenarioAsync()
        {
            //Build the configuration file named appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            //Get the value of the Headless parameter from the configuration file
            bool headless = _configuration.GetValue<bool>("Headless");
            if (_configuration.GetValue<string>("BaseUrl") == null)
            {
                throw new ArgumentNullException("BaseUrl", "BaseUrl cannot be null. Please check your appsettings.json file."); //check if config json data not found
            }
            string? baseUrl = _configuration.GetValue<string>("BaseUrl");
            _driver = new SimpleDriver();

            //Initialize the Playwright driver with the headless parameter
            await _driver.InitPlaywrightAsync(headless, baseUrl);

            //Register the driver and page instances in the container
            _container.RegisterInstanceAs(_driver);
            _container.RegisterInstanceAs(_driver.Page);
            _container.RegisterInstanceAs(_configuration);
        }

        //To close the Playwright driver before each scenario
        [AfterScenario]
        public async Task AfterScenarioAsync()
        {
            //Close the Playwright driver
            if (_driver != null)
            {
                await _driver.ClosePlaywrightAsync();
            }
                
        }
    }
}
