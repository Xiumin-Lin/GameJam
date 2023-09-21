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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RetryGameScene()
    {
        SceneManager.LoadScene("InGameUI");
    }

    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenuUI");
    }
}
