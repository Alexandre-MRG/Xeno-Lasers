using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{

    public float delayBetweenFormations = 0f;
    public FormationController[] formations;

    public static int formationCleared = 0;

    private int i=0;

    // Use this for initialization
    void Start()
    {
        Game.waveCleared = false;
        formationCleared = 0;

        StartCoroutine(SpawnFormation(delayBetweenFormations));
    }

    void Update()
    {
        if (Time.frameCount % 70 == 0)      // Verification code executed only once every 70 frames
        { 
            if (formationCleared >= formations.Length)
            {
                Game.waveCleared = true;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator SpawnFormation(float delay)
    {
        for (i = 0; i < formations.Length; i++)
        {
            if (delay > 0) /* Cette instruction cause un bug d'affichage, donc on la conditionne pour qu'elle ne soit pas utilisée si le délai est nul */
            {
                yield return new WaitForSeconds(delay * i);
            }
            Instantiate(formations[i], transform.position, Quaternion.identity);
        }  
    }
}
