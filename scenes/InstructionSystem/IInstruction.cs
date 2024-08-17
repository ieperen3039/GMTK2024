using Godot;

public interface IInstruction
{
    IAction GetAction(in Automaton automaton);
}

public class WaitInstruction : IInstruction
{
    public IAction GetAction(in Automaton automaton)
    {
        return new WaitAction();
    }

    public class WaitAction : IAction
    {
        public void Execute(Automaton automaton) { }
    }
}
