using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombScript : MonoBehaviour
{
    public float time = 2f;
    public int range = 1;
    public Tile explodeTileVertical;
    public Tile explodeTileHorizontal;
    public Tile explodeTileCenter;
    public Tile explodeTileEndUp;
    public Tile explodeTileEndDown;
    public Tile explodeTileEndLeft;
    public Tile explodeTileEndRight;
    public Tile grass;
    public Tile destroyWall;
    public Tile bomb;
    public GameObject rangeBoost;
    public GameObject healthBoost;
    public GameObject limitBoost;

    private GridMovement player;
    private Tilemap tilemap;
    private AudioSource explosionSound;

    public void Setup(GridMovement pl, int rang)
    {
        player = pl;
        range = rang;
    }

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GameObject.FindObjectOfType<Tilemap>();
        StartCoroutine(Counter());
        explosionSound = GetComponent<AudioSource>();
    }

    IEnumerator Counter()
    {
        tilemap.SetTile(tilemap.WorldToCell(transform.position),bomb);
        yield return new WaitForSeconds(time);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        explosionSound.Play();
        //kierunki wybuchu
        bool eUp = true;
        bool eDown = true;
        bool eLeft = true;
        bool eRight = true;

        //Explode loop
        tilemap.SetTile(tilemap.WorldToCell(transform.position), explodeTileCenter);
        for(var i = 1; i<=range; i++)
        {
            Vector3Int location;
            TileBase tile;
            //up
            if (eUp)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.up * 0.155f)*i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_61" || tile.name == "Sprites2_59")
                {
                    if (i == range)
                    {
                        tilemap.SetTile(location, explodeTileEndUp);
                    }
                    else
                    {
                        tilemap.SetTile(location, explodeTileVertical);
                    }
                    if (tile.name == "Sprites2_59")
                    {
                        player.AddPoint(100);
                        tilemap.SetTile(location, destroyWall);
                        eUp = false;
                    }
                }
                else
                {
                    eUp = false;
                }
            }
            if (eDown)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.down * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_61" || tile.name == "Sprites2_59")
                {
                    if (i == range)
                    {
                        tilemap.SetTile(location, explodeTileEndDown);
                    }
                    else
                    {
                        tilemap.SetTile(location, explodeTileVertical);
                    }
                    if (tile.name == "Sprites2_59")
                    {
                        player.AddPoint(100);
                        tilemap.SetTile(location, destroyWall);
                        eDown = false;
                    }
                }
                else
                {
                    eDown = false;
                }
            }
            if (eLeft)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.left * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_61" || tile.name == "Sprites2_59")
                {
                    if (i == range)
                    {
                        tilemap.SetTile(location, explodeTileEndLeft);
                    }
                    else
                    {
                        tilemap.SetTile(location, explodeTileHorizontal);
                    }
                    if (tile.name == "Sprites2_59")
                    {
                        player.AddPoint(100);
                        tilemap.SetTile(location, destroyWall);
                        eLeft = false;
                    }
                }
                else
                {
                    eLeft = false;
                }
            }
            if (eRight)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.right * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_61" || tile.name == "Sprites2_59")
                {
                    if (i == range)
                    {
                        tilemap.SetTile(location, explodeTileEndRight);
                    }
                    else
                    {
                        tilemap.SetTile(location, explodeTileHorizontal);
                    }
                    if (tile.name == "Sprites2_59")
                    {
                        player.AddPoint(100);
                        tilemap.SetTile(location, destroyWall);
                        eRight = false;
                    }
                }
                else
                {
                    eRight = false;
                }
            }
        }

        yield return new WaitForSeconds(1);

        eUp = true;
        eDown = true;
        eLeft = true;
        eRight = true;

        tilemap.SetTile(tilemap.WorldToCell(transform.position), grass);
        //Clear loop
        for (var i = 1; i <= range; i++)
        {
            Vector3Int location;
            TileBase tile;
            if (eUp)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.up * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_17" || tile.name == "Sprites2_0" || tile.name == "Sprites2_32")
                {
                    tilemap.SetTile(location, grass);
                    if(tile.name == "Sprites2_32")
                    {
                        RandomBoostDrop(location);
                    }
                }
                else
                {
                    eUp = false;
                }
            }
            if (eDown)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.down * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_17" || tile.name == "Sprites2_8" || tile.name == "Sprites2_32")
                {
                    tilemap.SetTile(location, grass);
                    if (tile.name == "Sprites2_32")
                    {
                        RandomBoostDrop(location);
                    }
                }
                else
                {
                    eDown = false;
                }
            }
            if (eLeft)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.left * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_21" || tile.name == "Sprites2_12" || tile.name == "Sprites2_32")
                {
                    tilemap.SetTile(location, grass);
                    if (tile.name == "Sprites2_32")
                    {
                        RandomBoostDrop(location);
                    }
                }
                else
                {
                    eLeft = false;
                }
            }
            if (eRight)
            {
                location = tilemap.WorldToCell(transform.position + ((Vector3.right * 0.155f) * i));
                tile = tilemap.GetTile(location);
                if (tile.name == "Sprites2_21" || tile.name == "Sprites2_4" || tile.name == "Sprites2_32")
                {
                    tilemap.SetTile(location, grass);
                    if (tile.name == "Sprites2_32")
                    {
                        RandomBoostDrop(location);
                    }
                }
                else
                {
                    eRight = false;
                }
            }
        }
        Destroy(gameObject);
    }

    void RandomBoostDrop(Vector3Int location)
    {
        if (Random.Range(0f, 1f) <= 0.2f)
        {
            List<GameObject> list = new List<GameObject>();
            if(player.bombLimit < 3)
            {
                list.Add(limitBoost);
            }
            if(player.health < 3)
            {
                list.Add(healthBoost);
            }
            if (player.bombRange < 4)
            {
                list.Add(rangeBoost);
                list.Add(rangeBoost);
            }
            if (list.Count != 0)
            {
                GameObject rand = list[(Random.Range(0, list.Count))];
                Instantiate(rand, tilemap.CellToWorld(location) + new Vector3(0.0775f, 0.0775f, 0), Quaternion.identity);
            }
        }
    }
}
