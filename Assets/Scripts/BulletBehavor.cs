using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavor : MonoBehaviour
{
    [SerializeField] private Transform vfxOnHit;
    [SerializeField] private Transform vfxMonsterHit;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 40f;
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BulletTarget>() != null)
        {
            Instantiate(vfxOnHit, transform.position, Quaternion.identity);
        } else
        {
            Instantiate(vfxMonsterHit, transform.position, Quaternion.identity);
        }

        if(other.GetComponent<EnemyAi>() != null)
        {
            other.GetComponent<EnemyAi>().Freeze(0f, 2f);
        }
        Destroy(gameObject);
    }
}
