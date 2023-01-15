using Godot;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Models;

[Tool]
public partial class ExtendedGraphNode: GraphNode
{
    public override void _Ready()
    {
        //TODO: implement disconnecting connected nodes
        CloseRequest += QueueFree;
        ResizeRequest += size => Size = size;
    }
}