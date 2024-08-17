using Godot;

public class TurnAction : IAction
{
    public enum TurnDirection
    {
        LEFT,
        RIGHT
    }

    public TurnDirection Direction;
    public TurnAction(TurnDirection aDirection)
    {
        Direction = aDirection;
    }

    public void Execute(Automaton automaton)
    {
        switch (Direction)
        {
            case TurnDirection.LEFT:
                automaton.TurnLeft();
                break;
            case TurnDirection.RIGHT:
                automaton.TurnRight();
                break;
        }
    }
}