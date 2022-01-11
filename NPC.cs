using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class NPC : MonoBehaviour
{
    [Header("Stats")]
    public int curHp;
    public int maxHp;
    [HideInInspector]
    public float attackDistance;

    [Header("Movement")]
    public float moveSpeed;
    public float attackRange;
    public float yPathOffset;

    private List<Vector3> path;
    public float safeDistance;

    private Weapon weapon;
    private GameObject target;
    private MeshRenderer[] meshRenderers;

    void Start()
    {
        //get the components
        weapon = GetComponent<Weapon>();
        target = FindObjectOfType<Player>().gameObject;

        InvokeRepeating("UpdatePath", 0.0f, 0.5f);
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist <= attackRange) {
            if(weapon.TryShoot())
                weapon.Shoot();
        }
        else {
            ChaseTarget();
        }

        //look at the target
        Vector3 dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        transform.eulerAngles = Vector3.up * angle;
    }

    void ChaseTarget()
    {
        if (path.Count == 0 && safeDistance <= attackDistance)
            return;
        else
            transform.position = Vector3.MoveTowards(transform.position, path[0] + new Vector3(0, yPathOffset, 0), moveSpeed * Time.deltaTime);

        if (transform.position == path[0] + new Vector3(0, yPathOffset, 0))
            path.RemoveAt(0);
    }

    void UpdatePath()
    {
        //calculate a path to the target
        NavMeshPath navMeshPath = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, navMeshPath);

        //save that as a list
        path = navMeshPath.corners.ToList();
    }

    public void Takedamage(int damage)
    {
        damage = 20;
        curHp -= damage;

        if (curHp <= 0)
            Die();
    }

    IEnumerator DamageFlash() 
    {
        for (int x = 0; x < meshRenderers.Length; x++) 
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);
        yield return new WaitForSeconds(0.1f);

        for (int x = 0; x < meshRenderers.Length; x++) 
            meshRenderers[x].material.color = Color.white;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}