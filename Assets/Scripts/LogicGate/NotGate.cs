using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotGate : BooleanSource
{
    // Get boolean value from input source or boolean gate
    // if type is buffer and invert, only for one input, but for the other accept two input
    public GameObject sourceRef;
    private BooleanSource boolSource;

    new void Awake()
    {
        base.Awake();
        boolSource = sourceRef.GetComponent<BooleanSource>();
        IsDrawingLine = false;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
        BooleanValue = !boolSource.BooleanValue;

    }
}
