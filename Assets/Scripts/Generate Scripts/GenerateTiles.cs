using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleRandomizer;

public class GenerateTiles : MonoBehaviour
{   
    /// <summary>
    /// A ScriptableObject of tilesets to use for the generation of tiles
    /// Contains the sprite, weight, rules, hasCollider, etc...
    /// </summary>
    public CellSprite cellSprite;

    /// <summary>
    /// Represents the height and width of the grid
    /// </summary>
    public int height, width;

    /// <summary>
    /// Represents the starting coordinates of the grid. (The position of the first cell of the grid. Usually at the bottom left)
    /// </summary>
    public int startX, startY;
    
    /// <summary>
    /// Represents the speed of generating a new tile
    /// Only for Playmode
    /// </summary>
    [Range(0.1f, 10f)]
    public float renderSpeed;

    /// <summary>
    /// A boolean used to let the code knows if the grid is extending
    /// </summary>
    public bool isExtend = false;

    /// <summary>
    /// Represents the parent object of the generated tiles/grid
    /// </summary>
    private GameObject parentObject;

    /// <summary>
    /// Used to center the grid
    /// </summary>
    private float gridHalfWidth, gridHalfHeight;
    public float GridHalfWidth { get{ return gridHalfWidth; } }
    public float GridHalfHeight { get{ return gridHalfHeight; } }

    /// <summary>
    /// Represent the initial candidates
    /// </summary>
    private CellSprite.SpriteList[] candidates;

    /// <summary>
    /// A dictionary that contains the coordinate and the cell object
    /// </summary>
    private Dictionary<(int, int), Cell> gridCells = new Dictionary<(int, int), Cell>();
    public Dictionary<(int, int), Cell> GridCells { get { return gridCells; } }

    /// <summary>
    /// A list of the group of tiles, which is also the parent object. Was used to separate each tiles to different tilegroup for easier debugging.
    /// </summary>
    private List<GameObject> tileGroup = new List<GameObject>();
    public List<GameObject> TileGroup { get { return tileGroup; } }

    /// <summary>
    /// Initialize, reset, and prepare all the necessary variables
    /// </summary>
    public void Start()
    {   
        /// If there is no existing parentObject, create one
        /// Add a compositeCollider2D to make the colliders of the cell objects use composite
        /// Set gravityScale to 0 to prevent the objects from falling
        if(parentObject == null)
        {
            tileGroup.Add(parentObject = new GameObject("TileSet Group"));
            parentObject.AddComponent<CompositeCollider2D>();
            parentObject.GetComponent<Rigidbody2D>().gravityScale = 0f; 
        }

        /// Call PrepareGeneration to setup the needed variables
        PrepareGeneration();
        /// Once prepared, draw the grid without sprites assigned
        DrawGridCells(startX, startY);

        /// If the grid has just been created, isExtend should be false and should run the following:
        if (isExtend == false)
        {   
            // If in playmode, use the coroutine to slowly generate tiles. This is for watching how the generation happens
            if(Application.isPlaying == true) StartCoroutine(GenerateSpritesOverTime());

            // If in editormode, generate the tiles instantly
            else GenerateSprites();
        }
    }

    /// <summary>
    /// In PrepareGeneration, the necessary variables are assigned with a value that will be used later. Most of this are the variables that will not be changed later on
    /// </summary>
    public void PrepareGeneration()
    {
        gridHalfWidth = (float)(width - 1f) / 2f;
        gridHalfHeight = (float)(height - 1f) / -2f;

        /// Initialize the size of the array and assign all the possible candidates
        candidates = new CellSprite.SpriteList[cellSprite.spriteLists.Length];

        for(int i = 0; i < candidates.Length; i++)
        {
            candidates[i] = cellSprite.spriteLists[i];
        }
    }

    /// <summary>
    /// In DrawGridCells, the code creates a Cell object and assign it a gameObject to represent it in the editor/playmode
    /// Cells are drawn from left to right starting from the bottom.
    /// </summary>
    /// <param name="startX"> Represents the X coordinate of the most bottom left cell (first cell of the grid) </param>
    /// <param name="startY"> Represents the Y coordinate of the most bottom left cell (first cell of the grid) </param>
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

    /// <summary>
    /// In GenerateSpritesOvertime, it loops with a delay to create some sort of animation on the sprite generation
    /// </summary>
    IEnumerator GenerateSpritesOverTime()
    {
        for(int i = 0; i < height * width; i++)
        {
            SelectGridCell();
            yield return new WaitForSeconds(Mathf.Lerp(1f, 0f, renderSpeed/10f));

        }
    }

    /// <summary>
    /// In GenerateSprites, it loops with no delay, so it is instant, to assign sprite to each grid
    /// </summary>
    public void GenerateSprites()
    {
        for(int i = 0; i < height * width; i++)
        {
            SelectGridCell();
        }
    }

