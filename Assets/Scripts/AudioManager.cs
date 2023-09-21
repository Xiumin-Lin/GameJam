using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [SerializeField] private AudioClip A1, A2, A3, A4;
    [SerializeField] private AudioClip B1, B2, B3, B4;
    [SerializeField] private AudioClip C1, C2, C3, C4;
    [SerializeField] private AudioClip D1, D2, D3, D4;
    [SerializeField] private AudioClip E1, E2, E3, E4;
    [SerializeField] private AudioClip F1, F2, F3, F4;
    [SerializeField] private AudioClip G1, G2, G3, G4;
    
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
        _audioSource.clip = note switch
        {
            Tile.PianoNote.A1 => A1,
            Tile.PianoNote.B1 => B1,
            Tile.PianoNote.C1 => C1,
            Tile.PianoNote.D1 => D1,
            Tile.PianoNote.E1 => E1,
            Tile.PianoNote.F1 => F1,
            Tile.PianoNote.G1 => G1,
            Tile.PianoNote.A2 => A2,
            Tile.PianoNote.B2 => B2,
            Tile.PianoNote.C2 => C2,
            Tile.PianoNote.D2 => D2,
            Tile.PianoNote.E2 => E2,
            Tile.PianoNote.F2 => F2,
            Tile.PianoNote.G2 => G2,
            Tile.PianoNote.A3 => A3,
            Tile.PianoNote.B3 => B3,
            Tile.PianoNote.C3 => C3,
            Tile.PianoNote.D3 => D3,
            Tile.PianoNote.E3 => E3,
            Tile.PianoNote.F3 => F3,
            Tile.PianoNote.G3 => G3,
            Tile.PianoNote.A4 => A4,
            Tile.PianoNote.B4 => B4,
            Tile.PianoNote.C4 => C4,
            Tile.PianoNote.D4 => D4,
            Tile.PianoNote.E4 => E4,
            Tile.PianoNote.F4 => F4,
            Tile.PianoNote.G4 => G4,
            _ => A1
        };
        _audioSource.Play(0);
    }
}
