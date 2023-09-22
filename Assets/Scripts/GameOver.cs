using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    void Awake()
    {
        retryButton.onClick.AddListener(RetryGameScene);
        mainMenuButton.onClick.AddListener(MainMenuScene);
    }

    private void RetryGameScene()
    {
        SceneManager.LoadScene("InGameUI", LoadSceneMode.Single);
    }

    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }
}
