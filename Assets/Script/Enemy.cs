using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GameBehaviour
{

    public static event Action<GameObject> OnEnemyHit = null;
    public static event Action<GameObject> OnEnemyDie = null;

    public PatrolType myPatrol;
    float baseSpeed = 1f;
    public float mySpeed = 1f;
    float moveDistance = 1000f;

    int baseHealth = 100;
    int maxHealth; //Week 6
    public int myHealth;
    public int myScore;
    EnemyHealthBar healthBar;//Week 6

    public string myName;//Week 6

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;      //Needed for loop patrol movement
    Transform endPos;        //Needed for loop patrol movement
    bool reverse;            //Needed for loop patrol movement
    int patrolPoint = 0;     //Needed for linear patrol movement


    
    void Start()
    {

        healthBar = GetComponentInChildren<EnemyHealthBar>(); 
        SetName(_EM.GetEnemyName());//Week 6

        switch(myType)
        {
            case EnemyType.OneHand:
                myHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Linear;
                myScore = 100;
                break;

            case EnemyType.TwoHand:
                myHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Random;
                myScore = 200;
                break;

            case EnemyType.Archer:
                myHealth = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Loop;
                myScore = 300;
                break;

        }

        SetupAI();
    }


    void SetupAI()
    {
        //Week 4
        startPos = Instantiate (new GameObject(), transform.position, transform.rotation).transform;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = endPos;
        StartCoroutine(Move());
    }

    private void Update()
    {
        //Week 4
        if (Input.GetKeyDown(KeyCode.Escape))
            StopAllCoroutines();

        if (Input.GetKeyDown(KeyCode.H))
            Hit(30);
    }

    //Week 6
    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name);
    }

    //Week5
    IEnumerator Move()
    {

        switch (myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;

            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;

            case PatrolType.Loop:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }

        transform.LookAt(moveToPos);
        while (Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }


        yield return new WaitForSeconds(1);
        StartCoroutine(Move());

    }

    //week 5
    private void Hit(int _damage)
    {
        myHealth -= _damage;
        healthBar.UpdateHealthBar(myHealth, baseHealth);//Week 6
        ScaleObject(this.gameObject, transform.localScale * 1.1f);
        
        if (myHealth <= 0)
        { 
            Die(); 
        }
        else 
        {
            OnEnemyHit?.Invoke(this.gameObject);
            //_GM.AddScore(myScore);
        }
    }

    private void Die()
    {
       
        StopAllCoroutines();
        OnEnemyDie?.Invoke(this.gameObject);
        // _GM.AddScore(myScore * 2);
        // _EM.KillEnemy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            Hit(collision.gameObject.GetComponent<Projectile>().damage);
            Destroy(collision.gameObject);
        }
    }


    /* IEnumerator Move()
     {   Week 4
        for (int i = 0; i < moveDistance; i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
            yield return null;

        }

        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(Move());
      }*/

}   