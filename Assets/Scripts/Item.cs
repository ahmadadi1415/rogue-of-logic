using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Item : MonoBehaviour
{
   public ContactFilter2D castFilter;
   public float groundDistance = 0.05f;
   public float wallCheckDistance = 0.2f;
   public float ceilingCheckDistance = 0.05f;
   public float itemSpeed = 1.5f;
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

    private void Awake() {
      touchingColl = GetComponent<BoxCollider2D>();
      rigidBody = GetComponent<Rigidbody2D>();
      firstPosition = transform.position;
      trajectory = new List<Vector3>();
   }

   private void Start() {
      trajectory.Add(firstPosition);
      nextWaypoint = firstPosition;
      waypointNum = 0;
   }

   private bool _isGrounded;

    public bool IsGrounded {
      get {
         return _isGrounded;
      }
      set {
         _isGrounded = value;
      }
   }

   [SerializeField] private bool _isReturning = false;

   public bool IsReturning {
      get {
         return _isReturning;
      }

      set {
         _isReturning = value;
      }
   }

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
      // transform.position = firstPosition;
      // waypointNum = trajectory.Count - 1;
      // nextWaypoint = trajectory[waypointNum];
      rigidBody.isKinematic = true;
      IsReturning = true;
   }

   private void FixedUpdate() {
      IsGrounded = touchingColl.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
      if (IsReturning) {
         Move();
         
         return;
      }

      if (!IsGrounded) return;


      Vector3 position = transform.position;
      SaveItemTrajectory(position);
   }

   private void SaveItemTrajectory(Vector3 position) {

      if (IsGrounded) {

         bool isPosDifferent = Mathf.Ceil(position.x) != Mathf.Ceil(trajectory[^1].x) && Mathf.Ceil(position.y) != Mathf.Ceil(trajectory[^1].y);
         if (isPosDifferent)
         {
            trajectory.Add(new Vector3(position.x, trajectory[^1].y, trajectory[^1].z));
            trajectory.Add(position);
            nextWaypoint = trajectory[^1];
            waypointNum = trajectory.Count - 1;
         }
         
         // // If the x position really different
         // if (isPosDifferent && Mathf.Ceil(position.x) != Mathf.Ceil(trajectory[^1].x))
         // {
         //    trajectory.Add(new Vector3(position.x, trajectory[^1].y, trajectory[^1].z));
         //    trajectory.Add(position);
         // }

         // if (isPosDifferent && Mathf.Ceil(position.y) != Mathf.Ceil(trajectory[^1].y))
         // {
         //    trajectory.Add(new Vector3(trajectory[^1].x, position.y, trajectory[^1].z));
         //    trajectory.Add(position);
         // }

      }
   }

   private void Move()
   {
      // Move to the next point
      Vector2 directionToWaypoint = (nextWaypoint - transform.position).normalized;

      // Check if the waypoint already reached
      float distance = Vector2.Distance(nextWaypoint, transform.position);
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
}