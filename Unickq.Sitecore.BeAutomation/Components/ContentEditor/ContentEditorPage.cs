using NLog;
using Seleniq.Attributes;
using Seleniq.Core;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentTreePanel;
using Unickq.Sitecore.BeAutomation.Components.ContentEditor.RibbonPanel;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor
{
    [PageUrl("/sitecore/shell/Applications/Content Editor.aspx")]
    public class ContentEditorPage : SeleniqBasePage
    {
        protected static readonly Logger Logger = LogManager.GetLogger("Sc:CE");

        /// <summary>Initializes a new instance of the <see cref="ContentEditorPage"/> class.</summary>
        public ContentEditorPage()
        {
            Logger.Info("Working in Content Editor");
        }

        /// <summary>Tree panel for navigation.</summary>
        /// <value>The tree panel.</value>
        public ContentTreeEl TreePanel => new ContentTreeEl();

        /// <summary>  Ribbon panel.</summary>
        /// <value>The ribbon panel.</value>
        public RibbonPanelEl RibbonPanel => new RibbonPanelEl();

        public ContentEditorEl EditorPanel => new ContentEditorEl();
    }
}
