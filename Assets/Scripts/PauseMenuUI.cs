using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI instance;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject panelInGameMenu;
    private bool isPaused;

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ChangeScene);
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isPaused) ResumeGame();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }

    public void ResumeGame()
    {
        if(!isPaused)
        {
            isPaused = !isPaused;
            //Mettre en pause le jeu
            panelInGameMenu.SetActive(true);
        }
        else
        {
            isPaused = !isPaused;
            //On reprend le jeu
            panelInGameMenu.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
