using System.Collections.Generic;
using System.Text;
using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.MessageBox;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.Popup
{
    public class ScPopupEl : SeleniqBaseElement
    {
        protected static Logger Logger = LogManager.GetLogger("CE:Tree:Popup");

        /// <summary>Initializes a new instance of the <see cref="ScPopupEl"/> class.</summary>
        public ScPopupEl() : base(By.CssSelector(".scPopup"))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ScPopupEl"/> class.</summary>
        /// <param name="root">The root.</param>
        public ScPopupEl(IWebElement root) : base(root)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ScPopupEl"/> class.</summary>
        /// <param name="root">The root.</param>
        /// <param name="elementName">Name of the element.</param>
        public ScPopupEl(IWebElement root, string elementName) : base(root)
        {
            Logger = LogManager.GetLogger($"CE:Tree>{elementName}");
            ElementName = elementName;
        }

        public string ElementName { get; }

        public List<string> GetItemsList(MenuLevel level)
        {
            var xpath = $"//*[@class='scPopup'][{(int) level}]/table/tbody/tr/td[@class='scMenuItemCaption']";
            return Root.GetElements(By.XPath(xpath)).GetElemetsText();
        }

        public enum MenuLevel
        {
            First = 1,
            Second = 2
        }

        public T ExpandByXpath<T>(params string[] names) where T : IFrameElement, new()
        {
            ExpandByXpath(names);
            return new T();
        }

        public void ExpandByXpath(params string[] names)
        {
            Logger.Debug($"Clicking context menu by xpath: {names.ToOneLine()}");
            var sb = new StringBuilder();
            for (var i = 0; i < names.Length; i++)
            {
                var prefixXpath = $"//*[@id='Popup{i + 1}']//table/tbody/tr/td[@class='scMenuItemCaption' and text()";
                var xpath = $"{prefixXpath} = '{names[i]}']";

                try
                {
                    var node = new ScPopupNode(Driver.Wait(5, xpath)
                        .Until(ExpectedConditions.ElementExists(By.XPath(xpath))));
                    sb.Append(names[i]);
                    if (i != names.Length - 1) sb.Append(" => ");
                    node.WrappedElement.Click();
                }
                catch (WebDriverTimeoutException e)
                {
                    Logger.Error($"{sb} Unable to find node '{names[i]}'");
                    throw new ScContentEditorException($"Unable to find node {names[i]}", e);
                }
            }

            Logger.Info($"Clicked '{sb}'");
        }
    }
}
