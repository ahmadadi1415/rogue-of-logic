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

    
    [SerializeField] private int _lineDrawnProgress = 100;
    public int LineDrawnProgress {
        get {
            return _lineDrawnProgress;
        }
        set {
            _lineDrawnProgress = value;
        }
    }

    [SerializeField] private bool _isDrawingLine = false;
    public bool IsDrawingLine {
        get {
            return _isDrawingLine;
        }
        set {
            _isDrawingLine = value;
        }
    }
}
