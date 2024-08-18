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

    private int currentLevel = 0;

    public override void _Ready()
    {
        instructionList = programEditor.GetNode<ProgramList>("%InstructionList");

        levelUi.ToEditorPressed += ToProgramEditorScene;
        levelUi.ResetPlayerPressed += LoadInstructions;
        levelUi.OpenMenuPressed += OpenOptionsMenu;
        programEditor.GetNode<Button>("%StartButton").Pressed += LoadInstructions;
        programEditor.GetNode<Button>("%CancelButton").Pressed += ToLevelScene;
        optionsMenu.GetNode<Button>("%BackButton").Pressed += ToLevelScene;
        level.LevelCompleted += LoadNextLevel;

        LoadNextLevel();
        ToLevelScene();
    }

    private void LoadInstructions()
    {
        
        level.SpawnPlayer(instructionList.GetInstructions());
        ToLevelScene();
    }

    private void LoadNextLevel() 
    {
        currentLevel += 1;
        string levelResString = string.Format("res://assets/levels/level_{}.png", currentLevel);
        GD.Print("Loading next level: ", currentLevel);
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
