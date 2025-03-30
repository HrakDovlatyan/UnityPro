using ShootEmUp.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace ShootEmUp.Systems
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private BulletSystem bulletSystem;


        private bool isPlayerBullet;
        private int damage;
        private float lifeTime = 3f;
        private float timer;
        private Vector2 velocity;

        public void Initialize(bool isPlayerBullet, int damage, Vector2 velocity, Color color)
        {
            this.isPlayerBullet = isPlayerBullet;
            this.damage = damage;
            this.velocity = velocity;

            if (rigidBody != null)
            {
                rigidBody.MovePosition(velocity);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }

            timer = 0f;
        }

        private void FixedUpdate()
        {

            rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);

        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= lifeTime)
            {
                DisableBullet();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<HealthComponent>(out var healthComponent))
                return;

            bool isEnemy = other.TryGetComponent<EnemyController>(out _);
            bool isPlayer = other.TryGetComponent<PlayerController>(out _);

            if ((isPlayerBullet && isEnemy) || (!isPlayerBullet && isPlayer))
            {
                healthComponent.TakeDamage(damage);

                string target = isEnemy ? "enemy" : "player";
                Debug.Log($"Hit {target} for {damage} damage!");

                DisableBullet();
            }

        }

        private void DisableBullet()
        {

            rigidBody.MovePosition(Vector2.zero);


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
}
