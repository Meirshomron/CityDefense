using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model containing data of the in-game general user progress.
/// Accessible from anywhere in the game.
/// </summary>
public class UserModel : MonoBehaviour 
{
    private static UserModel _instance;

    public static UserModel Instance
    {
        get { return _instance; }
    }

    [SerializeField] private int currentScore;
    [SerializeField] private int currentWaveIndex;
    [SerializeField] private int bombsSpawned;
    [SerializeField] private int bombsHit;
    [SerializeField] private int bombsSliced;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
    }

    public int CurrentScore
    {
        get
        {
            return currentScore;
        }

        set
        {
            currentScore = value;
        }
    }

    public int CurrentWaveIndex
    {
        get
        {
            return currentWaveIndex;
        }

        set
        {
            currentWaveIndex = value;
        }
    }

    public int BombsSliced
    {
        get
        {
            return bombsSliced;
        }

        set
        {
            bombsSliced = value;
        }
    }

    public int BombsSpawned
    {
        get
        {
            return bombsSpawned;
        }

        set
        {
            bombsSpawned = value;
        }
    }

    public int BombsHit
    {
        get
        {
            return bombsHit;
        }

        set
        {
            bombsHit = value;
        }
    }
}
