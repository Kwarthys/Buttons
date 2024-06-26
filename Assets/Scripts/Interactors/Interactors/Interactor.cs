using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    protected float value = 0f;
    public float getValue() { return value; }

    [SerializeField]
    protected float sensitivity = 0.1f;
    [HideInInspector]
    public BaseInstrument owner;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    virtual protected void init() { }
    virtual public void onInteractionStart(Vector2 mousePosition) {}
    virtual public void onInteractionEnd() {}
    virtual public void onInteractionUpdate(Vector2 mousePosition) {}
    virtual public void syncValue() { }
}
