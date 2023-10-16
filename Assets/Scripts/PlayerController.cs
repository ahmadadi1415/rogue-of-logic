using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 2.5f;
    public float jumpImpulse = 6f;


    Vector2 moveInput;
    Rigidbody2D rigidBody;
    Animator animator;
    InteractionItem interactionItem;
    TouchingDirections touchingDirections;

    [SerializeField]
    public float CurrentMovSpeed
    {
        get
        {
            // If the player is moving
            if (IsMoving)
            {
                if (IsRunning)
                {

                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }

            // if the player is not moving, idle
            else
            {
                return 0;
            }

        }
    }
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.horizontalMoving, moveInput.x != 0);
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool isInteracting = false;

    private bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        interactionItem = GetComponent<InteractionItem>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Start() {
        transform.position = SceneController.instance.GetPositionAfter();
        SetFacingDirection(SceneController.instance.GetFacingDirection());
    }

    private void FixedUpdate()
    {
        moveInput = InputManager.GetInstance().GetMoveDirection();
        if (moveInput.y > 0 && touchingDirections.IsGrounded) {
            Debug.Log("Jump");
            animator.SetTrigger(AnimationStrings.jump);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpImpulse);
        }

        if (!DialogueManager.GetInstance().DialogueIsPlaying)
        {
            rigidBody.velocity = new Vector2(moveInput.x * CurrentMovSpeed, rigidBody.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rigidBody.velocity.y);
    }


    private void Update()
    {
        if (IsTalking()) return;
        if (IsInteracting()) return;

        SetFacingDirection(moveInput);
        IsMoving = moveInput != Vector2.zero;
        IsRunning = InputManager.GetInstance().getRunPressed();
    }


    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     moveInput = context.ReadValue<Vector2>();

    //     SetFacingDirection(moveInput);
    //     IsMoving = moveInput != Vector2.zero;
    // }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // if the current move input is positive and the player is not facing right, face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // if the current move input is negative and the player is not facing left, face the left
            IsFacingRight = false;
        }
    }

    public void NotMoving()
    {
        rigidBody.velocity = Vector2.zero;
        IsMoving = false;
        IsRunning = false;
    }

    private bool IsInteracting()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("player_interact"))
        {
            // Debug.Log("interact play");
            NotMoving();
            if (!!interactionItem.detectedObject && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            {
                interactionItem.detectedObject.GetComponent<Item>().Interact();
            }
            return true;
        }

        if (interactionItem.anyObjectDetected && InputManager.GetInstance().GetInteractPressed())
        {
            NotMoving();
            float objectPosition = interactionItem.detectedObject.transform.position.x;
            float playerPosition = transform.position.x;
            SetFacingDirection((objectPosition < playerPosition) ? Vector2.left : Vector2.right);
            animator.SetTrigger(AnimationStrings.isInteracting);
            return true;
        }

        return false;
    }

    public bool IsTalking()
    {
        if (DialogueManager.GetInstance().DialogueIsPlaying)
        {
            // Do nothing when a dialogue is playing, next line of code will be skipped
            NotMoving();
            animator.SetBool(AnimationStrings.isTalking, true);
            return true;
        }
        else
        {
            animator.SetBool(AnimationStrings.isTalking, false);
            return false;
        }
    }

    // public void OnRun(InputAction.CallbackContext context)
    // {
    //     if (context.started)
    //     {
    //         IsRunning = true;
    //     }
    //     else if (context.canceled)
    //     {
    //         IsRunning = false;
    //     }
    // }

}
