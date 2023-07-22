using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(CellSprite))]
public class CellSpriteEditor : Editor 
{
    public override void OnInspectorGUI() 
    {
        CellSprite cellSprite = target as CellSprite;
        base.OnInspectorGUI();

        if(GUILayout.Button("SAVE AS JSON"))
        {
            string path = EditorUtility.SaveFilePanel("Save JSON File: ", EditorFilePathSave.lastFilePath != null ? EditorFilePathSave.lastFilePath : "Assets", "*.json", "json");

            if(path.Length != 0)
            {
                var jsonSave = JsonUtility.ToJson(cellSprite.GetCellSprite(), true);
                System.IO.File.WriteAllText(path, jsonSave);

                EditorFilePathSave.lastFilePath = path; /// Remembers the last file path
            }

            
        } 

        if(GUILayout.Button("LOAD JSON FILE"))
        {
            string path = EditorUtility.OpenFilePanel("Open JSON File: ", EditorFilePathSave.lastFilePath != null ? EditorFilePathSave.lastFilePath : "Assets", "json");

            if(path.Length != 0) 
            {
                var fileText = System.IO.File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(fileText, cellSprite.GetCellSprite());

                EditorFilePathSave.lastFilePath = path; /// Remembers the last file path
            }
        }
    }
}
