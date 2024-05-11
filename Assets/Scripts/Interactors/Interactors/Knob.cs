using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : Interactor
{
    private Vector3 lastMousePos = Vector3.zero;
    override public void onInteractionStart(Vector2 mousePosition) { lastMousePos = mousePosition; }

    [SerializeField]
    private float angleRange = 180f;

    override public void onInteractionUpdate(Vector2 mousePosition)
    {
        value = Mathf.Clamp(value + (mousePosition.x - lastMousePos.x) * sensitivity, 0f, 1f);
        lastMousePos = mousePosition;

        owner.notifyValueChanged(this);
        updateAnimation();
    }

    public override void onInteractionEnd()
    {
        value = owner.getClosestValidValue(value);
        owner.notifyInteractionEnded(this);
        updateAnimation();
    }

    private void updateAnimation()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rot.z = Mathf.Lerp(angleRange * 0.5f, - angleRange * 0.5f, value) - 90f;
        transform.localRotation = Quaternion.Euler(rot);
    }
}
