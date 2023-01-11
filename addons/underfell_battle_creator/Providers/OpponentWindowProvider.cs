using System;
using System.Linq;
using Godot;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Models.Battle;

namespace UNDERFELLBattleCreator.addons.underfell_battle_creator.Providers;

public class OpponentWindowProvider
{
    private static PackedScene _battlePreviewScene;
    private static PackedScene _draggableSprite;

    public static void OpenPositionWindow(Action<Node> onShown, Action<Node> onClosed, string debugLabel = "",
        params OpponentModel[] opponents)
    {
        _draggableSprite ??= GD.Load<PackedScene>("res://addons/underfell_battle_creator/DraggableSprite.tscn");
        _battlePreviewScene ??= GD.Load<PackedScene>("res://addons/underfell_battle_creator/Scenes/BattlePreview.tscn");
        var window = new Window();
        window.Title = "Battle Preview";
        window.MinSize = new Vector2i(640, 480);
        window.Unresizable = true;
        var node = _battlePreviewScene.Instantiate();
        if (!string.IsNullOrEmpty(debugLabel))
            node.GetNode<Label>("DebugLabel").Text = debugLabel;
        window.AddChild(node);
        if (opponents.Any(opponent => !AddOpponent(node, opponent)))
        {
            window.QueueFree();
            return;
        }

        window.CloseRequested += () =>
        {
            onClosed(node);
            window.QueueFree();
        };
        UnderfellBattleCreator.Instance.Editor.AddChild(window);
        window.PopupCentered();
        onShown(node);
    }

    private static bool AddOpponent(Node preview, OpponentModel opponent)
    {
        var draggableSprite = _draggableSprite.Instantiate<DraggableSprite>();
        draggableSprite.Scale = opponent.Scale;
        draggableSprite.Position = opponent.Position;
        draggableSprite.Name = opponent.Name;

        var frames = GD.Load<SpriteFrames>(opponent.Frames);
        if (frames == null || !frames.HasAnimation("idle"))
        {
            preview.MessageBox(
                "Failed to load opponent's SpriteFrames!\nPlease verify that the path is correct & that there is an \"idle\" animation in it.");
            return false;
        }

        draggableSprite.Texture = frames.GetFrameTexture("idle", 0);
        preview.AddChild(draggableSprite);
        return true;
    }
}