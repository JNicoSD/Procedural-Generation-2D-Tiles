using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(GenerateTiles))]
public class GenerateTilesEditor : Editor
{
    private static bool hasOrigin = false;
    public override void OnInspectorGUI()
    {
        GenerateTiles generateTiles = (GenerateTiles)target;
        base.OnInspectorGUI();

        if(hasOrigin == true)
        {
            if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} ABOVE "))
            {
                ExtendGrid.ExtendUp(generateTiles);
            } else if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} RIGHT "))
            {
                ExtendGrid.ExtendRight(generateTiles);
            } else if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} DOWN "))
            {
                ExtendGrid.ExtendDown(generateTiles);
            } else if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} LEFT "))
            {
                ExtendGrid.ExtendLeft(generateTiles);
            } else if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} AROUND "))
            {
                ExtendGrid.ExtendAllDirection(generateTiles);
            }
        } else if(hasOrigin == false)
        {
            if(GUILayout.Button($"Generate {generateTiles.width} x {generateTiles.height} Tiles "))
            {
                generateTiles.Start();
                hasOrigin = true;
                generateTiles.isExtend = true;
            }
            
        }
        
        if(GUILayout.Button($"RANDOM DIRECTION"))
        {
            AutomatedGeneration(generateTiles);
            
            //DeleteTileSet(generateTiles); 
        }

        if(GUILayout.Button($"Destroy Tiles "))
        {
            DeleteTileSet(generateTiles); 
        }
    }

    public void DeleteTileSet(GenerateTiles generateTiles)
    {
        foreach(var cell in generateTiles.GridCells)
        {
            DestroyImmediate(cell.Value.GetGameObject);
        }

        foreach(var g in generateTiles.TileGroup)
        {
            DestroyImmediate(g);
        }

        generateTiles.GridCells.Clear();
        generateTiles.startX = 0;
        generateTiles.startY = 0;
        generateTiles.isExtend = false;
        hasOrigin = false;
    }

    void AutomatedGeneration(GenerateTiles generateTiles)
    {
        if(hasOrigin == false)
        {
            generateTiles.Start();
            hasOrigin = true;
            generateTiles.isExtend = true;
        }


        int i = 0;
        int randomLength = UnityEngine.Random.Range(0, 15);
        for(int r = 0; r < randomLength; r++)
        {
            i = UnityEngine.Random.Range(0, 5);

            if(i == 0) ExtendGrid.ExtendUp(generateTiles);
            else if(i == 1) ExtendGrid.ExtendRight(generateTiles);
            else if(i == 2) ExtendGrid.ExtendDown(generateTiles);
            else if(i == 3) ExtendGrid.ExtendLeft(generateTiles);
            else if(i == 4) ExtendGrid.ExtendAllDirection(generateTiles);
        }
    }
}
