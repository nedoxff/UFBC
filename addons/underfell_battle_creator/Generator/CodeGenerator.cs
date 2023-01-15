using System;
using Godot;
using Godot.Collections;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Providers;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Generator;

public class CodeGenerator
{
    public static string Generate(Array<Dictionary> connections)
    {
        GD.Print("Attempting to generate C# code for the battle..");
        var editor = UnderfellBattleCreator.Instance;
        var (statusWindow, statusLabel) = StatusWindowProvider.StatusWindow("Generating code..", "Please wait..");

        void ChangeStatus(string text)
        {
            statusLabel.Text = "Analyzing node connections..";
            statusLabel.QueueRedraw();
        }
        
        try
        {
            ChangeStatus("Analyzing connections..");
            AnalyzeConnections(connections);
        }
        catch (Exception e)
        {
            statusWindow.QueueFree();
            editor.MessageBox(e.Message, "Error!");
        }
        return "";
    }

    private static void AnalyzeConnections(Array<Dictionary> connections)
    {
        if(connections.Count == 0)
            throw new Exception("There's nothing to compile!");
    }
}