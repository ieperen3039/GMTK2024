using Godot;
using System.Collections.Generic;
using System.Numerics;

public partial class World : Node2D 
{
    [Export]
    private Image layout;
    

    // fixed-size list of nullable elements
    private Grid[] grids;
    private int xSize;
    private int ySize;
    int currentGrid = 0;

    public override void _Ready()
    {
        layout = Image.LoadFromFile("res://assets/levels/level_1.png");
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

    public void RunCycle()
    {
        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            // remove automaton from the old place
            Automaton automaton = sourceGridElement.Automaton;
            IAction action = automaton.GetIntention();
            automaton.PreparedAction = action;
        }

        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            // check and update prepared action
        }

        foreach (Grid.Element sourceGridElement in GetCurrentGrid())
        {
            // remove automaton from the old place
            Automaton automaton = sourceGridElement.Automaton;
            sourceGridElement.Automaton = null;

            // execute instruction
            IAction action = automaton.PreparedAction;
            Vector2I targetPosition = action.GetTargetPosition();
            Grid.Element targetGridElement = GetFutureGrid().GetElement(targetPosition);
            
            // add to the new place
            automaton.FinalizeCycle(targetPosition);
            targetGridElement.Automaton = automaton;
        }
    }

    private Grid GetCurrentGrid() => grids[currentGrid];
    private Grid GetFutureGrid() => grids[(currentGrid + 1) % grids.Length];
}