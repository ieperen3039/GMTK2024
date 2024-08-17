using Godot;
using System;
using System.Collections.Generic;

public partial class Automaton : Node2D
{
    [Export]
    public int GridSize = 100;

    public Vector2I CoordinatePosition;
    public CardinalDirection Direction;
    public IList<IAction> PreparedActions = new List<IAction>();

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

    public IAction ReadInstruction()
    {
        if (instructions.Count == 0)
        {
            return new WaitInstruction.WaitAction();
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
            GD.Print("Add ForwardInstruction");
            instructions.Add(new ForwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_down"))
        {
            GD.Print("Add BackwardInstruction");
            instructions.Add(new BackwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_right"))
        {
            GD.Print("Add TurnRightInstruction");
            instructions.Add(new TurnRightInstruction());
        }
        else if (Input.IsActionJustPressed("player_left"))
        {
            GD.Print("Add TurnLeftInstruction");
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
        GD.Print("Automation dies");
        // TODO
    }

}
