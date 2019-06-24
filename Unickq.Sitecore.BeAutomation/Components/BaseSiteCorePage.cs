using System.Threading;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components
{
    internal class BaseSiteCorePage : SeleniqBasePage
    {
        private GlobalHeader Header { get; set; }
    }

    public class GlobalHeader : SeleniqBaseElement
    {
        public GlobalHeader() : base(By.CssSelector(".sc-globalHeader"))
        {
        }

        public string AccountName =>
            Root.FindElement(By.CssSelector(".sc-accountInformation > li:nth-of-type(2)")).Text;

        public void ClickLogOut()
        {
            Thread.Sleep(2000);
            Root.GetElement(By.CssSelector(".sc-accountInformation > li:nth-of-type(1) > a")).JsClick();
        }
    }
}
