using UnityEngine;

namespace ShootEmUp.Controllers
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PlayerInputHandler playerInput;
        [SerializeField] private bool isPlayer; 

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (playerInput != null && isPlayer)
            {
                Vector2 moveDirection = playerInput.GetMovementInput().normalized;
                Move(moveDirection);
            }
        }

        public void Move(Vector2 direction)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
    }
}
