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

    private KeyHintSetter uiHintSetter;
    private void Awake()
    {
        uiHintSetter = FindObjectOfType<KeyHintSetter>();
    }

    private void Update()
    {
        if (playerInArea && InputManager.GetInstance().GetSubmitPressed())
        {
            SceneController.instance.TransitionToScene(destinationScene, xPositionAfter, isFacingRight);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = true;
            uiHintSetter.ShowHints("[ ENTER ]");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInArea = false;
            uiHintSetter.HideHints();
        }
    }
}
