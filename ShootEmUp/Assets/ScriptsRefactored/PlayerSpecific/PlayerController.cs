using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BulletSystem bulletSystem;
    [SerializeField] private BulletConfig bulletConfig;
    [SerializeField] private WeaponComponent weapon;
    [SerializeField] private HealthComponent health;
    [SerializeField] private bool canOnlyShootUpward = true;

    private void OnEnable()
    {
        if (health != null)
            health.OnHealthEmpty += OnPlayerDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnHealthEmpty -= OnPlayerDeath;
    }

    private void OnPlayerDeath(GameObject player)
    {
        gameManager.FinishGame(false); // Player died, game over
    }

    public void RequestFire(Vector2 aimDirection)
    {
        if (weapon != null && weapon.CanFire)
        {
            FireBullet(aimDirection);
        }
    }

    private void FireBullet(Vector2 aimDirection)
    {
        Vector3 shootDirection;

        if (canOnlyShootUpward)
        {
            shootDirection = Vector3.up;
        }
        else
        {
            shootDirection = aimDirection.normalized;
        }

        if (bulletSystem == null)
        {
            bulletSystem = FindAnyObjectByType<BulletSystem>();
            if (bulletSystem == null)
            {
                Debug.LogError("BulletSystem reference not found!");
                return;
            }
        }

        bulletSystem.FireBullet(new BulletSystem.BulletArgs
        {
            IsPlayerBullet = true,
            Color = bulletConfig.color,
            Damage = bulletConfig.damage,
            Position = weapon.Position,
            Direction = shootDirection,
            Speed = bulletConfig.speed
        });
    }

    public void SetReferences(GameManager gameManager, BulletSystem bulletSystem)
    {
        this.gameManager = gameManager;
        this.bulletSystem = bulletSystem;
    }
}