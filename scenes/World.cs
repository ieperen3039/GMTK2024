using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class World : Node2D
{
    private const int NumGridBuffers = 2;

    [Export]
    private Image layout;

    [Export]
    private Automaton tmp_player;


    [Export]
    private double cycleTimeSec = 1;

    // fixed-size list of nullable elements
    private Grid[] grids;
    private int xSize;
    private int ySize;
    private int currentGrid = 0;

    private double cycleCooldownSec;
    private long currentCycleIndex;


    public override void _Ready()
    {
        layout = Image.LoadFromFile("res://assets/levels/level_1.png");
        // cycleCooldownSec = cycleTimeSec;
        cycleCooldownSec = double.MaxValue;

        grids = new Grid[NumGridBuffers];
        xSize = layout.GetWidth();
        ySize = layout.GetHeight();

        for (int i = 0; i < NumGridBuffers; i++)
        {
            grids[i] = new Grid(xSize, ySize);
        }

        // https://github.com/godotengine/godot/issues/65761
        // lock (layout)

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Color color = layout.GetPixel(x, y);

                for (int i = 0; i < NumGridBuffers; i++)
                {
                    Grid.Element element = new();

                    if (color.R > 0.5) // red pixel
                    {
                        element.HasFloor = false;
                    }

                    grids[i].SetElement(x, y, element);
                }
            }
        }

        // temporary
        Spawn(tmp_player);
    }

    private void Spawn(Automaton automaton)
    {
        // TODO randomize position + direction
        automaton.Spawn(new Vector2I(25, 25), CardinalDirection.NORTH, currentCycleIndex);
        GetCurrentGrid().GetElement(automaton.CoordinatePosition).Automaton = automaton;
    }

    public override void _Process(double aDelta)
    {
        cycleCooldownSec -= aDelta;

        if (cycleCooldownSec < 0)
        {
            cycleCooldownSec += cycleTimeSec;
            if (cycleCooldownSec < 0) cycleCooldownSec = 0;

            RunCycle();
        }

        if (Input.IsActionJustPressed("execute"))
        {
            GD.Print("Starting cycle every ", cycleTimeSec, " seconds");
            cycleCooldownSec = 0;
        }
    }

    public void RunCycle()
    {
        // collect intentions
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            if (sourceGridElement.Automaton != null)
            {
                Automaton automaton = sourceGridElement.Automaton;
                IAction action = automaton.GetIntention();
                automaton.PreparedAction = action;
            }
        }

        // check and update prepared action
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
        }

        // execute the actions
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            if (sourceGridElement.Automaton != null)
            {
                // remove automaton from the old place
                Automaton automaton = sourceGridElement.Automaton;
                sourceGridElement.Automaton = null;

                // execute instruction
                IAction action = automaton.PreparedAction;
                Vector2I targetPosition = action.GetTargetPosition();
                action.Execute(automaton);

                // add to the new place
                Grid.Element targetGridElement = GetFutureGrid().GetElement(targetPosition);
                if (targetGridElement.HasFloor)
                {
                    targetGridElement.Automaton = automaton;
                }
                else
                {
                    automaton.Die();
                }
            }
        }

        currentGrid = (currentGrid + 1) % NumGridBuffers;
    }

    private Grid GetCurrentGrid() => grids[currentGrid];
    private Grid GetFutureGrid() => grids[(currentGrid + 1) % NumGridBuffers];
}
