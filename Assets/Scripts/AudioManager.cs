using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] private AudioClip A1;
    [SerializeField] private AudioClip B1;
    [SerializeField] private AudioClip C1;
    [SerializeField] private AudioClip D1;
    [SerializeField] private AudioClip E1;
    [SerializeField] private AudioClip F1;
    [SerializeField] private AudioClip G1;
    
    private AudioSource _audioSource;
    
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPianoAudio(Tile.PianoNote note)
    {
        switch (note)
        {
            case Tile.PianoNote.A: _audioSource.clip = A1; break;
            case Tile.PianoNote.B: _audioSource.clip = B1; break;
            case Tile.PianoNote.C: _audioSource.clip = C1; break;
            case Tile.PianoNote.D: _audioSource.clip = D1; break;
            case Tile.PianoNote.E: _audioSource.clip = E1; break;
            case Tile.PianoNote.F: _audioSource.clip = F1; break;
            case Tile.PianoNote.G: _audioSource.clip = G1; break;
        }
        _audioSource.Play(0);
    }
}
