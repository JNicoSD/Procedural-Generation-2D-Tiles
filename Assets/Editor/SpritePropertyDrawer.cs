/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sprite))]
public class SpritePropertyDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
    {
        EditorGUI.BeginProperty(position, label, property);
    
        Sprite sprite = property.objectReferenceValue as Sprite;

        var buttonRect = new Rect(position.x, position.y, 350f, 15f);
        var spriteRect = new Rect(buttonRect.x + buttonRect.width + 10f, position.y, 20f, 20f);

        //DrawEditSprite(buttonRect, property, sprite);
        EditorGUI.ObjectField(buttonRect, "Select Sprite: ", sprite, typeof(Sprite));
        if(sprite != null) EditorGUI.DrawPreviewTexture(spriteRect, sprite.texture, null, ScaleMode.StretchToFill);

        EditorGUI.EndProperty();
    }

    string buttonLabel;
    private void DrawEditSprite(Rect buttonRect, SerializedProperty property, Sprite sprite)
    {   
        buttonLabel = sprite != null ? "Edit Sprite: " : "Add Sprite";

        if(GUI.Button(buttonRect, buttonLabel))
        {
            string spritePath = EditorUtility.OpenFilePanel("Select Sprite: ", "Assets", "png, jpg, jpeg");
            if(string.IsNullOrEmpty(spritePath) == false)
            {
                string projectRelativePath = "Assets" + spritePath.Replace(Application.dataPath, "");
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(projectRelativePath);
                property.objectReferenceValue = sprite;
            }
        }
    }
} 

 */