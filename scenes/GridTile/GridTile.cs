using Godot;

public enum GridTileType {
    GAP = 0,
    NORMAL = 1,
    SPAWN = 2,
    GOAL = 3,
    WALL = 4,
}

public partial class GridTile : Sprite2D
{
    [Export]
    public GridTileType tileType;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Frame = (int)tileType;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
