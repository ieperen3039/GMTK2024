using Godot;
using System;

public partial class JumpInstructionWrapper : InstructionWrapper
{
    [Export]
    private SpinBox lineEdit;


    private void OnJumpTargetChange(double value)
    {
        if (Instruction is JumpInstruction jumpInstruction)
        {
            jumpInstruction.TargetId = (int)value;
        }
        else
        {
            throw new Exception("Expected to have some JumpInstruction, but had " + Instruction);
        }
    }
}
