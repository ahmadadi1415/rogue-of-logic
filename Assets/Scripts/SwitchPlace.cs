using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchPlace : MonoBehaviour
{
    [SerializeField] private string destinationScene;
    [SerializeField] private float xPositionAfter;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool playerInArea = false;
    [SerializeField] private bool isTeleported = false;

    [SerializeField] private KeyHintSetter uiHintSetter;

    private void Update()
    {
        if (playerInArea && InputManager.GetInstance().GetSubmitPressed() && !isTeleported)
        {
            isTeleported = true;
            SceneController.instance.TransitionToScene(destinationScene, xPositionAfter, isFacingRight);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleported)
        {
            playerInArea = true;
            uiHintSetter.ShowHints("[ ENTER ]");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTeleported)
        {
            playerInArea = false;
            uiHintSetter.HideHints();
        }
    }
}
