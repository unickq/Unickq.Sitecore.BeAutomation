namespace Unickq.Sitecore.BeAutomation.Components.ContentEditor
{
    public interface ITabElement
    {
        string ActiveElementNameButton { get; }
        TPanel SwitchTo<TPanel>() where TPanel : ITabElement, new();
        ITabElement SwitchTo(string tabName);
    }
}
