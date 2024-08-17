using Godot;
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

    internal void FinalizeCycle(Vector2I targetPosition)
    {
        CoordinatePosition = targetPosition;
        PreparedAction = null;
    }

    public void MoveGrid(Vector2I direction)
    {
        Translate(direction * GridSize);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("player_up")) {
            MoveForward();
        } else if (Input.IsActionJustPressed("player_down")) {
            MoveBackward();
        } else if (Input.IsActionJustPressed("player_right")){
            TurnLeft();
        } else if (Input.IsActionJustPressed("player_left")) {
            TurnRight();
        }
    }


    public void MoveForward()
    {
        Vector2 direction = new Vector2(0, -1);
        direction = direction.Rotated(Rotation).Round();
        Vector2I fixedDirection = new Vector2I((int)direction.X, (int)direction.Y);
        MoveGrid(fixedDirection);        
    }

    public void MoveBackward()
    {
        Vector2 direction = new Vector2(0, 1);
        direction = direction.Rotated(Rotation).Round();
        Vector2I fixedDirection = new Vector2I((int)direction.X, (int)direction.Y);
        MoveGrid(fixedDirection);   
    }

    public void TurnLeft()
    {
        int currentRotationDegrees = (int)RotationDegrees;
        currentRotationDegrees = (currentRotationDegrees + 90) % 360;
        RotationDegrees = currentRotationDegrees;
    }

    public void TurnRight()
    {
        int currentRotationDegrees = (int)RotationDegrees;
        currentRotationDegrees = (currentRotationDegrees - 90) % 360;
        RotationDegrees = currentRotationDegrees;
    }

}
