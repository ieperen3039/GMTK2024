using Godot;
using System;

public class JumpInstruction : IInstruction
{
    public int TargetId;

    public IAction GetAction(in Automaton automaton)
    {
        return null;
    }
}
