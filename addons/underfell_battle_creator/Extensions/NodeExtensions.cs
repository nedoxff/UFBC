using System;
using Godot;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;

[Tool]
public static class NodeExtensions
{
    public static void MessageBox(this Node node, string text, string title = "", string buttonText = "OK",
        Action closed = null)
    {
        var dialog = new AcceptDialog();
        dialog.Confirmed += () => closed?.Invoke();
        dialog.Cancelled += () => closed?.Invoke();
        dialog.Title = title;
        dialog.DialogText = text;
        dialog.OkButtonText = buttonText;
        node.GetTree().Root.AddChild(dialog);
        dialog.PopupCentered();
    }

    //god
    public static T SpinBoxValue<T>(this Node node, string childName) => (T)Convert.ChangeType(node.GetNode<SpinBox>(childName).Value, typeof(T));
}