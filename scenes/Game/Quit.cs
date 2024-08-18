using Godot;
using System;

public partial class Quit : Button
{
    private void _on_pressed()
    {
        GetTree().Quit();
    }
}


