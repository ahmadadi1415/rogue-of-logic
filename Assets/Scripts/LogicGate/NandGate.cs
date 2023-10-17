using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NandGate : BooleanSource
{
    // Get boolean value from input source or boolean gate
    // if type is buffer and invert, only for one input, but for the other accept two input
    public GameObject sourceRef1, sourceRef2;
    private BooleanSource boolSource1, boolSource2;

    private void Awake() {
        if (sourceRef1 == null) {
            BooleanValue = false;
        }
        else {
            boolSource1 = sourceRef1.GetComponent<BooleanSource>();
            boolSource2 = sourceRef2.GetComponent<BooleanSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boolSource1 == null && boolSource2 == null) {
            return;
        }
        BooleanValue = !(boolSource1.BooleanValue && boolSource2.BooleanValue);
    }
}
