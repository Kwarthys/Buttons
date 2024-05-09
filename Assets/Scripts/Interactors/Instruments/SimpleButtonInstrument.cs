using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButtonInstrument : Instrument<bool>
{
    override protected bool convertValueFromInteractor(float interactorValue)
    {
        return interactorValue == 100.0f;
    }

    override public void notifyValueChanged(Interactor interactor)
    {
        value = convertValueFromInteractor(interactor.getValue());

        setName("Button: " + value.ToString());
    }
}
