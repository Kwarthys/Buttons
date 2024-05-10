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

    public T getValue() { return value; }
}
