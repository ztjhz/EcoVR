using UnityEngine;
using System.Collections;

public class BackrockStudiosController : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 1f; // Movement speed
    public float turnSpeed = 50f; // Rotation speed
    public float changeDirectionInterval = 6f; // Time interval to change direction
    public float idleDuration = 4f; // Time spent idle

    private float directionChangeTimer;
    private float currentTurnDirection;
    private bool isInWater = false; // Tracks if the creature is in water
    private bool isIdle = false; // Tracks if the creature is idling

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>(); // Auto-assign if not set
        }

        directionChangeTimer = changeDirectionInterval;
        PickNewDirection();
        StartCoroutine(RandomIdleRoutine());
    }

    private void Update()
    {
        if (isIdle) return; // If idle, don't move

        // Move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        // Handle random turning logic
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            PickNewDirection();
            directionChangeTimer = changeDirectionInterval;
        }

        // Apply rotation
        transform.Rotate(Vector3.up, currentTurnDirection * turnSpeed * Time.deltaTime);

        // Update animation states
        //animator.SetBool("isWalking", !isInWater && !isIdle);
        //animator.SetBool("isSwimming", isInWater && !isIdle);
    }

    private void PickNewDirection()
    {
        // Randomly decide to turn left, right, or go straight
        float[] possibleTurns = { -1f, 0f, 1f }; // -1 = Left, 0 = Straight, 1 = Right
        currentTurnDirection = possibleTurns[Random.Range(0, possibleTurns.Length)];
    }

    private IEnumerator RandomIdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 15f)); // Random wait before idling

            isIdle = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isSwimming", false);

            yield return new WaitForSeconds(idleDuration); // Stay idle for 4 seconds

            isIdle = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect if entering water
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detect if leaving water
        if (other.CompareTag("Water"))
        {
            isInWater = false;
        }
    }
}
