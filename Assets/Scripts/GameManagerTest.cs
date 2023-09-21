using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;

public class GameManagerTest : MonoBehaviour
{
    public static GameManagerTest instance;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Transform scoreTMPro;
    [SerializeField] private Transform ATMPro;
    [SerializeField] private Transform ZTMPro;
    [SerializeField] private Transform ETMPro;
    [SerializeField] private Transform RTMPro;
    private float score;
    private float playerVolumePercent;
    private float multiplicateurVitesse;

    private float timeRemaining = 2;
    private bool isVanish = false;

    void Awake()
    {
        instance = this;

        pauseMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);

        score = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";

        playerVolumePercent = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
        CheckPlayerVolume();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!PauseMenuUI.instance.IsPaused())
            {
                PauseMenuUI.instance.ResumeGame();
            }
        }

        //Multiplicateur de vitesse
        int volumePercent = PlayerPrefs.GetInt("volumeSliderPercent");
        multiplicateurVitesse = 1f;
        if (volumePercent != 50)
        {
            multiplicateurVitesse = 1f - ((50f - (float)volumePercent) * 2) / 100;   // Compris entre 0 et 1
        }
        Debug.Log("Multiplicateur : " + multiplicateurVitesse);

        //Pour augmenter le score
        /*score++;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();*/
    }

    void CheckPlayerVolume()
    {
        /*var devEnum = new MMDeviceEnumerator();
        var defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);*/
        /*var enumerator = new MMDeviceEnumerator();
        var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
        var volume = defaultDevice.AudioEndpointVolume;
        float masterVolumePercent = volume.MasterVolumeLevelScalar * 100;
        if (playerVolumePercent != masterVolumePercent)
        {
            playerVolumePercent = masterVolumePercent;
            scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = playerVolumePercent.ToString();
        }*/
    }
}
