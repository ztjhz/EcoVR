using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PreyAI : MonoBehaviour
{
    public enum AIState { Idle, Walking, Eating, Running, Dead }
    public AIState currentState = AIState.Idle;

    public float detectionRange = 15f; // How far the prey detects predators
    public float walkingSpeed = 3.5f;
    public float runningSpeed = 7f;
    public float fleeDistance = 20f; // How far the prey should flee
    public Animator animator;

    private NavMeshAgent agent;
    private Transform detectedPredator;
    private bool switchAction = false;
    private float actionTimer = 0;
    private AnimalSpawner spawner;

    [SerializeField] private List<GameObject> predatorPrefabs; // Drag predator prefabs here

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0;
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
            case AIState.Idle: HandleIdleState(); break;
            case AIState.Walking: HandleWalkingState(); break;
            case AIState.Eating: HandleEatingState(); break;
            case AIState.Running: HandleRunningState(); break;
            case AIState.Dead: break;
        }
    }

    void HandleIdleState()
    {
        if (Random.value < 0.01f) // 1% chance per frame to move
        {
            Vector3 wanderTarget = RandomNavSphere(transform.position, 10f);

            if (agent.isOnNavMesh)
            {
                agent.SetDestination(wanderTarget);
                currentState = AIState.Walking;
                SwitchAnimationState(currentState);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " is not on NavMesh! Trying to reposition.");
                RepositionToNavMesh();
            }
        }
        DetectPredator();
    }

    void HandleWalkingState()
    {
        agent.speed = walkingSpeed;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }

        DetectPredator();
    }

    void HandleEatingState()
    {
        if (switchAction)
        {
            if (!animator || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                Vector3 newDestination = RandomNavSphere(transform.position, Random.Range(3, 7));
                agent.destination = newDestination;
                currentState = AIState.Walking;
                SwitchAnimationState(currentState);
            }
        }

        DetectPredator();
    }

    void HandleRunningState()
    {
        agent.speed = runningSpeed;

        if (detectedPredator)
        {
            Vector3 fleeDirection = (transform.position - detectedPredator.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;
            Vector3 validFleePosition = RandomNavSphere(fleeTarget, 10f);

            if (agent.isOnNavMesh)
            {
                agent.SetDestination(validFleePosition);
            }
            else
            {
                RepositionToNavMesh();
            }
        }
    }

    void DetectPredator()
    {
        GameObject closestPredator = null;
        float closestDistance = detectionRange;

        GameObject[] allPredators = FindObjectsOfType<GameObject>().Where(obj => predatorPrefabs.Contains(obj)).ToArray();

        foreach (GameObject predator in allPredators)
        {
            if (!predatorPrefabs.Contains(predator)) continue;

            float distance = Vector3.Distance(transform.position, predator.transform.position);
            if (distance < closestDistance)
            {
                closestPredator = predator;
                closestDistance = distance;
            }
        }

        if (closestPredator != null)
        {
            detectedPredator = closestPredator.transform;
            currentState = AIState.Running;
            SwitchAnimationState(currentState);
        }
    }

    public void Die()
    {
        FindObjectOfType<AnimalSpawner>()?.DecrementPreyCount(gameObject);
        
        if (spawner != null)
        {
            spawner.DecrementPreyCount(gameObject);
        }

        currentState = AIState.Dead;
        SwitchAnimationState(currentState);
        Destroy(gameObject, 2f);
    }

    void SwitchAnimationState(AIState state)
    {
        if (animator)
        {
            animator.SetBool("isWalking", state == AIState.Walking);
            animator.SetBool("isEating", state == AIState.Eating);
            animator.SetBool("isRunning", state == AIState.Running);
            animator.SetBool("isDead", state == AIState.Dead);
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        NavMeshHit navHit;
        bool isValid = NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

        return isValid ? navHit.position : origin; // Return original position if NavMesh position isn't found
    }


    void RepositionToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            agent.Warp(hit.position);
            Debug.Log(gameObject.name + " repositioned onto NavMesh.");
        }
        else
        {
            Debug.LogError(gameObject.name + " could not be placed on a NavMesh!");
        }
    }

}