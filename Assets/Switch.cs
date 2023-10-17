using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private BooleanSource boolSource;
    private void Awake()
    {
        boolSource = GetComponent<BooleanSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boolSource.BooleanValue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boolSource.BooleanValue = false;
        }
    }
}
