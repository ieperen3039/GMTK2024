using Godot;
using System;

public partial class Game : Node2D
{

    public void ToggleProgramEditor()
    {
        GetNode<Control>("MenuUI/ProgramEditor").Visible = !GetNode<Control>("MenuUI/ProgramEditor").Visible;        
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Control programEditor = GetNode<Control>("MenuUI/ProgramEditor");
        programEditor.Visible = false;

        programEditor.GetNode<Button>("%CancelButton").Pressed += ToggleProgramEditor;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_program_editor")) {
            ToggleProgramEditor();
        }
    }
}
