using ShootEmUp.Systems;
using UnityEngine;

namespace ShootEmUp.Controllers
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private MovementController movement;
        [SerializeField] private WeaponComponent weapon;
        [SerializeField] private HealthComponent health;
        [SerializeField] private BulletSystem bulletSystem;
        [SerializeField] private BulletConfig bulletConfig;
        [SerializeField] private float attackPositionReachedThreshold = 0.5f;


        [SerializeField] private float attackInterval = 1.5f;
        private float lastAttackTime = 0f;


        private Transform player;
        private Vector3 attackPosition;
        private bool hasReachedAttackPosition = false;
        private bool attackPositionAssigned = false;

        private enum EnemyState
        {
            MovingToAttackPosition,
            Attacking
        }

        private EnemyState currentState = EnemyState.MovingToAttackPosition;

        private void Start()
        {
            // Find player
            GameObject playerObject = GameObject.FindGameObjectWithTag("Character");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("No GameObject with 'Character' tag found!");
            }

            // Subscribe to health events
            if (health != null)
                health.OnHealthEmpty += OnEnemyDeath;
        }

        private void OnDestroy()
        {
            if (health != null)
                health.OnHealthEmpty -= OnEnemyDeath;
        }

        private void Update()
        {
            if (player == null || !attackPositionAssigned) return;

            switch (currentState)
            {
                case EnemyState.MovingToAttackPosition:
                    HandleMovingToAttackPosition();
                    break;

                case EnemyState.Attacking:
                    HandleAttacking();
                    break;
            }
        }

        private void HandleMovingToAttackPosition()
        {
            float distanceToAttackPosition = Vector2.Distance(transform.position, attackPosition);

            if (distanceToAttackPosition <= attackPositionReachedThreshold)
            {
                hasReachedAttackPosition = true;
                currentState = EnemyState.Attacking;

                if (movement != null)
                {
                    movement.Move(Vector2.zero);
                }
            }
            else
            {
                Vector2 direction = (attackPosition - transform.position).normalized;
                if (movement != null)
                {
                    movement.Move(direction);
                }
            }
        }
        private void HandleAttacking()
        {
            if (player == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);


            if (weapon != null && weapon.CanFire && Time.time >= lastAttackTime + attackInterval)
            {
                FireAtPlayer();
                lastAttackTime = Time.time;
            }

        }
        private void FireAtPlayer()
        {
            if (player == null || bulletSystem == null) return;

            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            bulletSystem.FireBullet(new BulletArgs
            {
                IsPlayerBullet = false,
                Color = bulletConfig.Color,
                Damage = bulletConfig.Damage,
                Position = weapon.Position,
                Direction = directionToPlayer,
                Speed = bulletConfig.Speed
            });
        }

        private void OnEnemyDeath(GameObject enemy)
        {
            Destroy(gameObject);
        }

        public void SetReferences(BulletSystem bulletSystem)
        {
            this.bulletSystem = bulletSystem;
        }

        public void SetAttackPosition(Vector3 position)
        {
            attackPosition = position;
            attackPositionAssigned = true;
            currentState = EnemyState.MovingToAttackPosition;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = hasReachedAttackPosition ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(attackPosition, 0.5f);
        }
    }
}