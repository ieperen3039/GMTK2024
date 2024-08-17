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
    public static Vector2I ToVectorI(in CardinalDirection aDirection)
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

    public static Vector2 ToVector(in CardinalDirection aDirection)
    {
        return aDirection switch
        {
            CardinalDirection.EAST => new Vector2(0, 1),
            CardinalDirection.NORTH => new Vector2(1, 0),
            CardinalDirection.WEST => new Vector2(0, -1),
            CardinalDirection.SOUTH => new Vector2(-1, 0),
            _ => new(),
        };
    }

    public static CardinalDirection RotateClockwise(in CardinalDirection aDirection)
    {
        return aDirection switch
        {
            CardinalDirection.EAST => CardinalDirection.SOUTH,
            CardinalDirection.NORTH => CardinalDirection.EAST,
            CardinalDirection.WEST => CardinalDirection.NORTH,
            CardinalDirection.SOUTH => CardinalDirection.WEST,
            _ => new(),
        };
    }

    public static CardinalDirection RotateCounterClockwise(in CardinalDirection aDirection)
    {
        return aDirection switch
        {
            CardinalDirection.EAST => CardinalDirection.NORTH,
            CardinalDirection.NORTH => CardinalDirection.WEST,
            CardinalDirection.WEST => CardinalDirection.SOUTH,
            CardinalDirection.SOUTH => CardinalDirection.EAST,
            _ => new(),
        };
    }
}
