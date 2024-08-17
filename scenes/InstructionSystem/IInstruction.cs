using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IInstruction
{
    IAction GetAction(in Automaton automaton);
}

public class ForwardInstruction : IInstruction 
{
    public IAction GetAction(in Automaton automaton)
    {
        return new MoveAction(automaton.CoordinatePosition, automaton.Direction);
    }

}