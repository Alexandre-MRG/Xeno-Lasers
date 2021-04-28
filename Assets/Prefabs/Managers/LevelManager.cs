 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static int previousScene = 0;
    private static int actualScene = 0;

    void Start()
    {
        if (actualScene >= 1)
            previousScene = actualScene; // On stocke la scène précédente dans previousScene avant d'obtenir l'ID de la scène actuelle.

        print("Previous Level " + previousScene);
        actualScene = SceneManager.GetActiveScene().buildIndex; // On stocke l'ID de la scène actuelle.
        print("Actual Level " + actualScene);
    }

    public void LoadLevel(string name)
    {
        Debug.Log("Level load requested for: " + name);
        SceneManager.LoadScene(name);
    }

    public void LoadPreviousLevel()
    {
        SceneManager.LoadScene(previousScene);   // On recharge la scène précédente à partir de l'écran de défaite.
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // On charge le prochain niveau dans l'index.
    }

    public void LoadEndMenu()
    {
        SceneManager.LoadScene("EndMenu");
    }

    public void QuitRequest()
    {
        Debug.Log("Quit Request !");
        Application.Quit();
    }

}

