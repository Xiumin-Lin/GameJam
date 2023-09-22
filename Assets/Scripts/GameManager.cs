using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Transform scoreTMPro;
    [SerializeField] private Transform ATMPro;
    [SerializeField] private Transform ZTMPro;
    [SerializeField] private Transform ETMPro;
    [SerializeField] private Transform RTMPro;

    [SerializeField] private GameObject lives;
    
    [SerializeField] private GameObject lineColliderA;
    [SerializeField] private GameObject lineColliderZ;
    [SerializeField] private GameObject lineColliderE;
    [SerializeField] private GameObject lineColliderR;

    private int _score;
    private float _multiplicateurVitesse;
    private int _nbLives;

    public bool GlitchIsActivate { get; private set; }

    private float _timeRemaining = 2;
    private bool _isVanish = false;
    
    [SerializeField] private GameObject tilePrefab;
    private MidiFile _midiFile;

    private Note[] _notes;
    private List<GameObject> _tiles;
    private TempoMap _tempoMap;
    private int _index;
    private float _delayGlitchBonus;
    
    private bool _gameIsEnd;
    
    [SerializeField] private GameObject spawner;

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }

        pauseMenuUI.SetActive(true);    // awake pauseMenu 
        pauseMenuUI.SetActive(false);

        _nbLives = 3;
        _score = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
        
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/french_cancan.mid");
        _notes = _midiFile.GetNotes().ToArray();
        _tiles = new List<GameObject>();
        _tempoMap = _midiFile.GetTempoMap();
        _index = 0;
        _delayGlitchBonus = 0;
        _gameIsEnd = false;
        _eightLength = _notes.Length / 8;
    }

    // Start is called before the first frame update
    private void Start()
    {
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
        
        lineColliderA.SetActive(false);
        lineColliderZ.SetActive(false);
        lineColliderE.SetActive(false);
        lineColliderR.SetActive(false);
    }

    private int _eightLength;

    private void FixedUpdate()
    {
        if(PauseMenuUI.Instance.IsPaused() || _gameIsEnd) return;
        
        if(_tiles.Count <= 0) Victory();
        
        for (int i = _index; i < _notes.Length; i++)
        {
            var note = _notes[i];
            var totalTimeInMilli = note.TimeAs<MetricTimeSpan>(_tempoMap).TotalSeconds;
            if (totalTimeInMilli * (3 + _delayGlitchBonus) <= Time.timeSinceLevelLoad)
            {
                var tile = _tiles[i];
                if (i % _eightLength == 0) Tile.Vitesse += 1f;
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(!PauseMenuUI.Instance.IsPaused())
            {
                PauseMenuUI.Instance.ResumeGame();
            }
        }
        
        if(PauseMenuUI.Instance.IsPaused() || _gameIsEnd) return;
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            lineColliderA.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderA));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            lineColliderZ.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderZ));
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            lineColliderE.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderE));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            lineColliderR.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderR));
        }
        
        if(!_isVanish)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _isVanish = true;
                ATMPro.gameObject.SetActive(false);
                ZTMPro.gameObject.SetActive(false);
                ETMPro.gameObject.SetActive(false);
                RTMPro.gameObject.SetActive(false);
            }
        }

        //Multiplicateur de vitesse
        // int volumePercent = PlayerPrefs.GetInt("volumeSliderPercent");
        if (PlayerPrefs.GetInt("volumeSliderPercent") == 100)
        {
            GlitchIsActivate = true;
            _delayGlitchBonus = 1.2f;
        }
        /// A MODIFIER SELON LA VITESSE DE BASE DE LA MUSIQUE
        /*multiplicateurVitesse = 1f;
        if (volumePercent > 80)
        {
            multiplicateurVitesse = 1f + ((20f - (float)volumePercent) * 2) / 100;   // Compris entre 0 et 1
        }

        Debug.Log("Multiplicateur : " + multiplicateurVitesse);*/
    }
    
    public void IncreaseScore()
    {
        _score++;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = _score.ToString();
    }

    public void DecreaseHp()
    {
        _nbLives--;
        if(_nbLives > 0)
        {
            lives.transform.GetChild(_nbLives).gameObject.SetActive(false);
        } else GameOver();
    }

    private IEnumerator DeactivateLineCollider(GameObject line)
    {
        yield return new WaitForSeconds(0.05f);
        line.SetActive(false);
    }
    
    void Victory()
    {
        _gameIsEnd = true;
        SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
    }

    void GameOver()
    {
        _gameIsEnd = true;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
