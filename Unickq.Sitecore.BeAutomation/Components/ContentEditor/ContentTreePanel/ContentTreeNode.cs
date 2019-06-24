using System.Collections.Generic;
using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.Popup;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentTreePanel
{
    public class ContentTreeNode : SeleniqBaseElement
    {
        protected static readonly Logger Logger = LogManager.GetLogger("CE:Tree:Node");

        public ContentTreeNode(string text) : base(
            By.XPath($"//*[@class='scContentTreeNode']/a/span[contains(text(),'{text}')]/../.."))
        {
        }

        public ContentTreeNode(IWebElement root) : base(root)
        {
        }

        public IWebElement WrappedElement => Root;
        private IWebElement NoteImageElement => Root.FindElement(By.CssSelector("img"));
        private IWebElement NodeTextElement => Root.FindElement(By.CssSelector("a > span"));

        /// <summary>  Returns the current tree node state.</summary>
        /// <value>  Tree Node State</value>
        public State CurrentState
        {
            get
            {
                try
                {
                    var imageSrc = NoteImageElement.GetAttribute("src");
                    if (imageSrc.Contains("expanded")) return State.Expanded;

                    if (imageSrc.Contains("collapsed")) return State.Collapsed;

                    if (imageSrc.Contains("noexpand")) return State.NotExpandable;

                    if (imageSrc.Contains("spinner")) return State.Loading;

                    return State.Null;
                }
                catch (NoSuchElementException)
                {
                    return State.Null;
                }
            }
        }

        /// <summary>Gets the name of current Tree node.</summary>
        /// <value>The name.</value>
        public string Name => NodeTextElement.Text;

        public override string ToString()
        {
            return $"Name: {Name}; State: {CurrentState}";
        }

        /// <summary>
        ///   <para>State of TreeNode</para>
        /// </summary>
        public enum State
        {
            Expanded,
            Collapsed,
            Loading,
            NotExpandable,
            Null
        }

        /// <summary>Expands the current TreeNode</summary>
        /// <param name="withException">if set to <c>true</c> [with exception].</param>
        /// <exception cref="ScContentEditorException">Element is not collapsed</exception>
        public void Expand(bool withException = true)
        {
            if (CurrentState == State.Collapsed)
            {
                Logger.Trace($"Expanding {Name}");
                NoteImageElement.Click();
                Driver.Wait(10, "Expanding time").Until(d => CurrentState != State.Loading);
            }
            else
            {
                if (withException)
                    throw new ScContentEditorException("Element is not collapsed");
            }
        }

        /// <summary>  Focuses this TreeNode by Click.</summary>
        /// <returns>
        ///   <para>this</para>
        /// </returns>
        public ContentTreeNode SetFocus()
        {
            Logger.Debug($"Focus on {Name}");
            NodeTextElement.Click();
            return this;
        }

        /// <summary>  Perform mouse rights click on the TreeNode.</summary>
        /// <returns>New instance of the <see cref="ScPopupEl"/>.</returns>
        public ScPopupEl RightClick()
        {
            SetFocus();
            Logger.Debug($"Right click on {Name}");
            NodeTextElement.RightClick();
            var popupElement = Driver.Wait(10, $"Right click over {Name}")
                .Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".scPopup")));
            return new ScPopupEl(popupElement, Name);
        }

        /// <summary>  Return child nodes texts list.</summary>
        public IList<string> ChildNodesList =>
            Root.FindElements(By.ClassName("scContentTreeNode")).GetElemetsText();
    

        /// <summary>Collapses current TreeNode.</summary>
        /// <exception cref="ScContentEditorException">Element is not expanded</exception>
        public void Collapse()
        {
            if (CurrentState == State.Expanded)
            {
                Logger.Trace($"Collapsing {Name}");
                NoteImageElement.Click();
            }
            else
            {
                throw new ScContentEditorException("Element is not expanded");
            }
        }
    }
}
