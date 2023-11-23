using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid)), ExecuteAlways]
public class TilemapToObjects : MonoBehaviour
{
    [field: SerializeField] public Vector2 CellSize { get; set; }
    [field: SerializeField] public Transform Dump { get; set; }
    [field: SerializeField] public TileSpriteCorrelationMap CorrelationMap { get; set; }
    public Dictionary<Sprite, TileSpriteCorrelationMap.SpriteTileCombo> Glossary { get; set; }

    private Tilemap _tileMap;
    void Start()
    {
       _tileMap = GetComponentInChildren<Tilemap>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Glossary == null)
            Glossary = CorrelationMap.Links;
    }
    public void DestroyObjects() { 
        foreach(Transform obj in Dump.GetComponentsInChildren<Transform>())
        {
            if(obj != Dump)
                DestroyImmediate(obj.gameObject);
        }
    }
    public void GenerateObjects()
    {
        BoundsInt bounds = _tileMap.cellBounds;
        TileBase[] allTiles = _tileMap.GetTilesBlock(bounds);

        foreach (var pos in _tileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = _tileMap.CellToWorld(localPlace);
            if (_tileMap.HasTile(localPlace))
            {
                Sprite s = _tileMap.GetSprite(localPlace);
                if (s != null && Glossary.ContainsKey(s))
                {
                    //Debug.Log(s.name);
                    GameObject obj = Instantiate(Glossary[s].Tile);
                    obj.transform.SetParent(Dump, false);
                    obj.transform.position = new(localPlace.x * CellSize.x, 0, localPlace.y * CellSize.y);
                    obj.transform.rotation = Quaternion.Euler(Glossary[s].Rotation);
                    obj.isStatic = true;
                };
            }
        }
    }

    public void ResetGlossary() => Glossary = CorrelationMap.Links;
}
