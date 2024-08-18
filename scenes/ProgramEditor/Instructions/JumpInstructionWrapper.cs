using Godot;
using System;

public partial class JumpInstructionWrapper : InstructionWrapper
{
    [Export]
    private LineEdit lineEdit;


    private void OnJumpTargetChange(string aNewText)
    {
        if (Instruction is JumpInstruction jumpInstruction)
        {
            try
            {
                jumpInstruction.TargetId = aNewText.ToInt();
            }
            catch (Exception)
            {
                lineEdit.Text = "";
            }
        }
        else
        {
            throw new Exception("Expected to have some JumpInstruction, but had " + Instruction);
        }
    }
}


