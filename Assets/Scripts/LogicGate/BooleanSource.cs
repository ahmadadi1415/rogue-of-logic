using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BooleanSource : MonoBehaviour
{
    [SerializeField] private bool _booleanValue;
    protected LineRenderer lineRenderer;
    [SerializeField] public Color trueColor;
    [SerializeField] public Color falseColor;
    [SerializeField] public Color defaultColor = Color.gray;

    protected float animDuration = 1.5f;
    protected Vector3[] points = new Vector3[4];
    [SerializeField] private BooleanSource nextGate;
    [SerializeField] private bool isLineGoingVertical;

    private Light2D light;
    [SerializeField] private bool puzzleSolved = false;

    [SerializeField] private bool isGate = true;

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

    protected void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        light = gameObject.GetComponent<Light2D>();
    }

    [SerializeField] private int _lineDrawnProgress = 0;
    public int LineDrawnProgress
    {
        get
        {
            return _lineDrawnProgress;
        }
        set
        {
            _lineDrawnProgress = value;
        }
    }

    [SerializeField] public bool _isDrawingLine = true;
    public bool IsDrawingLine
    {
        get
        {
            return _isDrawingLine;
        }
        set
        {
            _isDrawingLine = value;
        }
    }
    
    private void Start()
    {
        if (nextGate != null)
        {                        
            SetupPoints();
            lineRenderer.startColor = defaultColor;
            lineRenderer.endColor = defaultColor;
            lineRenderer.enabled = true;
        }

        if (isGate)
        {
            light.intensity = BooleanValue ? 0.6f : 0.2f;
        }
    }

    protected void Update() {
        // if (puzzleSolved) {
        //     ChangeLineColor();
        // }
        ChangeLineColor();

        if (isGate) {
            light.intensity = BooleanValue ? 0.6f : 0.2f;
        }

        if (LineDrawnProgress == 0)
        {
            if (!IsDrawingLine)
            {
                if (nextGate == null) {
                    return;
                }
                SetupPoints();
                lineRenderer.SetPositions(points);
            }

            else
            {
                ChangeLineColor();
                lineRenderer.enabled = true;
                puzzleSolved = true;
                StartCoroutine(DrawWireLine());
            }
        }

        if (nextGate != null && !IsDrawingLine && LineDrawnProgress == 100) {
            SetupPoints();
            lineRenderer.SetPositions(points);
        }
    }
    
    // TODO: Setup Points will be called in start in all logic gate
    public void SetupPoints()
    {
        Vector3 wireStartPos = transform.position;
        Vector3 wireEndPos = nextGate.transform.position;
        SetLineDirection(wireStartPos, wireEndPos);

        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, points[0]);
    }

    // TODO: DrawWireLine will be called if the puzzle is ready by player or the previous line already drawn
    public IEnumerator DrawWireLine()
    {
        LineDrawnProgress = 1;
        float segmentDuration = animDuration / lineRenderer.positionCount;

        for (int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            float startTime = Time.time;

            Vector3 startPos = points[i];
            Vector3 endPos = points[i + 1];

            Vector3 pos = startPos;
            while (pos != endPos)
            {
                float t = (Time.time - startTime) / segmentDuration;
                pos = Vector3.Lerp(startPos, endPos, t);

                for (int j = i + 1; j < lineRenderer.positionCount; j++)
                {
                    lineRenderer.SetPosition(j, pos);
                }
                yield return null;
            }
        }

        if (nextGate != null)
        {
            nextGate.IsDrawingLine = true;
            nextGate.lineRenderer.enabled = true;
        }
        LineDrawnProgress = 100;
        IsDrawingLine = false;
        
    }

    private void OnDrawGizmos() {
        if (nextGate != null)
        {
            Gizmos.DrawLine(transform.position, nextGate.transform.position);
        }
    }

    private void SetLineDirection(Vector3 wireStartPos, Vector3 wireEndPos)
    {
        if (isLineGoingVertical)
        {
            float yMidPos = wireStartPos.y + (wireEndPos.y - wireStartPos.y);
            points[0] = wireStartPos;
            points[1] = new Vector3(wireStartPos.x, yMidPos, wireStartPos.z);
            points[2] = new Vector3(wireEndPos.x, yMidPos, wireEndPos.z);
            points[3] = wireEndPos;
        }

        else {
            float xMidPos = wireStartPos.x + (wireEndPos.x - wireStartPos.x) * 0.75f;
            points[0] = wireStartPos;
            points[1] = new Vector3(xMidPos, wireStartPos.y, wireStartPos.z);
            points[2] = new Vector3(xMidPos, wireEndPos.y, wireEndPos.z);
            points[3] = wireEndPos;
        }
    }

    public void ChangeLineColor()
    {
        Color lineColor = BooleanValue ? trueColor : falseColor;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }
}
