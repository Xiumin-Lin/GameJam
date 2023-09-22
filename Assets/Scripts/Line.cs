using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private KeyCode[] keys;

    private void OnTriggerStay2D(Collider2D other)
    {
        Tile go = other.gameObject.GetComponent<Tile>();
        if (go != null)
        {
            GameManager.instance.IncreaseScore();
            AudioManager.instance.PlayPianoAudio(go.ID);
            Destroy(other.gameObject, 0);
        }
    }
}
