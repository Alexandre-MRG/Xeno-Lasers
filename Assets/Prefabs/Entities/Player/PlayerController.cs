using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerScript requires the GameObject to have a Rigidbody2D component
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    // Editable fields in Unity's Inspector
    [SerializeField] float health = 200;
    public float Health { get { return health; }  }

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject damagedShipEffect;
    [SerializeField] AudioClip hitSound;
    [SerializeField] GameObject shieldObject;
    [SerializeField] GameObject missileLauncherObject;
    [SerializeField] GameObject powerControllerObject;
    [SerializeField] float speed = 150;
    [SerializeField] GameObject projectileObject;
    [SerializeField] AudioClip projectileSound;
    [SerializeField] float projectileSpeed = 7;
    [SerializeField] float spaceBetweenProjectile = 0.5f;
    [SerializeField] GameObject secondaryProjectileObject;
    [SerializeField] float secondaryProjectileSpeed = 7;
    [SerializeField] float firingRate = 0.2f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] AudioClip deathSound;

    // Fields only used for the script
    private float healthMax;
    private float startSpeed;
    private float startFiringRate;
    private bool healthRecentlyHit = false;
    private bool fireEnabledAtStart = true;
    private bool fireEnabled = false;

    // Limits of the screen
    private float minX = 0.5f, maxX = 15.5f;
    private float minY = 0.5f, maxY = 8.5f;

    // Object to instantiate to use it in this class
    private Rigidbody2D rb2d;
    private LevelManager levelManager;
    private Shield shield;
    private MissileLauncher missileLauncher;
    private PowerController powerController;
    private GameObject damagedObject;

    // Use this for initialization
    void Start () {
        // Required initializations
        healthMax = health;
        startSpeed = speed;
        startFiringRate = firingRate;
        rb2d = GetComponent<Rigidbody2D>();
        levelManager = FindObjectOfType<LevelManager>();

        // Start of the Coroutine that allows to fire projectile with dynamic firingRate.
        StartCoroutine(FireCoroutine());

        // Initialisation of the damagedShipEffect used if the player health is <= 50%.
        damagedObject = Instantiate(damagedShipEffect, transform.position, Quaternion.identity) as GameObject;
        damagedObject.transform.parent = transform;
        damagedObject.GetComponent<ParticleSystem>().Pause();

        // Creation & assignation of the player's shield.
        Vector3 offset = new Vector3(0, -0.15f, 0);
        GameObject playerShield = Instantiate(shieldObject, transform.position + offset, Quaternion.identity) as GameObject;
        playerShield.transform.parent = transform;
        shield = GetComponentInChildren<Shield>();

        // Creation & assignation of the player's missileLauncher.
        GameObject playerMissileLauncher = Instantiate(missileLauncherObject, transform.position, Quaternion.identity) as GameObject;
        playerMissileLauncher.transform.parent = transform;
        missileLauncher = GetComponentInChildren<MissileLauncher>();

        // Creation & assignation of the player's powerController.
        GameObject playerPower = Instantiate(powerControllerObject, transform.position, Quaternion.identity) as GameObject;
        playerPower.transform.parent = transform;
        powerController = GetComponentInChildren<PowerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireEnabled = true;
            //InvokeRepeating("FireProjectile", 0.0000001f, firingRate); [OLD METHOD]
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            fireEnabled = false;
            //CancelInvoke("FireProjectile"); [OLD METHOD]
        }
        if(Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (missileLauncher.ActualAmmo > 0)
            {
                missileLauncher.multipleLaunch(powerController.getMissileNumber());
                missileLauncher.manageAmmo(-1);
            }
        }


        if (Time.frameCount % 30 == 0) // Verification code executed only once every 30 frames
        {
            checkDamageStatus();
            CheckIfPlayerGotHit();
        }

        if (Time.frameCount % 30 == 1) // Verification code executed only once every 30 frames not one the same frame than the upper function.
        {
            updatePower();
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    public float GetActualShield()
    {
        return shield.ActualShield;
    }

    public float GetMaxShield()
    {
        return shield.MaxShield;
    }

    public float GetActualPower()
    {
        return powerController.ActualPower;
    }

    public float GetMaxPower()
    {
        return powerController.MaxPower;
    }

    public void healPlayer(float healAmount)
    {
        if (health + healAmount <= healthMax)
        {
            health += healAmount;
        }
        if (health + healAmount > healthMax)
        {
            health = healthMax;
        }
    }

    public void boostShieldPlayer(float boostAmount)
    {
        shield.BoostShield(boostAmount);
    }

    public void boostPowerPlayer(float levelOfPowerAmount)
    {
        powerController.BoostPower(levelOfPowerAmount);
    }

    public void RestartPowerDecayDelayPlayer()
    {
        powerController.RestartDecayDelay();
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.AddForce(movement * speed);

        rb2d.position = new Vector2
        (
            Mathf.Clamp(rb2d.position.x, minX, maxX),
            Mathf.Clamp(rb2d.position.y, minY, maxY)
        );
    }


    // Old method, not used anymore.
    /*void FireProjectile()
    {
        Vector3 offset = new Vector3(0f, 0.7f, 0f);
        GameObject projectile = Instantiate(projectileObject, transform.position + offset, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(projectileSound, transform.position, 1f);
    }*/

    IEnumerator FireCoroutine()
    {
        // Instantiate projectiles at the specified firingRate.
        while (fireEnabledAtStart)
        {
            yield return new WaitForSeconds(firingRate);

            if (fireEnabled)
            {
                if (powerController.ActualPower < 2 || powerController.ActualPower > 2)
                {
                    fireMainProjectile();
                }

                if (powerController.ActualPower == 2)
                {
                    fireSecondaryProjectile();
                    AudioSource.PlayClipAtPoint(projectileSound, transform.position, 1f);
                }

                else if (powerController.ActualPower > 2)
                {
                    fireMainProjectile();
                    fireSecondaryProjectile();
                }
                
            }
        }
    }

    void fireMainProjectile()
    {
        Vector3 offset = new Vector3(0f, 0.3f, 0f);
        GameObject projectile = Instantiate(projectileObject, transform.position + offset, Quaternion.identity) as GameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
        AudioSource.PlayClipAtPoint(projectileSound, transform.position, 1f);
    }

    void fireSecondaryProjectile()
    {
        Vector3 offset = new Vector3(0f, 0.3f, 0f);

        Vector3 offset1 = new Vector3(-spaceBetweenProjectile, 0f, 0f);
        GameObject projectile1 = Instantiate(secondaryProjectileObject, transform.position + offset, Quaternion.identity) as GameObject;
        projectile1.GetComponent<Rigidbody2D>().velocity = new Vector3(-0.5f, secondaryProjectileSpeed);

        Vector3 offset2 = new Vector3(spaceBetweenProjectile, 0f, 0f);
        GameObject projectile2 = Instantiate(secondaryProjectileObject, transform.position + offset, Quaternion.identity) as GameObject;
        projectile2.GetComponent<Rigidbody2D>().velocity = new Vector3(0.5f, secondaryProjectileSpeed);
    }

    void updatePower()
    {
        firingRate = startFiringRate * powerController.getFirerateBonusMultiplier;
    }

    void checkDamageStatus()
    {
        // Enable or disable particles effects + shield based on the player's health.
        if (health < healthMax / 2)
        {
            damagedObject.GetComponent<ParticleSystem>().Play();

            speed = startSpeed / 4;

            if (shield.ShieldEnabled)
            {
                shield.DisableShield();
            }
        }

        if (health >= healthMax / 2)
        {
            damagedObject.GetComponent<ParticleSystem>().Pause();
            damagedObject.GetComponent<ParticleSystem>().Clear();

            speed = startSpeed;

            if (!shield.ShieldEnabled)
            {
                shield.EnableShield();
            }
        }
    }

    void CheckIfPlayerGotHit()
    {
        // If the player receive a hit, lose 1 power charge.

        if (healthRecentlyHit || shield.shieldRecentlyHit)
        {
            powerController.BoostPower(-1f);
            powerController.RestartDecayDelay();

            healthRecentlyHit = false;
            shield.shieldRecentlyHit = false;
        }
    }

    void Die()
    {
        // Make the player die & load the EndMenu.
        health = 0;
        GameObject explosion = Instantiate(deathEffect, transform.position, Quaternion.identity) as GameObject;
        AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        levelManager.Invoke("LoadEndMenu", 2);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player is hit by a projectile, handle damage.
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            missile.Hit();
            if (shield.ActualShield < missile.getDamage())
            {
                health -= missile.getDamage();
                healthRecentlyHit = true;

                if (health > 0)
                {
                    Vector3 offset = new Vector3(0f, -0.3f, -1f);
                    GameObject explosion = Instantiate(hitEffect, transform.position + offset, Quaternion.identity) as GameObject;
                    AudioSource.PlayClipAtPoint(hitSound, transform.position, 1f);
                }
                else
                {
                    Die();
                }
            }
        }
    }
}
