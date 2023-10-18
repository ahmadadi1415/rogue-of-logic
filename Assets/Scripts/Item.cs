using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{

   public enum InteractionType { NONE, PickUp, Examine }
   public InteractionType type;
   private Vector3 firstPosition;
   private void Awake() {
      firstPosition = transform.position;
   }

   //Collider Trigger
   //Interaction Type
   // private void Reset()
   // {
   //    GetComponent<Collider2D>().isTrigger = true;
   //    gameObject.layer = 8;
   // }

   public void Interact()
   {
      switch (type)
      {
         case InteractionType.PickUp:
            FindObjectOfType<InteractionItem>().PickUpItem(gameObject);
            break;
         case InteractionType.Examine:
            ItemManager.GetInstance().ShowInteractionPanel();
            Debug.Log("EXAMINE");
            break;
         default:
            Debug.Log("NULL ITEM");
            break;
      }
   }

   public void ResetPosition() {
      transform.position = firstPosition;
   }

}