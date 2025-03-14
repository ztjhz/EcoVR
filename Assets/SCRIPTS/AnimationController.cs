using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour
    {
        public enum AIState { Idle, Walking, Eating, Running, Dead }
        public AIState currentState = AIState.Idle;

        public Animator animator;
        public string walkForwardAnimation = "walk_forward";
        public string idleAnimation = "idle";
        public string runForwardAnimation = "run_forward";
        public string trotForwardAnimation = "trot_forward";
        public string walkBackwardsAnimation = "walk_backwards";
        public string turn90LeftAnimation = "turn_90_L";
        public string turn90RightAnimation = "turn_90_R";
        public string standToSitAnimation = "stand_to_sit";
        public string sitToStandAnimation = "sit_to_stand";

        public float moveSpeed = 2f;
        public float rotationSpeed = 100f;
        public float detectionRange = 10f;  // Range to detect predators
        public float fleeDistance = 20f;   // Distance to flee from predators

        private NavMeshAgent agent;
        private Transform detectedPredator;
        private Vector3 targetDirection;
        private float actionTime;

        private AnimalSpawner spawner;
        private float animationCooldownTime = 0.5f;  // Cooldown time between animations (in seconds)
        private float currentCooldownTime = 0f;  // Current cooldown time


        void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            spawner = FindObjectOfType<AnimalSpawner>();

            // Initialize the state to idle
            SetState(AIState.Idle);
        }

        void Update()
        {
            // Handle state-specific actions
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

            DetectPredator();
        }

        void HandleIdleState()
        {
            // Lower chance of moving to Walking state
            if (Random.value < 0.05f)  // 5% chance to move randomly
            {
                Vector3 wanderTarget = RandomNavSphere(transform.position, 10f);
                agent.SetDestination(wanderTarget);
                SetState(AIState.Walking);
            }
            else if (Random.value < 0.1f)  // 10% chance to start eating
            {
                SetState(AIState.Eating);  // Transition to Eating state
            }
            Debug.Log("Current State: Idle");
        }

        void HandleWalkingState()
        {
            // Decrease cooldown time
            currentCooldownTime -= Time.deltaTime;

            // Only change the animation if the cooldown has passed
            if (currentCooldownTime <= 0)
            {
                // Randomly choose between different walking animations
                float randomChoice = Random.value;

                if (randomChoice < 0.25f)  // 25% chance to turn left
                {
                    animator.Play(turn90LeftAnimation);
                }
                else if (randomChoice < 0.5f)  // 25% chance to turn right
                {
                    animator.Play(turn90RightAnimation);
                }
                else if (randomChoice < 0.75f)  // 25% chance to walk backwards
                {
                    animator.Play(walkBackwardsAnimation);
                }
                else  // 25% chance to trot forward
                {
                    animator.Play(trotForwardAnimation);
                }

                // Reset the cooldown time after a transition
                currentCooldownTime = animationCooldownTime;
            }

            agent.speed = moveSpeed;

            // Check for predator detection while walking
            DetectPredator();

            // Check if the animal has stopped moving and transition to Idle
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
            {
                SetState(AIState.Idle);
            }

            //Debug.Log("Current State: Walking");
        }



        void HandleEatingState()
        {
            // If it's the first time entering the Eating state, set a random eating duration
            if (actionTime <= 0)
            {
                // Play the 'stand to sit' animation first
                animator.Play(standToSitAnimation);
                actionTime = Random.Range(2f, 5f);  // Random time to simulate eating, adjust range as needed
            }

            // Decrease the action time to simulate the eating duration
            actionTime -= Time.deltaTime;

            // Detect predators while eating
            DetectPredator();  // Check for predators

            // Once the action time is finished, transition to Idle or Walking
            if (actionTime <= 0)
            {
                SetState(AIState.Idle);  // Transition to Idle (or Walking if needed)
            }

            Debug.Log("Current State: Eating");
        }



        void HandleRunningState()
        {
            if (detectedPredator != null)
            {
                Vector3 fleeDirection = (transform.position - detectedPredator.position).normalized;
                Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

                agent.SetDestination(fleeTarget);
                animator.Play(runForwardAnimation);
            }
            else
            {
                // If no predator is detected, stop running and transition back to Idle
                SetState(AIState.Idle);
            }

            Debug.Log("Current State: Running");
        }


        void HandleDeadState()
        {
            FindObjectOfType<AnimalSpawner>()?.DecrementPreyCount(gameObject);
            if (spawner != null)
                spawner.DecrementPreyCount(gameObject);

            Destroy(gameObject, 2f);  // Destroy the object after 2 seconds.
            Debug.Log("Current State: Dead");
        }

        // Switches the current state and updates the animation parameters
        void SetState(AIState newState)
        {
            if (currentState != newState)  // Prevent redundant log messages
            {
                currentState = newState;
                switch (currentState)
                {
                    case AIState.Idle:
                        animator.Play(idleAnimation);
                        break;

                    case AIState.Walking:
                        // Check if the cooldown has expired before playing a new animation
                        if (currentCooldownTime <= 0)
                        {
                            // Randomly choose between different walking animations when entering Walking state
                            float randomChoice = Random.value;

                            if (randomChoice < 0.25f)  // 25% chance to turn left
                            {
                                animator.Play(turn90LeftAnimation);
                            }
                            else if (randomChoice < 0.5f)  // 25% chance to turn right
                            {
                                animator.Play(turn90RightAnimation);
                            }
                            else if (randomChoice < 0.75f)  // 25% chance to walk backwards
                            {
                                animator.Play(walkBackwardsAnimation);
                            }
                            else  // 25% chance to trot forward
                            {
                                animator.Play(trotForwardAnimation);
                            }

                            // Reset the cooldown time after a transition
                            currentCooldownTime = animationCooldownTime;
                        }
                        break;

                    case AIState.Eating:
                        animator.Play(standToSitAnimation);  
                        break;

                    case AIState.Running:
                        animator.Play(runForwardAnimation);
                        break;

                    case AIState.Dead:
                        break;
                }
            }
        }



        // Detects predators from the serialized list of predator prefabs
        void DetectPredator()
        {
            GameObject closestPredator = null;
            float closestDistance = detectionRange;

            GameObject[] allPredators = GameObject.FindGameObjectsWithTag("predator");
            foreach (GameObject predator in allPredators)
            {
                // Calculate the distance from the prey to the predator
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
                Debug.Log("Predator detected! Transitioning to Running.");
                SetState(AIState.Running);
            }
        }

        // Called by predator
        public void Die()
        {
            SetState(AIState.Dead);
        }

        // Get a random position on the NavMesh
        Vector3 RandomNavSphere(Vector3 origin, float distance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;

            NavMeshHit navHit;
            bool isValid = NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);

            return isValid ? navHit.position : origin;  // Return original position if NavMesh position isn't found
        }
    }
}
