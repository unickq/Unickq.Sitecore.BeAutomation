using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.MessageBox
{
    public class PublishItemFrame : IFrameElement
    {
//        protected readonly Logger Logger = LogManager.GetLogger("CE:Publish Item");

        public PublishItemFrame() : base(By.CssSelector("form"))
        {
            Logger.Debug($"Working with '{DialogHeader}' dialog");
        }

        public void ClickCancel()
        {
            Logger.Debug("Clicking cancel");
            Root.GetElement(By.Id("CancelButton")).Click();
        }

        public void ClickNext()
        {
            Logger.Debug("Clicking next");
            Root.GetElement(By.Id("NextButton")).Click();
        }

        public void ClickBack()
        {
            Logger.Debug("Clicking next");
            Root.GetElement(By.Id("BackButton")).Click();
        }
    }
}
