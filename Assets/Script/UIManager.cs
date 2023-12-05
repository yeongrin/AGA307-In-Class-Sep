using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    //Week 6
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text enemyCountText;

    [Header("Game Data Stuff")]
    public TMP_Text totalKillText;
    public TMP_Text totalPlayedTimeText;//Week 11

    private void Start()
    {
        UpdateScore(0);
        UpdateTime(0);
        UpdateEnemyCount(0);
        UpdateKillTotal();
        UpdatePlayTimeTotal();
    }

    public void UpdateScore(int _score)
    {
      scoreText.text = "score:" + _score.ToString();
    }

    public void UpdateTime(float _time)
    {
        timerText.text = _time.ToString("F2");
    }

    public void UpdateEnemyCount(int _count)
    {
        enemyCountText.text = "Enemy Count:" + _count.ToString();
    }

    public void UpdateKillTotal()
    {
        totalKillText.text = "Total Kills:" + _DATA.GetEnemyKillTotal();
    }

    public void UpdatePlayTimeTotal()
    {
        totalPlayedTimeText.text = "Total PlayTime:" + _DATA.GetTimeFormatted();
    }

}
