using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1.75f;
    public float airWalkSpeed = 2.25f;
    public float runSpeed = 2.5f;
    public float jumpImpulse = 6f;
    public int maxHealth = 100;
	public int currentHealth;

	public HealthBar healthBar;

    Vector2 moveInput;
    Rigidbody2D rigidBody;
    Animator animator;
    InteractionItem interactionItem;
    public OutputPuzzle outputPuzzle;
    public Item interactedItem;
    public Vector2 itemVelocity;
    TouchingDirections touchingDirections;
    CinemachineFramingTransposer frameTransporter;

    public float CurrentMovSpeed
    {
        get
        {
            // If the player is moving
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
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
                else
                {
                    return airWalkSpeed;
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
            animator.SetBool(AnimationStrings.isCrouch, moveInput.y < 0);
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

    public bool _isOnBox = false;
    public bool IsOnBox {
        get {
            return _isOnBox;
        }
        set {
            _isOnBox = value;
        }
    }


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        interactionItem = GetComponent<InteractionItem>();
        touchingDirections = GetComponent<TouchingDirections>();
        frameTransporter = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    [SerializeField] private AudioSource jumpSoundEffect;
    public AudioSource footstepsSound;

    private void Start() {
        currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
        transform.position = SceneController.instance.GetPositionAfter();
        SetFacingDirection(SceneController.instance.GetFacingDirection());
    }

    private void FixedUpdate()
    {
        if (IsTalking || IsInteracting) return;

        moveInput = InputManager.GetInstance().GetMoveDirection();
        if (moveInput.y > 0 && touchingDirections.IsGrounded) {
            jumpSoundEffect.Play();
            // Debug.Log(moveInput.y);
            animator.SetTrigger(AnimationStrings.jump);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpImpulse);
        }

        if (!DialogueManager.GetInstance().DialogueIsPlaying)
        {
            
            rigidBody.velocity = new Vector2(moveInput.x * CurrentMovSpeed, rigidBody.velocity.y);
            if (IsOnBox)
            {
                MoveOnBox();
            }
        }

        animator.SetFloat(AnimationStrings.yVelocity, rigidBody.velocity.y);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamage(20);
		}
        if (interactionItem.anyObjectDetected && InputManager.GetInstance().GetInteractPressed() && touchingDirections.IsGrounded){
            IsInteracting = true;
        }

        if (IsInteracting && (InputManager.GetInstance().GetQuitPressed() || !ItemManager.GetInstance().ItemInteracting)) {
            IsInteracting = false;
        }

        IsTalking = DialogueManager.GetInstance().DialogueIsPlaying;
        if (IsTalking || IsInteracting) return;

        if (outputPuzzle != null && InputManager.GetInstance().GetSubmitPressed()) {
            bool solved = outputPuzzle.SolvePuzzle();
            Debug.Log("Puzzle is solved?" + solved);
        }

        SetFacingDirection(moveInput);
        LookDown();
        IsMoving = moveInput != Vector2.zero;
        IsRunning = InputManager.GetInstance().getRunPressed();
        
    }

    void TakeDamage(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}

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

    private bool _isInteracting;
    public bool IsInteracting {
        get {
            return _isInteracting;
        }
        set {
            _isInteracting = value;
            animator.SetBool(AnimationStrings.isInteracting, value);
            if (_isInteracting) Interacting();
            else {
                ItemManager.GetInstance().HideInteractionPanel();
            }
        }
    }
    private void Interacting()
    {
        NotMoving();
        float objectPosition = interactionItem.interactedItem.transform.position.x;
        float playerPosition = transform.position.x;
        SetFacingDirection((objectPosition < playerPosition) ? Vector2.left : Vector2.right);
        ItemManager.GetInstance().ShowInteractionPanel();
    }

    private bool _isTalking;
    public bool IsTalking {
        get {
            return _isTalking;
        }
        set {
            _isTalking = value;
            animator.SetBool(AnimationStrings.isTalking, value);
            if(_isTalking) NotMoving(); 
        }
    }

    private void LookDown() {
        frameTransporter.m_ScreenY = (moveInput.y < 0) ? 0.3f : 0.7f;
    }

    private void MoveOnBox() {
        // rigidBody.position = Vector3.MoveTowards(transform.position, interactedItem.nextWaypoint, Time.deltaTime * interactedItem.itemSpeed);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x + itemVelocity.x, rigidBody.velocity.y);
    }
}
