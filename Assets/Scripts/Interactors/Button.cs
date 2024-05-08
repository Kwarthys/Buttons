using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Interactor
{
    private Vector3 savedScale;

    override public void onInteractionStart(Vector2 mousePosition)
    {
        value = 100f;

        savedScale = transform.localScale;
        transform.localScale *= 0.5f;
    }
    override public void onInteractionEnd()
    {
        value = 0f;
        transform.localScale = savedScale;
    }
}
