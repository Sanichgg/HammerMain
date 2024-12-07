using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    [SerializeField] bool thisIsTheMainMenuScene;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject gameplayUI;
    [SerializeField] int nextSceneNumber;
    [SerializeField] int tutorialScene;
    [SerializeField] int gameplayScene;

    private bool pauseIsActive = false;


    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ActivateGameOverPanel()
    {
        if (!thisIsTheMainMenuScene)
        {
            gameOverPanel.SetActive(true);
            gameplayUI.SetActive(false);
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(nextSceneNumber);
    }

    public void RestartScenes()
    {
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameplayScene);
    }
    public void TutorialStart()
    {
        SceneManager.LoadScene(tutorialScene);
    }
    public void Pause()
    {
        if(pauseIsActive) Time.timeScale = 1.0f;
        else if(!pauseIsActive) Time.timeScale = 0f;
    }
}
