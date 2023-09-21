using System;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

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

        score = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";

        playerVolumePercent = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
        CheckPlayerVolume();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/little_star.mid");
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
            // Debug.Log($"{note.NoteName} Total: {Time.time} - {totalTimeInMilli} = {totalTimeInMilli < Time.time}");
            if (totalTimeInMilli <= Time.time)
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
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.A);
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.B);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.C);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.instance.PlayPianoAudio(Tile.PianoNote.D);
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

        //Multiplicateur de vitesse
        int volumePercent = PlayerPrefs.GetInt("volumeSliderPercent");
        multiplicateurVitesse = 1f;
        if (volumePercent != 50)
        {
            multiplicateurVitesse = 1f - ((50f - (float)volumePercent) * 2) / 100;   // Compris entre 0 et 1
        }
        // Debug.Log("Multiplicateur : " + multiplicateurVitesse);

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
