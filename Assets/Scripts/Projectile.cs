using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float damage = 100f;

    public virtual float getDamage()
    {
        return damage;
    }
    public virtual void Hit()
    {
        Destroy(gameObject);
    }
}
