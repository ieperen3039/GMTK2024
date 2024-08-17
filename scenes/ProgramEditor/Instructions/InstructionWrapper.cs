
using System;
using Godot;

public partial class InstructionWrapper : Panel
{
    [Export]
    public InstructionType Type;

    [Export]
    private PackedScene dragPreview;

    public IInstruction Instruction;

    public override void _Ready() {
        Instruction = Type switch
        {
            InstructionType.WAIT => new WaitInstruction(),
            InstructionType.FORWARD => new ForwardInstruction(),
            InstructionType.BACKWARD => new BackwardInstruction(),
            InstructionType.TURN_LEFT => new TurnLeftInstruction(),
            InstructionType.TURN_RIGHT => new TurnRightInstruction(),
            _ => throw new Exception("Unhandled instruction type " + Type),
        };

        GetNode<Label>("Text").Text = Type.ToString();
    }
}
