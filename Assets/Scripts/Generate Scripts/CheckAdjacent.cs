using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckAdjacent
{
    enum Direction
    {
        Top,
        Right,
        Down,
        Left
    }

    static CellSprite.SpriteList[] spriteLists;
    static Dictionary<(int, int), Cell> gridCells = new Dictionary<(int, int), Cell>();
    public static void CheckCells(Dictionary<(int, int), Cell> _gridCells, (int, int) _origin, CellSprite.SpriteList[] _spriteList)
    {
        gridCells = _gridCells;
        spriteLists = _spriteList;

        for(int i = 0; i < 4; i++)
        {
            StartCheck((Direction)i, _origin, false);
        }
    }

    static (int, int) ruleDirection;
    static (int, int) lastKey;
    private static void StartCheck(Direction direction, (int, int) origin, bool endImmediate)
    {
        if(direction == Direction.Top)
        {
            ruleDirection = (0, 2);
            
            for(int y = origin.Item2; gridCells.ContainsKey((origin.Item1, y + 1)) == true && gridCells[(origin.Item1, y + 1)].IsFilled == false; y++)
            {
                UpdateAdjacentCells(ruleDirection, (origin.Item1, y), (origin.Item1, y + 1));

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

                if(gridCells.ContainsKey((x - 1, origin.Item2 + 1)) == true) UpdateAdjacentCells((0, 2), (x - 1, origin.Item2), (x - 1, origin.Item2 + 1)); /// Top
                if(gridCells.ContainsKey((x - 1, origin.Item2 - 1)) == true) UpdateAdjacentCells((2, 0), (x - 1, origin.Item2), (x - 1, origin.Item2 - 1)); /// Down

                lastKey = (x, origin.Item2);
            }
            direction = Direction.Right;
        }

        if(endImmediate == false)
        {
            StartCheck(direction, lastKey, true);
        }
    }

    static List<CellSprite.SpriteList> newCandidatesList = new List<CellSprite.SpriteList>();
    private static void UpdateAdjacentCells((int, int) ruleDirection, (int, int) origin, (int, int) current)
    {
        newCandidatesList.Clear();
        if(gridCells[current].IsFilled == false)
        {
            if(gridCells[origin].GetSpriteList == null)
            {
                foreach(CellSprite.SpriteList sprite in gridCells[origin].GetCandidates)
                {
                    for(int i = 0; i < spriteLists.Length; i++)
                    {
                        /* if(sprite.GetSpriteRules[ruleDirection.Item1] == spriteLists[i].GetSpriteRules[ruleDirection.Item2])
                        {
                            newCandidatesList.Add(spriteLists[i]);
                        } */

                        for(int x = 0; x < sprite.GetSpriteRules[ruleDirection.Item1].Length; x++)
                        {
                            for(int y = 0; y < spriteLists[i].GetSpriteRules[ruleDirection.Item2].Length; y++)
                            {
                                if(sprite.GetSpriteRules[ruleDirection.Item1][x] == spriteLists[i].GetSpriteRules[ruleDirection.Item2][y])
                                {
                                    newCandidatesList.Add(spriteLists[i]);
                                }
                            }
                        }
                    }
                }
            } else
            {
                foreach(CellSprite.SpriteList sprite in spriteLists)
                {
                    /* if(gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1] == sprite.GetSpriteRules[ruleDirection.Item2])
                    {
                        newCandidatesList.Add(sprite);
                    } */
                    for(int x = 0; x < gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1].Length; x++)
                    {
                        for(int y = 0; y < sprite.GetSpriteRules[ruleDirection.Item2].Length; y++)
                        {
                            if(gridCells[origin].GetSpriteList.GetSpriteRules[ruleDirection.Item1][x] == sprite.GetSpriteRules[ruleDirection.Item2][y])
                            {
                                newCandidatesList.Add(sprite);
                            }
                        }
                    }
                }
            }
        }
        if(newCandidatesList.Count > 0) gridCells[current].UpdateCandidates(newCandidatesList);

        //if(gridCells[current].GetCandidates.Count <= 0 && gridCells[current].IsFilled == false) Debug.Log($"{current}");
        //Debug.Log(gridCells[current].GetCandidates.Count);
    }
}
