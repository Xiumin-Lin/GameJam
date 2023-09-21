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
    [SerializeField] private AudioClip A;
    [SerializeField] private AudioClip B;
    [SerializeField] private AudioClip C;
    [SerializeField] private AudioClip D;
    [SerializeField] private AudioClip E;
    [SerializeField] private AudioClip F;
    [SerializeField] private AudioClip G;
    
    private AudioSource _audioSource;
    private MidiFile _midiFile;

    private Note[] _notes;
    private Stack<GameObject> _tiles;
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
        _audioSource = GetComponent<AudioSource>();
        
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/little_star.mid");
        _notes = _midiFile.GetNotes().ToArray();
        _tiles = new Stack<GameObject>();
        _tempoMap = _midiFile.GetTempoMap();

        var spawnerScript = spawner.GetComponent<Spawner>();
        var spawnerSize = spawnerScript.GetSpawnersSize();
        
        foreach (var note in _midiFile.GetNotes()) // preload each note
        {
            GameObject go = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            Tile tile = go.GetComponent<Tile>();
            tile.SetTile(spawnerScript.GetSpawnerById(Random.Range(0, spawnerSize)), note);
            go.SetActive(false);
            _tiles.Push(go);
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
            // var diff = Time.time - totalTimeInMilli;
            if (totalTimeInMilli <= Time.time)
            {
                var tile = _tiles.Pop();
                tile.SetActive(true);
                Destroy(tile, 10);

                char letter = note.NoteName.ToString().ToLower()[0];
                switch (letter)
                {
                    case 'a': _audioSource.clip = A; break;
                    case 'b': _audioSource.clip = B; break;
                    case 'c': _audioSource.clip = C; break;
                    case 'd': _audioSource.clip = D; break;
                    case 'e': _audioSource.clip = E; break;
                    case 'f': _audioSource.clip = F; break;
                    case 'g': _audioSource.clip = G; break;
                }
                _audioSource.Play(0);
                _index = i + 1;
            }
            else
            {
                break;
            }
        }
        Debug.Log($"{Time.time} END Update {_index}");
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
