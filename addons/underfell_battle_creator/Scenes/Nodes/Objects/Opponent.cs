using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Models;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Models.Battle;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Providers;

[Tool]
public partial class Opponent : ExtendedGraphNode
{
    private Button _animationSelectButton => GetNode<Button>("AnimationSelectButton");
    private Button _transformSelectButton => GetNode<Button>("TransformSelectButton");
    private TextureRect _spritePreview => GetNode<TextureRect>("SpritePreview");
    private Texture2D _texture;

    public string SpritesPath;

    public OpponentModel ToModel()
    {
        return new OpponentModel
        {
            Name = GetNode<LineEdit>("Name").Text,
            Attack = this.SpinBoxValue<int>("ATK"),
            Defense = this.SpinBoxValue<int>("DEF"),
            Frames = SpritesPath,
            Position = new Vector2(this.SpinBoxValue<int>("PositionContainer/X"),
                this.SpinBoxValue<int>("PositionContainer/Y")),
            Scale = new Vector2(this.SpinBoxValue<float>("ScaleContainer/X"),
                this.SpinBoxValue<float>("ScaleContainer/Y"))
        };
    }

    public override void _Ready()
    {
        base._Ready();
        _transformSelectButton.Disabled = true;
        _animationSelectButton.Pressed += () =>
        {
            UnderfellBattleCreator.Instance.FileDialog(s =>
            {
                SpritesPath = s;
                var frames = GD.Load<SpriteFrames>(s);
                _texture = frames.GetFrameTexture("idle", 0);
                _spritePreview.Texture = _texture;
                _spritePreview.CustomMinimumSize = _spritePreview.Texture.GetSize() * 2;
                _transformSelectButton.Disabled = false;
            }, "*.tres", "SpriteFrames files");
        };

        _transformSelectButton.Pressed += () =>
        {
            var models = TryGetOpponents();
            if (models == null)
                return;
            OpponentWindowProvider.OpenPositionWindow(node => { _transformSelectButton.Disabled = true; }, node =>
                {
                    var rect = node.GetNode<DraggableSprite>(GetNode<LineEdit>("Name").Text);
                    GetNode<SpinBox>("PositionContainer/X").Value = rect.Position.x;
                    GetNode<SpinBox>("PositionContainer/Y").Value = rect.Position.y;
                    GetNode<SpinBox>("ScaleContainer/X").Value = rect.Scale.x;
                    GetNode<SpinBox>("ScaleContainer/Y").Value = rect.Scale.y;
                    _transformSelectButton.Disabled = false;
                },
                "Use +/- to change opponent's scale.\nUse arrows to move the player accurately (or use the mouse).\nClose the window when you're done.",
                models);
        };
    }

    private OpponentModel[] TryGetOpponents()
    {
        var graphEdit = UnderfellBattleCreator.Instance.Editor.GetNode<GraphEdit>("GraphEdit");
        var setOpponents = graphEdit.GetNodeOrNull<SetOpponents>("SetOpponents");
        if (setOpponents == null)
        {
            this.MessageBox("Please add a \"Set Opponents\" node to preview the battle!");
            return null;
        }

        if (string.IsNullOrEmpty(GetNode<LineEdit>("Name").Text))
        {
            this.MessageBox("Please add a name of the opponent!");
            return null;
        }

        var connections = graphEdit.GetConnectionList().Where(x => x["from"].AsString() == setOpponents.Name);
        return connections.Select(connection => graphEdit.GetNode<Opponent>(connection["to"].AsString()))
            .Select(node => node.ToModel()).ToArray();
    }
}