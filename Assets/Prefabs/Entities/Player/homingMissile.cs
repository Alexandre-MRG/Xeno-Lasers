using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class homingMissile : Projectile {

    [SerializeField] float speed = 5f;
    [SerializeField] float rotateSpeed = 200f;
    [SerializeField] float autoDestroyAfter = 10f;
    [SerializeField] GameObject explosion;
    [SerializeField] bool multiTarget = true;
    [SerializeField] bool playerTarget = false;

    EnemyBehaviour enemy;
    Transform target;
    FindClosest findClosest;
    private Rigidbody2D rb;

    void Start()
    {
        StartCoroutine("AutoExplode");

        if (!multiTarget)
        {
            if (!playerTarget)
            {
                findClosest = FindObjectOfType<FindClosest>();
                enemy = findClosest.getClosestEnemy();

                if (enemy != null)
                    target = enemy.transform;
            }
            else
            {
                if(FindObjectOfType<PlayerController>())
                    target = FindObjectOfType<PlayerController>().transform;
            }
        }

        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (rb)
        {
            if (target != null)
            {
                Vector2 direction = (Vector2)target.position - rb.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                rb.angularVelocity = -rotateAmount * rotateSpeed;

                rb.velocity = transform.up * speed;
            }
            else
            {
                rb.velocity = transform.up * speed;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        //Damage the other gameobject & then destroy self
        ExplodeSelf();
    }

    public void setTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void ExplodeSelf()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private IEnumerator AutoExplode()
    {
        yield return new WaitForSeconds(autoDestroyAfter);
        print("procedure explosion");
        ExplodeSelf();
    }
}
