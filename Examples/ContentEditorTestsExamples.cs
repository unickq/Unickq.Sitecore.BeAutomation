using System;
using NUnit.Framework;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.MessageBox;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.Popup;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.RibbonPanel.Entities;

namespace Examples
{
    public class ContentEditorTestsExamples : BaseTest
    {
        private readonly string[] _homePath =
            {"sitecore", "Content", "Site", "Brand Sites", "LRP", "Master", "QA", "Home"};

        [Test]
        public void BasePageCreation()
        {
            var contentEditor = Sitecore.GoTo<ContentEditorPage>();
            contentEditor.TreePanel.ExpandByXpath(_homePath)
                .SetFocus()
                .RightClick()
                .ExpandByXpath<MessageBoxFrame>("Insert", "Page")
                .SetText("a_123")
                .ClickOk();

            var contentPanel = contentEditor.EditorPanel.Content;
            Console.WriteLine(contentPanel.Info);
            Console.WriteLine(contentPanel.MessageBarText);

            contentPanel.GetControl<ScTextBox>("Styling", "Body Css Class").SetText("SSSSSSSSSSSSSS");
            contentPanel.GetControl<ScCheckBox>("Sitemap Settings", "No-index").Click();
            contentPanel.GetControl<ScCombobox>("Sitemap Settings", "Change frequency").SelectByText("yearly");
        }

        [Test]
        public void ElementCreateAndDelete()
        {
            var pageName = "NickQ";

            var contentTree = Sitecore.GoTo<ContentEditorPage>().TreePanel;

            contentTree.ExpandByXpath(_homePath)
                .RightClick()
                .ExpandByXpath<MessageBoxFrame>("Insert", "Page")
                .SetText(pageName)
                .ClickOk();

            contentTree.ExpandByXpath(pageName)
                .RightClick()
                .ExpandByXpath<MessageBoxFrame>("Delete")
                .ClickOk();

            Sitecore.Dispose();
        }

        [Test]
        public void ElementPublishCancel()
        {
            var contentEditor = Sitecore.GoTo<ContentEditorPage>();
            contentEditor.TreePanel.ExpandByXpath(_homePath);
            contentEditor.RibbonPanel.SwitchTo<PublishRibbonPanel>().ClickPublishItem();
            new PublishItemFrame().ClickCancel();
        }

        [Test]
        public void ValidateInsertOptions()
        {
            var contentEditor = Sitecore.GoTo<ContentEditorPage>();

            //Create node
            contentEditor.TreePanel.ExpandByXpath(_homePath)
                .RightClick()
                .ExpandByXpath<MessageBoxFrame>("Insert", "PageToTest")
                .SetText("PageToTest Name")
                .ClickOk();

            var popup = contentEditor.TreePanel
                .ExpandByXpath("Data")
                .RightClick();

            popup.ExpandByXpath("Insert");

            try
            {
                CollectionAssert.AreEquivalent("InsertOption to test", popup.GetItemsList(ScPopupEl.MenuLevel.Second));
            }
            finally
            {
                //Remove created node
                contentEditor.TreePanel
                    .ExpandByXpath("PageToTest")
                    .RightClick()
                    .ExpandByXpath<MessageBoxFrame>("Delete")
                    .ClickOk();
            }
        }
    }
}