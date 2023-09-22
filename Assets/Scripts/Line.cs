using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private KeyCode[] keys;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tile go = other.gameObject.GetComponent<Tile>();
        if (go != null)
        {
            GameManager.Instance.IncreaseScore();
            AudioManager.Instance.PlayPianoAudio(go.ID);
            Destroy(other.gameObject, 0);
        }
    }
}
