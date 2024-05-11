using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSlider : Instrument<int>
{
    [SerializeField]
    int valueRange = 20;

    public override string generateNewTask()
    {
        task = new Task<int>();

        int halfRange = Mathf.FloorToInt(valueRange * 0.5f);

        int targetValue = value;
        while(isValueInRange(targetValue))
        {
            targetValue = Mathf.RoundToInt(Mathf.Lerp(valueMin + halfRange, valueMax - halfRange, Random.value));
        }
        task.targetValue = targetValue;
        string prompt = "Set " + instrumentName + " between " + (task.targetValue - halfRange) + " and " + (task.targetValue + halfRange);
        task.prompt = prompt;
        return prompt;
    }

    public override void notifyValueChanged(Interactor interactor)
    {
        value = Mathf.RoundToInt(Mathf.Lerp(valueMin, valueMax, interactor.getValue()));
        setDisplay(instrumentName + ": " + value.ToString());
    }

    protected override void checkTask()
    {
        if(task == null)
            return;

        float halfRange = valueRange * 0.5f;
        if(isValueInRange(task.targetValue))
        {
            TaskManager.instance.notifyTaskComplete(this);
            task = null;
        }
    }

    protected bool isValueInRange(int center)
    {
        float halfRange = valueRange * 0.5f;
        return value > center - halfRange && value < center + halfRange;
    }
}
