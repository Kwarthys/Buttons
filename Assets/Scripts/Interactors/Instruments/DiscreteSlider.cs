using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteSlider : Instrument<int>
{
    public override void notifyValueChanged(Interactor interactor)
    {
        value = Mathf.RoundToInt(Mathf.Lerp(valueMin, valueMax, interactor.getValue()));
        setDisplay(instrumentName + ": " + value.ToString());
    }

    override public float getClosestValidValue(float _value)
    {
        return Mathf.InverseLerp(valueMin, valueMax, value);
    }
    override public string generateNewTask()
    {
        task = new Task<int>();

        int targetValue = value;
        while(targetValue == value)
        {
            targetValue = Mathf.RoundToInt(Random.value * (valueMax - valueMin)) + valueMin;
        }
        task.targetValue = targetValue;
        string prompt = "Set " + instrumentName + " to " + task.targetValue;
        task.prompt = prompt;
        return prompt;
    }
}
