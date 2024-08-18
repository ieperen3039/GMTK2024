using Godot;

public interface IAction
{
    Vector2I GetRelativeMovement() => Vector2I.Zero;

    void Execute(Automaton automaton);
}

public class BlockedAction : IAction
{
    public void Execute(Automaton automaton) {}
}