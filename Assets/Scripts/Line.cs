using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private KeyCode[] keys;

    private void OnTriggerStay2D(Collider2D other)
    {
        Tile go = other.gameObject.GetComponent<Tile>();
        if (go != null)
        {
            AudioManager.instance.PlayPianoAudio(go.ID);
            Destroy(other.gameObject, 0);
        }
    }
}
