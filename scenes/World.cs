using System;
using Godot;



public partial class World : Node2D 
{
    const string GAP_COLOR = "ff0000ff";
    const string GOAL_COLOR = "00ff00ff";
    const string SPAWN_COLOR = "0000ffff";
    const string NORMAL_COLOR = "ffffffff";

    [Export]
    private Image layout;

    [Export]
    public PackedScene GridTileScene;
    

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

        var GridNode = GetNode("Grid");

        // https://github.com/godotengine/godot/issues/65761
        // lock (layout)
        
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                Color color = layout.GetPixel(x, y);
                Grid.Element element = new();
                GridTile gridTile = GridTileScene.Instantiate<GridTile>();

                switch(color.ToHtml()) {
                    case GAP_COLOR:{
                        element.HasFloor = false;
                        gridTile.tileType = GridTileType.GAP;
                        break;
                    }
                    case SPAWN_COLOR: {
                        gridTile.tileType = GridTileType.SPAWN;
                        break;
                    }
                    case GOAL_COLOR: {
                        gridTile.tileType = GridTileType.GOAL;
                        break;
                    }
                    default: {
                        gridTile.tileType = GridTileType.NORMAL;
                        break;
                    }
                }

                grids[0].SetElement(x, y, element);
                var tilePosition = new Vector2I(x*16, y*16);
                gridTile.Position = tilePosition;
                GridNode.AddChild(gridTile);
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