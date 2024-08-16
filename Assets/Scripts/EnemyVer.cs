using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyVer : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 origPos, targetPos;
    private Animator animator;
    private bool canMove = true;
    private Tilemap tilemap;
    private AudioSource hitSound;

    public float timeToMove = 0.4f;
    public bool moveUp = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        tilemap = FindObjectOfType<Tilemap>();
        hitSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        TileBase currentTile = tilemap.GetTile(tilemap.WorldToCell(transform.position));
        if (currentTile.name == "Sprites2_0" || currentTile.name == "Sprites2_4" ||
            currentTile.name == "Sprites2_8" || currentTile.name == "Sprites2_12" ||
            currentTile.name == "Sprites2_17" || currentTile.name == "Sprites2_21" ||
            currentTile.name == "Sprites2_24" || currentTile.name == "Sprites2_25")
        {
            if (canMove)
            {
                canMove = false;
                StartCoroutine(Death());
            }
        }
        if (canMove)
        {
            if (moveUp && !isMoving)
            {
                StartCoroutine(Move(Vector3.up));
            }
            if (!moveUp && !isMoving)
            {
                StartCoroutine(Move(Vector3.down));
            }
        }
    }

    IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;
        direction *= 0.155f;

        origPos = transform.position;
        targetPos = origPos + direction;
        TileBase targetTile = tilemap.GetTile(tilemap.WorldToCell(targetPos));

        if (!(targetTile != null && (targetTile.name == "Sprites2_61" ||
                targetTile.name == "Sprites2_0" || targetTile.name == "Sprites2_4" ||
                targetTile.name == "Sprites2_8" || targetTile.name == "Sprites2_12" ||
                targetTile.name == "Sprites2_17" || targetTile.name == "Sprites2_21" ||
                targetTile.name == "Sprites2_24" || targetTile.name == "Sprites2_25")))
        {
            isMoving = false;
            moveUp = !moveUp;
            StopAllCoroutines();
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GridMovement player = collision.GetComponent<GridMovement>();
            player.GetDamage();
        }
    }

    IEnumerator Death()
    {
        GridMovement player = FindObjectOfType<GridMovement>();
        hitSound.Play();
        player.AddPoint(200);
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
