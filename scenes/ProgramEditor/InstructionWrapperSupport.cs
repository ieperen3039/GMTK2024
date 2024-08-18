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
    [Export]
    private Label idLabel;
    [Export]
    private CommandIcon commandIcon;

    public int Id { get; private set; }

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
        // 12 is 2x the visual border (it depends on the button graphics)
        CustomMinimumSize = new Vector2(CustomMinimumSize.X, wrapper.CustomMinimumSize.Y + 12);
        instructionWrapperTarget.AddChild(wrapper);

        // AFTER add child, or the SimpleInstruction doesnt know its instruction yet
        Instruction = wrapper.GetInstruction();
        commandIcon.SetInstruction(Instruction);

        if (Instruction == null)
        {
            throw new Exception("Null instruction");
        }
    }

    internal void SetId(int newId)
    {
        Id = newId;
        idLabel.Text = Id.ToString();
    }

}
