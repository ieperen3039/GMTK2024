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
    private PackedScene instructionSupport;

    private int nextId = 0;


    public override void _Ready()
    {
        if (dropCollider.Shape is RectangleShape2D rec)
        {
            rec.Size = instructionList.Size;
            dropCollider.Position = instructionList.Position + (rec.Size / 2);
        }
    }

    public IList<IInstruction> GetInstructions()
    {
        IList<IInstruction> instructions = new List<IInstruction>();

        foreach (Node child in instructionList.GetChildren())
        {
            if (child is InstructionWrapperSupport wrapper)
            {
                // change TargetId to be the index instead
                if (wrapper.Instruction is JumpInstruction jumpInstr)
                {
                    int i = 0;
                    foreach (Node other in instructionList.GetChildren())
                    {
                        if (other is InstructionWrapperSupport otherWrapper)
                        {
                            if (otherWrapper.Id == jumpInstr.TargetId)
                            {
                                jumpInstr.TargetId = i;
                                break;
                            }
                            i++;
                        }
                    }
                }

                instructions.Add(wrapper.Instruction);
            }

        }

        return instructions;
    }

    private InstructionWrapperSupport FindInstruction(int id)
    {
        foreach (Node child in instructionList.GetChildren())
        {
            if (child is InstructionWrapperSupport wrapper)
            {
                if (wrapper.Id == id)
                {
                    return wrapper;
                }
            }
        }

        return null;
    }

    public void InstructionMoveUp(int id)
    {
        InstructionWrapperSupport wrapper = FindInstruction(id);
        int idx = wrapper.GetIndex();
        instructionList.MoveChild(wrapper, idx - 1);
    }

    public void InstructionMoveDown(int id)
    {
        InstructionWrapperSupport wrapper = FindInstruction(id);
        int idx = wrapper.GetIndex();
        instructionList.MoveChild(wrapper, idx + 1);
    }
    public void InstructionDelete(int id)
    {
        InstructionWrapperSupport wrapper = FindInstruction(id);
        wrapper.QueueFree();
    }

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return true;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        InstructionWrapper wrapper = data.As<InstructionWrapper>();
        AddInstruction(wrapper);
    }

    private void AddInstruction(InstructionWrapper wrapper)
    {
        InstructionWrapperSupport support = instructionSupport.Instantiate<InstructionWrapperSupport>();
        support.SetId(nextId++);
        support.InstructionMoveUp += InstructionMoveUp;
        support.InstructionMoveDown += InstructionMoveDown;
        support.InstructionDelete += InstructionDelete;

        // put the InstructionWrapper inside a InstructionWrapperSupport and the result into the `instructionList`
        support.SetContent(wrapper);
        instructionList.AddChild(support);
    }
}
