using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.AI;

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
    public int myDamage = 20;

    EnemyHealthBar healthBar;//Week 6
    public string myName;//Week 6

    [Header("AI")]
    public EnemyType myType;
    public Transform moveToPos; //Needed for all patrols
    Transform startPos;      //Needed for loop patrol movement
    Transform endPos;        //Needed for loop patrol movement
    bool reverse;            //Needed for loop patrol movement
    int patrolPoint = 0;     //Needed for linear patrol movement
    public float attackDistance = 2f; //Week7
    public float detectTime = 5f; //Week 9
    public float detectDistance = 10f;//Week 9
    int currentWaypoint; //Week 9
    NavMeshAgent agent;

    Animator anim; //Week 7
    
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<EnemyHealthBar>(); 
        SetName(_EM.GetEnemyName());//Week 6

        switch(myType)
        {
            case EnemyType.OneHand:
                myHealth = maxHealth = baseHealth;
                mySpeed = baseSpeed;
                myPatrol = PatrolType.Patrol;
                myScore = 100;
                myDamage = 20;
                break;

            case EnemyType.TwoHand:
                myHealth = maxHealth = baseHealth * 2;
                mySpeed = baseSpeed / 2;
                myPatrol = PatrolType.Detect;
                myScore = 200;
                myDamage = 30;
                break;

            case EnemyType.Archer:
                myHealth = maxHealth = baseHealth / 2;
                mySpeed = baseSpeed * 2;
                myPatrol = PatrolType.Chase;
                myScore = 300;
                myDamage = 50;
                break;

        }

        SetupAI();

        //Week 7
        if (GetComponentInChildren<EnemyWeapon>() != null)
        GetComponentInChildren<EnemyWeapon>().damage = myDamage;
    }


    void SetupAI()
    {
        currentWaypoint = UnityEngine.Random.Range(0, _EM.spawnPoints.Length);
        agent.SetDestination(_EM.spawnPoints[currentWaypoint].position);
        ChangeSpeed(mySpeed);  


        //Week 4
        //startPos = Instantiate (new GameObject(), transform.position, transform.rotation).transform;
        //endPos = _EM.GetRandomSpawnPoint();
        //moveToPos = endPos;
        //StartCoroutine(Move());
    }

    //week 
    void ChangeSpeed(float _speed)
    {
        agent.speed = _speed;
    }

    private void Update()
    {
        if (myPatrol == PatrolType.Die)
            return;

        //Always get the distance between the player and me
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);

        if(distToPlayer <= detectDistance && myPatrol != PatrolType.Attack)
        {
            if(myPatrol != PatrolType.Chase)
            {
                myPatrol = PatrolType.Detect;
            }
        }

        //Set the animator speed paramater to that of my speed
        anim.SetFloat("Speed", mySpeed);

        //Switching

        switch (myPatrol)
        {
            case PatrolType.Patrol:
                //Get the distance between us and the current waypoint
                float distToWaypoint = Vector3.Distance(transform.position, _EM.spawnPoints[currentWaypoint].position);
                //If the distance is close enough, get a new waypoint
                if (distToWaypoint < 1)
                    SetupAI();
                //Reset the detect time
                detectTime = 5;
                break;

            case PatrolType.Detect:
                //Set the destination to ourself, essentionally stopping us
                agent.SetDestination(transform.position);
                //Stop our Speed
                ChangeSpeed(0);
                //Decrement our detect time
                detectTime -= Time.deltaTime;
                if(distToPlayer <= detectDistance)
                {
                    myPatrol = PatrolType.Chase;
                    detectTime = 5;
                }
                if(detectTime <= 0)
                {
                    myPatrol = PatrolType.Patrol;
                    SetupAI();
                }
                break;

            case PatrolType.Chase:
                //Set the destination to that of player
                agent.SetDestination(_PLAYER.transform.position);
                //Increase th speed of which to chase the player
                ChangeSpeed(mySpeed * 2);
                //If the player gets ouside the detect distance, go back to the detect state
                if (distToPlayer > detectDistance)
                    myPatrol = PatrolType.Detect;
                //If we are close to the player, then attack
                if (distToPlayer <= attackDistance)
                    StartCoroutine(Attack());
                break;
        }
    }

    //Week 6
    public void SetName(string _name)
    {
        name = _name;
        healthBar.SetName(_name);
    }

    //Week5
   /* IEnumerator Move()
    {

        switch (myPatrol)
        {
            case PatrolType.Patrol:
                moveToPos = _EM.spawnPoints[patrolPoint];
                patrolPoint = patrolPoint != _EM.spawnPoints.Length ? patrolPoint + 1 : 0;
                break;

            case PatrolType.Detect:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;

            case PatrolType.Chase:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }

        transform.LookAt(moveToPos);
        while (Vector3.Distance(transform.position, moveToPos.position) > 0.3f)
        {
            if(Vector3.Distance(transform.position, _PLAYER.transform.position) < attackDistance)
            {
                StopAllCoroutines();//Week 7
                StartCoroutine(Attack());
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }


        yield return new WaitForSeconds(1);
        StartCoroutine(Move());

    }*/

    //Week7
    IEnumerator Attack()
    {
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack");
        yield return new WaitForSeconds(1);
        ChangeSpeed(mySpeed);
        myPatrol = PatrolType.Chase;
        
    }

    //week 5
    private void Hit(int _damage)
    {
        myHealth -= _damage;
        healthBar.UpdateHealthBar(myHealth, baseHealth);//Week 6
        //ScaleObject(this.gameObject, transform.localScale * 1.1f);
        
        if (myHealth <= 0)
        { 
            Die(); 
        }
        else 
        {
            PlayAnimation("Hit");
            OnEnemyHit?.Invoke(this.gameObject);
            //_GM.AddScore(myScore);
        }
    }

    private void Die()
    {
        myPatrol = PatrolType.Die;
        ChangeSpeed(0);
        GetComponent<Collider>().enabled = false; //Week7
        PlayAnimation("Die"); //Week7
        StopAllCoroutines();
        OnEnemyDie?.Invoke(this.gameObject);
        
        // _GM.AddScore(myScore * 2);
        // _EM.KillEnemy(this.gameObject);
    }

    void PlayAnimation(string _animationName)
    {
        int rnd = UnityEngine.Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
        {
            Hit(collision.gameObject.GetComponent<Projectile>().damage);
            Destroy(collision.gameObject);
        }
    }

}   