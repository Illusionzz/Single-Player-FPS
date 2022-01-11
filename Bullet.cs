using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float lifeTime;
    private float shootTime;

    public GameObject hitParticle;

    void OnEnable()
    {
        shootTime = Time.time;
    }

    void Update()
    {
        if (Time.time - shootTime >= lifeTime)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            other.GetComponent<Player>().TakeDamage(damage);
        else if (other.CompareTag("Enemy"))
            other.GetComponent<NPC>().Takedamage(damage);

        GameObject obj = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(obj, 0.5f);

        gameObject.SetActive(false);
    }
}
