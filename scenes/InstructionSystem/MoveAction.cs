using Godot;

public class MoveAction : IAction 
{
    public Vector2I RelativeMovement;

    public MoveAction(Vector2I aRelativeMovement)
    {
        RelativeMovement = aRelativeMovement;
    }

    public Vector2I GetRelativeMovement() => RelativeMovement;
    public void Execute(Automaton automaton) => automaton.SetNewGridPosition(automaton.GridCoordinate + RelativeMovement);
}
