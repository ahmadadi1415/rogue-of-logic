using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputPuzzle : MonoBehaviour
{
    [SerializeField] private BooleanSource lastLogicGate;
    [SerializeField] private BooleanSource[] inputSources;
    [SerializeField] private GameObject doorToOpen;
    private DoorController doorController;
    private LineRenderer lineRenderer;
    [SerializeField] private bool _hasPlayer = false;
    private bool HasPlayer
    {
        get
        {
            return _hasPlayer;
        }
        set
        {
            _hasPlayer = value;
        }
    }

    private void Awake()
    {
        doorController = doorToOpen.GetComponent<DoorController>();
        lineRenderer = GetComponent<LineRenderer>();
        HasPlayer = false;
    }

    private void Start()
    {
        lineRenderer.startColor = lastLogicGate.defaultColor;
        lineRenderer.endColor = lastLogicGate.defaultColor;
        ConnectLastLogicGate();
    }

    private void Update()
    {
        if (lastLogicGate.LineDrawnProgress == 0) {
            ConnectLastLogicGate();
        }

        if (ItemManager.GetInstance().IsItemPanelActive)
        {
            return;
        }

        // Open The Door
        if (!lastLogicGate.IsDrawingLine && lastLogicGate.LineDrawnProgress == 100)
        {
            ConnectLastLogicGate();
            lineRenderer.startColor = lastLogicGate.trueColor;
            lineRenderer.endColor = lastLogicGate.trueColor;
            doorController.DoorOpened = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HasPlayer = true;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().outputPuzzle = this;
            // Debug.Log("player in");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HasPlayer = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().outputPuzzle = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            HasPlayer = true;
            // Debug.Log(HasPlayer);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, lastLogicGate.transform.position);
    }

    private void ConnectLastLogicGate()
    {
        lineRenderer.SetPosition(0, lastLogicGate.transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, lastLogicGate.transform.position.y, lastLogicGate.transform.position.z));
        lineRenderer.SetPosition(2, transform.position);
    }

    public bool SolvePuzzle()
    {
        if (!doorController.DoorOpened)
        {
            Debug.Log("Pressed");
            if (!lastLogicGate.BooleanValue)
            {
                return false;
            }

            // animate all logic gate and the source
            foreach (BooleanSource inputSource in inputSources)
            {
                inputSource.IsDrawingLine = true;
            }
            return true;
        }
        else
        {
            return true;
        }
    }
}
