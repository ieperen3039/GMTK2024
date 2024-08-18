using Godot;
using System;
using System.Collections.Generic;

public class CheckInstruction : JumpInstruction
{
    public enum WhatToCheck
    {
        HOLE,
        AUTOMATON,
        FLOOR,
        WALL
    }

    private const int gridXForw = 3;
    private const int gridYLeft = 3;

    public WhatToCheck ThingToCheck = WhatToCheck.AUTOMATON;

    private bool[] checkActive = new bool[gridXForw * gridYLeft];

    public bool GetActive(int forwardSteps, int leftSteps) => checkActive[IndexOf(forwardSteps, leftSteps)];
    public void SetActive(int forwardSteps, int leftSteps, bool aValue)
    {
        checkActive[IndexOf(forwardSteps, leftSteps)] = aValue;
    }

    public bool Check(Grid.Element element)
    {
        if (ThingToCheck == WhatToCheck.AUTOMATON && element.Automaton != null)
        {
            return true;
        }
        if (ThingToCheck == WhatToCheck.FLOOR && element.HasFloor)
        {
            return true;
        }
        if (ThingToCheck == WhatToCheck.WALL && element.IsWall)
        {
            return true;
        }
        if (ThingToCheck == WhatToCheck.HOLE && !element.HasFloor)
        {
            return true;
        }
        return false;
    }

    private static int IndexOf(int forwardSteps, int leftSteps)
    {
        int x = forwardSteps + (gridXForw / 2);
        int y = leftSteps + (gridYLeft / 2);

        if (x < 0 || x > gridXForw) throw new Exception("x = " + x + ", forwardSteps = " + forwardSteps);
        if (y < 0 || y > gridYLeft) throw new Exception("y = " + y + ", leftSteps = " + leftSteps);

        return x * gridYLeft + y;
    }

    internal IEnumerable<Vector2I> GetCheckCoordinates()
    {
        for (int x = 0; x < gridXForw; x++)
        {
            for (int y = 0; y < gridYLeft; y++)
            {
                if (checkActive[x * gridYLeft + y])
                {
                    int forw = x - (gridXForw / 2);
                    int left = y - (gridYLeft / 2);
                    yield return new Vector2I(forw, left);
                }
            }
        }
    }
}
