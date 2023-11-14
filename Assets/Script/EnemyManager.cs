using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    OneHand, 
    TwoHand, 
    Archer
}

public enum PatrolType
{
    Linear, Random, Loop
}

public class EnemyManager : Singleton<EnemyManager>
{
    // Start is called before the first frame update
    public Transform[] spawnPoints;
    public string[] enemyNames;
    public GameObject[] enemyTypes;

    public List<GameObject> enemies;
    public string killCondition = "Two";


    void Start()
    {
        ///check the what is different between 0-10 and 0f-10f in Random

        //SpawnEnemies();
        SpawnEnemies();
        SpawnAtRandom();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SpawnAtRandom();

        if (Input.GetKeyDown(KeyCode.K))
            KillEnemy(enemies[0]);

        if (Input.GetKeyDown(KeyCode.P))
            KillSpecificEnemis(killCondition);
    }


    IEnumerator SpawnEnemiesWithDelay()
    {
        //Week 4
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
            enemies.Add(enemy);
            ShowEnemyCount();
            yield return new WaitForSeconds(2);
        }

    }

    /// <summary>
    /// Spaqns an enemy at every spawn point
    /// </summary>
    void SpawnEnemies()
    {

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            int rnd = Random.Range(0, enemyTypes.Length);
            GameObject enemy = Instantiate(enemyTypes[rnd], spawnPoints[i].position, spawnPoints[i].rotation);
            enemies.Add(enemy);
            SetEnemyName(enemy);
        }
        ShowEnemyCount();
    }


    /// <summary>
    /// Spawn a random enemy at a random spawn point
    /// </summary>
    /// public mean can acess on public area
    public void SpawnAtRandom()
    {
        int rndEnemy = Random.Range(0, enemyTypes.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyTypes[rndEnemy], spawnPoints[rndSpawn].position, spawnPoints[rndSpawn].rotation);
        enemies.Add(enemy);
        ShowEnemyCount();
    }

    /// <summary>
    /// Shows the amount of enemies in the stage
    /// </summary>
    void ShowEnemyCount()
    {
        _UI.UpdateEnemyCount(enemies.Count);
    }

    /// <summary>
    /// Sets the enemy Name//Week 6
    /// </summary>
    /// <param name="_enemy"></param>
    void SetEnemyName(GameObject _enemy)
    {
        //_enemy.GetComponent<Enemy>().SetName(enemyNames[Random.Range(0, enemyNames.Length)]);
    }

    public string GetEnemyName()
    { //Week 6
        return enemyNames[Random.Range(0, enemyNames.Length)];
    }

    /// <summary>
    /// Kills a specific enemy
    /// </summary>
    /// <param name="_enemy">The enemy we want to kill></param/>
    public void KillEnemy(GameObject _enemy)
    {
        if (enemies.Count == 0)
            return;

        //Destroy(_enemy, 5); Week7
        enemies.Remove(_enemy);
        ShowEnemyCount();
    }

    void KillAllEnemies()
    {
        if (enemies.Count == 0)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Kill specific enemies
    /// </summary>
    /// <param name="_condition">The condition of the enemy we want to kill/param>
    void KillSpecificEnemis(string _condition)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].name.Contains(_condition))
                KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Get a random spawn Point/Week 4
    /// </summary>
    /// <returns></returns>
    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDie += KillEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDie -= KillEnemy;
    }

    void Examples()
    {
        int numberRepetitions = 2000;
        for (int i = 0; i <= 100; i++)
        {
            print(i);
        }


        GameObject first = Instantiate(enemyTypes[0], spawnPoints[0].position, spawnPoints[0].rotation);
        first.name = enemyNames[0];

        int lastEnemy = enemyTypes.Length - 1;
        GameObject last = Instantiate(enemyTypes[lastEnemy], spawnPoints[lastEnemy].position, spawnPoints[lastEnemy].rotation);
        last.name = enemyNames[lastEnemy];

        //Creat loop within a loop far a wall
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Instantiate(wall, new Vector3(i, 0, 0), transform.rotation);
            }

        }

    }


}