using Godot;
using System;

[Tool]
public partial class ResourcePickerHelper : Node
{
	[Export(PropertyHint.TypeString)] public string Type; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var picker = new EditorResourcePicker();
		picker.BaseType = Type;
		picker.Editable = true;
		picker.ResourceSelected += (resource, inspect) => GD.Print("e");
		picker.Visible = true;
		AddChild(picker);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
