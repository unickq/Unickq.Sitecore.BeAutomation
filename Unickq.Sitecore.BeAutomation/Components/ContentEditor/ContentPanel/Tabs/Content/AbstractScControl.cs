using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content
{
    public abstract class AbstractScControl : ContentEditorTabContent
    {
        protected IWebElement ControlElement { get; }
        protected string ControlLabel { get; }
        protected string ControlPanelLabel { get; }

        protected static IWebElement BuildTable(string parentLabel)
        {
            var panelXpath = $"//*[contains(@class,'scEditorSectionCaption') and text()='{parentLabel}']";
            var panel = Driver.GetElement(By.XPath(panelXpath), message: $"Unable to find panel '{parentLabel}'");
            if (panel.GetAttribute("class").Contains("Collapsed"))
                panel.Click();

            var xpath =
                $"//div[@class='scEditorSectionCaptionExpanded' and text()='{parentLabel}']/following-sibling::table[1]";
            return Driver.GetElement(By.XPath(xpath), message: $"Panel '{parentLabel}' doesn't have following-sibling");
        }

        private AbstractScControl(string panelLabel) : base(BuildTable(panelLabel))
        {
        }

        protected AbstractScControl(string panelLabel, string itemLabel, FieldType type) : this(panelLabel)
        {
            ControlLabel = itemLabel;
            ControlPanelLabel = panelLabel;
            string elementXpath = null;
            switch (type)
            {
                case FieldType.CheckBox:
                    elementXpath = $".//div[@class='scEditorFieldLabel' and contains(text(),'{itemLabel}')]/..//input";
                    break;
                case FieldType.ComboBox:
                    elementXpath =
                        $".//div[@class='scEditorFieldLabel' and contains(text(),'{itemLabel}')]/following-sibling::div/select";
                    break;
                case FieldType.TextBox:
                    elementXpath =
                        $".//div[@class='scEditorFieldLabel' and contains(text(),'{itemLabel}')]/following-sibling::div/input";
                    break;
            }

            Root.JsHighlight("Green");
            ControlElement = Root.GetElement(By.XPath(elementXpath),
                message: $"Unable to inialize {type} with label {itemLabel} under {panelLabel} panel");
        }

        public enum FieldType
        {
            CheckBox,
            ComboBox,
            TextBox
        }
    }
}
