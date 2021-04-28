using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public float delayBetweenWaves = 1f;
    public WaveController[] waves;

    public static bool waveCleared;

    private int waveIndex;
    private LevelManager levelmanager;

	// Use this for initialization
	void Start () {
        waveIndex = 0;
        waveCleared = true;

        levelmanager = FindObjectOfType<LevelManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (waveCleared)
        {
            waveCleared = false;
            WaveController.formationCleared = 0;
            StartCoroutine(NextWave(waveIndex, delayBetweenWaves * waveIndex));
            waveIndex++;
        }

        if ((waveIndex-1) >= waves.Length)  // Check Victory
        {
            StartCoroutine(EndGame(5));
        }
    }

    IEnumerator NextWave(int waveIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (waveIndex < waves.Length)
        {
            Instantiate(waves[waveIndex], transform.position, Quaternion.identity);
        }
    }

    IEnumerator EndGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        levelmanager.LoadLevel("WinMenu");
    }
}
