using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Transform scoreTMPro;

    void Awake()
    {
        playAgainButton.onClick.AddListener(PlayAgainScene);
        mainMenuButton.onClick.AddListener(MainMenuScene);

        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("score");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayAgainScene()
    {
        SceneManager.LoadScene("InGameUI");
    }

    private void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenuUI");
    }
}
