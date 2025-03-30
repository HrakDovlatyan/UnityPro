using ShootEmUp.Systems;
using UnityEngine;

namespace ShootEmUp.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BulletSystem bulletSystem;
        [SerializeField] private BulletConfig bulletConfig;
        [SerializeField] private WeaponComponent weapon;
        [SerializeField] private HealthComponent health;
        [SerializeField] private bool canOnlyShootUpward = true;
        [SerializeField] private PlayerInputHandler playerInput; 

        private void OnEnable()
        {
            health.OnHealthEmpty += OnPlayerDeath;
        }

        private void OnDisable()
        {
            health.OnHealthEmpty -= OnPlayerDeath;
        }

        private void Update()
        {
            if (playerInput.IsFiring())
            {
                Vector2 aimDirection = playerInput.GetAimDirection();
                RequestFire(aimDirection);
            }
        }

        private void OnPlayerDeath(GameObject player)
        {
            gameManager.FinishGame(false);
        }

        public void RequestFire(Vector2 aimDirection)
        {
            if (weapon.CanFire)
            {
                FireBullet(aimDirection);
            }
        }

        private void FireBullet(Vector2 aimDirection)
        {
            Vector3 shootDirection = canOnlyShootUpward ? Vector3.up : aimDirection.normalized;

           
            bulletSystem = FindAnyObjectByType<BulletSystem>();
            if (bulletSystem == null)
            {
                Debug.LogError("BulletSystem reference not found!");
                return;
            }

            bulletSystem.FireBullet(new BulletArgs
            {
                IsPlayerBullet = true,
                Color = bulletConfig.Color,
                Damage = bulletConfig.Damage,
                Position = weapon.Position,
                Direction = shootDirection,
                Speed = bulletConfig.Speed
            });
        }
    }
}
