using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanSource : MonoBehaviour
{
    [SerializeField] private bool _booleanValue;

    public bool BooleanValue
    {
        get
        {
            return _booleanValue;
        }
        set
        {
            _booleanValue = value;
        }
    }

    private void Awake() {
        _booleanValue = false;
    }

}
