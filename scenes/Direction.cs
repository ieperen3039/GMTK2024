// order is chosen to reflect incrementing the angle of the sprite
using Godot;

public enum CardinalDirection
{
    EAST,
    NORTH,
    WEST,
    SOUTH,
}

public class CardinalDirections
{
    public static Vector2I ToVector(in CardinalDirection aDirection)
    {
        return aDirection switch
        {
            CardinalDirection.EAST => new Vector2I(0, 1),
            CardinalDirection.NORTH => new Vector2I(1, 0),
            CardinalDirection.WEST => new Vector2I(0, -1),
            CardinalDirection.SOUTH => new Vector2I(-1, 0),
            _ => new(),
        };
    }
}
