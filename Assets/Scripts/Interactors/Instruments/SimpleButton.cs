using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : Instrument<bool>
{
    override public void notifyValueChanged(Interactor interactor)
    {
        value = interactor.getValue() == 1f;
        setDisplay(instrumentName + ": " + value.ToString());
    }
}
