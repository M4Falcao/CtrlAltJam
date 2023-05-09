using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAi : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;


    public float viewAngle = 120f; // Ângulo de visão do objeto
    public float viewRadius = 10f; // Raio de visão do objeto
    public float walkSpeed = 1.5f;
    public float runSpeed = 4f;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange, killRange;
    public bool playerInSightRange, playerInAttackRange, isAttacking;
    public bool started;


    private bool isEnemySlow = false;
    private float secondsOfSlow = 0;
    private float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        

        if (started)
        {
            if (isEnemySlow)
            {
                if(agent.speed == 0f)
                {
                    //animator.SetBool("isWalking", false);
                } else
                {
                    //animator.SetBool("isWalking", true);
                    //animator.SetFloat("WalkSpeed", agent.speed);
                    //animator.SetFloat("RunSpeed", agent.speed);

                }
                timer = timer + Time.deltaTime;
                if (timer >= (secondsOfSlow))
                {
                    isEnemySlow = false;
                }
            }
            else
            {
                if (PlayerSighted())
                {
                    agent.speed = runSpeed;
                    //animator.SetFloat("RunSpeed", agent.speed);
                    //animator.SetBool("isWalking", false);
                    //animator.SetBool("isRunning", true);
                }
                else
                {
                    agent.speed = walkSpeed;
                    //animator.SetFloat("WalkSpeed", agent.speed);
                    //animator.SetBool("isWalking", true);
                    //animator.SetBool("isRunning", false);
                }
            }

            animator.SetFloat("Speed", agent.speed);

            if (isAttacking)
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                ChasePlayer();
            }
            

            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (playerInAttackRange && PlayerSighted()) AttackPlayer();
        }

    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }


    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private bool PlayerSighted()
    {
        if (player == null)
        {
            // Jogador não atribuído, portanto, não está visível
            return false;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 1f;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);
        // Verifica se o jogador está dentro do ângulo de visão do objeto
        if (angleToPlayer <= viewAngle / 2f)
        {
            // Verifica se há obstáculos entre o objeto e o jogador
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // O jogador está dentro do campo de visão e não há obstáculos
                    playerInSightRange = true;
                    return true;
                }
            }
        }
        playerInSightRange = false;
        return false;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            ///
            animator.SetFloat("Speed", 0f);
            animator.SetTrigger("Attack");
            //Debug.Log("Attack");
            //GameObject.FindGameObjectWithTag("Player")
            //    .GetComponent<CharacterController>().Move(transform.forward * Time.deltaTime * 32f);

            ///End of attack code

            alreadyAttacked = true;
            isAttacking = true;
            //Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void Freeze(float speed, float seconds)
    {
        if (!isEnemySlow)
        {
            timer = 0;
            secondsOfSlow = seconds;
            isEnemySlow = true;
            agent.speed = speed;
        }

    }

    void KillPlayer()
    {
        float dist = (player.position - transform.position).magnitude;
        if(dist < killRange)
        {
            Debug.Log("Game over");
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, killRange);
    }
}
