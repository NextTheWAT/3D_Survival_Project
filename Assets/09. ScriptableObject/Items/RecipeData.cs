
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecipeIngredient
{
    public int itemId;
    public int amount;
}
[CreateAssetMenu(fileName = "RecipeData", menuName = "Database/New Recipe Data")]
public class RecipeData : ScriptableObject
{
    public List<RecipeIngredient> resources;
    public int outputItemId;
    public float processTime;
}

