using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node2D
{
    [Export]
    private Control programEditor;
    [Export]
    private LevelUi levelUi;
    [Export]
    private World level;
    [Export]
    private Control optionsMenu;

    private ProgramList instructionList;

    public override void _Ready()
    {
        instructionList = programEditor.GetNode<ProgramList>("%InstructionList");

        levelUi.ToEditorPressed += ToProgramEditorScene;
        levelUi.OpenMenuPressed += OpenOptionsMenu;
        programEditor.GetNode<Button>("%StartButton").Pressed += LoadInstructions;
        programEditor.GetNode<Button>("%CancelButton").Pressed += ToLevelScene;
        optionsMenu.GetNode<Button>("%BackButton").Pressed += ToLevelScene;

        ToLevelScene();
    }

    private void LoadInstructions()
    {
        level.SpawnPlayer(instructionList.GetInstructions());
        ToLevelScene();
    }


    public void OpenOptionsMenu()
    {
        programEditor.Visible = false;
        levelUi.Visible = true;
        level.SetActive(true);
        optionsMenu.Visible = true;
    }

    public void ToProgramEditorScene()
    {
        programEditor.Visible = true;
        levelUi.Visible = false;
        level.SetActive(false);
        optionsMenu.Visible = false;
    }

    public void ToLevelScene()
    {
        programEditor.Visible = false;
        optionsMenu.Visible = false;
        levelUi.Visible = true;
        level.SetActive(true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("toggle_program_editor"))
        {
            ToProgramEditorScene();
        }
    }
}
