using System.Collections;
using System.Collections.Generic;
using Godot;

public class Grid : IEnumerable<Grid.Element>
{
    public Element[] elements;

    private int xSize;
    private int ySize;

    public Grid(int xSize, int ySize)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        elements = new Element[xSize * ySize];
    }

    public Element GetElement(Vector2I coordinate) => GetElement(coordinate.X, coordinate.Y);
    public Element GetElement(int x, int y) => elements[x * xSize + y];

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<Element> GetEnumerator()
    {
        foreach (Element e in elements) { 
            yield return e; 
        }
    }

    public void SetElement(Vector2I coordinate, Element element) => SetElement(coordinate.X, coordinate.Y, element);
    public void SetElement(int x, int y, Element element)
    {
        elements[x * xSize + y] = element;
    }


    public class Element
    {
        public bool HasFloor = true;
        public bool IsWall = false;
        public bool IsGoal = false;
        public Automaton Automaton = null;
        public int numReservations = 0;

        public Element() { }

        public Element(Element element)
        {
            HasFloor = element.HasFloor;
        }
    }
}
