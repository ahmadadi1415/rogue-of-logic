
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInformation : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        UpdateInformation();
    }

    public void UpdateInformation()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int currentLevel = MainManager.Instance.CurrentLevel;
        string text = "";
        if (currentLevel == currentSceneIndex || currentLevel == currentSceneIndex + 1)
        {
            text = MainManager.Instance.CurrentInformation;
        }
        else
        {
            text = "Head towards the light to proceed to the next stage";
        }
        StartCoroutine(Typing(text, 0.04f));
    }

    private IEnumerator Typing(string text, float delay)
    {
        tmp.text = "";

        foreach (char letter in text.ToCharArray())
        {
            tmp.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}
