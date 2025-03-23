using System.Collections.Generic;
using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 30;

    private List<GameObject> bulletPool;

    public class BulletArgs
    {
        public bool IsPlayerBullet;
        public int TargetLayer;
        public Color Color;
        public int Damage;
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;
    }

    private void Awake()
    {
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public void FireBullet(BulletArgs args)
    {
        GameObject bullet = GetBulletFromPool();
        if (bullet == null) return;

        bullet.transform.position = args.Position;

        float angle = Mathf.Atan2(args.Direction.y, args.Direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Initialize(
                args.IsPlayerBullet,
                args.Damage,
                args.Direction.normalized * args.Speed,
                args.Color
            );
        }

        bullet.layer = args.TargetLayer;

        bullet.SetActive(true);
    }

    private GameObject GetBulletFromPool()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab, transform);
        newBullet.SetActive(false);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
