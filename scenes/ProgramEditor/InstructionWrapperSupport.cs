using Godot;
using System;

public partial class InstructionWrapperSupport : Panel
{
    [Signal]
    public delegate void InstructionMoveDownEventHandler(int id);
    [Signal]
    public delegate void InstructionMoveUpEventHandler(int id);
    [Signal]
    public delegate void InstructionDeleteEventHandler(int id);

    [Export]
    private Control instructionWrapperTarget;

    public int Id;

    public IInstruction Instruction { get; private set; }


    public override void _Ready()
    {
        base._Ready();
    }

    public void MoveDown()
    {
        EmitSignal(SignalName.InstructionMoveDown, Id);
    }
    public void MoveUp()
    {
        EmitSignal(SignalName.InstructionMoveUp, Id);
    }
    public void Delete()
    {
        EmitSignal(SignalName.InstructionDelete, Id);
    }

    internal void SetContent(InstructionWrapper wrapper)
    {
        Instruction = wrapper.Instruction;
        instructionWrapperTarget.AddChild(wrapper);
    }
}
