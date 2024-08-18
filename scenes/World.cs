using System;
using System.Collections.Generic;
using System.Linq;
using Godot;



public partial class World : Node2D
{
    private const int NumGridBuffers = 2;

    const string GAP_COLOR = "ff0000ff";
    const string GOAL_COLOR = "00ff00ff";
    const string SPAWN_COLOR = "0000ffff";
    const string WALL_COLOR = "303030ff";
    const string NORMAL_COLOR = "ffffffff";

    [Export]
    private Image layout;

    [Export]
    public PackedScene GridTileScene;

    [Export]
    private Automaton playerAutomaton;
    [Export]
    private PackedScene automatonScene;


    [Export]
    private double cycleTimeSec = 1;

    // fixed-size list of nullable elements
    private Grid[] grids;
    private int xSize;
    private int ySize;
    private int currentGridIdx = 0;

    private RandomNumberGenerator rng;

    private double cycleCooldownSec;
    private long currentCycleIndex;
    private IList<Vector2I> spawnPositions = new List<Vector2I>();

    public bool Suspend { get; internal set; }


    public override void _Ready()
    {
        layout = Image.LoadFromFile("res://assets/levels/level_1.png");
        cycleCooldownSec = cycleTimeSec;
        // cycleCooldownSec = double.MaxValue;
        rng = new RandomNumberGenerator();

        grids = new Grid[NumGridBuffers];
        xSize = layout.GetWidth();
        ySize = layout.GetHeight();

        for (int i = 0; i < NumGridBuffers; i++)
        {
            grids[i] = new Grid(xSize, ySize);
        }

        var GridNode = GetNode("Grid");

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
                    GridTile gridTile = GridTileScene.Instantiate<GridTile>();

                    switch (color.ToHtml())
                    {
                        case GAP_COLOR:
                            {
                                element.HasFloor = false;
                                gridTile.tileType = GridTileType.GAP;
                                break;
                            }
                        case SPAWN_COLOR:
                            {
                                gridTile.tileType = GridTileType.SPAWN;
                                spawnPositions.Add(new Vector2I(x, y));
                                break;
                            }
                        case GOAL_COLOR:
                            {
                                gridTile.tileType = GridTileType.GOAL;
                                break;
                            }
                        case WALL_COLOR:
                            {
                                gridTile.tileType = GridTileType.WALL;
                                element.IsWall = true;
                                break;
                            }
                        default:
                            {
                                gridTile.tileType = GridTileType.NORMAL;
                                break;
                            }
                    }

                    grids[i].SetElement(x, y, element);
                    var tilePosition = new Vector2I(x * 64, y * 64);
                    gridTile.Position = tilePosition;
                    GridNode.AddChild(gridTile);
                }
            }
        }

        Spawn(playerAutomaton, spawnPositions[0]);
        GD.Print("Starting cycle every ", cycleTimeSec, " seconds");
    }

    public void SpawnPlayer(IList<IInstruction> instructions)
    {
        if (instructions.Any((i) => i == null))
        {
            throw new Exception("null instruction");
        };

        // Automaton automaton = automatonScene.Instantiate<Automaton>();
        // automaton.Plunder(playerAutomaton);
        // AddChild(automaton);

        playerAutomaton.Instructions = instructions;

        int targetSpawnIndex = rng.RandiRange(0, spawnPositions.Count - 1);
        Spawn(playerAutomaton, spawnPositions[targetSpawnIndex]);
    }

    private void Spawn(Automaton automaton, Vector2I spawnPosition)
    {
        automaton.Spawn(spawnPosition, CardinalDirections.Random(rng), currentCycleIndex, cycleTimeSec);
        GetCurrentGrid().GetElement(automaton.GridCoordinate).Automaton = automaton;
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
            if (cycleCooldownSec <= cycleTimeSec)
            {
                GD.Print("Cycle disabled");
                cycleCooldownSec = double.PositiveInfinity;
            }
            else
            {
                GD.Print("Starting cycle every ", cycleTimeSec, " seconds");
                cycleCooldownSec = 0;
            }
        }
    }

    public void RunCycle()
    {
        currentCycleIndex++;
        GD.Print("Cycle " + currentCycleIndex);

        Grid currentGrid = GetCurrentGrid();
        Grid futureGrid = GetFutureGrid();

        IList<Automaton> automatons = new List<Automaton>();

        // reset future grid
        foreach (Grid.Element tile in futureGrid)
        {
            tile.numReservations = 0;
            tile.Automaton = null;
        }

        // collect automatons
        foreach (Grid.Element tile in currentGrid)
        {
            if (tile.Automaton != null)
            {
                Automaton automaton = tile.Automaton;
                automaton.PreparedAction = automaton.ReadInstruction(currentGrid);
                
                // only move actions can block other automatons
                if (automaton.PreparedAction is MoveAction)
                {
                    Vector2I targetPosition = automaton.GetTargetPosition();
                    Grid.Element targetTile = futureGrid.GetElement(targetPosition);
                    targetTile.numReservations++;

                    GD.Print("Automaton ", automaton, " moves to ", targetPosition);
                }

                automatons.Add(automaton);
            }
        }

        BlockedAction blocked = new();

        bool resolved = false;
        int iterationsRemaining = 10;
        // check and update prepared actions
        while (iterationsRemaining-- > 0 && !resolved)
        {
            resolved = true;

            foreach (Automaton thisAutomaton in automatons)
            {
                Vector2I targetPosition = thisAutomaton.GetTargetPosition();
                Grid.Element targetTile = futureGrid.GetElement(targetPosition);

                if (targetTile.numReservations > 1)
                {
                    GD.Print("Automaton ", thisAutomaton, " is blocked (reservation)");
                    // multiple automatons tried to enter this tile
                    thisAutomaton.PreparedAction = blocked;
                    resolved = false;
                }

                Automaton targetAutomaton = targetTile.Automaton;
                if (targetAutomaton == thisAutomaton)
                { 
                    // this automaton was resolved
                }
                if (targetAutomaton != null)
                {
                    if (targetAutomaton.PreparedAction is BlockedAction)
                    {
                        GD.Print("Automaton ", thisAutomaton, " is blocked (BlockedAction)");
                        // target was blocked, we cannot move it
                        thisAutomaton.PreparedAction = blocked;
                        resolved = false;
                    }
                    else
                    {
                        GD.Print("Automaton ", targetAutomaton, " is moved (pushed)");
                        targetAutomaton.PreparedAction = thisAutomaton.PreparedAction;
                        targetTile.Automaton = null;
                        resolved = false;
                    }

                }
                else if (targetTile.IsWall)
                {
                    GD.Print("Automaton ", thisAutomaton, " is blocked (Wall)");
                    thisAutomaton.PreparedAction = blocked;
                    resolved = false;
                }
                else
                {
                    targetTile.Automaton = thisAutomaton;
                }
            }
        }

        GD.Print("resolved in " + (10 - iterationsRemaining) + " iterations");

        // execute the actions
        foreach (Grid.Element sourceGridElement in currentGrid)
        {
            if (sourceGridElement.Automaton != null)
            {
                // remove automaton from the old place
                Automaton automaton = sourceGridElement.Automaton;
                sourceGridElement.Automaton = null;

                // execute actions
                automaton.PreparedAction.Execute(automaton);

                Grid.Element targetGridElement = futureGrid.GetElement(automaton.GridCoordinate);
                if (!targetGridElement.HasFloor)
                {
                    automaton.Die();
                }
            }
        }

        currentGridIdx = (currentGridIdx + 1) % NumGridBuffers;
    }

    private Grid GetCurrentGrid() => grids[currentGridIdx];
    private Grid GetFutureGrid() => grids[(currentGridIdx + 1) % NumGridBuffers];

    public void SetActive(bool aToActive)
    {
        Visible = aToActive;
        SetProcess(aToActive);

        // reset cycle to start of cycle
        if (aToActive) cycleCooldownSec = cycleTimeSec;
    }
}
