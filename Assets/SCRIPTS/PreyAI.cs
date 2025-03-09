using UnityEngine;
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

    private Transform detectedPredator;
    private bool switchAction = false;
    private float actionTimer = 0;
    private AnimalSpawner spawner;

    [SerializeField] private List<GameObject> predatorPrefabs;

    void Start()
    {
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
            Vector3 wanderTarget = RandomWanderTarget(transform.position, 10f);
            MoveTowards(wanderTarget);
            currentState = AIState.Walking;
            SwitchAnimationState(currentState);
        }

        DetectPredator();
    }

    void HandleWalkingState()
    {
        MoveTowards(transform.position); // Simplified movement, no agent required
        DetectPredator();
    }

    void HandleEatingState()
    {
        if (switchAction)
        {
            if (!animator || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                Vector3 newDestination = RandomWanderTarget(transform.position, Random.Range(3, 7));
                MoveTowards(newDestination);
                currentState = AIState.Walking;
                SwitchAnimationState(currentState);
            }
        }

        DetectPredator();
    }

    void HandleRunningState()
    {
        if (detectedPredator)
        {
            Vector3 fleeDirection = (transform.position - detectedPredator.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;
            Vector3 validFleePosition = RandomWanderTarget(fleeTarget, 10f);

            MoveTowards(validFleePosition);
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

    Vector3 RandomWanderTarget(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;
        randomDirection += origin;

        return randomDirection;
    }

    void MoveTowards(Vector3 target)
    {
        float speed = (currentState == AIState.Running) ? runningSpeed : walkingSpeed;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}