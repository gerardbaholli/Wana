using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] float delayOnSceneLoad = 2f;

    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            StartCoroutine(WaitForTime());
    }

    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(delayOnSceneLoad);
        LoadGameScreen();
    }

    public void LoadStartingScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
