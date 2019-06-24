using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using OpenQA.Selenium;
using Seleniq.Core;
using Seleniq.Extensions.Selenium;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentTreePanel
{
    public class ContentTreeEl : SeleniqBaseElement
    {
        protected static readonly Logger Logger = LogManager.GetLogger("CE:Tree");

        /// <summary>Initializes a new instance of the <see cref="ContentTreeEl"/> class.</summary>
        public ContentTreeEl() : base(By.Id("ContentTreePanel"))
        {
        }

        /// <summary>Navigates through the tree using XPath.</summary>
        /// <param name="names">Elements to open.</param>
        /// <returns>Last opened <see cref="ContentTreeNode"/> tree item.</returns>
        /// <exception cref="ScContentEditorException">Node not found: {sb}{names[i]}</exception>
        public ContentTreeNode ExpandByXpath(params string[] names)
        {
            Logger.Debug($"Expanding CE tree by xpath: {names.ToOneLine()}");
            var sb = new StringBuilder();
            ContentTreeNode node = null;
            for (var i = 0; i < names.Length; i++)
            {
                if(string.IsNullOrEmpty(names[i])) continue;
                
                var xpath = $"//*[@class='scContentTreeNode']/a/span[contains(text(),'{names[i]}')]/../..";
                if (i != 0)
                    xpath = string.Concat(
                        $"//*[@class='scContentTreeNode']/a/span[contains(text(),'{names[i - 1]}')]/../..",
                        $"//*[@class='scContentTreeNode']/a/span[contains(text(),'{names[i]}')]/../..");

                if (string.IsNullOrEmpty(names[i])) continue;

                try
                {
                    node = new ContentTreeNode(Driver.FindElement(By.XPath(xpath)));
                    sb.Append(names[i]);
                    if (i != names.Length - 1)
                    {
                        sb.Append(" => ");
                        node.Expand(false);
                    }
                    else
                    {
                        Logger.Info($"Navigated to '{sb}'");
                        return node;
                    }
                }
                catch (NoSuchElementException e)
                {
                    Logger.Error($"{sb} Unable to find node '{names[i]}'");
                    throw new ScContentEditorException($"Node not found: {sb}{names[i]} ", e);
                }
            }

            return node;
        }

        public ContentTreeNode Expand(params string[] names)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < names.Length; i++)
            {
                if (i == 0) continue;
                sb.Append(names[i]);
                sb.Append(" => ");
                var node = NodesTree.FindTreeNode(e =>
                    e.Parent != null
                    && e.Parent.Data.Name.Equals(names[i - 1])
                    && e.Data.Name.Equals(names[i])).Data;

                if (i != names.Length - 1)
                    node.Expand(false);
                else
                    return node;
            }

            return null;
        }

        private TreeNode<ContentTreeNode> BuildTreeNode(TreeNode<ContentTreeNode> node)
        {
            if (node.Data.CurrentState == ContentTreeNode.State.Expanded)
            {
                foreach (var child in node.Data.WrappedElement.FindElements(By.XPath("./div/div")))
                    node.AddChild(new ContentTreeNode(child));

                for (var i = 0; i < node.Children.Count; i++) node.Children[i] = BuildTreeNode(node.Children[i]);
            }

            return node;
        }

        public TreeNode<ContentTreeNode> NodesTree
        {
            get
            {
                var rootEl = Driver.GetElement(By.CssSelector(".scContentTreeNode"));
                return BuildTreeNode(new TreeNode<ContentTreeNode>(new ContentTreeNode(rootEl)));
            }
        }

        public List<ContentTreeNode> NodesList
        {
            get
            {
                return Driver.GetElements(By.CssSelector(".scContentTreeNode"))
                    .Select(x => new ContentTreeNode(x)).ToList();
            }
        }

        public ContentTreeNode GetNodeByText(string text)
        {
            return new ContentTreeNode(text);
        }
    }
}
