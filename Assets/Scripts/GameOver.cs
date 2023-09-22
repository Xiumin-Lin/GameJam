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
        PlayerPrefs.SetFloat("volume", -20f);
        PlayerPrefs.SetInt("volumeSliderPercent", 50);
        SceneManager.LoadScene("InGameUI", LoadSceneMode.Single);
    }

    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }
}
