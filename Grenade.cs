using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3f;
    public float radius = 5f;
    public float force = 700;

    public int damage;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    private NPC npc;

    void Awake()
    {
        npc = GetComponent<NPC>();
    }

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f && !hasExploded) {
            Explode();
            hasExploded = true;
        }
    }

    void Explode() 
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in colliders) {
            Rigidbody rig = nearbyObject.GetComponent<Rigidbody>();
            if (rig != null) {
                rig.AddExplosionForce(force, transform.position, radius);
                // npc.Takedamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
