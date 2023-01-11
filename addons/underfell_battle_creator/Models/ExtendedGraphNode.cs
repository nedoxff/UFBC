using Godot;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Models;

[Tool]
public partial class ExtendedGraphNode: GraphNode
{
    public override void _Ready()
    {
        CloseRequest += QueueFree;
        ResizeRequest += size => Size = size;
        SelectedSignal += () => Editor.SelectedNodes.Add(this);
        Deselected += () => Editor.SelectedNodes.Remove(this);
    }
}