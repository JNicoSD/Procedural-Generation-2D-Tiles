using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cell
{
    GameObject gameObject;
    public GameObject GetGameObject { get { return gameObject; } }

    List<CellSprite.SpriteList> candidates;
    public List<CellSprite.SpriteList> GetCandidates { get { return candidates; }}

    CellSprite.SpriteList spriteList;
    public CellSprite.SpriteList GetSpriteList { get { return spriteList;} }

    bool isFilled;
    public bool IsFilled { get { return isFilled; }}

    public Cell(string _name, List<CellSprite.SpriteList> _candidates)
    {
        if(this.candidates == null) // Check IF null
        {
            // Assign a new List of CellSprite if null
            this.candidates = new List<CellSprite.SpriteList>();
        }

        // Create new GameObject named depending on the passed name variable
        this.gameObject = new GameObject(_name);

        // Copy the list _candidates to the candidates of this list
        foreach(var c in _candidates)
        {
            this.candidates.Add(c);
        }

        // Since this is the initialization, set isFilled to FALSE
        this.isFilled = false;
    }

    public void AssignSprite(CellSprite.SpriteList _spriteList)
    {   
        // Assign cellSprite to this cell, is used to check what sprite this cell is using
        this.spriteList = _spriteList;
        // Assign a sprite to the cell Sprite Renderer
        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.spriteList.GetSprite[Random.Range(0,  this.spriteList.GetSprite.Length)];

        // Add BoxCollider2D if hasCollider is enabled
        if(this.spriteList.hasCollider == true) this.gameObject.AddComponent<BoxCollider2D>().usedByComposite = true;
        
        // Set isFilled to TRUE since the cell has an assigned sprite
        this.isFilled = true;
        // Clear the candidates list since there is no need to assign a new sprite in this cell
        this.candidates.Clear();

        this.gameObject.SetActive(true);
    }

    public void UpdateCandidates(List<CellSprite.SpriteList> newCandidates)
    {
        // Update the candidates list by getting the SAME CellSprite inside the list of cell candidates, and newCandidates
        this.candidates = this.candidates.Intersect(newCandidates).ToList();
        //this.candidates.Sort();
    }
}
