using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class PredatorAI : MonoBehaviour
{
    public enum AIState { Idle, Walking, Hunting, Attacking, Dead }
    public AIState currentState = AIState.Idle;

    public float detectionRange = 30f;
    public float walkingSpeed = 4.5f;
    public float huntingSpeed = 8.5f;
    public float attackRange = 3.5f;
    public Animator animator;
    public float failedHuntDeathTimer = 3 * 24 * 60 * 60; // 3 days in seconds
    public float attackCooldown = 2f;  // Cooldown between attacks
    private float currentAttackCooldown = 0f;
    private NavMeshAgent agent;
    private GameObject targetPrey; // Changed to GameObject
    private float timeSinceLastHunt = 0f;
    private AnimalSpawner spawner;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange; // Set stopping distance to attack range
        agent.autoBraking = true;
        spawner = FindObjectOfType<AnimalSpawner>();

        currentState = AIState.Idle;
        SwitchAnimationState(currentState);
        currentAttackCooldown = 0;

        if (spawner == null)
        {
            Debug.LogError("AnimalSpawner not found in the scene!");
        }
    }

    void Update()
    {
        if (currentState != AIState.Dead)
        {
            // Update Hunt Timer
            if (currentState != AIState.Hunting && currentState != AIState.Attacking)
            {
                timeSinceLastHunt += Time.deltaTime;
            }

            // Check for Death by Starvation
            if (timeSinceLastHunt >= failedHuntDeathTimer)
            {
                Die();
                return;
            }

            if (currentAttackCooldown > 0)
            {
                currentAttackCooldown -= Time.deltaTime;
            }
        }

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.Walking:
                HandleWalkingState();
                break;
            case AIState.Hunting:
                HandleHuntingState();
                break;
            case AIState.Attacking:
                HandleAttackingState();
                break;
            case AIState.Dead:
                // Do nothing in dead state
                break;
        }
    }

    void HandleIdleState()
    {
        if (Random.value < 0.01f) // Small chance to start walking
        {
            currentState = AIState.Walking;
            Vector3 wanderTarget = RandomNavSphere(transform.position, 10f);
            agent.SetDestination(wanderTarget);
            SwitchAnimationState(currentState);
        }

        //Try to detect prey even while idling
        DetectPrey();

    }

    void HandleWalkingState()
    {
        agent.speed = walkingSpeed;
        if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

        //Try to detect prey even while walking
        DetectPrey();
    }

    void HandleHuntingState()
    {
        agent.speed = huntingSpeed;
        if (targetPrey == null)
        {
            //Lost the prey, go back to idle
            timeSinceLastHunt += Time.deltaTime;
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
            return;
        }

        agent.SetDestination(targetPrey.transform.position);

        float distanceToPrey = Vector3.Distance(transform.position, targetPrey.transform.position);
        if (distanceToPrey <= attackRange && currentAttackCooldown <= 0)
        {
            currentState = AIState.Attacking;
            SwitchAnimationState(currentState);
        }
    }

    void HandleAttackingState()
    {
        if (targetPrey != null)
        {
            //Stop moving
            agent.SetDestination(transform.position);
            AttackPrey();
            currentAttackCooldown = attackCooldown;
            timeSinceLastHunt = 0f; // Reset timer when attacking.
        }
        else
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
            timeSinceLastHunt += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If we weren't hunting anything, and this is prey, start hunting it
        if (currentState != AIState.Hunting && other.CompareTag("prey"))
        {
            //Check if this potential prey is closer than what we're already hunting.
            if (targetPrey == null)
            {
                targetPrey = other.gameObject;
                currentState = AIState.Hunting;
                SwitchAnimationState(currentState);
            }
            else
            {
                if (Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, targetPrey.transform.position))
                {
                    targetPrey = other.gameObject;
                    currentState = AIState.Hunting;
                    SwitchAnimationState(currentState);
                }
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        //Same as trigger enter, but for stay. This is to avoid missing anything while we're attacking.
        if (currentState != AIState.Hunting && other.CompareTag("prey"))
        {
            //Check if this potential prey is closer than what we're already hunting.
            if (targetPrey == null)
            {
                targetPrey = other.gameObject;
                currentState = AIState.Hunting;
                SwitchAnimationState(currentState);
            }
            else
            {
                if (Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, targetPrey.transform.position))
                {
                    targetPrey = other.gameObject;
                    currentState = AIState.Hunting;
                    SwitchAnimationState(currentState);
                }
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        //If the prey we're hunting exits, stop hunting it.
        if (other.gameObject == targetPrey && other.CompareTag("prey"))
        {
            targetPrey = null;
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }
    }


    void DetectPrey()
    {
        //Find the closest prey.
        GameObject closestPrey = null;
        float closestDistance = detectionRange; //only find things in our detection range.

        //Find all prey in the scene.
        GameObject[] allPrey = GameObject.FindGameObjectsWithTag("prey");
        foreach (GameObject prey in allPrey)
        {
            float distance = Vector3.Distance(transform.position, prey.transform.position);
            if (distance < closestDistance)
            {
                closestPrey = prey;
                closestDistance = distance;
            }
        }

        //If we found something, hunt it!
        if (closestPrey != null)
        {
            targetPrey = closestPrey;
            currentState = AIState.Hunting;
            SwitchAnimationState(currentState);
        }
    }

    void AttackPrey()
    {
        if (targetPrey == null)
        {
            // Prey is already dead or missing
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
            return;
        }

        // Play attack animation
        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        Debug.Log(gameObject.name + " is attacking " + targetPrey.name);

        // Damage the prey (assuming the prey has a health script)
        PreyAI prey = targetPrey.GetComponent<PreyAI>();
        if (prey != null)
        {
            prey.Die(); // Call the prey's Die method
        }
        else
        {
            Debug.LogWarning("Prey doesn't have a PreyAI script!");
        }

        // Go back to idle after attack
        currentState = AIState.Idle;
        SwitchAnimationState(currentState);
        currentAttackCooldown = attackCooldown;
        targetPrey = null;
    }

    void Die()
    {
        if (spawner != null)
        {
            spawner.DecrementPredatorCount();
        }
        else
        {
            Debug.LogError("AnimalSpawner not found, unable to decrement predator count.");
        }

        currentState = AIState.Dead;
        SwitchAnimationState(currentState);

        // Destroy the GameObject after a delay
        Destroy(gameObject, 2f);
    }

    void SwitchAnimationState(AIState state)
    {
        if (animator)
        {
            animator.SetBool("isWalking", state == AIState.Walking);
            animator.SetBool("isHunting", state == AIState.Hunting);
            animator.SetBool("isAttacking", state == AIState.Attacking);
            animator.SetBool("isDead", state == AIState.Dead);
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return navHit.position;
    }
}