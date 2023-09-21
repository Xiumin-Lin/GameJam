using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using Melanchall.DryWetMidi.Standards;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Note = Melanchall.DryWetMidi.Interaction.Note;
using MusicNote = Melanchall.DryWetMidi.MusicTheory.Note;

public class Test : MonoBehaviour
{
    [SerializeField] private GameObject notePrefabs;
    [SerializeField] private AudioClip A;
    [SerializeField] private AudioClip B;
    [SerializeField] private AudioClip C;
    [SerializeField] private AudioClip D;
    [SerializeField] private AudioClip E;
    [SerializeField] private AudioClip F;
    [SerializeField] private AudioClip G;
    private const string OutputDeviceName = "Microsoft GS Wavetable Synth";
    private AudioSource _audioSource;
    
    private OutputDevice _outputDevice;
    private Playback _playback;
    private MidiFile _midiFile;

    private Note[] _notes;
    private Stack<GameObject> _tiles;
    private TempoMap _tempoMap;
    private Dictionary<SevenBitNumber, Playback> _musicNote;

    private void Start()
    {
        InitializeOutputDevice();
        _audioSource = GetComponent<AudioSource>();
        
        _midiFile = MidiFile.Read("./Assets/Resources/Audio/little_star.mid");
        _notes = _midiFile.GetNotes().ToArray();
        _tiles = new Stack<GameObject>();
        _tempoMap = _midiFile.GetTempoMap();
        // _musicNote = new Dictionary<SevenBitNumber, Playback>();
        
        foreach (var note in _midiFile.GetNotes())
        {
            GameObject sphere = Instantiate(notePrefabs, new Vector2(note.Octave * 2, 10), Quaternion.identity);
            sphere.SetActive(false);
            _tiles.Push(sphere);

            // _musicNote.TryAdd(note.NoteNumber, CreateMusicNote(note));
        }

        // var notes = midiFile.GetNotes();
        // var tempoMap = midiFile.GetTempoMap();
        // foreach (var note in notes)
        // {
        //     var timeInSeconds = ((TimeSpan)note.TimeAs<MetricTimeSpan>(tempoMap)).Milliseconds;
        //     var lengthInSeconds = ((TimeSpan)note.LengthAs<MetricTimeSpan>(tempoMap)).Milliseconds;
        //     Debug.Log($"NoteName : {note.NoteName}");
        //     Debug.Log($"timeInSeconds : {timeInSeconds}");
        //     Debug.Log($"lengthInSeconds : {lengthInSeconds}");
        //     Debug.Log($"Time : {note.Time}");
        //     Debug.Log($"EndTime : {note.EndTime}");
        //     Debug.Log($"Velocity : {note.Velocity}");
        //     Debug.Log($"Length : {note.Length}");
        // }
        // InitializeFilePlayback(_midiFile);
        // StartPlayback();
    }

    private int _index;

    private void FixedUpdate()
    {
        if(_tiles.Count <= 0) return;
        for (int i = _index; i < _notes.Length; i++)
        {
            var note = _notes[i];
            var totalTimeInMilli = ((TimeSpan)note.TimeAs<MetricTimeSpan>(_tempoMap)).TotalSeconds;
            Debug.Log($"{note.NoteName} Total: {Time.time} - {totalTimeInMilli} = {totalTimeInMilli < Time.time}");
            // var diff = Time.time - totalTimeInMilli;
            if (totalTimeInMilli <= Time.time)
            {
                var tile = _tiles.Pop();
                tile.SetActive(true);
                Destroy(tile, 2);

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

    private void OnApplicationQuit() 
    {
        Debug.Log("Releasing playback and device...");

        if (_playback != null)
        {
            _playback.NotesPlaybackStarted -= OnNotesPlaybackStarted;
            _playback.NotesPlaybackFinished -= OnNotesPlaybackFinished;
            _playback.Dispose();
        }
        
        // if (_musicNote != null)
        // {
        //     foreach (var notePlay in _musicNote.Values)
        //     {
        //         notePlay.Dispose();
        //     }
        // }

        if (_outputDevice != null) _outputDevice.Dispose();

        Debug.Log("Playback and device released.");
    }

    private void InitializeOutputDevice()
    {
        Debug.Log($"Initializing output device [{OutputDeviceName}]...");

        var allOutputDevices = OutputDevice.GetAll();
        if (allOutputDevices.All(d => d.Name != OutputDeviceName))
        {
            var allDevicesList = string.Join(Environment.NewLine, allOutputDevices.Select(d => $"  {d.Name}"));
            Debug.Log($"There is no [{OutputDeviceName}] device presented in the system. Here the list of all device:{Environment.NewLine}{allDevicesList}");
            return;
        }

        _outputDevice = OutputDevice.GetByName(OutputDeviceName);
        Debug.Log($"Output device [{OutputDeviceName}] initialized.");
    }

    private void InitializeFilePlayback(MidiFile midiFile)
    {
        Debug.Log("Initializing playback...");
        
        _playback = midiFile.GetPlayback(_outputDevice);
        // _playback.Loop = true;
        _playback.NotesPlaybackStarted += OnNotesPlaybackStarted;
        _playback.NotesPlaybackFinished += OnNotesPlaybackFinished;
       
        Debug.Log("Playback initialized.");
    }

    private void StartPlayback()
    {
        Debug.Log("Starting playback...");
        _playback.Start();
    }

    private void OnNotesPlaybackStarted(object sender, NotesEventArgs e)
    {
        LogNotes("Notes started:", e);
    }
    
    private void OnNotesPlaybackFinished(object sender, NotesEventArgs e)
    {
        LogNotes("Notes finished:", e);
    }

    private void LogNotes(string title, NotesEventArgs e)
    {
        var message = new StringBuilder()
            .AppendLine(title)
            .AppendLine(string.Join(Environment.NewLine, e.Notes.Select(n => $"  {n}")))
            .ToString();
        Debug.Log(message.Trim());
    }
    
    private Playback CreateMusicNote(Note note)
    {
        Debug.Log($"Creating note {note.NoteName}");
        var patternBuilder = new PatternBuilder()
            .SetNoteLength(note.LengthAs<MetricTimeSpan>(_tempoMap))
            .SetVelocity(note.Velocity)
            .ProgramChange(GeneralMidiProgram.Harpsichord);

        patternBuilder.Note(MusicNote.Get(note.NoteNumber));

        var musicNote = patternBuilder.Build().ToFile(_tempoMap);
        return musicNote.GetPlayback(_outputDevice);
    }
}
