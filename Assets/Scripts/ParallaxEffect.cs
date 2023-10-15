using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Starting position of the parallax GameObject
    Vector2 startingPosition;

    // Starting Z value of the parallax GameObject
    float startingZ;

    // How far camera has moved from the starting position of the parallax object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.nearClipPlane : cam.farClipPlane));

    // The further the object from the player, the faster the parallax object will move. Drag its Z value closer to the target to make it slower
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        // When the target moves, move the parallax object the same distance as the multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        // The x y position of parallax object changes based on how fast target travel times parallax factor, but Z is constant
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
