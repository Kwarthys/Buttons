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

    public void setName(string newName)
    {
        instrumentName = newName;
        title.text = newName;
    }

    abstract public void notifyValueChanged(Interactor interactor);

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

    abstract protected T convertValueFromInteractor(float interactorValue);
}
