using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInstrument : MonoBehaviour
{
    [SerializeField]
    protected string instrumentName;
    [SerializeField]
    private TMPro.TextMeshPro title;

    [SerializeField]
    protected Interactor[] interactors = null;

    public string getName() { return instrumentName; }
    public void setDisplay(string display) { title.text = display; }
    abstract public void notifyValueChanged(Interactor interactor);
    abstract public void notifyInteractionEnded(Interactor interactor);
    abstract public string getPrompt();
    abstract public string generateNewTask();
    virtual public float getClosestValidValue(float value) { return value; }

    virtual protected void init() { }

    private void Start()
    {
        foreach(Interactor i in interactors)
        {
            i.owner = this;
        }
        init();
    }
}

public abstract class Instrument<T> : BaseInstrument
{
    [SerializeField]
    protected T valueMin;
    [SerializeField]
    protected T valueMax;
    [SerializeField]
    protected T value;

    protected Task<T> task;

    public T getValue() { return value; }
    override public string getPrompt()
    {
        if(task == null)
            return "";
        return task.prompt;
    }

    protected virtual void checkTask()
    {
        if(task == null)
            return;

        if(EqualityComparer<T>.Default.Equals(value, task.targetValue))
        {
            TaskManager.instance.notifyTaskComplete(this);
            task = null;
        }
    }

    override public void notifyInteractionEnded(Interactor interactor)
    {
        checkTask();
    }
}
