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

public partial class InstructionProto : Button
{
    [Signal]
    public delegate void PressedEventHandler(InstructionWrapper contents);

    [Export]
    private PackedScene asInstance;

    [Export]
    private PackedScene dragPreview;

    public override void _Ready()
    {
        GetNode<Label>("Text").Text = asInstance.Instantiate<InstructionWrapper>().DisplayName;
    }

    public override Variant _GetDragData(Vector2 atPosition)
    {
        InstructionWrapper instructionWrapper = asInstance.Instantiate<InstructionWrapper>();
        
        Control preview = dragPreview.Instantiate<Control>();
        preview.AddChild(instructionWrapper);
        SetDragPreview(preview);

        // NOTE: we must re-instantiate the wrapper, or it will be freed when the preview ends
        instructionWrapper = asInstance.Instantiate<InstructionWrapper>();
        return Variant.CreateFrom(instructionWrapper);
    }
    private void OnPressed()
    {
        EmitSignal(SignalName.Pressed, asInstance.Instantiate<InstructionWrapper>());
    }
}

