using Godot;

public interface IInstruction
{
    IAction GetAction(in Automaton automaton);
}

public class WaitInstruction : IInstruction
{
    public IAction GetAction(in Automaton automaton)
    {
        return new WaitAction(automaton.CoordinatePosition);
    }

    public class WaitAction : IAction
    {
        private Vector2I Position;

        public WaitAction(Vector2I coordinatePosition)
        {
            Position = coordinatePosition;
        }

        public void Execute(Automaton automaton) { }

        public Vector2I GetSourcePosition() => Position;

        public Vector2I GetTargetPosition() => Position;
    }
}
