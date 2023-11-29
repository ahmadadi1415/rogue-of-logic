using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetection : MonoBehaviour
{
    private PlayerController player;
    public Item item;
    [SerializeField] private Rigidbody2D itemrg;

    public bool playerAbove = false;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        item = gameObject.GetComponentInParent<Item>();
        itemrg = item.gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.IsOnBox = true;
            player.interactedItem = item;
            player.itemVelocity = itemrg.velocity;
            playerAbove = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            player.itemVelocity = itemrg.velocity;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            player.IsOnBox = false;
            player.interactedItem = null;
            playerAbove = false;
        }
    }
}
