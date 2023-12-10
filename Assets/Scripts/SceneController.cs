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
        CloseTransition();
        StartCoroutine(LoadingScene(sceneName));
    }

    private void Update()
    {
        while (asyncScene != null && asyncScene.isDone)
        {
            OpenTransition();
            asyncScene = null;
        }
    }

    public void TransitionToScene(string sceneName, float xPlayerPosAfter, bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
        CloseTransition();
        StartCoroutine(LoadingScene(sceneName));
    }

    IEnumerator LoadingScene(string sceneName)
    {
        if (MainManager.Instance != null)
        {
            MainManager.Instance.StartingPlayerHealth = MainManager.Instance.PlayerHealth;
        }
        yield return new WaitForSeconds(1f);
        asyncScene = SceneManager.LoadSceneAsync(sceneName);
    }

    public Vector2 GetFacingDirection()
    {
        return isFacingRight ? Vector2.right : Vector2.left;
    }

    public void OpenTransition() {
        animator.SetTrigger(AnimationStrings.openTransition);
    }

    public void CloseTransition()
    {
        animator.SetTrigger(AnimationStrings.closeTransition);
    }
}
