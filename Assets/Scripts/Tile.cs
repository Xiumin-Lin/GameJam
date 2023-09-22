using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum PianoNote
    {
        A1, A2, A3, A4,
        B1, B2, B3, B4,
        C1, C2, C3, C4,
        D1, D2, D3, D4,
        E1, E2, E3, E4,
        F1, F2, F3, F4,
        G1, G2, G3, G4
    }
    
    private Note _note;
    public static float Vitesse = 1f;
    public PianoNote ID { get; private set; }

    // Update is called once per frame
    private void Update()
    {
        if(PauseMenuUI.Instance.IsPaused()) return;
        
        var currentSpeed = Vitesse;
        if (GameManager.Instance.GlitchIsActivate) currentSpeed = 1f;
        transform.Translate(-Vector2.up * currentSpeed * Time.deltaTime);
    }

    public void SetTile(GameObject spawn, Note note)
    {
        _note = note;
        Vector2 spawnPos = spawn.transform.position; // recupere la position locale de l'objet
        transform.position = new Vector2(spawnPos.x, spawnPos.y);

        ID = ParseNoteName2PianoNote(note.NoteName.ToString(), note.Octave);
    }

    public PianoNote ParseNoteName2PianoNote(string noteName, int octave)
    {
        var letter = noteName.ToUpper()[0];
        return letter switch
        {
            'A' => octave switch
            {
                1 => PianoNote.A1,
                2 => PianoNote.A2,
                3 => PianoNote.A3,
                _ => PianoNote.A4
            },
            'B' => octave switch
            {
                1 => PianoNote.B1,
                2 => PianoNote.B2,
                3 => PianoNote.B3,
                _ => PianoNote.B4
            },
            'C' => octave switch
            {
                1 => PianoNote.C1,
                2 => PianoNote.C2,
                3 => PianoNote.C3,
                _ => PianoNote.C4
            },
            'D' => octave switch
            {
                1 => PianoNote.D1,
                2 => PianoNote.D2,
                3 => PianoNote.D3,
                _ => PianoNote.D4
            },
            'E' => octave switch
            {
                1 => PianoNote.E1,
                2 => PianoNote.E2,
                3 => PianoNote.E3,
                _ => PianoNote.E4
            },
            'F' => octave switch
            {
                1 => PianoNote.F1,
                2 => PianoNote.F2,
                3 => PianoNote.F3,
                _ => PianoNote.F4
            },
            'G' => octave switch
            {
                1 => PianoNote.G1,
                2 => PianoNote.G2,
                3 => PianoNote.G3,
                _ => PianoNote.G4
            },
            _ => PianoNote.A1
        };
    }
}
