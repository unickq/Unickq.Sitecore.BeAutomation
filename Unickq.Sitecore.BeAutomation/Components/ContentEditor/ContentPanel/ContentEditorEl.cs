using System;
using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel
{
    public class ContentEditorEl : SeleniqBaseElement, ITabElement
    {
        protected static readonly Logger Logger = LogManager.GetLogger("CE:Editor");

        public ContentEditorEl() : base(By.Id("ContentEditor"))
        {
        }

        protected ContentEditorEl(IWebElement element) : base(element)
        {
        }

        public ContentEditorTabFolder Folder
        {
            get
            {
                try
                {
                    SwitchTo("Folder");
                    return new ContentEditorTabFolder();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public ContentEditorTabContent Content
        {
            get
            {
                try
                {
                    SwitchTo("Content");
                    return new ContentEditorTabContent();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        public string ActiveElementNameButton => Root.GetElement(By.ClassName("scEditorTabHeaderActive")).Text;

        public TPanel SwitchTo<TPanel>() where TPanel : ITabElement, new()
        {
            if (typeof(TPanel) == typeof(ContentEditorTabFolder))
            {
                SwitchTo("Folder");
                return new TPanel();
            }
            else if (typeof(TPanel) == typeof(ContentEditorTabContent))
            {
                SwitchTo("Content");
                return new TPanel();
            }

            throw new ScContentEditorException($"{typeof(TPanel).Name} is not supported");
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
                try
                {
                    Driver.GetElement(
                        By.XPath($"//*[contains(@class,'scRibbonEditorTab')]//span[contains(text(), '{text}')]"),
                        message: $"{text} is not available on the editro tabs").JsHighlight().Click();
                }
                catch (WebDriverTimeoutException e)
                {
                    Logger.Error($"Unable to switch to {text}");
                    throw new ScContentEditorException(e.Message, e);
                }
            }

            return this;
        }


        protected ContentEditorEl(By by) : base(by)
        {
        }
    }
}
