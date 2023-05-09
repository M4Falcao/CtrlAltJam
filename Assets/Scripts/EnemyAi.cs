using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private NavMeshAgent agent;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer;


    public float viewAngle = 120f; // �ngulo de vis�o do objeto
    public float viewRadius = 10f; // Raio de vis�o do objeto
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
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool started;


    private bool isEnemySlow = false;
    private float secondsOfSlow = 0;
    private float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (started)
        {
            if (isEnemySlow)
            {
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
                }
                else
                {
                    agent.speed = walkSpeed;
                }
            }

            ChasePlayer();

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
            // Jogador n�o atribu�do, portanto, n�o est� vis�vel
            return false;
        }

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 1f;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);
        // Verifica se o jogador est� dentro do �ngulo de vis�o do objeto
        if (angleToPlayer <= viewAngle / 2f)
        {
            // Verifica se h� obst�culos entre o objeto e o jogador
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, viewRadius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // O jogador est� dentro do campo de vis�o e n�o h� obst�culos
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
            Debug.Log("Atack");
            GameObject.FindGameObjectWithTag("Player")
                .GetComponent<CharacterController>().Move(transform.forward * Time.deltaTime * 32f);

            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
