using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    private bool isattackingnow;
    public NavMeshAgent enemy;
    public float damage =10;
    public Transform player;
    Animator anim;

    [SerializeField] float stoppingDistance;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", true);
            Debug.Log("В триггере");
            isattackingnow = true;
            InvokeRepeating("DealDamage", 1f, 1f);
            enemy.SetDestination(Vector3.zero);
            enemy.enabled = false;
        }
        

    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("isAttacking", false);
            Debug.Log("Не в триггере");
            isattackingnow = false;
            CancelInvoke("DealDamage"); // Остановить нанесение урона
            enemy.enabled = true;
        }
            
    }
    void DealDamage()
    {
        if (isattackingnow)
        {
            HealthController health = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
            if(health.hp > 0)
            {
                health.takeDamage(10);
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        goToTarget();
        if((player.transform.position - transform.position).magnitude <= 2.5)
        {
            enemy.ResetPath();
        }

    }

    private void goToTarget() {
        enemy.isStopped = false;
        enemy.SetDestination(player.transform.position);
        anim.SetBool("isWalking", true);
        transform.LookAt(player);
    }

    private void stopEnemy() { 
        enemy.isStopped = true;
        anim.SetBool("isWalking", false);
    }
}
