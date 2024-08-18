using Godot;
using System;
using System.Collections.Generic;

public partial class Automaton : Node2D
{
    [Export]
    public int GridTileSize = 64;

    public Vector2I GridCoordinate;
    public CardinalDirection Direction;
    public IList<IAction> PreparedActions = new List<IAction>();
    public IList<IInstruction> Instructions = new List<IInstruction>();

    private long birthCycle = 0;

    private int instructionIndexCurrent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void Spawn(Vector2I aGridCoordinate, CardinalDirection aDirection, long currentCycle)
    {
        GridCoordinate = aGridCoordinate;
        GlobalPosition = GridCoordinate * GridTileSize;
        Direction = aDirection;
        Rotation = CardinalDirections.ToVector(Direction).Angle();
        birthCycle = currentCycle;
    }

    public IAction ReadInstruction(Grid game)
    {
        if (Instructions.Count == 0)
        {
            return new WaitInstruction.WaitAction();
        }

        IAction action = null;
        int remainingIterations = 50;

        while (action == null)
        {
            // too long processing
            if (remainingIterations-- < 0)
            {
                return new WaitInstruction.WaitAction();
            }

            IInstruction instruction = Instructions[instructionIndexCurrent];

            // too much effort to solve this nicely
            if (instruction is CheckInstruction checkInstruction)
            {
                bool success = ExecuteCheck(checkInstruction, game);
                if (success)
                {
                    instructionIndexCurrent = checkInstruction.TargetId;
                }
            }
            if (instruction is JumpInstruction jumpInstruction)
            {
                instructionIndexCurrent = jumpInstruction.TargetId;
            }
            else
            {
                instructionIndexCurrent = (instructionIndexCurrent + 1) % Instructions.Count;
            }

            action = instruction.GetAction(this);
        }

        return action;
    }

    private bool ExecuteCheck(CheckInstruction checkInstruction, Grid game)
    {
        foreach (Vector2I delta in checkInstruction.GetCheckCoordinates())
        {
            Vector2I globalLocation = LocalToGlobal(delta);
            Grid.Element element = game.GetElement(globalLocation);
            bool found = checkInstruction.Check(element);

            if (found) return true;
        }
        return false;
    }


    public void MoveGrid(Vector2I direction)
    {
        Translate(direction * GridTileSize);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("player_up"))
        {
            GD.Print("Add ForwardInstruction");
            Instructions.Add(new ForwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_down"))
        {
            GD.Print("Add BackwardInstruction");
            Instructions.Add(new BackwardInstruction());
        }
        else if (Input.IsActionJustPressed("player_right"))
        {
            GD.Print("Add TurnRightInstruction");
            Instructions.Add(new TurnRightInstruction());
        }
        else if (Input.IsActionJustPressed("player_left"))
        {
            GD.Print("Add TurnLeftInstruction");
            Instructions.Add(new TurnLeftInstruction());
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

    public Vector2I LocalToGlobal(Vector2I vector)
    {
        // EAST is the default/global rotation
        Vector2I unrotated = Direction switch
        {
            CardinalDirection.EAST => vector,
            CardinalDirection.NORTH => new Vector2I(vector.Y, -vector.X),
            CardinalDirection.WEST => -vector,
            CardinalDirection.SOUTH => new Vector2I(-vector.Y, vector.X),
            _ => vector,
        };

        return GridCoordinate + unrotated;
    }

    public void Die()
    {
        GD.Print("Automation dies");
        // TODO
    }

}
