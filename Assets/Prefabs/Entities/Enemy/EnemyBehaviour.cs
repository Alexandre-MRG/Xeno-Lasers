using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField] float health = 100;
    [SerializeField] GameObject hitEffect;
    [SerializeField] AudioClip hitSound;
    [SerializeField] float scoreForKill = 50f;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected AudioClip projectileSound;
    [SerializeField] protected float projectileSpeed = 5;
    [SerializeField] float shotsPerSeconds = 0.5f;
    [SerializeField] bool isDead = false;
    [SerializeField] GameObject deathEffect;
    [SerializeField] AudioClip deathSound;
    [SerializeField] float weightOnDrop = 1;

    GameObject explosion;

    private ScoreManager scoreManager;
    private DropManager dropManager;
    private Animator animator;
    private int collidedHash = Animator.StringToHash("Collided");
    private int arrivalStateHash = Animator.StringToHash("Base Layer.Arrival");

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        dropManager = FindObjectOfType<DropManager>();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        float probability = Time.deltaTime * shotsPerSeconds;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if ((Random.value < probability) && (stateInfo.fullPathHash != arrivalStateHash))
        {
            FireProjectile(); 
        }

        if (isDead)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();

        if (this.animator)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (missile && (stateInfo.fullPathHash != arrivalStateHash))
            {
                health -= missile.getDamage();
                missile.Hit();


                if (health > 0)
                {
                    animator.SetTrigger(collidedHash);
                    explosion = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
                    AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.5f);
                }

                if (health <= 0)
                {
                    Die();
                }

            }
        }
    }

    void Die()
    {
        scoreManager.AddScore(scoreForKill);

        for (int i = 0; i < weightOnDrop; i++)
        {
            dropManager.dropRandomItem(transform.position);
        }

        explosion = Instantiate(deathEffect, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(deathSound, transform.position, 0.5f);
        Destroy(gameObject);
    }

    protected virtual void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y - 0.35f, -1), Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(projectileSound, transform.position, 0.7f);
    }
}
