using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    public float detectionRadius = 5f;    // Radius within which the slime detects the player
    public float attackRadius = 2f;        // Radius within which the slime initiates an attack
    public float moveSpeed = 5f;           // Speed at which the slime moves
    public float attackDelay = 1f;         // Delay between slime attacks

    private Transform player;              // Reference to the player's transform
    private NavMeshAgent navMeshAgent;     // Reference to the NavMeshAgent component
    private bool isChasing = false;        // Flag to indicate whether the slime is currently chasing the player
    private bool isAttacking = false;      // Flag to indicate whether the slime is currently attacking
    private float attackTimer = 0f;        // Timer to track the attack delay

    private enum SlimeState
    {
        Idle,
        Chase,
        Attack
    }

    private SlimeState currentState = SlimeState.Idle;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case SlimeState.Idle:
                if (distanceToPlayer <= detectionRadius)
                {
                    currentState = SlimeState.Chase;
                }
                break;

            case SlimeState.Chase:
                if (distanceToPlayer <= attackRadius)
                {
                    currentState = SlimeState.Attack;
                }
                else if (distanceToPlayer > detectionRadius)
                {
                    currentState = SlimeState.Idle;
                }

                Chase();
                break;

            case SlimeState.Attack:
                if (distanceToPlayer > attackRadius)
                {
                    currentState = SlimeState.Chase;
                    Chase();
                }
                else
                {
                    if (!isAttacking)
                    {
                        Attack();
                    }
                }
                break;
        }

        // Update attack timer
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                attackTimer = 0f;
                isAttacking = false;
            }
        }
    }

    private void Chase()
    {
        isChasing = true;
        navMeshAgent.SetDestination(player.position);
    }

    private void Attack()
    {
        // Perform attack logic here
        Debug.Log("Slime attacking the player!");

        // Set the attacking flag and start the attack timer
        isAttacking = true;
        attackTimer = 0f;
    }
}