using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Rigidbody2D player = other.gameObject.GetComponent<Rigidbody2D>();
            other.transform.SetParent(transform.parent);
            player.velocity =  transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.transform.SetParent(null);
        }
    }
}
