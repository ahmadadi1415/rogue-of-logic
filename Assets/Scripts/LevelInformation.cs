
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private bool isTextChanging = false;
    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        tmp.text = "";
        if (!isTextChanging) UpdateInformation();
    }

    public void UpdateInformation()
    {
        if (!isTextChanging)
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
    }

    public void UpdateInformation(string text)
    {
        if (!isTextChanging)
        {
            StartCoroutine(ChangeInformation(text));
        }
    }

    private IEnumerator Typing(string text, float delay)
    {
        if (!isTextChanging && tmp != null)
        {
            isTextChanging = true;
            tmp.text = "";
            foreach (char letter in text.ToCharArray())
            {
                tmp.text += letter;
                yield return new WaitForSeconds(delay);
            }
            isTextChanging = false;
        }
    }

    private IEnumerator ChangeInformation(string text)
    {
        StartCoroutine(Typing(text, 0.04f));
        yield return new WaitForSeconds(5);
        UpdateInformation();
    }

}
