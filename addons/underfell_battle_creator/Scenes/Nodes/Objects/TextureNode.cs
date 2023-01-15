using Godot;
using System;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Models;

[Tool]
public partial class TextureNode : ExtendedGraphNode
{
	// Called when the node enters the scene tree for the first time.
	private LineEdit _pathEdit => GetNode<LineEdit>("PathContainer/Path");
	private Button _browseButton => GetNode<Button>("PathContainer/Browse");
	private TextureRect _texture => GetNode<TextureRect>("Texture");
	public override void _Ready()
	{
		base._Ready();
		_pathEdit.TextChanged += Load;
		_browseButton.Pressed += () =>
		{
			UnderfellBattleCreator.Instance.FileDialog(s =>
			{
				_pathEdit.Text = s;
				Load(s);
			}, "*.png, *.jpg, *.gif", "Images");
		};
	}

	private void Load(string path)
	{
		if (!ResourceLoader.Exists(path)) return;
		var texture = GD.Load<Texture2D>(path);
		if (texture == null) return;
		_texture.Texture = texture;
	}
}
