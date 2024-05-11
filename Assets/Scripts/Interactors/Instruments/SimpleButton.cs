using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : Instrument<bool>
{
    override public void notifyValueChanged(Interactor interactor)
    {
        value = interactor.getValue() == 1f;
        setDisplay(instrumentName);
    }

    override public string generateNewTask()
    {
        task = new Task<bool>();
        task.targetValue = !value;
        string prompt = (value ? "Deactivate" : "Activate") + " " + instrumentName;
        task.prompt = prompt;
        return prompt;
    }
}
