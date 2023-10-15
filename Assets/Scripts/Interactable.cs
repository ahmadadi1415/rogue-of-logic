using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject interactBubble;
    [SerializeField]
    private string interactionTag;
    private bool playerInRange = false;

    [Header("Ink JSON")]
    [SerializeField]
    private TextAsset inkJSON;
    private PlayerController player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerInRange)
        {
            interactBubble.SetActive(true);
            if (InputManager.GetInstance().GetInteractPressed() && !DialogueManager.GetInstance().DialogueIsPlaying)
            {
                // Debug.Log(inkJSON.text);
                Vector2 intrBubblePos = interactBubble.transform.position;
                // Transform player = GameObject.FindGameObjectWithTag("Player").transform;

                Transform npc = interactBubble.transform.parent.transform;
                bool isPlayerRightOfNPC = player.gameObject.transform.position.x > intrBubblePos.x;
                npc.localScale = isPlayerRightOfNPC ? new Vector2(1, 1) : new Vector2(-1, 1);
                player.IsFacingRight = !isPlayerRightOfNPC;
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, interactBubble.transform);
            }
        }
        else
        {
            interactBubble.SetActive(false);
            DialogueManager.GetInstance().ExitDialogueMode();
        }


    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        interactionTag = other.tag;
        if (other.tag == "Player")
        {
            Debug.Log("Can Interact With");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            interactionTag = "";
            playerInRange = false;
        }
    }
}