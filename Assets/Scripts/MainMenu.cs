using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] menuChoices;

    private void Awake() {
        if (MainManager.Instance == null) {
            menuChoices[1].SetActive(false);
        }
    }
    private void Start() {
        StartCoroutine(SelectFirstChoice());
    }
    public void StartGame() {
        FindObjectOfType<SceneController>().TransitionToScene("Stage One");
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void Continue() {
        FindObjectOfType<SceneController>().TransitionToScene(SceneManager.GetSceneByBuildIndex(MainManager.Instance.CurrentLevel).name);
    }
    
    private IEnumerator SelectFirstChoice() {
        // Unity event system need to be cleared, and then select it after at least one frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(menuChoices[0].gameObject);
    }

}
