using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float vitesse = 0.5f;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject spawn;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 new_position = new Vector3(spawner.transform.position.x + spawn.transform.position.x,
                                           spawn.transform.position.y + this.transform.localScale.y,
                                           0);
        this.transform.position = new_position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.up * vitesse * Time.deltaTime);
    }
}
