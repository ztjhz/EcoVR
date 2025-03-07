using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PreyAI : MonoBehaviour
{
    public enum AIState { Idle, Walking, Eating, Running, Dead }
    public AIState currentState = AIState.Idle;

    public float detectionRange = 15f;
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 7f;
    public float fleeDistance = 10f;
    public Animator animator;
    public float deathTimer = 3f; // Time before despawning after death

    private NavMeshAgent agent;
    private Transform predator;
    private float deathCountdown = 0f;
    private AnimalSpawner spawner;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0f;
        agent.autoBraking = true;
        spawner = FindObjectOfType<AnimalSpawner>();

        currentState = AIState.Idle;
        SwitchAnimationState(currentState);

        if (spawner == null)
        {
            Debug.LogError("AnimalSpawner not found in the scene!");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.Walking:
                HandleWalkingState();
                break;
            case AIState.Eating:
                HandleEatingState();
                break;
            case AIState.Running:
                HandleRunningState();
                break;
            case AIState.Dead:
                HandleDeadState();
                break;
        }
    }

    void HandleIdleState()
    {
        void HandleIdleState()
        {
            if (Random.value < 0.01f) // Small chance to start walking
            {
                currentState = AIState.Walking;
                Vector3 wanderTarget = RandomNavSphere(transform.position, 5f);

                // Ensure agent is active and on NavMesh before setting destination
                if (agent != null && agent.isActiveAndEnabled)
                {
                    NavMeshHit hit; // Declare hit here
                    if (NavMesh.SamplePosition(wanderTarget, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        agent.SetDestination(hit.position);
                    }
                    else
                    {
                        Debug.LogError("Failed to find valid NavMesh position.");
                    }
                }
                else
                {
                    Debug.LogError("NavMeshAgent is not active or null.");
                }

                SwitchAnimationState(currentState);
            }

            DetectPredator();
        }
    }

    void HandleWalkingState()
    {
        agent.speed = walkingSpeed;
        if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

        DetectPredator();
    }

    void HandleEatingState()
    {
        if (Random.value < 0.005f)
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

        DetectPredator();
    }

    void HandleRunningState()
    {
        if (predator != null)
        {
            Vector3 runTo = transform.position + ((transform.position - predator.position).normalized * fleeDistance);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(runTo, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
        else
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

        if (Vector3.Distance(transform.position, predator.position) > 10)
        {
            predator = null;
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

    }

    void HandleDeadState()
    {
        deathCountdown -= Time.deltaTime;
        if (deathCountdown <= 0f)
        {
            // Time to despawn
            DieNow();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("predator"))
        {
            predator = other.transform;
            currentState = AIState.Running;
            SwitchAnimationState(currentState);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("predator"))
        {
            if (Vector3.Distance(transform.position, other.transform.position) > 10)
            {
                predator = null;
            }
        }
    }

    void DetectPredator()
    {
        //Detect closest predator
        GameObject closestPredator = null;
        float closestDistance = detectionRange; //only find things in our detection range.

        //Find all predators in the scene.
        GameObject[] allPredators = GameObject.FindGameObjectsWithTag("predator");
        foreach (GameObject predator in allPredators)
        {
            float distance = Vector3.Distance(transform.position, predator.transform.position);
            if (distance < closestDistance)
            {
                closestPredator = predator;
                closestDistance = distance;
            }
        }

        //If we found something, run!
        if (closestPredator != null)
        {
            predator = closestPredator.transform;
            currentState = AIState.Running;
            SwitchAnimationState(currentState);
        }
    }

    public void Die()
    {
        if (spawner != null)
        {
            spawner.DecrementPreyCount();
        }
        else
        {
            Debug.LogError("AnimalSpawner not found, unable to decrement prey count.");
        }

        currentState = AIState.Dead;
        SwitchAnimationState(currentState);
        agent.isStopped = true; //stop moving when dead
        deathCountdown = deathTimer; // Start the death countdown
    }

    void DieNow()
    {
        Destroy(gameObject); // Now destroy the object
    }

    void SwitchAnimationState(AIState state)
    {
        if (animator)
        {
            animator.SetBool("isEating", state == AIState.Eating);
            animator.SetBool("isRunning", state == AIState.Running);
            animator.SetBool("isWalking", state == AIState.Walking);
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
