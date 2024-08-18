using Godot;
using System;

public partial class JumpInstructionWrapper : InstructionWrapper
{
    private JumpInstruction instruction = new();

    public override IInstruction GetInstruction() => instruction;

    private void OnJumpTargetChange(double value)
    {
        instruction.TargetId = (int)value;
    }
}