    /// <summary>
    /// Will be used to represent the selectedSprite of the randomization
    /// </summary>
    CellSprite.SpriteList selectedSprite;

    /// <summary>
    /// A dictionary that will contain the coordinates of the cell with the lowest entropy/ least random
    /// </summary>
    IDictionary<(int, int), float> lowestEntropy = new Dictionary<(int, int), float>();

    /// <summary>
    /// A dictionary that will be used for the randomization. 
    /// 'CellSprite.Spritelist' will be the sprites that can be possibly selected, and the 
    /// 'float' will be the weight of that sprite
    /// </summary>
    IDictionary<CellSprite.SpriteList, float> currentCandidates = new Dictionary<CellSprite.SpriteList, float>();
    private void SelectGridCell()
    {
        /// Reset lowestEntropy and currentCandidates
        /// Add a single value to lowestEntropy that will contain float.MaxValue for comparison. We are looking for the ones with the least value
        /// so assigning it with the maxValue will ensure that we will get the lowest entropy
        lowestEntropy.Clear();
        lowestEntropy.Add((-1, -1), float.MaxValue);
        currentCandidates.Clear();

        /// Create a float entropy that will contain the result of the entropy calculation
        float entropy;

        /// Loop over the current grid. Since grid are extended with a fixed height and width, by using the startY and startX as starting point
        /// will ensure that we are only looping throughout the newly generated grid
        for(int y = startY; y < startY + height; y++)
        {
            for(int x = startX; x < startX + width; x++)
            {
                /// If the current is not filled, calculate the entropy
                /// This is to prevent from calculating the entropy of an already filled grid.
                if(gridCells[(x,y)].IsFilled == false)
                {
                    /// Assign to entropy the result of the EntropyCalculator.
                    /// EntropyCalculator has a List<CellSprite.SpriteList> parameter
                    entropy = EntropyCalculator(gridCells[(x,y)].GetCandidates);

                    /// If the calculated entropy is lower than the entropy in the dictionary, clear the dictionary then add the new coordinates and entropy
                    /// If the calculated entropy is equal than the entropy in the dictionary, just add it as an additional element
                    if(entropy <= lowestEntropy.ElementAt(0).Value && entropy >= 0)
                    {
                        if(entropy != lowestEntropy.ElementAt(0).Value) lowestEntropy.Clear();

                        lowestEntropy.Add((x,y), entropy);
                    }
                }
            }
        }
        
        /// Create a tuple (int, int) to contain the coordinates of the selected cell
        (int, int) selectedCell;

        /// This if statement is to ensure that there will be no errors
        if(lowestEntropy.Count > 0 && lowestEntropy.ElementAt(0).Value != float.MaxValue)
        {
            /// Get a random coordinate from the dictionary of lowest Entropy and assign it to selectedCell (selectedCell is a tuple that contains the coordinates)
            selectedCell = lowestEntropy.ElementAt(UnityEngine.Random.Range(0, lowestEntropy.Count)).Key;

            /// Assign the candidates of the selectedCell to currentCandidates
            currentCandidates = gridCells[selectedCell].GetCandidates.ToDictionary(o => o, o => o.GetWeight);

            /// Using WeightedRandomKey method from the WeightedRandom class, get a sprite from the weighted randomization
            /// then assign it to selectSprite
            selectedSprite = WeightedRandom.WeightedRandomKey<CellSprite.SpriteList>(currentCandidates);

            /// Call AssignSpriteToGridCell to assign the selected sprite to the selectedCell
            AssignSpriteToGridCell(gridCells[selectedCell], selectedSprite);

            /// Once the cell has an assigned sprite, check and update the possible candidates of the adjacent cells
            CheckAdjacent.CheckCells(gridCells, selectedCell, cellSprite.spriteLists);
        }
    }
    
    /// <summary>
    /// In AssignSpriteToGridCell, call the AssignSprite of the Cell Object to assign it the selectedSprite
    /// </summary>
    /// <param name="cell"> Cell object of the selectedCell </param>
    /// <param name="sprite"> represents the selectedSprite </param>
    private void AssignSpriteToGridCell(Cell cell, CellSprite.SpriteList sprite)
    {
        if(sprite != null)
        {
            cell.AssignSprite(sprite);
        }
    }

    /// <summary>
    /// In EntropyCalculator, we calculate the entropy of each cell using the Shannon Entropy / Information Entropy formula
    /// >> Negative Summation of the probability of the outcome multiplied by log base 2 of the probability of the outcome
    /// Probability of the outcome => c.GetWeight / totalSum
    /// </summary>
    /// <param name="cs"> represents the CellSprite.SpriteList </param>
    /// <returns> the result of the entropy equation </returns>
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
