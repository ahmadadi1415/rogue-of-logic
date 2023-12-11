using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour

{
    public static bool GameIsPaused = false;
    [SerializeField] private GameObject[] menuChoices;

    public GameObject pauseMenuUI;
    private PlayerController player;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!player.IsInteracting && InputManager.GetInstance().GetQuitPressed()){
            if (GameIsPaused){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void ShowPauseMenu() {
        if (MainManager.Instance.PlayerHealth <= 0) {
            menuChoices[0].SetActive(false);
        }
        pauseMenuUI.SetActive(true);
    }
    
    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        // Debug.Log("Resume");
    }

    public void Pause(){
        ShowPauseMenu();
        StartCoroutine(SelectFirstChoice());
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Retry() {
        Time.timeScale = 1f;
        MainManager.Instance.RestartStage();
    }

    public void LoadMenu(){
        // Debug.Log("Loading to Menu");
        Time.timeScale = 1f;
        // MainManager.Instance.Reset();
        SceneController.instance.TransitionToScene("MainMenu");
    }

    public void QuitGame(){
        Application.Quit();
    }

    private IEnumerator SelectFirstChoice() {
        // Unity event system need to be cleared, and then select it after at least one frame
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(menuChoices.Where(menu => menu.activeSelf).First());
    }
}
