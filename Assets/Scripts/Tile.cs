using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum PianoNote
    {
        A, B, C, D, E, F, G
    }
    
    [SerializeField] private Note _note;
    private PianoNote _code;
    private float vitesse = 3f;
    private bool isCheating = false;
    public void SetTile(GameObject spawn, Note note)
    {
        _note = note;
        Vector2 spawnPos = spawn.transform.position; // Recupere la position locale de l'objet
        this.transform.position = new Vector2(spawnPos.x, spawnPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        var currentSpeed = vitesse;
        if (isCheating) currentSpeed /= 2;
        transform.Translate(-Vector2.up * currentSpeed * Time.deltaTime);
    }
}
