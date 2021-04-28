using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsManager : MonoBehaviour {

    private void Awake()
    {
        /*
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 144; 
        */


        // 0 for no sync, 1 for panel refresh rate, 2 for 1/2 panel rate
        QualitySettings.vSyncCount = 0;

    }

}
