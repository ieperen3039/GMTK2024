using Godot;

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

    public Vector2I GetTargetPosition() { return Position += CardinalDirections.ToVectorI(Direction); }

    public void Execute(Automaton automaton) 
    {
        automaton.CoordinatePosition = GetTargetPosition();
        automaton.MoveForward();
    }
}