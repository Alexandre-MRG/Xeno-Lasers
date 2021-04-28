using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClosest : MonoBehaviour {

    EnemyBehaviour closestEnemy = null;
    public EnemyBehaviour[] closestEnemies = null;

    // Update is called once per frame
    void Update () {
        FindClosestEnemy ();
        //FindClosestEnemies(3);
	}

	void FindClosestEnemy()
	{
		float distanceToClosestEnemy = Mathf.Infinity;

		EnemyBehaviour[] allEnemies = GameObject.FindObjectsOfType<EnemyBehaviour>();

		foreach (EnemyBehaviour currentEnemy in allEnemies) {
			float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToEnemy < distanceToClosestEnemy) {
				distanceToClosestEnemy = distanceToEnemy;
				closestEnemy = currentEnemy;
			}
		}

        if(closestEnemy != null)
		    Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
	}

    public EnemyBehaviour[] FindClosestEnemies(int numberOfTargets)
    {
        EnemyBehaviour[] allEnemies = GameObject.FindObjectsOfType<EnemyBehaviour>();
        closestEnemies = new EnemyBehaviour[numberOfTargets];

        for (int i = 0; i < numberOfTargets; i++)
        {
            float distanceToClosestEnemy = Mathf.Infinity;

            foreach (EnemyBehaviour currentEnemy in allEnemies)
            {
                float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;

                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    bool newEnemy = true;

                    for(int j = 0; j <= i; j++)     // Looping trough previous found closestEnemies, if the currentEnemy is already found, then it is not a newEnemy.
                    {
                        if (closestEnemies[j] == currentEnemy)
                        {
                        newEnemy = false;
                        }
                    }

                    if (newEnemy)
                    {
                        distanceToClosestEnemy = distanceToEnemy;
                        closestEnemy = currentEnemy;
                    }
                }
            }
            closestEnemies[i] = closestEnemy;
        }

        for (int i = 0; i < numberOfTargets; i++)
        {
            if (closestEnemies[i] != null)
                Debug.DrawLine(this.transform.position, closestEnemies[i].transform.position);
        }

        return closestEnemies;
    }

    public EnemyBehaviour getClosestEnemy()
    {
        return closestEnemy;
    }

}
