
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public int _playerHealth;
    public int _maxHealth = 100;
    public int _startingPlayerHealth;
    public int _currentLevel;
    public string _currentInformation;
    [SerializeField] private List<string> informationList;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerHealth = 100;
        StartingPlayerHealth = 100;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage One":
                CurrentLevel = 1;
                break;
            case "Stage Two":
                CurrentLevel = 3;
                break;
            case "Stage Three":
                CurrentLevel = 5;
                break;
            case "Stage Four":
                CurrentLevel = 7;
                break;
            case "Stage Five":
                CurrentLevel = 9;
                break;
            default:
                CurrentLevel = 0;
                break;
        }

    }

    public int PlayerHealth
    {
        get
        {
            return _playerHealth;
        }
        set
        {
            _playerHealth = value > _maxHealth ? _maxHealth : value;
        }
    }

    public int StartingPlayerHealth
    {
        get
        {
            return _startingPlayerHealth;
        }
        set
        {
            _startingPlayerHealth = value;
        }
    }

    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            if (value - 1 >= 0)
            {
                _currentLevel = value;
                _currentInformation = informationList[value - 1];
                FindObjectOfType<LevelInformation>().UpdateInformation();
            }
        }
    }

    public string CurrentInformation
    {
        get
        {
            return _currentInformation;
        }
    }

    public void RestartStage()
    {
        SceneController.instance.TransitionToScene(SceneManager.GetActiveScene().name);
        PlayerHealth = StartingPlayerHealth;
    }
}
