using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid)), ExecuteInEditMode]
public class TilemapToObjects : MonoBehaviour
{
    [field: SerializeField] public Transform Dump { get; set; }

    private Tilemap _tileMap;
    void Start()
    {
       _tileMap = GetComponentInChildren<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyObjects() { }
    public void GenerateObject()
    {
        BoundsInt bounds = _tileMap.cellBounds;
        TileBase[] allTiles = _tileMap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    RuleTile rt = (RuleTile)tile;
                   // rt.
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
    }
}
