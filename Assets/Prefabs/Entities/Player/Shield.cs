using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    // Editable fields in Unity's Inspector
    [SerializeField] bool shieldEnabled = true;
    public bool ShieldEnabled { get { return shieldEnabled; } }    // Read only in other classes
    [SerializeField] bool shieldAlwaysVisible = false;
    [SerializeField] AudioClip activationSound;
    [SerializeField] AudioClip downSound;
    [SerializeField] float maxShield = 300;
    public float MaxShield { get { return maxShield; } }    // Read only in other classes
    [SerializeField] float actualShield = 100;
    public float ActualShield { get { return actualShield; } }      // Read only in other classes
    [SerializeField] float shieldRechargeRate = 100;
    [SerializeField] float shieldRechargeDelay = 2;
    [SerializeField] bool rechargeDownIfDamaged = false;
    [SerializeField] float rechargeDownDelay = 5;

    // Fields used by other classes.
    public bool shieldRecentlyHit = false;

    // Fields only used for the script
    private bool shieldEnabledAtStart;
    private bool rechargeEnabled = true;

    // Object to instantiate to use it in this class
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        // Required initializations
        sprite = GetComponent<SpriteRenderer>();
        shieldEnabledAtStart = shieldEnabled;

        // StartCoroutine that enable the passiveRecharge of the shield over time.
        if (shieldEnabledAtStart)
        {
            StartCoroutine(passiveRecharge());
        }

        if (!shieldAlwaysVisible)
        {
            sprite.enabled = false;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		if (!shieldEnabled)
        {
            actualShield = 0;
        }

        Color color = sprite.color;
        color.a = actualShield / maxShield;
        sprite.color = color;
	}

    public void EnableShield()
    {
        if (shieldEnabledAtStart)
        {
            AudioSource.PlayClipAtPoint(activationSound, transform.position, 1f);
            shieldEnabled = true;
            actualShield = maxShield / 4;
        }
    }
    public void DisableShield()
    {
        if (shieldEnabledAtStart)
        {
            AudioSource.PlayClipAtPoint(downSound, transform.position, 1f);
            shieldEnabled = false;
            actualShield = 0;
        }
    }

    public void BoostShield(float boostAmount)
    {
        if (actualShield + boostAmount <= maxShield)
        {
            actualShield += boostAmount;
        }
        else
        {
            actualShield = maxShield;
        }
    }


    public void HitShield(float damage)
    {
        if (actualShield - damage >= 0)
        {
            actualShield -= damage;
        }
        else
        {
            actualShield = 0;
        }
    }


    IEnumerator passiveRecharge()
    {
        while (shieldEnabledAtStart)
        {
            yield return new WaitForSeconds(shieldRechargeDelay);

            if (rechargeEnabled & shieldEnabled)
            {
                if (actualShield + shieldRechargeRate <= maxShield)
                {
                    actualShield += shieldRechargeRate;
                }
                else
                {
                    actualShield = maxShield;
                }
            }
        }
    }

    IEnumerator rechargeDown(float delay)
    {
        rechargeEnabled = false;
        shieldRecentlyHit = true;

        yield return new WaitForSeconds(delay);

        rechargeEnabled = true;
        shieldRecentlyHit = false;
    }

    IEnumerator shieldVisible(float delay)
    {
        if (!sprite.enabled)
        {
            AudioSource.PlayClipAtPoint(activationSound, transform.position, 1f);
        }

        sprite.enabled = true;
        yield return new WaitForSeconds(delay);
        sprite.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If collision with a projectile, handle damage & effects on the shield.
        Projectile missile = collision.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            if (actualShield - missile.getDamage() >= 0)
            {
                missile.Hit();
                HitShield(missile.getDamage());

                if (!shieldAlwaysVisible && shieldEnabled)
                {
                    StartCoroutine(shieldVisible(2f));
                }
            }

            if (rechargeDownIfDamaged)
            {
                StartCoroutine(rechargeDown(rechargeDownDelay));
            }
        }
    }
}
