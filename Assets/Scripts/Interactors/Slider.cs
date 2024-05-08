using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : Interactor
{
    public enum Orientation { Vertical, Horizontal}

    public float range = 1.0f;

    [SerializeField]
    private Orientation orientation = Orientation.Vertical;

    override public void onInteractionUpdate(Vector2 mousePosition)
    {
        float axisValue = isVertical() ? mousePosition.y - transform.parent.position.y : mousePosition.x - transform.parent.position.x;

        value = Mathf.Clamp(Mathf.InverseLerp(-range * 0.5f, range * 0.5f, axisValue), -1f, 1f);
        value *= 100f;

        transform.localPosition = getValueFromPos();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 vrange;
        if(isVertical())
            vrange = new Vector3(0, range * 0.5f, 0);
        else
            vrange = new Vector3(range * 0.5f, 0, 0);

        Gizmos.DrawLine(transform.position - vrange, transform.position + vrange);
    }

    private Vector3 getValueFromPos()
    {
        if(isVertical())
            return new Vector3(0f, value * range * 0.01f - range * 0.5f, 0f);
        else
            return new Vector3(value * range * 0.01f - range * 0.5f, 0f, 0f);
    }

    private bool isVertical() { return orientation == Orientation.Vertical; }
}
