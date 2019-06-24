using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using Unickq.Sitecore.BeAutomation;

namespace Examples
{
    public class BaseTest
    {
        private static readonly ThreadLocal<SiteCoreApp> ThreadLocalSitecore = new ThreadLocal<SiteCoreApp>();
        protected SiteCoreApp Sitecore { get; private set; } = ThreadLocalSitecore.Value;

        private const string Url = "https://yourWebsite.sitecore.local";
        private const string Username = "admin";
        private const string Password = "admin";

        [SetUp]
        public void SetUp()
        {
            Sitecore = new SiteCoreApp(() =>
            {
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("--start-maximized");
                return new ChromeDriver(service, chromeOptions);
            }, Url);

            Sitecore.LogIn(Username, Password);
        }


        [TearDown]
        public void TearDown()
        {
            Sitecore?.Dispose();
        }
    }
}