using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{

    public NavMeshAgent enemy;
    public Transform player;
    GameObject target;
    Animator anim;

    [SerializeField] float stoppingDistance;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist < stoppingDistance) {
            stopEnemy();
            //attack();
        }
        else {
            goToTarget();
        }
    }

    private void goToTarget() {
        enemy.isStopped = false;
        enemy.SetDestination(player.position);
        anim.SetBool("isWalking", true);
    }

    // private void attack() {
    //     int x;
    //     x = 0;
    // }

    private void stopEnemy() { 
        enemy.isStopped = true;
        anim.SetBool("isWalking", false);
    }
}
