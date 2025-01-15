using System;
using Cinemachine;
using UnityEngine;

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
    private KeyHintSetter keyHintSetter;
    private IPlayerAttribute attribute;

    [SerializeField] private bool hasDied = false;
    private float maxFallSpeed = -13f;

    private void SetCurrentMovementSpeed()
    {
        // If the player is moving
        if (attribute.IsMoving && !touchingDirections.IsOnWall)
        {
            if (touchingDirections.IsGrounded)
            {
                attribute.CurrentMovementSpeed = attribute.IsRunning ? runSpeed : walkSpeed;
            }
            else
            {
                attribute.CurrentMovementSpeed = airWalkSpeed;
            }
        }

        // if the player is not moving, idle
        else
        {
            attribute.CurrentMovementSpeed = 0;
        }
    }

    private void SetIsMoving(bool isMoving)
    {
        attribute.IsMoving = isMoving;
        animator.SetBool(AnimationStrings.horizontalMoving, moveInput.x != 0);
        animator.SetBool(AnimationStrings.isMoving, isMoving);
        animator.SetBool(AnimationStrings.isCrouch, moveInput.y < 0);
    }

    private void SetIsRunning(bool isRunning)
    {
        attribute.IsRunning = isRunning;
        animator.SetBool(AnimationStrings.isRunning, isRunning);
    }

    public bool IsFacingRight { get; private set; }
    public void SetIsFacingRight(bool isFacingRight)
    {
        if (isFacingRight != IsFacingRight)
        {
            transform.localScale *= new Vector2(-1, 1);
        }

        IsFacingRight = isFacingRight;
    }

    public bool IsOnBox { get; private set; }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        interactionItem = GetComponent<InteractionItem>();
        touchingDirections = GetComponent<TouchingDirections>();
        frameTransporter = FindObjectOfType<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        keyHintSetter = FindObjectOfType<KeyHintSetter>();
        attribute = GetComponent<IPlayerAttribute>();
    }

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource footstepsSound;
    [SerializeField] private AudioSource hurtSound;
    [SerializeField] private AudioSource fellSound;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource puzzleSolvedSound;

    private void Start()
    {
        currentHealth = MainManager.Instance.PlayerHealth;
        SetFacingDirection(SceneController.instance.GetFacingDirection());
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0 && hasDied) return;

        if (IsTalking || IsInteracting) return;

        if (!touchingDirections.IsGrounded && rigidBody.velocity.y < maxFallSpeed)
        {
            IsFallDamaged = true;
        }

        if (touchingDirections.IsGrounded && IsFallDamaged)
        {
            IsFallDamaged = false;
            TakeDamage(20);
            if (!fellSound.isPlaying)
            {
                fellSound.Play();
            }
        }

        moveInput = InputManager.GetInstance().GetMoveDirection();
        if (moveInput.y > 0 && touchingDirections.IsGrounded)
        {
            if (jumpSoundEffect != null)
            {
                jumpSoundEffect.Play();
            }
            // Debug.Log(moveInput.y);
            animator.SetTrigger(AnimationStrings.jump);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpImpulse);
        }

        if (moveInput.x != 0 && !attribute.IsRunning && touchingDirections.IsGrounded)
        {
            if (!footstepsSound.isPlaying)
            {
                footstepsSound.PlayDelayed(0.3f);
            }
        }
        else
        {
            footstepsSound.Stop();
        }

        if (!DialogueManager.GetInstance().DialogueIsPlaying)
        {
            SetCurrentMovementSpeed();
            rigidBody.velocity = new Vector2(moveInput.x * attribute.CurrentMovementSpeed, rigidBody.velocity.y);
            if (IsOnBox)
            {
                MoveOnBox();
            }
        }

        animator.SetFloat(AnimationStrings.yVelocity, rigidBody.velocity.y);
    }


    private void Update()
    {
        if (currentHealth <= 0)
        {
            if (!hasDied)
            {
                hasDied = true;
                if (!deathSound.isPlaying)
                {
                    deathSound.Play();
                }
                Debug.Log("Player Died");
            }
            // else {
            //     // SceneController.instance.CloseTransition();
            // }
            return;
        }

        if (interactionItem.anyObjectDetected && InputManager.GetInstance().GetInteractPressed() && touchingDirections.IsGrounded)
        {
            SetIsInteracting(true);
        }

        if (IsInteracting && (InputManager.GetInstance().GetQuitPressed() || !ItemManager.GetInstance().ItemInteracting))
        {
            SetIsInteracting(false);
            keyHintSetter.HideHints();
        }

        SetIsTalking(DialogueManager.GetInstance().DialogueIsPlaying);
        if (IsTalking)
        {
            return;
        }

        if (IsInteracting)
        {
            keyHintSetter.ShowHints("[ ENTER ] to select");
            return;
        }

        SetFacingDirection(moveInput);
        LookDown();
        SetIsMoving(moveInput != Vector2.zero);
        SetIsRunning(InputManager.GetInstance().getRunPressed());
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
        MainManager.Instance.PlayerHealth = currentHealth;
        if (currentHealth <= 0)
        {
            MainManager.Instance.RestartStage();
        }
    }

    public void Heal(int heal)
    {
        _ = currentHealth + heal > maxHealth ? currentHealth = maxHealth : currentHealth += heal;

        healthBar.SetHealth(currentHealth);
        MainManager.Instance.PlayerHealth = currentHealth;
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // if the current move input is positive and the player is not facing right, face the right
            SetIsFacingRight(true);
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // if the current move input is negative and the player is not facing left, face the left
            SetIsFacingRight(false);
        }
    }

    public void StopMovement()
    {
        rigidBody.velocity = Vector2.zero;
        SetIsMoving(false);
        SetIsRunning(false);
    }

    public bool IsInteracting { get; private set; }
    private void SetIsInteracting(bool isInteracting)
    {
        IsInteracting = isInteracting;

        animator.SetBool(AnimationStrings.isInteracting, IsInteracting);
        if (IsInteracting)
        {
            StopMovement();
            float objectPosition = interactionItem.interactedItem.transform.position.x;
            float playerPosition = transform.position.x;
            SetFacingDirection((objectPosition < playerPosition) ? Vector2.left : Vector2.right);
            ItemManager.GetInstance().ShowInteractionPanel();
        }
        else
        {
            ItemManager.GetInstance().HideInteractionPanel();
        }
    }

    public bool IsTalking { get; private set; }
    private void SetIsTalking(bool isTalking)
    {
        IsTalking = isTalking;

        animator.SetBool(AnimationStrings.isTalking, IsTalking);
        if (IsTalking) StopMovement();
    }

    public bool IsFallDamaged { get; private set; }

    private void LookDown()
    {
        frameTransporter.m_ScreenY = (moveInput.y < 0) ? 0.3f : 0.7f;
    }

    private void MoveOnBox()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x + itemVelocity.x, rigidBody.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!IsFallDamaged && !footstepsSound.isPlaying)
            {
                footstepsSound.Play();
            }
        }
    }

    public void SetIsOnBox(bool isOnBox)
    {
        IsOnBox = isOnBox;
    }
}
