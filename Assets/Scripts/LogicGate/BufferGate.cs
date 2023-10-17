using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferGate : BooleanSource
{
    // Get boolean value from input source or boolean gate
    // if type is buffer and invert, only for one input, but for the other accept two input
    public GameObject sourceRef;
    private BooleanSource boolSource;

    private void Awake() {
        if (sourceRef == null) {
            BooleanValue = false;
        }
        else {
            boolSource = sourceRef.GetComponent<BooleanSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boolSource != null) {
            BooleanValue = boolSource.BooleanValue;
            return;
        }
    }
}
