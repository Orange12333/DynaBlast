using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private AudioSource nextLevelSound;
    private MovementJoystick joystick;

    private void Start()
    {
        nextLevelSound = GetComponent<AudioSource>();
        joystick = FindObjectOfType<MovementJoystick>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        StartCoroutine(PlaneFadeIn());
    }

    IEnumerator PlaneFadeIn()
    {
        joystick.StopAudio();
        nextLevelSound.Play();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
