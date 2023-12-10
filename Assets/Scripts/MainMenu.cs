using UnityEngine;

public class MainMenu : MonoBehaviour
{
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

}
