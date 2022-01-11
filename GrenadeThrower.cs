using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab;
    [HideInInspector]
    public int curGrenadeAmmo;
    public int maxGrenadeAmmo = 2;

    public static GrenadeThrower instance;

    void Awake()
    {
        curGrenadeAmmo = maxGrenadeAmmo;
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && curGrenadeAmmo > 0) {
            ThrowGrenade();
        }
    }

    void ThrowGrenade() 
    {
        curGrenadeAmmo--;

        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rig = grenade.GetComponent<Rigidbody>();

        rig.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);

        UI.instance.UpdateGrenadeText();
    }
}
