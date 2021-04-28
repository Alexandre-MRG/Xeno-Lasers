using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    // Editable fields in Unity's Inspector
    [SerializeField] Text hpNumber;
    [SerializeField] Image hpBar;
    [SerializeField] Text shieldNumber;
    [SerializeField] Image shieldBar;
    [SerializeField] Text powerNumber;
    [SerializeField] Image powerBar;
    [SerializeField] Text ammoNumber;
    [SerializeField] Image ammoBar;
    [SerializeField] Text scoreText;

    // Fields only used for the script
    private float healthMax;

    // Object to instantiate to use it in this class
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        playerController = FindObjectOfType<PlayerController>();
        healthMax = playerController.Health;
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.frameCount % 10 == 0)      // Display code executed only once every 10 frames
        {
            updateUI();
        }
    }

    void updateUI()
    {
        if (playerController)
        {
            hpNumber.text = playerController.Health.ToString();
            hpBar.fillAmount = playerController.Health / healthMax;

            shieldNumber.text = playerController.GetActualShield().ToString();
            shieldBar.fillAmount = playerController.GetActualShield() / playerController.GetMaxShield();

            powerNumber.text = playerController.GetActualPower().ToString();
            powerBar.fillAmount = playerController.GetActualPower() / playerController.GetMaxPower();

            ammoNumber.text = playerController.GetComponentInChildren<MissileLauncher>().ActualAmmo.ToString();
            ammoBar.fillAmount = playerController.GetComponentInChildren<MissileLauncher>().ActualAmmo / playerController.GetComponentInChildren<MissileLauncher>().MaxAmmo;
        }

        scoreText.text = ScoreManager.score.ToString();
    }

}
