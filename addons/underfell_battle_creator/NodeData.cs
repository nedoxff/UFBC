using System.Collections.Generic;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator;

public struct NodeData
{
    public static Dictionary<string, NodeData> Data = new()
    {
        {"SetOpponents", new(allowMultipleConnections: false)},
        {"Texture", new NodeData(allowMultipleConnections: true)}
    };

    public bool AllowMultipleConnections;

    public NodeData(bool allowMultipleConnections)
    {
        AllowMultipleConnections = allowMultipleConnections;
    }
}