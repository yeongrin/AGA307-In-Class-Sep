using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public enum GameState {  Title, Playing, Paused, GameOver }
public enum Difficulty {  Easy, Medium, Hard }

public class GameManager : Singleton<GameManager>
{

    public GameState gameState;
    public Difficulty difficulty;
    public int score = 0;
    int scoreMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                scoreMultiplier = 1;
                break;
            case Difficulty.Medium:
                scoreMultiplier = 2;
                break;
            case Difficulty.Hard:
                scoreMultiplier = 3;
                break;
        }
    }

    // Update is called once per frame
    public void AddScore(int _points)
    {
        score += _points * scoreMultiplier; 
    }

    void OnEnemyHit(GameObject _enemy)
    {
        int _score = _enemy.GetComponent<Enemy>().myScore;
        AddScore(_score);
    }

    private void OnEnable()
    {
        Enemy.OnEnemyHit += OnEnemyHit;
        Enemy.OnEnemyDie += OnEnemyHit;

    }

    private void OnDisable()
    {
        Enemy.OnEnemyHit -= OnEnemyHit;
        Enemy.OnEnemyDie -= OnEnemyHit;
    }



}
