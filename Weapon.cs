using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public int curAmmo;
    public int maxAmmo;
    private int magAmmo = 30;
    public float bulletSpeed;
    private int leftOverAmmo;

    private float lastShootTime;
    public float shootRate;
    private bool isPlayer;    

    public ObjectPool bulletPool;
    public Transform bulletSpawnLocation;

    //components
    private Player player;
    private Rigidbody rig;
    private UI uI;
    private Bullet bullet;

    public static Weapon instance;

    void Awake()
    {
        instance = this;
        player = GetComponent<Player>();
        uI = GetComponent<UI>();
    }

    void Start()
    {
        leftOverAmmo = 0;
    }

    public bool TryShoot() 
    {
        if (curAmmo > 0 && Time.time - lastShootTime >= shootRate)
            return true;
        else
            return false;
    }

    public void Shoot()
    {
        curAmmo --;
        leftOverAmmo++;
        lastShootTime = Time.time;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetpoint;
        if (Physics.Raycast(ray, out hit))
            targetpoint = hit.point;
        else
            targetpoint = ray.GetPoint(1000);

        GameObject bullet = bulletPool.GetObject();

        bullet.transform.position = bulletSpawnLocation.position;
        bullet.transform.rotation = bulletSpawnLocation.rotation;

        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnLocation.forward * bulletSpeed;

        UI.instance.UpdateAmmoText();
    }

    public void Reload() 
    {
        if (curAmmo == 0 || curAmmo < 30) {
            curAmmo = magAmmo;
            maxAmmo = maxAmmo - leftOverAmmo;
            new WaitForSeconds(1.5f);
            UI.instance.UpdateAmmoText();
        }
    }
}