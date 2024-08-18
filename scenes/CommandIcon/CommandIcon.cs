using Godot;
using System;

public enum CommandIconType
{
    MOVE_FORWARD = 0,
    MOVE_BACKWARD = 1,
    MOVE_LEFT = 2,
    MOVE_RIGHT = 3,
    TURN_LEFT = 8,
    TURN_RIGHT = 9,
}

public partial class CommandIcon : Sprite2D
{
    [Export]
    public CommandIconType commandIconType;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Frame = (int)commandIconType;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
