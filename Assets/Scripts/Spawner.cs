using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject left_line;
    [SerializeField] private GameObject rigth_line;
    [SerializeField] private GameObject[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 middle_up_screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0));
        Vector2 spawner_pos = new Vector2(middle_up_screen.x, middle_up_screen.y);
        this.transform.position = spawner_pos;

        // Placement des spawns
        float width = rigth_line.transform.position.x - left_line.transform.position.x;
        int i = 0;
        float pas = width / 4;
        Vector3 coinHautDroit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        foreach (GameObject spawn in this.spawners)
        {
            Vector3 pos = new(left_line.transform.position.x + (pas/2) +  (pas * i), coinHautDroit.y, 0);
            spawn.transform.position = pos;
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
