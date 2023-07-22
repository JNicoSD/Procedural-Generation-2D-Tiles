using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendGrid
{
    public static void ExtendUp(GenerateTiles generateTiles)
    {
        if(generateTiles != null) generateTiles.startY += generateTiles.height;

        Extend(generateTiles);
    }
    public static void ExtendRight(GenerateTiles generateTiles)
    {
        if(generateTiles != null) generateTiles.startX += generateTiles.width;

        Extend(generateTiles);
    }
    public static void ExtendDown(GenerateTiles generateTiles)
    {
        if(generateTiles != null) generateTiles.startY += -generateTiles.height;

        Extend(generateTiles);
    }
    public static void ExtendLeft(GenerateTiles generateTiles)
    {
        if(generateTiles != null) generateTiles.startX += -generateTiles.width;

        Extend(generateTiles);
    }
    public static void ExtendAllDirection(GenerateTiles generateTiles)
    {
        if(generateTiles != null)
        {
            generateTiles.startY += generateTiles.height;   /// TOP
            Extend(generateTiles);
            generateTiles.startY -= generateTiles.height;

            generateTiles.startX += generateTiles.width;    /// RIGHT
            Extend(generateTiles);
            generateTiles.startX -= generateTiles.width;

            generateTiles.startY += -generateTiles.height;  /// DOWN
            Extend(generateTiles);
            generateTiles.startY -= -generateTiles.height;

            generateTiles.startX += -generateTiles.width;   /// LEFT
            Extend(generateTiles);
            generateTiles.startX -= -generateTiles.width;
        }
        //Extend(generateTiles);
    }
    private static void Extend(GenerateTiles generateTiles)
    {
        if(generateTiles.GridCells.ContainsKey((generateTiles.startX, generateTiles.startY)) == false)
        {
            generateTiles.Start();
            UpdateBorders(generateTiles);
            generateTiles.GenerateSprites();
        }
    }
    private static void UpdateBorders(GenerateTiles generateTiles)
    {
        if(generateTiles.GridCells.ContainsKey((generateTiles.startX, generateTiles.startY + generateTiles.height)) == true && generateTiles.GridCells[(generateTiles.startX, generateTiles.startY + generateTiles.height)].IsFilled == true)
        { 
            // loop
            for(int x = generateTiles.startX; x < generateTiles.startX + generateTiles.width; x++)
            {
                CheckAdjacent.CheckCells(generateTiles.GridCells, (x, generateTiles.startY + generateTiles.height), generateTiles.cellSprite.spriteLists);
            }
        }
        if(generateTiles.GridCells.ContainsKey((generateTiles.startX + generateTiles.width, generateTiles.startY)) == true && generateTiles.GridCells[(generateTiles.startX + generateTiles.width, generateTiles.startY)].IsFilled == true)
        {
            // loop
            for(int y = generateTiles.startY; y < generateTiles.startY + generateTiles.height; y++)
            {
                CheckAdjacent.CheckCells(generateTiles.GridCells, (generateTiles.startX + generateTiles.width, y), generateTiles.cellSprite.spriteLists);
            }
        }
        if(generateTiles.GridCells.ContainsKey((generateTiles.startX, generateTiles.startY - 1)) == true && generateTiles.GridCells[(generateTiles.startX, generateTiles.startY - 1)].IsFilled == true)
        {
            // loop
            for(int x = generateTiles.startX; x < generateTiles.startX + generateTiles.width; x++)
            {
                CheckAdjacent.CheckCells(generateTiles.GridCells, (x, generateTiles.startY - 1), generateTiles.cellSprite.spriteLists);
            }
        }
        if(generateTiles.GridCells.ContainsKey((generateTiles.startX - 1, generateTiles.startY)) == true && generateTiles.GridCells[(generateTiles.startX - 1, generateTiles.startY)].IsFilled == true)
        {
            // loop
            for(int y = generateTiles.startY; y < generateTiles.startY + generateTiles.height; y++)
            {
                CheckAdjacent.CheckCells(generateTiles.GridCells, (generateTiles.startX - 1, y), generateTiles.cellSprite.spriteLists);
            }
        }
    }
}
