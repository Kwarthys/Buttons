using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactor
{
    private Vector3 savedScale;

    [SerializeField]
    private bool momentary = true;
    [SerializeField]
    private float pressedScale = 0.9f;

    override public void onInteractionStart(Vector2 mousePosition)
    {
        if(momentary)
            value = 100f;
        else
            toggle();

        updateScale();
        owner.notifyValueChanged(this);
    }
    override public void onInteractionEnd()
    {
        if(!momentary)
            return;

        value = 0f;
        updateScale();
        owner.notifyValueChanged(this);
    }

    private void updateScale()
    {
        transform.localScale = savedScale * (1.0f - ( 1.0f - pressedScale) * value * 0.01f);
    }

    override protected void init() { savedScale = transform.localScale; }

    private void toggle() { value = 100f - value; }
}
