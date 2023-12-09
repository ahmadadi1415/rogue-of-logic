using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public int _playerHealth;
    public int _maxHealth = 100;
    public int _startingPlayerHealth;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerHealth = 100;
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

    public int StartingPlayerHealth {
        get {
            return _startingPlayerHealth;
        }
        set {
            _startingPlayerHealth = value;
        }
    }
}
