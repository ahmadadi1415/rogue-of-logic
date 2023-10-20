using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3 destinationMove;
    private float startTime;
    private float journeyLength;

    public bool _doorOpened = false;

    public bool DoorOpened {
        get {
            return _doorOpened;
        }
        set {
            _doorOpened = value;
        }
    }

    private void Start() {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, destinationMove);
    }

    private void Update() {
        if (DoorOpened) {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, destinationMove, fracJourney);
        }    
    }
}
