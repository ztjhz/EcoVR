using UnityEngine;
using System.Collections;

public class BackrockStudiosController : MonoBehaviour, IAnimalStatus
{
    public Animator animator;
    public float moveSpeed = 1f;
    public float turnSpeed = 50f;
    public float changeDirectionInterval = 6f;
    public float idleDuration = 4f;

    private float directionChangeTimer;
    private float currentTurnDirection;
    private bool isIdle = false;

    [Header("Needs")]
    [SerializeField] private int fullnessLevel = 5; // Always full
    [SerializeField] private int hydrationLevel = 5;

    private AnimalSpawner spawner;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        spawner = FindObjectOfType<AnimalSpawner>();
        AnimalNeedsManager.Instance?.RegisterAnimal(this);

        directionChangeTimer = changeDirectionInterval;
        PickNewDirection();
        StartCoroutine(RandomIdleRoutine());
    }

    private void OnDestroy()
    {
        AnimalNeedsManager.Instance?.UnregisterAnimal(this);
    }

    private void Update()
    {
        if (isIdle) return;

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            PickNewDirection();
            directionChangeTimer = changeDirectionInterval;
        }

        transform.Rotate(Vector3.up, currentTurnDirection * turnSpeed * Time.deltaTime);
    }

    private void PickNewDirection()
    {
        float[] possibleTurns = { -1f, 0f, 1f };
        currentTurnDirection = possibleTurns[Random.Range(0, possibleTurns.Length)];
    }

    private IEnumerator RandomIdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f));

            isIdle = true;
            animator.SetBool("isWalking", false);

            yield return new WaitForSeconds(idleDuration);

            isIdle = false;
        }
    }

    public void Die()
    {
        spawner?.DecrementPreyCount(gameObject);
        Destroy(gameObject, 2f);
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
    public void IncreaseFullness()
    {
        fullnessLevel = Mathf.Min(5, fullnessLevel + 1);
    }


    public void ModifyHuntingRadius(float multiplier)
    {
        // Do nothing – not a predator
    }

    public bool IsPredator() => false;
}
