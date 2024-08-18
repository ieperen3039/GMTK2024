
using System;
using Godot;

public partial class InstructionWrapper : Panel
{
    [Export]
    private Label NameLabel;

    public IInstruction Instruction;

    public string DisplayName => NameLabel.Text;
}
