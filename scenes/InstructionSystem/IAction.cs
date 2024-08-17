using Godot;

public interface IAction
{
    Vector2I GetSourcePosition();
    Vector2I GetTargetPosition();

    void Execute(Automaton automaton);
}