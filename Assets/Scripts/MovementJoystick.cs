using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MovementJoystick : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public KeyCode joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        joystickOriginalPos = joystickBG.transform.position;
        music = GetComponent<AudioSource>();
    }

    public void PointerDown()
    {
        joystick.transform.position = Input.mousePosition;
        joystickBG.transform.position = Input.mousePosition;
        joystickTouchPos = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position - joystickTouchPos;
        if (Mathf.Abs(dragPos.x) > 25 || Mathf.Abs(dragPos.y) > 25)
        {
            if (Mathf.Abs(dragPos.x) > Mathf.Abs(dragPos.y))
            {
                if (dragPos.x > 0)
                {
                    joystickVec = KeyCode.D;
                }
                else
                {
                    joystickVec = KeyCode.A;
                }
            }
            else
            {
                if (dragPos.y > 0)
                {
                    joystickVec = KeyCode.W;
                }
                else
                {
                    joystickVec = KeyCode.S;
                }
            }
        }
        else
        {
            joystickVec = KeyCode.None;
        }

        float joystickDist = Vector2.Distance(dragPos, joystickTouchPos);

        if (joystickVec == KeyCode.A)
        {
            joystick.transform.position = joystickTouchPos + new Vector2(-50f, 0);
        }
        else if (joystickVec == KeyCode.D)
        {
            joystick.transform.position = joystickTouchPos + new Vector2(50f, 0);
        }
        else if (joystickVec == KeyCode.S)
        {
            joystick.transform.position = joystickTouchPos + new Vector2(0, -50f);
        }
        else if (joystickVec == KeyCode.W)
        {
            joystick.transform.position = joystickTouchPos + new Vector2(0, 50f);
        }
        else
        {
            joystick.transform.position = joystickTouchPos;
        }
    }

    public void PointerUp()
    {
        joystickVec = KeyCode.None;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }

    public void StopAudio()
    {
        music.Stop();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
