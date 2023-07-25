using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckAdjacent
{
    /// <summary>
    /// Enum to use for checking adjacent cells
    /// </summary>
    enum Direction
    {
        Top,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// An array of SpriteList to be used for checking the adjacent cells
    /// </summary>
    static CellSprite.SpriteList[] spriteLists;
    /// <summary>
    /// A dictionary of the gridCells, will be used to access the Cell and its candidates
    /// </summary>
    static Dictionary<(int, int), Cell> gridCells = new Dictionary<(int, int), Cell>();

    /// <summary>
    /// The callable method of the Class CheckAdjacent. Will be used to start the checking of adjacent cells
    /// </summary>
    /// <param name="_gridCells"> Represents the gridCell from the GenerateTiles class </param>
    /// <param name="_origin"> The coordinates of the selectedCell </param>
    /// <param name="_spriteList"> The SpriteList that contains all possible candidates </param>
    public static void CheckCells(Dictionary<(int, int), Cell> _gridCells, (int, int) _origin, CellSprite.SpriteList[] _spriteList)
    {
        gridCells = _gridCells;
        spriteLists = _spriteList;

        /// A for loop that will Check each direction (order: Top -> Right -> Down -> Left)
        for(int i = 0; i < 4; i++)
        {
            StartCheck((Direction)i, _origin, false);
        }
    }

    /// <summary>
    /// ruleDirection is the index of rules to compare Top is 0, Right is 1, Down is 2, and Left is 3
    /// so we will compare (0 and 2),  (1 and 3)
    /// </summary>
    static (int, int) ruleDirection;

    /// <summary>
    /// lastKey will be used to recheck the adjacent cell starting from the last key check back to the origin
    /// </summary>
    static (int, int) lastKey;

    /// <summary>
    /// In StartCheck, the code will call UpdateAdjacentCells in four different directions depending on the value of Direction
    /// </summary>
    /// <param name="direction"> the current direction to check </param>
    /// <param name="origin"> the coordinates of the cell that has been assigned with a sprite </param>
    /// <param name="endImmediate"> a boolean to check if the code will reupdate/adjust from the last key checked or not </param>
    private static void StartCheck(Direction direction, (int, int) origin, bool endImmediate)
    {
        if(direction == Direction.Top)
        {
            ruleDirection = (0, 2);
            
            for(int y = origin.Item2; gridCells.ContainsKey((origin.Item1, y + 1)) == true && gridCells[(origin.Item1, y + 1)].IsFilled == false; y++)
            {
                UpdateAdjacentCells(ruleDirection, (origin.Item1, y), (origin.Item1, y + 1));

                /// Once Top cell has been updated, try to update the Right and Left cell if it exist
                if(gridCells.ContainsKey((origin.Item1 + 1, y + 1)) == true) UpdateAdjacentCells((1, 3), (origin.Item1, y + 1), (origin.Item1 + 1, y + 1)); /// Right
                if(gridCells.ContainsKey((origin.Item1 - 1, y + 1)) == true) UpdateAdjacentCells((3, 1), (origin.Item1, y + 1), (origin.Item1 - 1, y + 1)); /// Left

                lastKey = (origin.Item1, y);
            }
            direction = Direction.Down;
            

        } else if(direction == Direction.Right)
        {
            ruleDirection = (1, 3);
            for(int x = origin.Item1; gridCells.ContainsKey((x + 1, origin.Item2)) == true && gridCells[(x + 1, origin.Item2)].IsFilled == false; x++)
            {
                UpdateAdjacentCells(ruleDirection, (x, origin.Item2), (x + 1, origin.Item2));

                /// Once Right cell has been updated, try to update the Top and Down cell if it exist
                if(gridCells.ContainsKey((x + 1, origin.Item2 + 1)) == true) UpdateAdjacentCells((0, 2), (x + 1, origin.Item2), (x + 1, origin.Item2 + 1)); /// Top
                if(gridCells.ContainsKey((x + 1, origin.Item2 - 1)) == true) UpdateAdjacentCells((2, 0), (x + 1, origin.Item2), (x + 1, origin.Item2 - 1)); /// Down

                lastKey = (x, origin.Item2);
            }
            direction = Direction.Left;
            /* if(endAfterLoop == false)
            {
                StartCheck(Direction.Left, lastKey, true);
            } */

        } else if(direction == Direction.Down)
        {
            ruleDirection = (2, 0);

            for(int y = origin.Item2; gridCells.ContainsKey((origin.Item1, y - 1)) == true && gridCells[(origin.Item1, y - 1)].IsFilled == false; y--)
            {
                UpdateAdjacentCells(ruleDirection, (origin.Item1, y), (origin.Item1, y - 1));

                /// Once Down cell has been updated, try to update the Right and Left cell if it exist
                if(gridCells.ContainsKey((origin.Item1 + 1, y - 1)) == true) UpdateAdjacentCells((1, 3), (origin.Item1, y - 1), (origin.Item1 + 1, y - 1)); /// Right
                if(gridCells.ContainsKey((origin.Item1 - 1, y - 1)) == true) UpdateAdjacentCells((3, 1), (origin.Item1, y - 1), (origin.Item1 - 1, y - 1)); /// Left

                lastKey = (origin.Item1, y);
            }
            direction = Direction.Top;


        } else /// (direction == Direction.Left)
        {
            ruleDirection = (3, 1);

            for(int x = origin.Item1; gridCells.ContainsKey((x - 1, origin.Item2)) == true && gridCells[(x - 1, origin.Item2)].IsFilled == false; x--)
            {
                UpdateAdjacentCells(ruleDirection, (x, origin.Item2), (x - 1, origin.Item2));

                /// Once Left cell has been updated, try to update the Top and Down cell if it exist
                if(gridCells.ContainsKey((x - 1, origin.Item2 + 1)) == true) UpdateAdjacentCells((0, 2), (x - 1, origin.Item2), (x - 1, origin.Item2 + 1)); /// Top
                if(gridCells.ContainsKey((x - 1, origin.Item2 - 1)) == true) UpdateAdjacentCells((2, 0), (x - 1, origin.Item2), (x - 1, origin.Item2 - 1)); /// Down

                lastKey = (x, origin.Item2);
            }
            direction = Direction.Right;
        }

        /// Restart from the lastKey check if endImmediate is false
        if(endImmediate == false)
        {
            StartCheck(direction, lastKey, true);
        }
    }

    /// <summary>
    /// A list that will contain the new possible candidates of the cell
    /// </summary>
    static List<CellSprite.SpriteList> newCandidatesList = new List<CellSprite.SpriteList>();

    /// <summary>
    /// In UpdateAdjacentCells, the cells candidates will be updated based on the candidates of the previous cell checked or origin
    /// If the previous cell is filled, get the rules and see if it matches with the rules of the current cell
    /// Example: if the current cell has only "sky" as a rule on its Top direction, the cells above it will be updated to only have sprites that have "sky" as the Down direction rule
    /// </summary>
    /// <param name="ruleDirection"> the rule direction to use. (0,2) or (2,0) or (1,3) or (3,1) </param>
    /// <param name="origin"> the coordinates of the previous cell </param>
    /// <param name="current"> the coordinates of the the current cell to update </param>
    private static void UpdateAdjacentCells((int, int) ruleDirection, (int, int) origin, (int, int) current)
    {
        newCandidatesList.Clear();
        
        if(gridCells[current].IsFilled == false) // If the current cell is already filled, skip updating since there is nothing to update
        {
            /// If the previous cell has no spriteList assigned (having a spriteList assigned means it has an assigned sprite)
            /// Iterate over all the possible candidates of the previous cell
            /// Then, compare it to all the possible candidates of the current cell
            /// Since the there may be more than 1 rule for each side, iterate to all and compare it to all possible rule
            if(gridCells[origin].GetSpriteList == null)
            {
                foreach(CellSprite.SpriteList previousCell in gridCells[origin].GetCandidates)
                {
                    /* for(int i = 0; i < spriteLists.Length; i++)
                    {
                        for(int x = 0; x < previousCell.GetSpriteRules[ruleDirection.Item1].Length; x++)
                        {
                            for(int y = 0; y < spriteLists[i].GetSpriteRules[ruleDirection.Item2].Length; y++)
                            {
                                if(previousCell.GetSpriteRules[ruleDirection.Item1][x] == spriteLists[i].GetSpriteRules[ruleDirection.Item2][y])
                                {
                                    newCandidatesList.Add(spriteLists[i]);
                                }
                            }
                        }
                    } */
                    foreach(CellSprite.SpriteList currentCell in gridCells[current].GetCandidates)
                    {
                        for(int x = 0; x < previousCell.GetSpriteRules[ruleDirection.Item1].Length; x++)
                        {
                            for(int y = 0; y < currentCell.GetSpriteRules[ruleDirection.Item2].Length; y++)
                            {
                                if(previousCell.GetSpriteRules[ruleDirection.Item1][x] == currentCell.GetSpriteRules[ruleDirection.Item2][y])
                                {
                                    newCandidatesList.Add(currentCell);
                                }
                            }
                        }
                    }

                }
            } else 
            {   /// If the previous cell has a spriteList assigned (having a spriteList assigned means it has an assigned sprite)
                /// Using the sprite and the spriteRule of the previous cell
                /// Compare it to all the possible candidates of the current cell
                /// Since the there may be more than 1 rule for each side, iterate to all and compare it to all possible rule
                foreach(CellSprite.SpriteList currentCell in gridCells[current].GetCandidates)
                {
                    /* if(gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1] == sprite.GetSpriteRules[ruleDirection.Item2])
                    {
                        newCandidatesList.Add(sprite);
                    } */
                    for(int x = 0; x < gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1].Length; x++)
                    {
                        for(int y = 0; y < currentCell.GetSpriteRules[ruleDirection.Item2].Length; y++)
                        {
                            if(gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1][x] == currentCell.GetSpriteRules[ruleDirection.Item2][y])
                            {
                                newCandidatesList.Add(currentCell);
                            }
                        }
                    }
                }
            }
        }

        /// Update the candidates of the current cell using the newCandidatesList
        /// If the newCandidatesList has no element, do not update since it may destroy the other cell candidates
        /// A cell having 0 candidates possible means that the sprite rule doesn't cover all possiblitie.
        /// Currently, this code has no solution to fix if that ever happens other than creating a new sprite to fill that gap
        if(newCandidatesList.Count > 0) gridCells[current].UpdateCandidates(newCandidatesList);

        //if(gridCells[current].GetCandidates.Count <= 0 && gridCells[current].IsFilled == false) Debug.Log($"{current}");
        //Debug.Log(gridCells[current].GetCandidates.Count);
    }
}
