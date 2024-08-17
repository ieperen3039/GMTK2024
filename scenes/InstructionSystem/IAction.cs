using Godot;

public interface IAction
{
    Vector2I GetRelativeMovement() => Vector2I.Zero;

    void Execute(Automaton automaton);
}