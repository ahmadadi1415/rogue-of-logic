using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Switch : MonoBehaviour
{
    private BooleanSource boolSource;
    private Light2D light;

    [SerializeField] private Color onColor, offColor;
    private void Awake()
    {
        boolSource = GetComponent<BooleanSource>();
        light = GetComponent<Light2D>();
    }

    private void Start() {
        light.color = boolSource.BooleanValue ? onColor : offColor;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boolSource.BooleanValue = true;
            light.color = onColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            boolSource.BooleanValue = false;
            light.color = offColor;
        }
    }
}
