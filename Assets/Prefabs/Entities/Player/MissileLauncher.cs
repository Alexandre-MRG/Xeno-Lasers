using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour {
    [SerializeField] GameObject missileObjectSingle;
    [SerializeField] GameObject missileObjectMulti;
    [SerializeField] AudioClip activationSound;
    [SerializeField] float actualAmmo = 0;
    public float ActualAmmo { get { return actualAmmo; } }      // Read only in other classes
    [SerializeField] float maxAmmo = 2;
    public float MaxAmmo { get { return maxAmmo; } }      // Read only in other classes
    [SerializeField] float spaceBetweenMissile = 0.5f;

    homingMissile myMissile;
    FindClosest findClosest;

    // Use this for initialization
    void Start () {
        findClosest = FindObjectOfType<FindClosest>();
    }
	
    public void multipleLaunch(int missileNumber)
    {
        AudioSource.PlayClipAtPoint(activationSound, transform.position, 1f);

        EnemyBehaviour[] enemies = null;
        enemies = new EnemyBehaviour[missileNumber];

        enemies = findClosest.FindClosestEnemies(missileNumber);

        for (int i = 0; i < missileNumber; i++)
        {
            Vector3 offset = new Vector3(spaceBetweenMissile * i, 0f, 0f);
            GameObject missile = Instantiate(missileObjectMulti, transform.position + offset, Quaternion.identity) as GameObject;
            myMissile = missile.GetComponent<homingMissile>();
            myMissile.setTarget(enemies[i].transform);
        }

    }

    public void manageAmmo(float ammoAmount)
    {
        if((actualAmmo + ammoAmount <= maxAmmo) && (actualAmmo + ammoAmount >= 0))
        {
            actualAmmo += ammoAmount;
        }
    }
}
