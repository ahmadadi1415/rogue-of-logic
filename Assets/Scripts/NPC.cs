using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float walkSpeed = 3f;
    public List<Vector3> waypoints;
    public float waypointReachedDistance = 0.01f;
    Rigidbody2D rigidbody;
    Animator animator;
    Vector3 nextWaypoint;
    int waypointNum;

    private bool _isInteracting = false;
    public bool IsInteracting { 
        get {
            return _isInteracting;
        } 
        private set {
            _isInteracting = value;
            
            // Switch the animation from walk to idle when interacting with player
            animator.SetBool(AnimationStrings.isInteracting, value);
        } 
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        CanMove = (waypoints.Count >= 2) ? true: false;
    }

    private void FixedUpdate()
    {
        // if this npc is interacting with the player, make it not moving
        if (!IsInteracting && waypoints.Count > 0) {

            // if the npc is not interacting and it has waypoints to be reached, move the npc
            Move();
        }

        else {
            rigidbody.velocity = Vector2.zero;
        }
    }

    private bool _canMove = false;
    public bool CanMove {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
        set {
            _canMove = value;
        }
    }

    private void Move()
    {
        // Move to the next point
        Vector2 directionToWaypoint = (nextWaypoint - transform.position).normalized;

        // Check if the waypoint already reached
        float distance = Vector2.Distance(nextWaypoint, transform.position);
        rigidbody.velocity = directionToWaypoint * walkSpeed;
        UpdateDirection();

        // See if its need to change the waypoint
        if (distance <= waypointReachedDistance)
        {

            // Switch to the next waypoint
            waypointNum++;

            if (waypointNum >= waypoints.Count)
            {
                // Loop back to the index 0
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if ((locScale.x > 0 && rigidbody.velocity.x < 0) || (locScale.x < 0 && rigidbody.velocity.x > 0)) {
            transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
        }
    }
}
