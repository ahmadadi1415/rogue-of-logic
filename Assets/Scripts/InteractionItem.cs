using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionItem : MonoBehaviour
{
    [Header("Detection Parameters")]
    // public Transform detectionPoint;
    // private const float detectionRadius = 0.2f;
    // public LayerMask detectionLayer;
    public GameObject detectedObject;

    [Header("Others")]
    public List<GameObject> pickedItems = new List<GameObject>();
    private bool objectDetected = false;

    // void Update()
    // {
    //     if (anyObjectDetected && InputManager.GetInstance().GetInteractPressed())
    //     {
    //         GameObject.FindWithTag("Player").GetComponent<Animator>().SetTrigger(AnimationStrings.isInteracting);
    //         detectedObject.GetComponent<Item>().Interact();
    //     }
    // }

    // bool InteractInput()
    // {
    //     return Input.GetKeyDown(KeyCode.E);
    // }

    // bool DetectObject()
    // {


    //     Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
    //     if (obj == null)
    //     {
    //         detectedObject = null;
    //         return false;
    //     }
    //     else
    //     {
    //         detectedObject = obj.gameObject;
    //         return true;
    //     }

    // }

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
            detectedObject = other.gameObject;
            anyObjectDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            detectedObject = null;
            anyObjectDetected = false;
        }
    }

    public void PickUpItem(GameObject item)
    {
        pickedItems.Add(item);
    }

}