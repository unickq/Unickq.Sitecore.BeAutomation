using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.RibbonPanel.Entities;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.RibbonPanel
{
    public class RibbonPanelEl : SeleniqBaseElement, ITabElement
    {
        protected static readonly Logger Logger = LogManager.GetLogger("CE:Ribbon");

        public RibbonPanelEl() : base(By.Id("RibbonPanel"))
        {
        }

        public bool IsToggleExpanded => ToggleButton.GetAttribute("class").Equals("scRibbonClose");

        public RibbonPanelEl Save()
        {
            Logger.Debug("Saving current state");
            Driver.ExecuteScript("scForm.invoke('contenteditor:save', event)");
            //ToDo: Add wait for save
            return this;
        }

        public ITabElement SwitchTo(string text)
        {
            if (ActiveElementNameButton.Equals(text))
            {
                Logger.Trace($"{text} is already opened");
            }
            else
            {
                Logger.Debug($"Clicking on {text}");
                Driver.GetElement(
                    By.XPath($"//*[contains(@class,'scRibbonNavigatorButtons') and contains(text(),'{text}')]"),
                    message: $"{text} is not available on the ribbon menu").JsHighlight().Click();
            }

            return this;
        }

        public string ActiveElementNameButton => Driver.GetElement(By.ClassName("scRibbonNavigatorButtonsActive")).Text;

        public TPanel SwitchTo<TPanel>() where TPanel : ITabElement, new()
        {
            if (typeof(TPanel) == typeof(PublishRibbonPanel))
            {
                SwitchTo("Publish");
                return new TPanel();
            }

            throw new ScContentEditorException($"{typeof(TPanel).Name} is not supported");
        }

        private IWebElement ToggleButton => Driver.GetElement(By.Id("scRibbonToggle"));


        public void ClickOnToggle()
        {
            Logger.Debug("Clicking on toggle button");
            ToggleButton.JsHighlight().Click();
        }
    }
}
