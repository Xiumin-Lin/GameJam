using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using Random = UnityEngine.Random;
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
    
    [SerializeField] private GameObject lineA;
    [SerializeField] private GameObject lineZ;
    [SerializeField] private GameObject lineE;
    [SerializeField] private GameObject lineR;

    private float score;
    private float multiplicateurVitesse;
    private int nbLives;

    private float timeRemaining = 2;
    private bool isVanish = false;
    
    [SerializeField] private GameObject tilePrefab;
    private MidiFile _midiFile;

    private Note[] _notes;
    private List<GameObject> _tiles;
    private TempoMap _tempoMap;
    
    [SerializeField] private GameObject spawner;

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
    private void Start()
    {
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/french_cancan.mid");
        _notes = _midiFile.GetNotes().ToArray();
        _tiles = new List<GameObject>();
        _tempoMap = _midiFile.GetTempoMap();

        var spawnerScript = spawner.GetComponent<SpawnManager>();
        var spawnerSize = spawnerScript.GetSpawnersSize();
        
        foreach (var note in _midiFile.GetNotes()) // preload each note
        {
            GameObject go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            Tile tile = go.GetComponent<Tile>();
            tile.SetTile(spawnerScript.GetSpawnerById(Random.Range(0, spawnerSize)), note);
            go.SetActive(false);
            _tiles.Add(go);
        }
    }

    private int _index;

    private void FixedUpdate()
    {
        if(_tiles.Count <= 0) return;
        for (int i = _index; i < _notes.Length; i++)
        {
            var note = _notes[i];
            var totalTimeInMilli = ((TimeSpan)note.TimeAs<MetricTimeSpan>(_tempoMap)).TotalSeconds;
            if (totalTimeInMilli * 1.8 <= Time.time)
            {
                var tile = _tiles[i];
                tile.SetActive(true);
                _index = i + 1;
            }
            else
            {
                break;
            }
        }
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
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.A1);
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.B1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.C1);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.D1);
        }
        
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
