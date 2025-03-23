using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private MovementController movement;
    [SerializeField] private PlayerController player;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 moveDirection = new Vector2(horizontal, vertical);

        if (movement != null)
        {
            movement.Move(moveDirection);
        }

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Vector2 aimDirection = (mousePosition - transform.position).normalized;

            if (player != null)
            {
                player.RequestFire(aimDirection);
            }
        }
    }
}