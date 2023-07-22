using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CellSprite", menuName = "WaveFunctionCollapse/Tileset/CellSprite", order = 1)]
public class CellSprite :  ScriptableObject
{
    [Serializable]
    public class SpriteList
    {
        public string name;
        [SerializeField] private Sprite[] sprite;
        public Sprite[] GetSprite { get{ return sprite; } }

        [SerializeField] float weight = 1f;
        public float GetWeight {get {return weight;} }

        public bool hasCollider;

        /* [SerializeField] List<string> spriteRules;
        public List<string> GetSpriteRules { get{ return spriteRules; } } */

        [Serializable]
        public struct SpriteRules
        {
            public string[] top;
            public string[] right;
            public string[] down;
            public string[] left;

            public List<string[]> ToList() => new List<string[]>()
                                            {
                                                top,
                                                right,
                                                down,
                                                left
                                            };
        }
        [SerializeField] private SpriteRules spriteRules;
        public List<string[]> GetSpriteRules { get{ return spriteRules.ToList(); } }

    }

    public SpriteList[] spriteLists;
    public CellSprite GetCellSprite() => this;

}
