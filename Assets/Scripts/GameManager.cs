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
    public static GameManager instance;
    
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

    private int score;
    private float multiplicateurVitesse;
    private int nbLives;

    public bool GlitchIsActivate { get; private set; }

    private float timeRemaining = 2;
    private bool isVanish = false;
    
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
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }

        pauseMenuUI.SetActive(true);    // awake pauseMenu 
        pauseMenuUI.SetActive(false);

        nbLives = 3;
        score = 0;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = "0";
        
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/french_cancan.mid");
        _notes = _midiFile.GetNotes().ToArray();
        _tiles = new List<GameObject>();
        _tempoMap = _midiFile.GetTempoMap();
        _index = 0;
        _delayGlitchBonus = 0;
        _gameIsEnd = false;
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

    private void FixedUpdate()
    {
        if(PauseMenuUI.instance.IsPaused() || _gameIsEnd) return;
        
        if(_tiles.Count <= 0) Victory();
        
        for (int i = _index; i < _notes.Length; i++)
        {
            var note = _notes[i];
            var totalTimeInMilli = note.TimeAs<MetricTimeSpan>(_tempoMap).TotalSeconds;
            if (totalTimeInMilli * (1.2 + _delayGlitchBonus) <= Time.timeSinceLevelLoad)
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
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(!PauseMenuUI.instance.IsPaused())
            {
                PauseMenuUI.instance.ResumeGame();
            }
        }
        
        if(PauseMenuUI.instance.IsPaused() || _gameIsEnd) return;
        
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
        {
            lineColliderA.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderA));
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W))
        {
            lineColliderZ.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderZ));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            lineColliderE.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderE));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            lineColliderR.SetActive(true);
            StartCoroutine(DeactivateLineCollider(lineColliderR));
        }
        
        if(!isVanish)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                isVanish = true;
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
        score++;
        scoreTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
    }

    public void DecreaseHP()
    {
        nbLives--;
        if(nbLives > 0)
        {
            lives.transform.GetChild(nbLives).gameObject.SetActive(false);
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
        foreach (GameObject go in _tiles)
        {
            Destroy(go);
        }
        SceneManager.LoadScene("VictoryScene", LoadSceneMode.Single);
    }

    void GameOver()
    {
        _gameIsEnd = true;
        foreach (GameObject go in _tiles)
        {
            Destroy(go);
        }
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        foreach (GameObject go in _tiles)
        {
            Destroy(go);
        }
    }
}
