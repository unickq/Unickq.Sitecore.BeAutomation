using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.RibbonPanel.Entities
{
    public class PublishRibbonPanel : RibbonPanelEl
    {
        public void ClickPublishItem()
        {
            Logger.Debug("Trying to publish item");
            Driver.FindElement(By.XPath("//td[@class='scMenuItemCaption' and contains(text(), 'Publish item')]"))
                .JsClick();
        }
    }
}
