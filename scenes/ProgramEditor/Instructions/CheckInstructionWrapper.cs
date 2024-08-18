using Godot;
using System;

public partial class CheckInstructionWrapper : InstructionWrapper
{
    [Export]
    private OptionButton whatToCheckSelector;

    private CheckInstruction instruction = new();

    public override IInstruction GetInstruction() => instruction;

    public override void _Ready()
    {
        foreach (CheckInstruction.WhatToCheck value in Enum.GetValues(typeof(CheckInstruction.WhatToCheck)))
        {
           whatToCheckSelector.AddItem(value.ToString()); 
        }
        whatToCheckSelector.Select(0);
        whatToCheckSelector.ItemSelected += OnWhatToCheckChange;
    }

    private void OnJumpTargetChange(double value)
    {
        instruction.TargetId = (int)value;
    }

    private void OnToggle(bool aSetActive, long forwardSteps, long leftSteps)
    {
        instruction.SetActive((int)forwardSteps, (int)leftSteps, aSetActive);
    }

    private void OnWhatToCheckChange(long index)
    {
        instruction.ThingToCheck = (CheckInstruction.WhatToCheck)index;
    }
}
