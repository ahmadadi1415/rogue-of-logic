using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferGate : BooleanSource
{
    // Get boolean value from input source or boolean gate
    // if type is buffer and invert, only for one input, but for the other accept two input
    public GameObject sourceRef;
    private BooleanSource boolSource;
    private LineRenderer lineRenderer;
    [SerializeField] private float animDuration = 3f;
    private Vector3 wireStartPos, wireMidPos, wireEndPos;
    [SerializeField] private Vector3[] points = new Vector3[4];

    private void Awake() {
        if (sourceRef == null) {
            BooleanValue = false;
        }
        else {
            boolSource = sourceRef.GetComponent<BooleanSource>();
            lineRenderer = GetComponent<LineRenderer>();
            
            wireStartPos = sourceRef.transform.position;
            wireEndPos = transform.position;
            float xmidpos = wireStartPos.x + (wireEndPos.x - wireStartPos.x) * 0.75f;
            // Debug.Log(wireEndPos.x);
            Debug.Log(xmidpos);
            // float ymidpos = (wireEndPos.y + wireStartPos.y) * 3 / 4;
            points[0] = wireStartPos;
            points[1] = new Vector3(xmidpos, wireStartPos.y, wireStartPos.z);
            points[2] = new Vector3(xmidpos, wireEndPos.y, wireEndPos.z);
            points[3] = wireEndPos;
            lineRenderer.positionCount = 4;
            lineRenderer.SetPosition(0, points[0]);
        }
    }

    private void Start()
    {
        if (sourceRef != null)
        {
            StartCoroutine(DrawWireLine());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (boolSource != null)
        {
            BooleanValue = boolSource.BooleanValue;
            return;
        }
    }

    private IEnumerator DrawWireLine()
    {
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

    }
}
