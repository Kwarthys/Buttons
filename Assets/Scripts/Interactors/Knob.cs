using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knob : Interactor
{
    private Vector3 lastMousePos = Vector3.zero;
    override public void onInteractionStart(Vector2 mousePosition) { lastMousePos = mousePosition; }

    override public void onInteractionUpdate(Vector2 mousePosition)
    {
        value = Mathf.Clamp(value + (mousePosition.x - lastMousePos.x) * sensitivity, 0f, 100f);
        lastMousePos = mousePosition;

        Vector3 rot = transform.localRotation.eulerAngles;
        rot.z = value * -1.8f;
        transform.localRotation = Quaternion.Euler(rot);
    }
}
