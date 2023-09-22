using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private GameObject panelInGameMenu;
    private bool _isPaused;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
        
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ChangeScene);
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Space))
        {
            if(_isPaused) ResumeGame();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("MainMenuUI", LoadSceneMode.Single);
    }

    public void ResumeGame()
    {
        if(!_isPaused)
        {
            _isPaused = !_isPaused;
            //Mettre en pause le jeu
            panelInGameMenu.SetActive(true);
        }
        else
        {
            _isPaused = !_isPaused;
            //On reprend le jeu
            panelInGameMenu.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return _isPaused;
    }
}
