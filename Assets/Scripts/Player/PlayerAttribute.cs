using UnityEngine;

public class PlayerAttribute : MonoBehaviour, IPlayerAttribute
{
    public float CurrentMovementSpeed { get; set; }
    public bool IsMoving { get; set; }
    public bool IsRunning { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsOnBox { get; set; }
    public bool IsInteracting { get; set; }
    public bool IsFallDamaged { get; set; }
}