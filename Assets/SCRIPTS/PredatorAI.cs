using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ursaanimation.CubicFarmAnimals;

public class PredatorAI : MonoBehaviour
{
    public enum AIState { Idle, Walking, Hunting, Attacking, Dead }
    public AIState currentState = AIState.Idle;

    public float detectionRange = 30f;
    public float walkingSpeed = 4.5f;
    public float huntingSpeed = 8.5f;
    public float attackRange = 3.5f;
    public Animator animator;
    public float failedHuntDeathTimer = 3 * 24 * 60 * 60;
    public float attackCooldown = 2f;
    private float currentAttackCooldown = 0f;
    public enum HungerState { NotHungry, Level1, Level2, Level3 }
    public HungerState hungerLevel = HungerState.NotHungry;
    private float hungerTimer = 0f;
    private const float dayDuration = 24 * 60 * 60; // 1 day in seconds

    private NavMeshAgent agent;
    private GameObject targetPrey;
    private float timeSinceLastHunt = 0f;
    private AnimalSpawner spawner;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
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
            if (currentState != AIState.Hunting && currentState != AIState.Attacking)
            {
                timeSinceLastHunt += Time.deltaTime;
                hungerTimer += Time.deltaTime; // Track hunger duration
            }

            if (hungerTimer >= dayDuration)
            {
                IncreaseHunger();
                hungerTimer = 0f; // Reset daily hunger check
            }

            if (hungerLevel == HungerState.Level3 && hungerTimer >= dayDuration)
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
            case AIState.Idle: HandleIdleState(); break;
            case AIState.Walking: HandleWalkingState(); break;
            case AIState.Hunting: HandleHuntingState(); break;
            case AIState.Attacking: HandleAttackingState(); break;
            case AIState.Dead: break;
        }
    }

    void IncreaseHunger()
    {
        if (hungerLevel == HungerState.NotHungry)
        {
            hungerLevel = HungerState.Level1;
        }
        else if (hungerLevel == HungerState.Level1)
        {
            hungerLevel = HungerState.Level2;
        }
        else if (hungerLevel == HungerState.Level2)
        {
            hungerLevel = HungerState.Level3;
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
        agent.speed = walkingSpeed;  // Ensure correct speed
        if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            currentState = AIState.Idle;
            SwitchAnimationState(currentState);
        }
        DetectPrey();
    }

    void HandleHuntingState()
    {
        agent.speed = huntingSpeed;  // Ensure correct speed
        if (targetPrey == null)
        {
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
            agent.SetDestination(transform.position);
            AttackPrey();
            currentAttackCooldown = attackCooldown;
            timeSinceLastHunt = 0f;
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

            // Reduce hunger level based on the current state
            if (hungerLevel == HungerState.Level3)
            {
                hungerLevel = HungerState.Level2; // Eating at Level 3 resets to Level 2
            }
            else
            {
                hungerLevel = HungerState.NotHungry; // Eating at Level 1 or 2 resets fully
            }

            timeSinceLastHunt = 0f;
            hungerTimer = 0f; // Reset hunger timer
        }

        currentState = AIState.Idle;
        SwitchAnimationState(currentState);
        currentAttackCooldown = attackCooldown;
        targetPrey = null;
    }

    void Die()
    {
        FindObjectOfType<AnimalSpawner>()?.DecrementPredatorCount(gameObject);

        if (spawner != null)
        {
            spawner.DecrementPredatorCount(gameObject);
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