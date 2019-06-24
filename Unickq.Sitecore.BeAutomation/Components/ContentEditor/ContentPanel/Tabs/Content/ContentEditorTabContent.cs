using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content
{
    public partial class ContentEditorTabContent : ContentEditorTab
    {
        protected ContentEditorTabContent(IWebElement element) : base(element)
        {
        }

        public ContentEditorTabContent()
        {
        }

        public void PrintElements()
        {
            foreach (var el in GetElements) Console.WriteLine(el);
        }

        public TControl GetControl<TControl>(string panelLabel, string controlLabel)
            where TControl : AbstractScControl
        {
            try
            {
                Logger.Info($"Looking for {typeof(TControl).Name} with label {controlLabel} in {panelLabel} panel");
                return (TControl) Activator.CreateInstance(typeof(TControl), panelLabel, controlLabel);
            }
            catch (TargetInvocationException e)
            {
                var msg =
                    $"Unable to init '{typeof(TControl).Name} with panel '{panelLabel}' and control '{controlLabel}'";
                Logger.Fatal(msg);
                throw new ScContentEditorException(msg, e);
            }
        }

        public QuickInfo Info => new QuickInfo();
        public string MessageBarText => Root.GetElement(By.ClassName("scMessageBarTextContainer")).Text;

//        public string QuickInfo => Root.GetElement(By.CssSelector(".scEditorQuickInfo")).Text;


        public IEnumerable<string> GetElements => Root
            .GetElements(By.XPath("//*[contains(@class,'scEditorSectionCaption')]"))
            .GetElemetsText().Where(x => !string.IsNullOrEmpty(x));
    }
}
