using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
   public ContactFilter2D castFilter;
   public float groundDistance = 0.05f;
   public float wallCheckDistance = 0.2f;
   public float ceilingCheckDistance = 0.05f;
   private float itemSpeed = 2f;
   private BoxCollider2D touchingColl;
   private Rigidbody2D rigidBody;
   RaycastHit2D[] groundHits = new RaycastHit2D[5];

   public enum InteractionType { NONE, PickUp, Examine }
   public InteractionType type;
   private Vector3 firstPosition;
   [SerializeField] private int waypointNum;
   [SerializeField] public Vector3 nextWaypoint;
   [SerializeField] private List<Vector3> trajectory;
   [SerializeField] private float distance = 0f;

   private KeyHintSetter uiHintSetter;
   private void Awake()
   {
      touchingColl = GetComponent<BoxCollider2D>();
      rigidBody = GetComponent<Rigidbody2D>();
      trajectory = new List<Vector3>();
      uiHintSetter = FindObjectOfType<KeyHintSetter>();
   }

   private void Start()
   {
      firstPosition = transform.localPosition;
      trajectory.Add(firstPosition);
      nextWaypoint = firstPosition;
      waypointNum = 0;
   }

   private bool _isGrounded;

   public bool IsGrounded
   {
      get
      {
         return _isGrounded;
      }
      set
      {
         _isGrounded = value;
      }
   }

   [SerializeField] private bool _isReturning = false;

   public bool IsReturning
   {
      get
      {
         return _isReturning;
      }

      set
      {
         _isReturning = value;
      }
   }

   // public void Interact()
   // {
   //    switch (type)
   //    {
   //       case InteractionType.PickUp:
   //          // FindObjectOfType<InteractionItem>().PickUpItem(gameObject);
   //          break;
   //       case InteractionType.Examine:
   //          ItemManager.GetInstance().ShowInteractionPanel();
   //          FindObjectOfType<LevelInformation>().UpdateInformation("The box, following its previous path, returns to its initial position.");
   //          Debug.Log("EXAMINE");
   //          break;
   //       default:
   //          Debug.Log("NULL ITEM");
   //          break;
   //    }
   // }

   public void ResetPosition()
   {
      // transform.position = firstPosition;
      // waypointNum = trajectory.Count - 1;
      // nextWaypoint = trajectory[waypointNum];
      rigidBody.isKinematic = true;
      IsReturning = true;
   }

   private void FixedUpdate()
   {
      IsGrounded = touchingColl.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
      if (IsReturning)
      {
         Move();

         return;
      }

      if (!IsGrounded) return;


      Vector3 position = transform.localPosition;
      SaveItemTrajectory(position);
   }

   private void SaveItemTrajectory(Vector3 position)
   {

      if (IsGrounded)
      {
         bool isPosDifferent = Mathf.Ceil(position.x) != Mathf.Ceil(trajectory[^1].x) && Mathf.Ceil(position.y) != Mathf.Ceil(trajectory[^1].y);
         if (isPosDifferent)
         {
            trajectory.Add(new Vector3(position.x, trajectory[^1].y, trajectory[^1].z));

            if (trajectory[0].x != trajectory[1].x)
            {
               trajectory[0] = new Vector3(trajectory[1].x, trajectory[0].y, trajectory[0].z);
            }
            trajectory.Add(position);
            nextWaypoint = trajectory[^1];
            waypointNum = trajectory.Count - 1;
         }
      }
   }

   private void Move()
   {
      // Move to the next point
      Vector2 directionToWaypoint = (nextWaypoint - transform.localPosition).normalized;

      // Check if the waypoint already reached
      float distance = Vector2.Distance(nextWaypoint, transform.localPosition);
      this.distance = distance;

      rigidBody.velocity = directionToWaypoint * itemSpeed;
      //   rigidBody.MovePosition(directionToWaypoint * 2);

      // See if its need to change the waypoint
      if (waypointNum < trajectory.Count && waypointNum >= 0 && distance <= 0.05f)
      {
         // Switch to the next waypoint

         if (waypointNum <= 1)
         {
            rigidBody.isKinematic = false;
            rigidBody.velocity = Vector2.zero;
            trajectory.Clear();
            trajectory.Add(firstPosition);
            nextWaypoint = firstPosition;
            waypointNum = 0;
            IsReturning = false;
            return;
         }
         else
         {
            waypointNum--;
            nextWaypoint = trajectory[waypointNum];
         }

      }
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Player") && !IsReturning && !gameObject.GetComponentInChildren<BoxDetection>().playerAbove)
      {
         uiHintSetter.ShowHints("[ SPACE ] to interact");
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Player"))
      {
         uiHintSetter.HideHints();
      }
   }
}