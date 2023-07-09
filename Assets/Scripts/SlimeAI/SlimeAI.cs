using UnityEngine;
using UnityEngine.AI;

public class SlimeAI : MonoBehaviour
{
    public float detectionRadius = 10f;    // Radius within which the slime detects the Granny
    public float attackRadius = 2f;        // Radius within which the slime initiates an attack
    public float moveSpeed = 5f;           // Speed at which the slime moves
    public float attackDelay = 1f;         // Delay between slime attacks
    public float wanderSpeed = 2f;         // Speed at which the slime wanders
    public float wanderDurationMin = 2f;   // Minimum duration for which the slime wanders
    public float wanderDurationMax = 5f;   // Maximum duration for which the slime wanders
    public float idleDurationMin = 1f;     // Minimum duration for which the slime stays idle
    public float idleDurationMax = 3f;     // Maximum duration for which the slime stays idle
    public float pushForce = 10f;

    private Transform granny;              // Reference to the Granny's transform
    private NavMeshAgent navMeshAgent;     // Reference to the NavMeshAgent component
    private bool isChasing = false;        // Flag to indicate whether the slime is currently chasing the Granny
    private bool isAttacking = false;      // Flag to indicate whether the slime is currently attacking
    private bool isWandering = false;      // Flag to indicate whether the slime is currently wandering
    private float attackTimer = 0f;        // Timer to track the attack delay
    private float wanderTimer = 0f;        // Timer to track the wander duration
    private float idleTimer = 0f;          // Timer to track the idle duration
    private Vector3 wanderDestination;     // Destination for the wander state

    private enum SlimeState
    {
        Idle,
        Chase,
        Attack,
        Wander
    }

    private SlimeState currentState = SlimeState.Idle;

    private void Start()
    {
        granny = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Initialize idle timer
        idleTimer = Random.Range(idleDurationMin, idleDurationMax);
    }

    private void Update()
    {
        float distanceToGranny = Vector3.Distance(transform.position, granny.position);

        switch (currentState)
        {
            case SlimeState.Idle:
                idleTimer -= Time.deltaTime;

                if (distanceToGranny <= detectionRadius)
                {
                    currentState = SlimeState.Chase;
                }
                else if (idleTimer <= 0f)
                {
                    currentState = SlimeState.Wander;
                    wanderTimer = Random.Range(wanderDurationMin, wanderDurationMax);

                    // Set a new random destination for wandering
                    wanderDestination = GetRandomPointInRadius(transform.position, 5f);
                }
                break;

            case SlimeState.Chase:
                if (distanceToGranny <= attackRadius)
                {
                    currentState = SlimeState.Attack;
                }
                else if (distanceToGranny > detectionRadius)
                {
                    currentState = SlimeState.Idle;

                    // Reset idle timer
                    idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                }

                Chase();
                break;

            case SlimeState.Attack:
                if (distanceToGranny > attackRadius)
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

            case SlimeState.Wander:
                wanderTimer -= Time.deltaTime;

                if (distanceToGranny <= detectionRadius)
                {
                    currentState = SlimeState.Chase;
                }
                else if (wanderTimer <= 0f)
                {
                    currentState = SlimeState.Idle;

                    // Reset idle timer
                    idleTimer = Random.Range(idleDurationMin, idleDurationMax);
                }
                else if (!isWandering)
                {
                    Wander();
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
        navMeshAgent.SetDestination(granny.position);
    }

    private void Attack()
    {
        Debug.Log("Attacking Granny");

        // Get the GrannyHealth component from the Granny object
        GrannyHealth grannyHealth = granny.GetComponent<GrannyHealth>();

        // Check if the Granny has the GrannyHealth component
        if (grannyHealth != null)
        {
            // Perform attack logic here
            grannyHealth.TakeDamage(1); // Or specify the desired damage value
        }

        // Calculate the direction from the slime to the player
        Vector3 attackDirection = granny.position - transform.position;
        //attackDirection.y = 0f; // Ignore vertical direction

        // Apply force to the player to simulate a push effect
        CharacterController playerController = granny.GetComponent<CharacterController>();
        ThirdPersonMovement ThirdPersonMovement = granny.GetComponent<ThirdPersonMovement>();
        if (playerController != null)
        {
            // Calculate the movement vector
            Vector3 movement = attackDirection.normalized * pushForce;

            // Apply the movement over a duration
            StartCoroutine(PushPlayer(playerController, ThirdPersonMovement, movement, 2f));
        }

        // Set the attacking flag and start the attack timer
        isAttacking = true;
        attackTimer = 0f;
    }

    private System.Collections.IEnumerator PushPlayer(CharacterController controller, ThirdPersonMovement movementScript, Vector3 movement, float duration)
    {
        // Disable the ThirdPersonMovement script
        movementScript.enabled = false;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Move the player character
            controller.Move(movement * Time.deltaTime);

            if (IsPlayerGrounded(controller))
            {
                yield return null;
                // Enable the ThirdPersonMovement script
                movementScript.enabled = true;
            }

            elapsed += Time.deltaTime;
            yield return null;

            // Enable the ThirdPersonMovement script
            movementScript.enabled = true;


        }
    }

    private bool IsPlayerGrounded(CharacterController playerController)
    {
        float groundDistance = 0.4f; // Adjust the ground distance as needed
        LayerMask groundMask = LayerMask.GetMask("Ground"); // Adjust the ground layer mask as needed

        Vector3 groundCheckPosition = playerController.transform.position + playerController.center;
        bool isGrounded = Physics.CheckSphere(groundCheckPosition, groundDistance, groundMask);

        return isGrounded;
    }

    private void Wander()
    {
        if (!isWandering)
        {
            isWandering = true;
        }

        // Move towards the wander destination at the wander speed
        navMeshAgent.SetDestination(wanderDestination);
    }

    private Vector3 GetRandomPointInRadius(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit navMeshHit;
        NavMesh.SamplePosition(randomDirection, out navMeshHit, radius, NavMesh.AllAreas);
        return navMeshHit.position;
    }
}