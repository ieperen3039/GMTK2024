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

    private long birthCycle = 0;

    private IList<IInstruction> instructions = new List<IInstruction>();
    private int instructionIndexCurrent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void Spawn(Vector2I aPosition, CardinalDirection aDirection, long currentCycle) 
    {
        CoordinatePosition = aPosition;
        Direction = aDirection;
        Rotation = CardinalDirections.ToVector(Direction).Angle();
        birthCycle = currentCycle;
    }

    public IAction GetIntention()
    {
        GD.Print("GetIntention ", instructionIndexCurrent, "/", instructions.Count);
        if (instructions.Count == 0)
        {
            return new WaitInstruction.WaitAction(CoordinatePosition);
        }

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
            instructions.Add(new BackwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_right"))
        {
            instructions.Add(new TurnRightInstruction());
        }
        else if (Input.IsActionJustPressed("player_left"))
        {
            instructions.Add(new TurnLeftInstruction());
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
        Direction = CardinalDirections.RotateCounterClockwise(Direction);
        Rotation = CardinalDirections.ToVector(Direction).Angle();
    }

    public void TurnRight()
    {
        Direction = CardinalDirections.RotateClockwise(Direction);
        Rotation = CardinalDirections.ToVector(Direction).Angle();
    }

    public void Die()
    {
        // TODO
    }

}
