using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.MessageBox
{
    public abstract class IFrameElement : SeleniqBaseElement
    {
        private static readonly Logger Log = LogManager.GetLogger("CE:IFrame");
        protected Logger Logger { get; set; }
        public string DialogHeader => Root.GetElement(By.CssSelector(".DialogHeader")).Text;
        public string DialogHeaderDescription => Root.GetElement(By.CssSelector(".DialogHeaderDescription")).Text;

        public static IWebElement FrameElementFinder(By by)
        {
            var rootBy = By.Id("jqueryModalDialogsFrame");
            Log.Trace("Switching to jqueryModalDialogsFrame");
            var rootElement = Driver.Wait(10, $"Unable to find {rootBy}")
                .Until(ExpectedConditions.ElementExists(rootBy));
            Driver.SwitchTo().Frame(rootElement);
            Log.Trace("Done");

            var subRootBy = By.CssSelector("iframe");
            Log.Trace("Switching to jqueryModalDialogsFrame child iFrame");
            var subRootElement = Driver.Wait(10, $"Unable to find {subRootBy}")
                .Until(ExpectedConditions.ElementExists(subRootBy));
            Driver.SwitchTo().Frame(subRootElement);
            Log.Trace("Done");
            return Driver.Wait(10, $"Unable to MessageBox {by}").Until(ExpectedConditions.ElementExists(by));
            ;
        }


        protected IFrameElement(By by) : base(FrameElementFinder(by))
        {
            Logger = LogManager.GetLogger($"CE:{DialogHeader}");
        }

//        public IWebElement WrappedElement => Root;
    }
}
