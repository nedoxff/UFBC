using Godot;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Providers;

public class StatusWindowProvider
{
    public static (AcceptDialog, Label) StatusWindow(string title, string initialStatus)
    {
        var dialog = new AcceptDialog();
        //dialog.Cancelled += () => dialog.QueueFree();
        dialog.Title = title;
        dialog.DialogText = initialStatus;
        dialog.DialogCloseOnEscape = false;
        dialog.GetOkButton().Visible = false;
        dialog.CloseRequested += () => { };
        UnderfellBattleCreator.Instance.GetTree().Root.AddChild(dialog);
        dialog.PopupCentered();
        return (dialog, dialog.GetLabel());
    }
}