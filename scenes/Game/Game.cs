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

    private int currentLevel = 1;

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

        // LoadNextLevel();
        ToLevelScene();
    }

    private void LoadInstructions()
    {
        
        level.SpawnPlayer(instructionList.GetInstructions());
        ToLevelScene();
    }

    private void LoadNextLevel() 
    {
        GD.Print("  >>> Load next level function");
        currentLevel += 1;

        if (currentLevel > 3){
            GetTree().ChangeSceneToFile("res://scenes/Game/game_finish.tscn");
        }

        // string levelResString = string.Format("res://assets/levels/level_{0}.png", currentLevel);
        // GD.Print("  >>> Loading next level: ", levelResString);
        level.SetLevelNumber(currentLevel);
        level.ReloadWorld();
        LoadInstructions();
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
