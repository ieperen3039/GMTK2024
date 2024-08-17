using Godot;
using System;
using System.Collections.Generic;

public partial class ProgramList : Panel
{
    [Export]
    private CollisionShape2D dropCollider;
    [Export]
    private Control instructionList;
    [Export]
    private PackedScene[] instructionWrappers;

    private IList<IInstruction> instructions = new List<IInstruction>();

    public override void _Ready()
    {
        if (dropCollider.Shape is RectangleShape2D rec)
        {
            rec.Size = instructionList.Size;
            dropCollider.Position = instructionList.Position + (rec.Size / 2);
        }
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return true;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        int typeInt = data.AsInt32();

        InstructionWrapper wrapper = instructionWrappers[typeInt].Instantiate<InstructionWrapper>();

        if ((int) wrapper.Type != typeInt) throw new Exception("Inconsistent type (" + wrapper.Type + " has value " + (int) wrapper.Type + ")");

        instructionList.AddChild(wrapper);
        instructions.Add(wrapper.Instruction);
    }
}
