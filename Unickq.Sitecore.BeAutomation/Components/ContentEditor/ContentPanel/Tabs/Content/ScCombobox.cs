using System;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content
{
    public class ScCombobox : AbstractScControl
    {
        public ScCombobox(string panelLabel, string itemLabel) : base(panelLabel, itemLabel, FieldType.ComboBox)
        {
        }

        public ScCombobox SelectByText(string value)
        {
            try
            {
                Logger.Debug($"Selecting {value} in {ControlLabel}");
                new SelectElement(ControlElement).SelectByText(value);
            }
            catch (Exception e)
            {
                var msg = $"Unable to select '{value}' in '{ControlLabel}'";
                Logger.Error(msg);
                throw new ScContentEditorException(msg, e);
            }

            return this;
        }
    }
}
