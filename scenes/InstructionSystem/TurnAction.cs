using Godot;

public class TurnAction : IAction
{
    public enum TurnDirection
    {
        LEFT,
        RIGHT
    }

    public TurnDirection Direction;
    public Vector2I Position;

    public TurnAction(Vector2I aPosition, TurnDirection aDirection)
    {
        Position = aPosition;
        Direction = aDirection;
    }

    public Vector2I GetSourcePosition() { return Position; }

    public Vector2I GetTargetPosition() { return Position; }

    public void Execute(Automaton automaton)
    {
        automaton.MoveForward();
    }
}