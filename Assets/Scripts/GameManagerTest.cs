using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using UnityEngine.SceneManagement;

public class GameManagerTest : MonoBehaviour
{
    public static GameManagerTest instance;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Transform scoreTMPro;
    [SerializeField] private Transform ATMPro;
    [SerializeField] private Transform ZTMPro;
    [SerializeField] private Transform ETMPro;
    [SerializeField] private Transform RTMPro;
    [SerializeField] private GameObject lives;
    private float score;
    private float multiplicateurVitesse;
    private int nbLives;

    private float timeRemaining = 2;
    private bool isVanish = false;

    void Awake()
    {
        instance = this;

        pauseMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);

        nbLives = 3;

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
        if(!isVanish)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //Debug.Log(timeRemaining);
            }
            else
            {
                isVanish = true;
                ATMPro.gameObject.SetActive(false);
                ZTMPro.gameObject.SetActive(false);
                ETMPro.gameObject.SetActive(false);
                RTMPro.gameObject.SetActive(false);
                //Debug.Log("Vanished");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(!PauseMenuUI.instance.IsPaused())
            {
                PauseMenuUI.instance.ResumeGame();
            }
        }

        //Multiplicateur de vitesse
        int volumePercent = PlayerPrefs.GetInt("volumeSliderPercent");
        /// A MODIFIER SELON LA VITESSE DE BASE DE LA MUSIQUE
        /*multiplicateurVitesse = 1f;
        if (volumePercent > 80)
        {
            multiplicateurVitesse = 1f + ((20f - (float)volumePercent) * 2) / 100;   // Compris entre 0 et 1
        }
        Debug.Log("Multiplicateur : " + multiplicateurVitesse);*/

        /// POUR AUGMENTER LE SCORE 
        /*score++;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();*/

        /// POUR DECREMENTER LE NOMBRE DE VIES
        //On a besoin de la méthode de collision => si pas de collision au moment de l'appui, 1 vie en moins

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(nbLives > 0)
            {
                nbLives--;
                lives.transform.GetChild(nbLives).gameObject.SetActive(false);
                if (nbLives == 0)
                {
                    GameOver();
                }
            }
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
