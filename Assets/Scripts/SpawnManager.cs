using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject left_line;
    [SerializeField] private GameObject rigth_line;
    [SerializeField] private GameObject[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 middleUpScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 0));
        Vector2 spawnerPos = new Vector2(middleUpScreen.x, middleUpScreen.y);
        this.transform.position = spawnerPos;

        // Placement des spawns
        float width = rigth_line.transform.position.x - left_line.transform.position.x;
        int i = 0;
        float pas = width / 4f;
        Vector3 coinHautDroit = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        foreach (GameObject spawn in this.spawners)
        {
            Vector3 pos = new(left_line.transform.position.x + (pas / 2f) +  (pas * i), coinHautDroit.y, 0);
            spawn.transform.position = pos;
            i++;
        }
    }

    public int GetSpawnersSize()
    {
        return spawners.Length;
    }

    public GameObject GetSpawnerById(int id)
    {
        return spawners[id];
    }
}
