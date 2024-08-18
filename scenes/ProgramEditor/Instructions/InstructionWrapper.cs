
using System;
using Godot;

public abstract partial class InstructionWrapper : Panel
{
    [Export]
    private Label NameLabel;

    public abstract IInstruction GetInstruction();

    public string DisplayName => NameLabel.Text;
}
