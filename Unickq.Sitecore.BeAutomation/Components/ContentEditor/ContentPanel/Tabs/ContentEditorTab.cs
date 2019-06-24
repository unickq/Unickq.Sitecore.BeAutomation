using OpenQA.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs
{
    public abstract class ContentEditorTab : ContentEditorEl
    {
        protected ContentEditorTab() : base(By.Id("EditorFrames"))
        {
        }

        protected ContentEditorTab(IWebElement el) : base(el)
        {
        }
    }
}
