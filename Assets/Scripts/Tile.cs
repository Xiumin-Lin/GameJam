using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float vitesse = 0.5f;
    [SerializeField] private GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 spawn_pos = spawn.transform.position; // Récupère la position locale de l'objet

        Vector2 new_position = new Vector2(spawn_pos.x, spawn_pos.y);
        this.transform.position = new_position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector2.up * vitesse * Time.deltaTime);
    }
}
