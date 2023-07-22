using System;
using UnityEngine;

public enum IngredientUnit { Spoon, Cup, Bowl, Piece }

// Custom serializable class
[Serializable]
public class Ingredient
{
    public string name;
    public Sprite sprite;
    public IngredientUnit unit;
}

[Serializable]
public class Recipe : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
}