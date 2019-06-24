using System;
using OpenQA.Selenium;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.MessageBox
{
    public class MessageBoxFrame : IFrameElement
    {
        public string ConfirmMessage => Root.GetElement(By.Id("ConfirmMessage")).Text;
        public string InputHelpMessage => Root.GetElement(By.Id("Header")).Text;

        public void WaitUntilDisappears()
        {
            Driver.Wait(3, "S").Until(ExpectedConditions.StalenessOf(Root));
            Console.WriteLine(DateTime.Now);
        }

        public void ClickOk()
        {
            Logger.Debug("Clicking OK button");
//            Console.WriteLine(DateTime.Now);
            Root.FindElement(By.Id("OK")).Click();
//            WaitUntilDissappears();
        }

        public void ClickCancel()
        {
            Logger.Debug("Clicking Cancel button");
            Root.FindElement(By.Id("Cancel")).Click();
        }

        public MessageBoxFrame SetText(string value)
        {
            Logger.Debug($"{InputHelpMessage} '{value}'");
            var inputField = Root.GetElement(By.Id("Value"));
            inputField.Clear();
            inputField.SendKeys(value);
            return this;
        }

        public MessageBoxFrame() : base(By.CssSelector("form"))
        {
            Logger.Debug($"Working with '{DialogHeader}' dialog");
        }
    }
}
