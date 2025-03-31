using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Ursaanimation.CubicFarmAnimals;
using System.Collections.Generic;

public class PredatorAI : MonoBehaviour, IAnimalStatus
{
    public enum AIState { Idle, Walking, Hunting, Attacking, Dead }
    public AIState currentState = AIState.Idle;

    public float detectionRange = 30f;
    public float walkingSpeed = 4.5f;
    public float huntingSpeed = 8.5f;
    public float attackRange = 3.5f;
    public Animator animator;
    public float attackCooldown = 2f;

    private float currentAttackCooldown = 0f;
    private NavMeshAgent agent;
    private GameObject targetPrey;
    private AnimalSpawner spawner;

    [Header("Needs")]
    [SerializeField] private int fullnessLevel = 5;
    [SerializeField] private int hydrationLevel = 5;
    private float baseDetectionRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.autoBraking = true;
        spawner = FindObjectOfType<AnimalSpawner>();

        baseDetectionRange = detectionRange;

        currentState = AIState.Idle;
        SwitchAnimationState(currentState);

        AnimalNeedsManager.Instance?.RegisterAnimal(this);
    }

    private void OnDestroy()
    {
        AnimalNeedsManager.Instance?.UnregisterAnimal(this);
    }

    void Update()
    {
        if (currentState == AIState.Dead) return;

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        switch (currentState)
        {
            case AIState.Idle: HandleIdleState(); break;
            case AIState.Walking: HandleWalkingState(); break;
            case AIState.Hunting: HandleHuntingState(); break;
            case AIState.Attacking: HandleAttackingState(); break;
        }
    }

    void HandleIdleState()
    {
        if (Random.value < 0.05f)
        {
            currentState = AIState.Walking;
            Vector3 wanderTarget = RandomNavSphere(transform.position, 10f);
            agent.SetDestination(wanderTarget);
            SwitchAnimationState(currentState);
        }

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
        DetectPrey();
    }

    void HandleHuntingState()
    {
        agent.speed = huntingSpeed;

        if (targetPrey == null)
        {
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
            agent.SetDestination(transform.position);
            AttackPrey();
            currentAttackCooldown = attackCooldown;
        }
        else
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }
    }

    void DetectPrey()
    {
        GameObject closestPrey = null;
        float closestDistance = detectionRange;

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
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
            return;
        }

        if (animator)
        {
            animator.SetTrigger("Attack");
        }

        AnimationController prey = targetPrey.GetComponent<AnimationController>();
        if (prey != null)
        {
            prey.Die();

            // Regain fullness
            if (fullnessLevel < 5)
                fullnessLevel += 1;
        }

        currentState = AIState.Idle;
        SwitchAnimationState(currentState);
        currentAttackCooldown = attackCooldown;
        targetPrey = null;
    }

    void Die()
    {
        spawner?.DecrementPredatorCount(gameObject);
        currentState = AIState.Dead;
        SwitchAnimationState(currentState);
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

    // === IAnimalStatus Implementation ===

    public void DecreaseHydration()
    {
        hydrationLevel = Mathf.Max(0, hydrationLevel - 1);
        if (hydrationLevel == 0) Die();
    }

    public void IncreaseHydration()
    {
        hydrationLevel = Mathf.Min(5, hydrationLevel + 1);
    }

    public void DecreaseFullness()
    {
        fullnessLevel = Mathf.Max(0, fullnessLevel - 1);
        if (fullnessLevel == 0) Die();
    }

    public void ModifyHuntingRadius(float multiplier)
    {
        detectionRange = baseDetectionRange * multiplier;
    }

    public bool IsPredator() => true;
}
