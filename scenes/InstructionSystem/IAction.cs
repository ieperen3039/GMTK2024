using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IAction
{
    Vector2I GetSourcePosition();
    Vector2I GetTargetPosition();

    void Execute(Automaton automaton);
}

public class MoveAction : IAction 
{
    public CardinalDirection Direction;
    public Vector2I Position;

    public MoveAction(Vector2I aPosition, CardinalDirection aDirection)
    {
        Position = aPosition;
        Direction = aDirection;
    }

    public Vector2I GetSourcePosition() { return Position; }

    public Vector2I GetTargetPosition() { return Position += CardinalDirections.ToVector(Direction); }

    public void Execute(Automaton automaton) 
    {
    }
}