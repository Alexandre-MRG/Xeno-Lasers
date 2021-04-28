using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerController : MonoBehaviour {

    // Editable fields in Unity's Inspector
    [SerializeField] bool powerEnabled = true;
    [SerializeField] AudioClip gainSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] float maxPower = 3;
    public float MaxPower { get { return maxPower; } }    // Read only in other classes
    [SerializeField] float minPower = 1;
    [SerializeField] float actualPower = 1;
    public float ActualPower { get { return actualPower; } }    // Read only in other classes
    [SerializeField] bool powerDecayOverTime = true;
    [SerializeField] float powerDecayDelay = 2f;
    [SerializeField] bool loseWhenHit = true;

    // Fields only used for the script
    Coroutine m_MyCoroutineReference;
    bool powerEnabledAtStart = true;
    float firerateBonusMultiplier = 1;

    // Use this for initialization
    void Start () {
        powerEnabledAtStart = powerEnabled;

        // Start of the coroutine that allow the power to decay over time.
        if (powerEnabledAtStart && powerDecayOverTime)
        {
            m_MyCoroutineReference = StartCoroutine(PowerDecay());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float getFirerateBonusMultiplier
    {
        // Get the Final firerate bonus multiplier that will be apply to the player's firerate.
        get
        {
            if (actualPower == 0)
            {
                firerateBonusMultiplier = 1;
            }
            else if (actualPower >= 1 && actualPower <= 2)
            {
                
                firerateBonusMultiplier = 1f * actualPower;
            }
            else if (actualPower > 2)
            {
                firerateBonusMultiplier = 2f;
            }

            return 1 / firerateBonusMultiplier;  
        }
    }

    public int getMissileNumber()
    {
        if(actualPower == 2)
        {
            return 3;
        }
        if(actualPower > 2)
        {
            return 6;
        }
        else
        {
            return 1;
        }
    }
    public void BoostPower(float powerAmount)
    {
        if ((actualPower + powerAmount <= maxPower) && (actualPower + powerAmount >= minPower))
        {
            actualPower += powerAmount;

            if(powerAmount > 0)
            {
                AudioSource.PlayClipAtPoint(gainSound, transform.position, 0.5f);
            }
            else if(powerAmount < 0)
            {
                AudioSource.PlayClipAtPoint(loseSound, transform.position, 0.5f);
            }
        }
    }

    public void RestartDecayDelay()
    {
        // Stop & Restart the PowerDecay's coroutine.
        StopCoroutine(m_MyCoroutineReference);
        m_MyCoroutineReference = StartCoroutine(PowerDecay());
    }

    IEnumerator PowerDecay()
    {
        while (powerEnabledAtStart)
        {
            yield return new WaitForSeconds(powerDecayDelay);

            if (actualPower > 1)
            { 
                actualPower--;
                AudioSource.PlayClipAtPoint(loseSound, transform.position, 0.5f);
            }
        } 
    }

}


