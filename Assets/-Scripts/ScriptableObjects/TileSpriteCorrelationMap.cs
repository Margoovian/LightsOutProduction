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
        [field: SerializeField] public Vector3 Rotation { get; set; }
    }

    public Dictionary<Sprite, SpriteTileCombo> Links
    {
        get {
            Dictionary<Sprite, SpriteTileCombo> ret = new();
            foreach (SpriteTileCombo combo in Linkages)
            {
                if (combo.Sprite != null && combo.Tile != null)
                    ret.Add(combo.Sprite, combo);
            }
            return ret;
        }
    }

    [field: SerializeField] public SpriteTileCombo[] Linkages { get; set; }
}
