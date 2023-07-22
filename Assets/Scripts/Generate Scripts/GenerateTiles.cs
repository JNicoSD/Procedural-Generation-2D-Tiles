using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleRandomizer;

public class GenerateTiles : MonoBehaviour
{
    public CellSprite cellSprite;

    public int height, width;

    public int startX, startY;
    
    [Range(0.1f, 10f)]
    public float renderSpeed;
    public bool isExtend = false;

    private GameObject parentObject;

    private float gridHalfWidth, gridHalfHeight;
    public float GridHalfWidth { get{ return gridHalfWidth; } }
    public float GridHalfHeight { get{ return gridHalfHeight; } }

    private CellSprite.SpriteList[] candidates;

    private Dictionary<(int, int), Cell> gridCells = new Dictionary<(int, int), Cell>();
    public Dictionary<(int, int), Cell> GridCells { get { return gridCells; } }

    private List<GameObject> tileGroup = new List<GameObject>();
    public List<GameObject> TileGroup { get { return tileGroup; } }

    public void Start()
    {
        if(parentObject == null)
        {
            tileGroup.Add(parentObject = new GameObject("TileSet Group"));
            parentObject.AddComponent<CompositeCollider2D>(); 
        }

        PrepareGeneration();
        DrawGridCells(startX, startY);

        if (isExtend == false)
        {
            if(Application.isPlaying == true) StartCoroutine(GenerateSpritesOverTime());
            else GenerateSprites();
        }
    }

    public void PrepareGeneration()
    {
        gridHalfWidth = (float)(width - 1f) / 2f;
        gridHalfHeight = (float)(height - 1f) / -2f;

        candidates = new CellSprite.SpriteList[cellSprite.spriteLists.Length];

        for(int i = 0; i < candidates.Length; i++)
        {
            candidates[i] = cellSprite.spriteLists[i];
        }
    }

    public void DrawGridCells(int startX, int startY)
    {
        for(int y = startY; y < startY + height; y++)
        {
            for(int x = startX; x < startX + width; x++)
            {
                gridCells.Add((x, y), new Cell($"GridCell: ({x}, {y})", candidates.ToList()));
                
                gridCells[(x, y)].GetGameObject.transform.position = new Vector3((float) x - gridHalfWidth, (float)y + gridHalfHeight, 0f);
                
                gridCells[(x, y)].GetGameObject.transform.SetParent(parentObject.transform);
                
                gridCells[(x, y)].GetGameObject.AddComponent<SpriteRenderer>();

                gridCells[(x, y)].GetGameObject.SetActive(false);
                
            }
        }
    }

    IEnumerator GenerateSpritesOverTime()
    {
        for(int i = 0; i < height * width; i++)
        {
            SelectGridCell();
            yield return new WaitForSeconds(Mathf.Lerp(1f, 0f, renderSpeed/10f));

        }
    }

    public void GenerateSprites()
    {
        for(int i = 0; i < height * width; i++)
        {
            SelectGridCell();
        }
    }

    CellSprite.SpriteList selectedSprite;
    IDictionary<(int, int), float> lowestEntropy = new Dictionary<(int, int), float>();
    IDictionary<CellSprite.SpriteList, float> currentCandidates = new Dictionary<CellSprite.SpriteList, float>();
    private void SelectGridCell()
    {
        lowestEntropy.Clear();
        lowestEntropy.Add((-1, -1), float.MaxValue);
        currentCandidates.Clear();

        float entropy;
        for(int y = startY; y < startY + height; y++)
        {
            for(int x = startX; x < startX + width; x++)
            {
                if(gridCells[(x,y)].IsFilled == false)
                {
                    entropy = EntropyCalculator(gridCells[(x,y)].GetCandidates);

                    if(entropy <= lowestEntropy.ElementAt(0).Value && entropy >= 0)
                    {
                        if(entropy != lowestEntropy.ElementAt(0).Value) lowestEntropy.Clear();

                        lowestEntropy.Add((x,y), entropy);
                    }
                }
            }
        }
        
        (int, int) selectedCell;
        if(lowestEntropy.Count > 0 && lowestEntropy.ElementAt(0).Value != float.MaxValue)
        {
            selectedCell = lowestEntropy.ElementAt(UnityEngine.Random.Range(0, lowestEntropy.Count)).Key;

            currentCandidates = gridCells[selectedCell].GetCandidates.ToDictionary(o => o, o => o.GetWeight);

            selectedSprite = WeightedRandom.WeightedRandomKey<CellSprite.SpriteList>(currentCandidates);

            AssignSpriteToGridCell(gridCells[selectedCell], selectedSprite);

            CheckAdjacent.CheckCells(gridCells, selectedCell, cellSprite.spriteLists);
        }
    }
    
    private void AssignSpriteToGridCell(Cell cell, CellSprite.SpriteList sprite)
    {
        if(sprite != null)
        {
            cell.AssignSprite(sprite);
        }
    }

    private float EntropyCalculator(List<CellSprite.SpriteList> cs)
    {
        float entropy = 0;
        float totalSum = 0;

        foreach(CellSprite.SpriteList c in cs)
        {
            totalSum += c.GetWeight;
        }

        foreach(CellSprite.SpriteList c in cs)
        {
            if(c.GetWeight > 0)
            entropy -= (c.GetWeight/totalSum) * Mathf.Log((c.GetWeight/totalSum), 2);
        }

        return entropy;
    }


}
