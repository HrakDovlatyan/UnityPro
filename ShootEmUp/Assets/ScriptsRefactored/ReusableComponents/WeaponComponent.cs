using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;

    private float nextFireTime;

    public Vector3 Position => firePoint != null ? firePoint.position : transform.position;
    public Quaternion Rotation => firePoint != null ? firePoint.rotation : transform.rotation;

    public bool CanFire => Time.time >= nextFireTime;

    public void Fire()
    {
        if (CanFire)
        {
            nextFireTime = Time.time + fireRate;
        }
    }
}
