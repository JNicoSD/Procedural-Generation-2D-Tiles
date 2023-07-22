using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridCellGizmo : MonoBehaviour
{
    public GenerateTiles generateTiles;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(generateTiles != null)
        {
            Vector3[] points = new Vector3[]
            {
                new Vector3((generateTiles.startX - generateTiles.GridHalfWidth - 0.5f), generateTiles.startY + generateTiles.GridHalfHeight- 0.5f),
                new Vector3(generateTiles.startX - generateTiles.GridHalfWidth - 0.5f, (generateTiles.startY + generateTiles.height / 2f)),

                new Vector3(generateTiles.startX - generateTiles.GridHalfWidth - 0.5f, (generateTiles.startY + generateTiles.height / 2f)),
                new Vector3((generateTiles.startX + generateTiles.width / 2f), (generateTiles.startY + generateTiles.height / 2f)),

                new Vector3((generateTiles.startX + generateTiles.width / 2f), (generateTiles.startY + generateTiles.height / 2f)),
                new Vector3((generateTiles.startX + generateTiles.width / 2f), generateTiles.startY + generateTiles.GridHalfHeight - 0.5f),

                new Vector3((generateTiles.startX + generateTiles.width / 2f), generateTiles.startY + generateTiles.GridHalfHeight - 0.5f),
                new Vector3(generateTiles.startX - generateTiles.GridHalfWidth - 0.5f, generateTiles.startY + generateTiles.GridHalfHeight - 0.5f)
            };

            Gizmos.color = Color.green;
            Gizmos.DrawLineList(points);
        }
    }
    #endif
}
