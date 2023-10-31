using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteObjectLinkage",menuName = "Lights Out/TileSpiteCorrelationMap")]
public class TileSpriteCorrelationMap : ScriptableObject
{
    [Serializable]
    public class SpriteTileCombo
    {
        [field: SerializeField] public Sprite Sprite { get; set; }
        [field: SerializeField] public GameObject Tile { get; set; }
    }
    public Dictionary<Sprite, GameObject> Links
    {
        get {
            Dictionary<Sprite, GameObject> ret = new();
            foreach (SpriteTileCombo combo in Linkages)
            {
                if (combo.Sprite != null && combo.Tile != null)
                    ret.Add(combo.Sprite, combo.Tile);
            }
            return ret;
        }
    }
    [field: SerializeField] public SpriteTileCombo[] Linkages { get; set; }
    
}
