using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour

{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject[] menuChoices;

    public GameObject pauseMenuUI;
    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetInstance().GetQuitPressed()){
            if (GameIsPaused){
                Resume();
            } else{
                Pause();
                StartCoroutine(SelectFirstChoice());
            }

        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        Debug.Log("Resume");
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu(){
        Debug.Log("Loading to Menu");
    }
    public void QuitGame(){
        Application.Quit();
    }

    private IEnumerator SelectFirstChoice() {
        // Unity event system need to be cleared, and then select it after at least one frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(menuChoices[0].gameObject);
    }
}
