using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrGate : BooleanSource
{
    // Get boolean value from input source or boolean gate
    // if type is buffer and invert, only for one input, but for the other accept two input
    public GameObject sourceRef1, sourceRef2;
    private BooleanSource boolSource1, boolSource2;


    new void Awake()
    {
        base.Awake();
        boolSource1 = sourceRef1.GetComponent<BooleanSource>();
        boolSource2 = sourceRef2.GetComponent<BooleanSource>();
        IsDrawingLine = false;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        BooleanValue = boolSource1.BooleanValue || boolSource2.BooleanValue;
    }
}
