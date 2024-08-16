using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBoost : MonoBehaviour
{
    private AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GridMovement player = collision.GetComponent<GridMovement>();
            sound.Play();
            GetComponent<SpriteRenderer>().enabled = false;
            player.IncreaseRange();
            Destroy(gameObject,1);
        }
    }
}
