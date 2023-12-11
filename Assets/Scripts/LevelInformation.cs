
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
        if (currentLevel == (2 * currentSceneIndex - 1) || currentLevel == (2 * currentSceneIndex))
        {
            text = MainManager.Instance.CurrentInformation;
        }
        else
        {
            text = "Head towards the light to proceed to the next stage";
        }
        StartCoroutine(Typing(text, 0.04f));
    }

    public void UpdateInformation(string text) 
    {
        StartCoroutine(ChangeInformation(text));
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

    private IEnumerator Deleting() {
        yield return new WaitForSeconds(2f);
        string currentText = tmp.text;
        foreach (char letter in currentText)
        {
            currentText = currentText.Substring(0, currentText.Length - 1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator ChangeInformation(string text) {
        StartCoroutine(Typing(text, 0.04f));
        yield return new WaitForSeconds(5);
        StartCoroutine(Deleting());
        yield return new WaitForSeconds(0.5f);
        UpdateInformation();
    }

}
