public class ForwardInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new MoveAction(automaton.CoordinatePosition, CardinalDirections.ToVectorI(automaton.Direction));
    }
}

public class BackwardInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new MoveAction(automaton.CoordinatePosition, -CardinalDirections.ToVectorI(automaton.Direction));
    }
}

public class TurnLeftInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new TurnAction(automaton.CoordinatePosition, TurnAction.TurnDirection.LEFT);
    }
}

public class TurnRightInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new TurnAction(automaton.CoordinatePosition, TurnAction.TurnDirection.RIGHT);
    }
}