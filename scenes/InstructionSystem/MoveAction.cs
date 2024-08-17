using Godot;

public class MoveAction : IAction 
{
    public Vector2I SourcePosition;    
    public Vector2I TargetPosition;

    public MoveAction(Vector2I aStartPosition, Vector2I aRelativeMovement)
    {
        SourcePosition = aStartPosition;    
        TargetPosition = aStartPosition + aRelativeMovement;
    }

    public Vector2I GetSourcePosition() { return SourcePosition; }

    public Vector2I GetTargetPosition() { return TargetPosition; }

    public void Execute(Automaton automaton) 
    {
        automaton.CoordinatePosition = GetTargetPosition();
        automaton.MoveForward();
    }
}
