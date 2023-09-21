using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private KeyCode[] keys;
    private Renderer _skin;
    
    private void Start()
    {
        _skin = GetComponent<Renderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Tile go = other.gameObject.GetComponent<Tile>();
        if (go != null)
        {
            // bool isKeyDown = false;
            // foreach (KeyCode key in keys)
            // {
            //     if (Input.GetKeyDown(key)) isKeyDown = true;
            // }

            // if (isKeyDown)
            {
                AudioManager.instance.PlayPianoAudio(go.ID);
                Destroy(other.gameObject, 0);
                _skin.material.color = Color.green;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit Tile!");
        Destroy(other.gameObject, 1);
        _skin.material.color = Color.red;
    }
}
