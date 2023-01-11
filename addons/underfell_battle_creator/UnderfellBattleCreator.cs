#if TOOLS
using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

[Tool]
public partial class UnderfellBattleCreator : EditorPlugin
{
	public static UnderfellBattleCreator Instance;
	public System.Collections.Generic.Dictionary<string, string> Nodes = new();
	public Control Editor;
	private EditorFileDialog _fileDialog;
	private PackedScene _battlePreviewScene;

	public override void _EnterTree()
	{
		Instance = this;
		LoadNodes();
		CheckActions();
		_battlePreviewScene = GD.Load<PackedScene>("addons/underfell_battle_creator/Scenes/BattlePreview.tscn");
		Editor = GD.Load<PackedScene>("addons/underfell_battle_creator/Scenes/Editor.tscn").Instantiate<Control>();
		GetEditorInterface().GetEditorMainScreen().AddChild(Editor);
		_MakeVisible(false);
	}

	private void CheckActions()
	{
		AddMouseAction("underfell_left_click", MouseButton.Left);
		AddKeyboardAction("underfell_increase", Key.Equal, true);
		AddKeyboardAction("underfell_decrease", Key.Minus);
		ProjectSettings.Save();
	}

	private void AddMouseAction(string name, MouseButton button)
	{
		if (InputMap.HasAction(name)) InputMap.EraseAction(name);
		GD.Print($"Adding mouse action {name}");
		var inputEvent = new InputEventMouseButton();
		inputEvent.Pressed = true;
		inputEvent.ButtonIndex = button;
		inputEvent.Device = 0;
		InputMap.AddAction(name);
		InputMap.ActionAddEvent(name, inputEvent);
	}

	private void AddKeyboardAction(string name, Key key, bool shiftPressed = false)
	{
		if (InputMap.HasAction(name)) InputMap.EraseAction(name);
		GD.Print($"Adding key action {name}");
		var inputEvent = new InputEventKey();
		inputEvent.Pressed = true;
		inputEvent.Keycode = key;
		inputEvent.ShiftPressed = shiftPressed;
		InputMap.AddAction(name);
		InputMap.ActionAddEvent(name, inputEvent);
	}
	
	private void LoadNodes()
	{
		GD.Print("----- Underfell Battle Creator -----\nLoading nodes");
		var nodeDirectory = DirAccess.Open("res://addons/underfell_battle_creator/Scenes/Nodes");
		foreach (var dir in nodeDirectory.GetDirectories())
		{
			var categoryDirectory = DirAccess.Open("res://addons/underfell_battle_creator/Scenes/Nodes/" + dir);
			var files = categoryDirectory.GetFiles();
			foreach (var file in files.Where(x => x.EndsWith(".tscn")))
			{
				var fullPath = $"res://addons/underfell_battle_creator/Scenes/Nodes/{dir}/{file}";
				GD.Print($"Adding {file} to {dir}");
				Nodes[fullPath] = dir;
			}
		}
		GD.Print($"Loaded {Nodes.Count} nodes");
	}

	public override Texture2D _GetPluginIcon()
	{
		return GetEditorInterface().GetBaseControl().GetThemeIcon("Node", "EditorIcons");
	}

	public override string _GetPluginName()
	{
		return "Underfell Battle Creator";
	}

	public override bool _HasMainScreen()
	{
		return true;
	}

	public override void _ExitTree()
	{
		Editor?.QueueFree();
	}

	public override void _MakeVisible(bool visible)
	{
		if (Editor != null)
			Editor.Visible = visible;
	}

	public void FileDialog(Action<string> onSelected, string filter = "", string description = "")
	{
		_fileDialog = new EditorFileDialog();
		_fileDialog.FileMode = EditorFileDialog.FileModeEnum.OpenFile;
		_fileDialog.Access = EditorFileDialog.AccessEnum.Resources;
		_fileDialog.CurrentPath = "res://";
		if(!string.IsNullOrEmpty(filter))
			_fileDialog.AddFilter(filter, description);
		_fileDialog.FileSelected += path =>
		{
			onSelected(path);
			_fileDialog.QueueFree();
		};
		GetEditorInterface().GetBaseControl().AddChild(_fileDialog);
		_fileDialog.SetMeta("_created_by", this);
		_fileDialog.PopupCentered();
	}
}
#endif
