using Godot;
using System;
using System.Collections.Generic;

public partial class Automaton : Node2D
{
    [Export]
    public int GridSize = 100;

    public Vector2I CoordinatePosition;
    public CardinalDirection Direction;
    public IAction PreparedAction;

    int nrOfCyclesAlive = 0;

    private IList<IInstruction> instructions;
    private int instructionIndexCurrent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    public IAction GetIntention()
    {
        IInstruction instruction = instructions[instructionIndexCurrent];

        instructionIndexCurrent = (instructionIndexCurrent + 1) % instructions.Count;

        return instruction.GetAction(this);
    }

    public void MoveGrid(Vector2I direction)
    {
        Translate(direction * GridSize);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("player_up"))
        {
            instructions.Add(new ForwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_down"))
        {
            instructions.Add(new TurnLeftInstruction());
        }
        else if (Input.IsActionJustPressed("player_right"))
        {
            instructions.Add(new TurnRightInstruction());
        }
        else if (Input.IsActionJustPressed("player_left"))
        {
            instructions.Add(new ForwardInstruction());
        }
    }

    public void MoveForward()
    {
        MoveGrid(CardinalDirections.ToVectorI(Direction));
    }

    public void MoveBackward()
    {
        Vector2I back = -CardinalDirections.ToVectorI(Direction);
        MoveGrid(back);
    }

    public void TurnLeft()
    {
        Direction = CardinalDirections.RotateClockwise(Direction);
        Rotation = CardinalDirections.ToVector(Direction).Angle();
    }

    public void TurnRight()
    {
        Direction = CardinalDirections.RotateCounterClockwise(Direction);
        Rotation = CardinalDirections.ToVector(Direction).Angle();
    }
}
