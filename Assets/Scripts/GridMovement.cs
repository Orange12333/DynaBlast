using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GridMovement : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 origPos, targetPos;
    private Animator animator;
    private bool dead = false;
    private bool invincible = false;
    private int score = 0;
    private MovementJoystick joystick;
    private Tilemap tilemap;
    private bool isExitSpawned = false;
    private AudioSource hitSound;
    private AudioSource deathSound;
    private GameObject deathScreen;

    public int bombLimit = 1;
    public int bombRange = 1;
    public int seconds = 200;
    public float countedSecond = 0f;
    public Text lifeText;
    public Text ScoreText;
    public Text TimeText;
    public int health;
    public float timeToMove = 0.4f;
    public GameObject Bomb;
    public TileBase dest;
    public GameObject exit;

    private void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        lifeText.text = health.ToString(); 
        joystick = FindObjectOfType<MovementJoystick>();
        tilemap = FindObjectOfType<Tilemap>();
        hitSound = GetComponents<AudioSource>()[0];
        deathSound = GetComponents<AudioSource>()[1];
        deathScreen = GameObject.FindGameObjectWithTag("DeathScreen");
        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!tilemap.ContainsTile(dest) && !isExitSpawned && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Instantiate(exit, new Vector3(0.0753f, 0.5454f, 0.0f), Quaternion.identity);
            isExitSpawned = true;
        }
        if(seconds <= 0)
        {
            health = 0;
        }

        if (!dead)
        {
            if (countedSecond>1f)
            {
                countedSecond = 0;
                TimeCountSecond();
            }
            else
            {
                countedSecond += Time.deltaTime;
            }
            TileBase currentTile = tilemap.GetTile(tilemap.WorldToCell(transform.position));
            if (currentTile.name == "Sprites2_0" || currentTile.name == "Sprites2_4" || 
                currentTile.name == "Sprites2_8" || currentTile.name == "Sprites2_12" || 
                currentTile.name == "Sprites2_17" || currentTile.name == "Sprites2_21" || 
                currentTile.name == "Sprites2_24" || currentTile.name == "Sprites2_25")
            {
                GetDamage();
            }    
            if (health==0)
            {
                dead = true;
                StartCoroutine(Death());
            }
            if (joystick.joystickVec == KeyCode.W && !isMoving)
            {
                animator.SetBool("moveUp", true);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", false);
                animator.SetBool("idle", false);
                StartCoroutine(MovePlayer(Vector3.up));
            }
            if (joystick.joystickVec == KeyCode.A && !isMoving)
            {
                animator.SetBool("moveLeft", true);
                animator.SetBool("moveUp", false);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveRight", false);
                animator.SetBool("idle", false);
                StartCoroutine(MovePlayer(Vector3.left));
            }
            if (joystick.joystickVec == KeyCode.S && !isMoving)
            {
                animator.SetBool("moveDown", true);
                animator.SetBool("moveUp", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", false);
                animator.SetBool("idle", false);
                StartCoroutine(MovePlayer(Vector3.down));
            }
            if (joystick.joystickVec == KeyCode.D && !isMoving)
            {
                animator.SetBool("moveRight", true);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveUp", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("idle", false);
                StartCoroutine(MovePlayer(Vector3.right));
            }
            if (joystick.joystickVec == KeyCode.None && !isMoving)
            {
                animator.SetBool("moveUp", false);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", false);
                animator.SetBool("idle", true);
            }
        }
    }

    public void GetDamage()
    {
        if (!invincible)
        {
            if (health != 1)
            {
                hitSound.Play();
            }
            health -= 1;
            lifeText.text = health.ToString();
            animator.SetTrigger("damage");
            invincible = true;
            Invoke("ResetInvincibility", 2);
        }
    }

    public void spawnBomb()
    {
        if(GameObject.FindGameObjectsWithTag("Bomb").Length < bombLimit)
        {
            GameObject newBomb = Instantiate(Bomb, transform.position, Quaternion.identity);
            newBomb.GetComponent<BombScript>().Setup(this, bombRange);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;
        direction *= 0.155f;

        origPos = transform.position;
        targetPos = origPos+direction;
        TileBase targetTile = tilemap.GetTile(tilemap.WorldToCell(targetPos));

        if(!(targetTile != null && (targetTile.name == "Sprites2_61" || 
                targetTile.name == "Sprites2_0" || targetTile.name == "Sprites2_4" ||
                targetTile.name == "Sprites2_8" || targetTile.name == "Sprites2_12" ||
                targetTile.name == "Sprites2_17" || targetTile.name == "Sprites2_21" ||
                targetTile.name == "Sprites2_24" || targetTile.name == "Sprites2_25")))
        {
            isMoving = false;
            StopAllCoroutines();
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime/timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    IEnumerator Death()
    {
        joystick.StopAudio();
        deathSound.Play();
        animator.SetTrigger("death");
        yield return new WaitForSeconds(1.5f);
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    void TimeCountSecond()
    {
        seconds -= 1;
        int second = seconds % 60;
        int minute = (seconds - second) / 60;
        TimeText.text = minute.ToString() + ":" + second.ToString("D2");
        countedSecond = 0f;
    }

    void ResetInvincibility()
    {
        invincible = false;
    }

    public void AddPoint(int points)
    {
        score += points;
        ScoreText.text = score.ToString();
    }

    public void IncreaseRange()
    {
        bombRange++;
    }
    public void IncreaseHealth()
    {
        health++;
        lifeText.text = health.ToString();
    }
    public void IncreaseLimit()
    {
        bombLimit++;
    }
}
