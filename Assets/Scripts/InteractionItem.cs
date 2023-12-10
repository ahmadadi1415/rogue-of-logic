using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{
    // Interaction with Item from Player
    [Header("Detection Parameters")]
    public GameObject interactedItem;

    [Header("Others")]
    private bool objectDetected = false;

    public bool anyObjectDetected
    {
        get
        {
            return objectDetected;
        }
        private set
        {
            objectDetected = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            // Debug.Log("This is an item");
            interactedItem = other.gameObject;
            anyObjectDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            interactedItem = null;
            anyObjectDetected = false;
        }
    }

}