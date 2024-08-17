using Godot;
using System;

public enum InstructionType
{
    WAIT = 0,
    FORWARD = 1,
    BACKWARD = 2,
    TURN_LEFT = 3,
    TURN_RIGHT = 4,
}

public partial class InstructionPanel : Panel
{
    [Export]
    private InstructionType type;

    [Export]
    private string text;

    [Export]
    private PackedScene dragPreview;

    public override void _Ready()
    {
        GetNode<Label>("Text").Text = text;
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        Control preview = dragPreview.Instantiate<Control>();
        SetDragPreview(preview);

        return Variant.CreateFrom((int)type);
    }
}
