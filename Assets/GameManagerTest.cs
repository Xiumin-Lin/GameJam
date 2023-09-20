using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerTest : MonoBehaviour
{
    public static GameManagerTest instance;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Transform scoreTMPro;
    private int score;

    void Awake()
    {
        instance = this;

        pauseMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);

        score = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!PauseMenuUI.instance.IsPaused())
            {
                PauseMenuUI.instance.ResumeGame();
            }
        }
        
        //Pour augmenter le score
        /*score++;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();*/
    }
}
