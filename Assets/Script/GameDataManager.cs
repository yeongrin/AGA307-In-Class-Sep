using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This GameData contains all the variables we wish to stor in our save file
/// </summary>
/// 
[Serializable]
public class ThisGameData
{
    //Game Specific
    public int highestScore;
    public int enemyKillTotal;
    public Vector3 lastCheckpoint;


    //Time
    public int hoursPlay;
    public int minutesPlayed;
    public int secondsPlayed;
    public int totalSeconds;
}
public class GameDataManager : GameData
{
    //Singleton Setup
    public static GameDataManager INSTANCE;

    //The game data for this game
    public ThisGameData data = new ThisGameData();

    //Time of the last save
    public DateTime timeOfLastSave;

    #region Initialisation
    public void Awake()
    {
      //Singleton Setup
      if(INSTANCE != null)
            return;
        INSTANCE = this;

        //Load the game data
        data = LoadDataObject<ThisGameData>();

        //If the data exsit, initialise it
        if(data == null)
        {
            data = new ThisGameData();
            Debug.Log("New Data File Created");

            //Initialise our default values;
            data.enemyKillTotal = 0;
            data.highestScore = 0;
        }

        //Set the time of our last save to now
        timeOfLastSave = DateTime.Now;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveData();
        }
    }
    #endregion

    #region Overrides
    public override void SaveData()
    {
        ElapsedTime();
        SaveDataObject(data);
    }

    public override void DeleteData()
    {
        DeleteDataObject();
    }
    #endregion

    #region Getting Data
    public void SetHighestScore(int _score)
    {
        if (_score > data.highestScore)
            data.highestScore = _score;
    }
    public void SetEnemyKillTotal()
    {
        data.enemyKillTotal++;
        SaveData();
    }

    public void SetLastCheckPoint(Vector3 _lastCheckpoint)
    {
        data.lastCheckpoint = _lastCheckpoint;
    }

    public void SetTimePlayed()
    {
        ElapsedTime();
    }
    #endregion

    

    #region Game Time
    public void ElapsedTime()
    {
        DateTime now = DateTime.Now;
        int seconds = (now - timeOfLastSave).Seconds;
        data.totalSeconds += seconds;
        data.hoursPlay = GetHours(data.totalSeconds);
        data.minutesPlayed = GetMinutes(data.totalSeconds);
        data.secondsPlayed = GetSeconds(data.totalSeconds);
        Debug.Log(data.hoursPlay + " Hours, " + data.minutesPlayed +
                  " Minutes, " + data.secondsPlayed + " Seconds");
        timeOfLastSave = DateTime.Now;
    }
    int GetSeconds(float _seconds)
    {
        return Mathf.FloorToInt(_seconds % 60);
    }
    int GetMinutes(float _seconds)
    {
        return Mathf.FloorToInt(_seconds / 60);
    }
    int GetHours(float _seconds)
    {
        return Mathf.FloorToInt(_seconds / 3600);
    }
    #endregion


    #region Getting Data
    public int GetHightestScore()
    {
        return data.highestScore;
    }

    public int GetEnemyKillTotal()
    {
        return data.enemyKillTotal;

    }

    public Vector3 GetLastCheckPoint()
    {
        return data.lastCheckpoint;
    }
    public string GetTimeFormatted()
    {
        ElapsedTime();
        return String.Format(" {0:00} : {1:00} : {2:00} ", data.hoursPlay, data.minutesPlayed, data.secondsPlayed);

    }
    #endregion
}
