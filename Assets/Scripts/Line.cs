using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Tile go = other.gameObject.GetComponent<Tile>();
        if (go != null)
        {
            AudioManager.instance.PlayPianoAudio(go.ID);
            Destroy(other.gameObject, 0);
        }
    }
}
