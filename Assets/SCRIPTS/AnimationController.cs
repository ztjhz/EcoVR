using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Ursaanimation.CubicFarmAnimals;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour, IAnimalStatus
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
        public float detectionRange = 10f;
        public float fleeDistance = 20f;

        private NavMeshAgent agent;
        private Transform detectedPredator;
        private Vector3 targetDirection;
        private float actionTime;

        private AnimalSpawner spawner;
        private float animationCooldownTime = 0.5f;
        private float currentCooldownTime = 0f;

        [Header("Needs")]
        [SerializeField] private int fullnessLevel = 5; // Always full
        [SerializeField] private int hydrationLevel = 5;

        void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            spawner = FindObjectOfType<AnimalSpawner>();

            SetState(AIState.Idle);

            AnimalNeedsManager.Instance?.RegisterAnimal(this);
        }

        private void OnDestroy()
        {
            AnimalNeedsManager.Instance?.UnregisterAnimal(this);
        }

        void Update()
        {
            switch (currentState)
            {
                case AIState.Idle: HandleIdleState(); break;
                case AIState.Walking: HandleWalkingState(); break;
                case AIState.Eating: HandleEatingState(); break;
                case AIState.Running: HandleRunningState(); break;
                case AIState.Dead: HandleDeadState(); break;
            }

            DetectPredator();
        }

        void HandleIdleState()
        {
            if (Random.value < 0.05f)
            {
                Vector3 wanderTarget = RandomNavSphere(transform.position, 10f);
                agent.SetDestination(wanderTarget);
                SetState(AIState.Walking);
            }
            else if (Random.value < 0.1f)
            {
                SetState(AIState.Eating);
            }
        }

        void HandleWalkingState()
        {
            currentCooldownTime -= Time.deltaTime;

            if (currentCooldownTime <= 0)
            {
                float randomChoice = Random.value;
                if (randomChoice < 0.25f) animator.Play(turn90LeftAnimation);
                else if (randomChoice < 0.5f) animator.Play(turn90RightAnimation);
                else if (randomChoice < 0.75f) animator.Play(walkBackwardsAnimation);
                else animator.Play(trotForwardAnimation);

                currentCooldownTime = animationCooldownTime;
            }

            agent.speed = moveSpeed;

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
            {
                SetState(AIState.Idle);
            }
        }

        void HandleEatingState()
        {
            if (actionTime <= 0)
            {
                animator.Play(standToSitAnimation);
                actionTime = Random.Range(2f, 5f);
            }

            actionTime -= Time.deltaTime;

            if (actionTime <= 0)
            {
                SetState(AIState.Idle);
            }
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
                SetState(AIState.Idle);
            }
        }

        void HandleDeadState()
        {
            spawner?.DecrementPreyCount(gameObject);
            Destroy(gameObject, 2f);
        }

        void SetState(AIState newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                switch (currentState)
                {
                    case AIState.Idle:
                        animator.Play(idleAnimation); break;
                    case AIState.Walking:
                        animator.Play(trotForwardAnimation); break;
                    case AIState.Eating:
                        animator.Play(standToSitAnimation); break;
                    case AIState.Running:
                        animator.Play(runForwardAnimation); break;
                }
            }
        }

        void DetectPredator()
        {
            GameObject closestPredator = null;
            float closestDistance = detectionRange;

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

            if (closestPredator != null)
            {
                detectedPredator = closestPredator.transform;
                SetState(AIState.Running);
            }
        }

        public void Die()
        {
            SetState(AIState.Dead);
        }

        Vector3 RandomNavSphere(Vector3 origin, float distance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance + origin;
            NavMeshHit navHit;
            bool isValid = NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas);
            return isValid ? navHit.position : origin;
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
            // Do nothing – prey are always full
        }

        public void ModifyHuntingRadius(float multiplier)
        {
            // Do nothing – prey don’t hunt
        }

        public bool IsPredator() => false;
    }
}
