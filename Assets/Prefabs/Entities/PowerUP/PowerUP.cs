using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUP : MonoBehaviour {


    protected float powerUpDuration;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void enablePowerUpPayload(ref PlayerController player)
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.gameObject.GetComponent<PlayerController>();

        if (player)
        {
            enablePowerUpPayload(ref player);
            Object.Destroy(gameObject);
        }
    }
}
