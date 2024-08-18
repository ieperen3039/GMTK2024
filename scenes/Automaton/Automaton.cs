using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;

public partial class Automaton : Node2D
{
    private const int TexturePixelSize = 16;
    [Export]
    public int GridTileSize = 64;
    [Export]
    public bool IsPlayer = false;

    public double CycleTimeSec = 0.5f;

    public Vector2I GridCoordinate;
    public CardinalDirection Direction;
    public IAction PreparedAction;
    public IList<IInstruction> Instructions = new List<IInstruction>();
    Tween movementTween;
    Tween rotationTween;

    private long birthCycle = 0;

    private int instructionIndexCurrent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Scale = new Vector2(GridTileSize / TexturePixelSize, GridTileSize / TexturePixelSize);
    }

    public void Spawn(Vector2I aGridCoordinate, CardinalDirection aDirection, long currentCycle, double aCycleTimeSec)
    {
        CycleTimeSec = aCycleTimeSec;
        GridCoordinate = aGridCoordinate;
        GlobalPosition = GridCoordinate * GridTileSize;
        Direction = aDirection;
        Rotation = CardinalDirections.ToVector(Direction).Angle();
        birthCycle = currentCycle;
        instructionIndexCurrent = 0;
    }

    public IAction ReadInstruction(Grid game)
    {
        if (Instructions.Count == 0)
        {
            return new WaitInstruction.WaitAction();
        }

        IAction action = null;
        int remainingIterations = 20;

        while (action == null && remainingIterations-- > 0)
        {
            IInstruction instruction = Instructions[instructionIndexCurrent];

            // too much effort to solve this nicely
            if (instruction == null)
            {
                // throw new Exception("Instruction " + instructionIndexCurrent + " is null, out of" + Instructions.Count);
                instructionIndexCurrent = NextInstruction();
            }
            else if (instruction is CheckInstruction checkInstruction)
            {
                bool success = ExecuteCheck(checkInstruction, game);
                instructionIndexCurrent = success ? checkInstruction.TargetId : NextInstruction();
            }
            else if (instruction is JumpInstruction jumpInstruction)
            {
                instructionIndexCurrent = jumpInstruction.TargetId;
            }
            else
            {
                instructionIndexCurrent = NextInstruction();
            }

            action = instruction.GetAction(this);
        }

        return action ?? new WaitInstruction.WaitAction();
    }

    private int NextInstruction()
    {
        return (instructionIndexCurrent + 1) % Instructions.Count;
    }


    public Vector2I GetTargetPosition()
    {
        return GridCoordinate + PreparedAction.GetRelativeMovement();
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


    public void SetNewGridPosition(Vector2I aNewPosition)
    {
        GridCoordinate = aNewPosition;

        // Calculate the new position
        Vector2 newPosition = GridCoordinate * GridTileSize;

        // reset tween
        movementTween?.Kill();
        movementTween = GetTree().CreateTween();

        // create tween
        movementTween.TweenProperty(this, "global_position", newPosition, CycleTimeSec / 2);
    }

    private void SetNewGridRotation(CardinalDirection aNewDirection)
    {
        float newRotation = Rotation + CardinalDirections.ToVector(Direction).AngleTo(CardinalDirections.ToVector(aNewDirection));
        Direction = aNewDirection;

        // reset tween
        rotationTween?.Kill();
        rotationTween = GetTree().CreateTween();

        // create tween
        rotationTween.TweenProperty(this, "rotation", newRotation, CycleTimeSec / 2);
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
        SetNewGridPosition(GridCoordinate + CardinalDirections.ToVectorI(Direction));
    }

    public void MoveBackward()
    {
        SetNewGridPosition(GridCoordinate - CardinalDirections.ToVectorI(Direction));
    }

    public void TurnLeft()
    {
        SetNewGridRotation(CardinalDirections.RotateCounterClockwise(Direction));
    }

    public void TurnRight()
    {
        SetNewGridRotation(CardinalDirections.RotateClockwise(Direction));
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
        if (!IsPlayer) QueueFree();
        else Visible = false;
    }

    // move all elements from other to this, leaving other empty
    public void Plunder(Automaton otherAutomaton)
    {
        otherAutomaton.movementTween?.Kill();
        otherAutomaton.rotationTween?.Kill();

        Instructions = otherAutomaton.Instructions;
        otherAutomaton.Instructions = new List<IInstruction>();

        instructionIndexCurrent = otherAutomaton.instructionIndexCurrent;
        otherAutomaton.instructionIndexCurrent = 0;

        birthCycle = otherAutomaton.birthCycle;
        otherAutomaton.birthCycle = 0;

        GridCoordinate = otherAutomaton.GridCoordinate;
        otherAutomaton.GridCoordinate = Vector2I.Zero;
        Direction = otherAutomaton.Direction;

        PreparedAction = otherAutomaton.PreparedAction;

        GlobalPosition = GridCoordinate * GridTileSize;
        Rotation = CardinalDirections.ToVector(Direction).Angle();
    }
}
