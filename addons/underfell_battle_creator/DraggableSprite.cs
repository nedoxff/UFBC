using Godot;
using System;

[Tool]
public partial class DraggableSprite : TextureRect
{
    private bool _selected;
    public override void _Ready()
    {
        var rect = new RectangleShape2D();
        rect.Size = Texture.GetSize();

        GuiInput += OnGuiInput;
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("underfell_left_click"))
            _selected = true;
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton button)
            if (!button.Pressed && button.ButtonIndex == MouseButton.Left)
                _selected = false;
        if (Input.IsActionPressed("underfell_increase"))
            Scale += new Vector2(0.1f, 0.1f);
        if (Input.IsActionPressed("underfell_decrease"))
            Scale -= new Vector2(0.1f, 0.1f);
        if (Input.IsActionPressed("ui_left"))
            Position -= new Vector2(5, 0);
        if (Input.IsActionPressed("ui_up"))
            Position -= new Vector2(0, 5);
        if (Input.IsActionPressed("ui_right"))
            Position += new Vector2(5, 0);
        if (Input.IsActionPressed("ui_down"))
            Position += new Vector2(0, 5);
    }

    public override void _Process(double delta)
    {
        if (_selected)
            GlobalPosition = GetGlobalMousePosition() - Texture.GetSize() / 2 * Scale;
    } 
}
