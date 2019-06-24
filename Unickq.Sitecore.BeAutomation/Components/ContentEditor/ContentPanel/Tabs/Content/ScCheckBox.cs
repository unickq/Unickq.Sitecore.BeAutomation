using System;
using Unickq.Sitecore.BeAutomation.Utils;

namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor.ContentPanel.Tabs.Content
{
    public class ScCheckBox : AbstractScControl
    {
        public ScCheckBox(string panelLabel, string itemLabel) : base(panelLabel, itemLabel, FieldType.CheckBox)
        {
        }

        public ScCheckBox Click()
        {
            Logger.Debug($"Clicking checkbox with lable {ControlLabel}");
            ControlElement.Click();
            return this;
        }
    }

    public class ScTextBox : AbstractScControl
    {
        public ScTextBox(string panelLabel, string itemLabel) : base(panelLabel, itemLabel, FieldType.TextBox)
        {
        }


        public ScTextBox SetText(string value)
        {
            try
            {
                Logger.Debug($"Setting {value} to {ControlLabel}");
                ControlElement.Clear();
                ControlElement.SendKeys(value);
            }
            catch (Exception e)
            {
                var msg = $"Unable to set value to {ControlLabel} in {ControlPanelLabel}";
                Logger.Error(msg);
                throw new ScContentEditorException(msg, e);
            }

            return this;
        }
    }
}
