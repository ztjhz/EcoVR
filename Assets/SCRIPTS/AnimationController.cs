using UnityEngine;

namespace Ursaanimation.CubicFarmAnimals
{
    public class AnimationController : MonoBehaviour
    {
        public Animator animator;
        public string walkForwardAnimation = "walk_forward";
        public string idleAnimation = "idle";
        public float moveSpeed = 2f;
        public float rotationSpeed = 100f;

        private Vector3 targetDirection;
        private float actionTime;

        void Start()
        {
            animator = GetComponent<Animator>();
            ChooseNewAction();
        }

        void Update()
        {
            actionTime -= Time.deltaTime;

            if (actionTime <= 0)
            {
                ChooseNewAction();
            }

            MoveAnimal();
        }

        void ChooseNewAction()
        {
            // Randomly decide whether to walk or idle
            bool shouldMove = Random.value > 0.5f;

            if (shouldMove)
            {
                animator.Play(walkForwardAnimation);
                // Pick a random direction
                float randomAngle = Random.Range(0f, 360f);
                targetDirection = Quaternion.Euler(0, randomAngle, 0) * Vector3.forward;

                // Move for 2 to 5 seconds
                actionTime = Random.Range(2f, 5f);
            }
            else
            {
                animator.Play(idleAnimation);
                targetDirection = Vector3.zero;

                // Idle for 1 to 3 seconds
                actionTime = Random.Range(1f, 3f);
            }
        }

        void MoveAnimal()
        {
            if (targetDirection != Vector3.zero)
            {
                // Rotate towards the target direction smoothly
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                // Move forward
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
    }
}
