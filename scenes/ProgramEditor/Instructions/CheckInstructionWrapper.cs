using Godot;
using System;

public partial class CheckInstructionWrapper : JumpInstructionWrapper
{
    [Export]
    private OptionButton whatToCheckSelector;

    public override void _Ready()
    {
        foreach (CheckInstruction.WhatToCheck value in Enum.GetValues(typeof(CheckInstruction.WhatToCheck)))
        {
           whatToCheckSelector.AddItem(value.ToString()); 
        }
        whatToCheckSelector.Select(0);
        whatToCheckSelector.ItemSelected += OnWhatToCheckChange;
    }

    private void OnToggle(bool aSetActive, long forwardSteps, long leftSteps)
    {
        if (Instruction is CheckInstruction checkInstruction)
        {
            checkInstruction.SetActive((int)forwardSteps, (int)leftSteps, aSetActive);
        }
    }

    private void OnWhatToCheckChange(long index)
    {
        if (Instruction is CheckInstruction checkInstruction)
        {
            checkInstruction.ThingToCheck = (CheckInstruction.WhatToCheck)index;
        }
    }
}
