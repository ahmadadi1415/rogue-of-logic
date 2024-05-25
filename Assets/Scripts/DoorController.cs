using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3 destinationMove;
    [SerializeField] private Vector3 initPosition;
    private float startTime;
    private float journeyLength;

    [SerializeField] private bool isOpening = true;
    public bool _doorOpened = false;

    public bool DoorOpened {
        get {
            return _doorOpened;
        }
        set {
            _doorOpened = value;
            gameObject.SetActive(!value);
        }
    }

    private void Start() {
        startTime = Time.time;
        initPosition = new Vector3(transform.position.x, transform.position.y, 0);
        journeyLength = Vector3.Distance(initPosition, destinationMove);
    }

    private void Update() {

        // if (DoorOpened) {
        //     if (isOpening)
        //     {
        //         if (Vector3.Distance(transform.position, destinationMove) >= 0.01f)
        //         {
        //             float distCovered = (Time.time - startTime) * moveSpeed;
        //             float fracJourney = distCovered / journeyLength;
        //             transform.position = Vector3.Lerp(transform.position, destinationMove, fracJourney);
        //         }
        //         else
        //         {
        //             isOpening = false;
        //         }
        //     }

        //     else {
        //         transform.position = destinationMove;
        //         gameObject.SetActive(false);
        //     }
        // }
    }
}
