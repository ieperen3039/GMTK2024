using Godot;
using System;

public partial class SimpleInstructionWrapper : InstructionWrapper
{
    public enum InstructionType
    {
        WAIT = 0,
        FORWARD = 1,
        BACKWARD = 2,
        TURN_LEFT = 3,
        TURN_RIGHT = 4,
    }

    [Export]
    public InstructionType Type;

    public override IInstruction GetInstruction() => Type switch
    {
        InstructionType.WAIT => new WaitInstruction(),
        InstructionType.FORWARD => new ForwardInstruction(),
        InstructionType.BACKWARD => new BackwardInstruction(),
        InstructionType.TURN_LEFT => new TurnLeftInstruction(),
        InstructionType.TURN_RIGHT => new TurnRightInstruction(),
        _ => throw new Exception("Unhandled instruction type " + Type),
    };
}
