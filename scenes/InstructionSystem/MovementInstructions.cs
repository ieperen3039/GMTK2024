public class ForwardInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new MoveAction(CardinalDirections.ToVectorI(automaton.Direction));
    }
}

public class BackwardInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new MoveAction(-CardinalDirections.ToVectorI(automaton.Direction));
    }
}

public class TurnLeftInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new TurnAction(TurnAction.TurnDirection.LEFT);
    }
}

public class TurnRightInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new TurnAction(TurnAction.TurnDirection.RIGHT);
    }
}