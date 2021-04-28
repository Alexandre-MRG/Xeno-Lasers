using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMultipleFire : EnemyBehaviour {
    [SerializeField] bool multipleFire = true;
    [SerializeField] int projectileNumber = 2;
    [SerializeField] float spaceBetweenProjectile = 0.4f;
    [SerializeField] float angleOfFire = 0.2f;
    protected override void FireProjectile()
    {
        if (!multipleFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.position.x, transform.position.y - 0.35f, 1), Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(projectileSound, transform.position, 0.7f);
        }
        else
        {
            if (projectileNumber == 2)
            {
                Vector3 offset1 = new Vector3(-spaceBetweenProjectile, 0f, 0f);
                GameObject projectile1 = Instantiate(projectilePrefab, transform.position + offset1, Quaternion.identity) as GameObject;
                projectile1.GetComponent<Rigidbody2D>().velocity = new Vector3(-angleOfFire, -projectileSpeed);

                Vector3 offset2 = new Vector3(spaceBetweenProjectile, 0f, 0f);
                GameObject projectile2 = Instantiate(projectilePrefab, transform.position + offset2, Quaternion.identity) as GameObject;
                projectile2.GetComponent<Rigidbody2D>().velocity = new Vector3(angleOfFire, -projectileSpeed);
            }
            else
            {
                for (int i = 0; i < projectileNumber/2; i++)
                {
                    Vector3 offset1 = new Vector3(-spaceBetweenProjectile - i*0.1f, 0f, 0f);
                    GameObject[] projectile3 = new GameObject[projectileNumber];
                    projectile3[i] = Instantiate(projectilePrefab, transform.position + offset1, Quaternion.identity) as GameObject;
                    projectile3[i].GetComponent<Rigidbody2D>().velocity = new Vector3(-angleOfFire-i*0.1f, -projectileSpeed);

                    Vector3 offset2 = new Vector3(spaceBetweenProjectile + i * 0.1f, 0f, 0f);
                    GameObject[] projectile4 = new GameObject[projectileNumber];
                    projectile4[i] = Instantiate(projectilePrefab, transform.position + offset2, Quaternion.identity) as GameObject;
                    projectile4[i].GetComponent<Rigidbody2D>().velocity = new Vector3(angleOfFire + i * 0.1f, -projectileSpeed);
                }
            }
        }
    }


}
