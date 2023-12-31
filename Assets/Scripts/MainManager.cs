
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
    public int _currentStage;
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
        CurrentStage = SceneManager.GetActiveScene().buildIndex;
        CurrentLevel = SceneManager.GetActiveScene().name switch
        {
            "Stage One" => 1,
            "Stage Two" => 3,
            "Stage Three" => 5,
            "Stage Four" => 7,
            "Stage Five" => 9,
            _ => 0,
        };
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
                if (value > informationList.Count) {
                    _currentInformation = "To unravel the complex ones, start thinking from the end";
                }
                else {
                    _currentInformation = informationList[value - 1];
                }
                LevelInformation levelInformation = FindObjectOfType<LevelInformation>();
                if (levelInformation != null) {
                    levelInformation.UpdateInformation();
                }
            }
        }
    }

    public int CurrentStage 
    {
        get {
            return _currentStage;
        }
        set {
            _currentStage = value;
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
        PlayerHealth = StartingPlayerHealth;
        SceneController.instance.TransitionToScene(SceneManager.GetActiveScene().name);
    }

    public void Reset() {
        PlayerHealth = 100;
        StartingPlayerHealth = 100;
        CurrentLevel = 1;
    }
}
