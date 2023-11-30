using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] private Animator animator;
    [SerializeField] private AsyncOperation asyncScene = null;
    [SerializeField] private bool isFacingRight = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isFacingRight = true;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionToScene(string sceneName)
    {
        animator.SetTrigger(AnimationStrings.closeTransition);
        StartCoroutine(LoadingScene(sceneName));
    }

    private void Update()
    {
        while (asyncScene != null && asyncScene.isDone)
        {
            animator.SetTrigger(AnimationStrings.openTransition);
            asyncScene = null;
        }
    }

    public void TransitionToScene(string sceneName, float xPlayerPosAfter, bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
        animator.SetTrigger(AnimationStrings.closeTransition);
        StartCoroutine(LoadingScene(sceneName));
    }

    IEnumerator LoadingScene(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        asyncScene = SceneManager.LoadSceneAsync(sceneName);

    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }


    public Vector2 GetFacingDirection()
    {
        return isFacingRight ? Vector2.right : Vector2.left;
    }
}
