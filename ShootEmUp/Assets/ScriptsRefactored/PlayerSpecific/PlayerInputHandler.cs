using ShootEmUp.Controllers;
using UnityEngine;


public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        return new Vector2(horizontal, vertical);
    }

    public bool IsFiring()
    {
        return Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space);
    }

    public Vector2 GetAimDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        return (mousePosition - transform.position).normalized;
    }
}