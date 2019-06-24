using OpenQA.Selenium;
using Seleniq.Core;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.Popup
{
    public class ScPopupNode : SeleniqBaseElement
    {
        public ScPopupNode(string text) : base(
            By.CssSelector($"//table/tbody/tr/td[@class='scMenuItemCaption' and text() = '{text}']"))
        {
        }

        public ScPopupNode(IWebElement root) : base(root)
        {
        }

        public IWebElement WrappedElement => Root;
    }
}
