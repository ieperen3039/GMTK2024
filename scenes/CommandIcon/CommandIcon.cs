using Godot;
using System;

public enum CommandIconType
{
    MOVE_FORWARD = 0,
    MOVE_LEFT = 1,
    MOVE_BACKWARD = 2,
    MOVE_RIGHT = 3,
    TURN_LEFT = 8,
    TURN_RIGHT = 9,
    NONE = 63,
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

    public void SetInstruction(IInstruction aInstruction)
    {
        if (aInstruction is ForwardInstruction)
        {
            commandIconType = CommandIconType.MOVE_FORWARD;
        }
        else if (aInstruction is BackwardInstruction)
        {
            commandIconType = CommandIconType.MOVE_BACKWARD;
        }
        else if (aInstruction is TurnLeftInstruction)
        {
            commandIconType = CommandIconType.TURN_LEFT;
        }
        else if (aInstruction is TurnRightInstruction)
        {
            commandIconType = CommandIconType.TURN_RIGHT;
        } else {
            commandIconType = CommandIconType.NONE;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
