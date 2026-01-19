using System.Security.Cryptography;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    public float ShootRate { get => shootRate; set => shootRate = value; }

    private ObjectPool objectPool;
    private float lastShootTime;

    private bool isPlayer;

    private void Awake()
    {
        //Check if I am a player
        isPlayer = gameObject.CompareTag("Player");

        //get ObjectPool
        objectPool = GetComponent<ObjectPool>();
    }
    
    /// <summary>
    /// Handle Weapon Shoot
    /// </summary>
    public void Shoot()
    {
        //update last shoot time
        lastShootTime = Time.time;

        if (!infiniteAmmo) currentAmmo--;

        //get a new active bullet
        GameObject bullet = objectPool.GetGameObject();

        if (isPlayer)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;
            Vector3 targetPoint;

            //Check if the ray hit with something and adjust direction
            if(Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(5); //Get a point at 5m
            }
            bullet.GetComponent<Rigidbody>().linearVelocity = (targetPoint - barrel.position).normalized * bulletSpeed;

        }
        //if Enemy
        else
        {
            //ToDO Random directions near player
            bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
        }


            //position and rotation
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;

        bullet.GetComponent<BulletController>().Damage = damage;
       
    }

    /// <summary>
    /// Check if its posible to shoot
    /// </summary>
    /// <returns></returns>
    public bool CanShoot()
    {
        //Check shootRate
        if(Time.time - lastShootTime >= shootRate)
        {
            //Check Ammo
            if (currentAmmo > 0 || infiniteAmmo)
            {
                return true;
            }

        }
        return false;
    }
}
