using Godot;
using System;

public partial class LevelUi : Control
{
    [Signal]
    public delegate void ToEditorPressedEventHandler();
    [Signal]
    public delegate void OpenMenuPressedEventHandler();
    [Signal]
    public delegate void ResetPlayerPressedEventHandler();

    private void OnToEditorPressed() => EmitSignal(SignalName.ToEditorPressed);
    private void OnResetPlayerPressed() => EmitSignal(SignalName.ResetPlayerPressed);
    private void OnOpenMenuPressed() => EmitSignal(SignalName.OpenMenuPressed);
}
