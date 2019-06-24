using System.Collections.Generic;
using OpenQA.Selenium;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs
{
    public class QuickInfo : ContentEditorTabContent
    {
        public QuickInfo()
        {
            Elements = Root.GetElements(By.XPath("//table[@class='scEditorQuickInfo']/tbody/tr/td[2]"));
            if (Elements.Count != 6) throw new ScContentEditorException($"Unable to init QuickInfo");
        }

        private IList<IWebElement> Elements { get; }
        public string Id => Elements[0].GetChild().GetValue();

        public string Name => Elements[1].Text;

        public string Path => Elements[2].GetChild().GetValue();

        public string Template =>
            string.Concat(Elements[3].Text, Elements[3].GetElement(By.XPath("./input")).GetValue());

        public string CreatedFrom =>
            string.Concat(Elements[4].Text, Elements[4].GetElement(By.XPath("./input")).GetValue());

        public string ItemOwner => Elements[5].GetChild().GetValue();

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}\n{nameof(Name)}: {Name}\n{nameof(Path)}: {Path}\n{nameof(Template)}: {Template}\n{nameof(CreatedFrom)}: {CreatedFrom}\n{nameof(ItemOwner)}: {ItemOwner}";
        }
    }
}
