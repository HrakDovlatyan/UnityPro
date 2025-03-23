using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isPlayerBullet;
    private int damage;
    private float lifetime = 3f;
    private float timer;
    private Vector2 velocity;

    public void Initialize(bool isPlayerBullet, int damage, Vector2 velocity, Color color)
    {
        this.isPlayerBullet = isPlayerBullet;
        this.damage = damage;
        this.velocity = velocity;

        if (rb != null)
        {
            rb.MovePosition(velocity);
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }

        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            DisableBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            return;
        }

        bool hitTarget = false;

        if (isPlayerBullet && other.CompareTag("Enemy"))
        {
            var healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
                hitTarget = true;

                Debug.Log("Hit enemy for " + damage + " damage!");
            }
        }
        else if (!isPlayerBullet && other.CompareTag("Character"))
        {
            var healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(damage);
                hitTarget = true;

                Debug.Log("Player hit for " + damage + " damage!");
            }
        }

        if (hitTarget)
        {
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        if (rb != null)
        {
            rb.MovePosition(Vector2.zero);
        }

        BulletSystem bulletSystem = GetComponentInParent<BulletSystem>();
        if (bulletSystem != null)
        {
            bulletSystem.ReturnBulletToPool(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
