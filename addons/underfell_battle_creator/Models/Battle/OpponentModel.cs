using Godot;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Models.Battle;

public struct OpponentModel
{
    public string Name;
    public int Defense;
    public int Attack;
    public string Frames; // Path to SpriteFrames
    public Vector2 Position;
    public Vector2 Scale;
}