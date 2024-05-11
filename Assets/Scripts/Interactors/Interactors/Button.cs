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

    [SerializeField]
    private Gradient colors;
    private SpriteRenderer buttonRenderer;

    override public void onInteractionStart(Vector2 mousePosition)
    {
        if(momentary)
            value = 1f;
        else
            toggle();

        updateAnimation();
        owner.notifyValueChanged(this);
        owner.notifyInteractionEnded(this);
    }
    override public void onInteractionEnd()
    {
        if(momentary)
        {
            value = 0f;
            updateAnimation();
            owner.notifyValueChanged(this);
        }

        owner.notifyInteractionEnded(this);
    }

    private void updateAnimation()
    {
        updateScale();
        updateColor();
    }

    private void updateColor()
    {
        buttonRenderer.color = colors.Evaluate(value);
    }

    private void updateScale()
    {
        transform.localScale = savedScale * (1.0f - ( 1.0f - pressedScale) * value);
    }

    override protected void init()
    {
        savedScale = transform.localScale;
        buttonRenderer = GetComponent<SpriteRenderer>();
        updateColor();
    }

    private void toggle() { value = 1f - value; }
}
