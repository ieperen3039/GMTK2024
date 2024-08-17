using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class World : Node2D 
{
    [Export]
    private Image layout;
    

    [Export]
    private double cycleTimeSec;

    // fixed-size list of nullable elements
    private Grid[] grids;
    private int xSize;
    private int ySize;
    private int currentGrid = 0;

    private double cycleCooldownSec;

    public override void _Ready()
    {
        layout = Image.LoadFromFile("res://assets/levels/level_1.png");
        cycleCooldownSec = cycleTimeSec;

        grids = new Grid[2];
        xSize = layout.GetWidth();
        ySize = layout.GetHeight();
        grids[0] = new Grid(xSize, ySize);
        grids[1] = new Grid(xSize, ySize);

        // https://github.com/godotengine/godot/issues/65761
        // lock (layout)
        
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Color color = layout.GetPixel(x, y);
                Grid.Element element = new();

                if (color.R > 0.5) // red pixel
                {
                    element.HasFloor = false;
                }

                grids[0].SetElement(x, y, element);
            }
        }
    }

    public override void _Process(double aDelta){
        cycleCooldownSec -= aDelta;

        if (cycleCooldownSec < 0)
        {
            RunCycle();
            cycleCooldownSec += 1;

            if (cycleCooldownSec < 0) cycleCooldownSec = 0;
        }
    }

    public void RunCycle()
    {
        // collect intentions
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            Automaton automaton = sourceGridElement.Automaton;
            IAction action = automaton.GetIntention();
            automaton.PreparedAction = action;
        }

        // check and update prepared action
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {

        }

        // execute the actions
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
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
            targetGridElement.Automaton = automaton;
        }
    }

    private Grid GetCurrentGrid() => grids[currentGrid];
    private Grid GetFutureGrid() => grids[(currentGrid + 1) % grids.Length];
}