using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyHintSetter : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private void Awake() {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    public void ShowHints(string text) {
        tmp.text = text;
        gameObject.SetActive(true);
    }

    public void HideHints() {
        tmp.text = "";
        gameObject.SetActive(false);
    }
}
