using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSystem
{
    private List<RecipeData> recipes = new List<RecipeData>();

    private InventoryManager manager;

    public CraftSystem(InventoryManager manager)
    {
        this.manager = manager;


        //임시 테스트용 데이터 넣기
        RecipeData recipe = ScriptableObject.CreateInstance<RecipeData>();

        recipe.resources = new List<RecipeIngredient>
        {
            new RecipeIngredient { itemId = 1, amount = 1 }
        };
        recipe.outputItemId = 4;
        recipe.processTime = 5f;

        recipes.Add(recipe);
    }

    public bool CanCraft(List<int> items)   //일단 각 아이템 1개씩만 조합된다고 가정함.
    {
        foreach (var recipe in recipes)
        {
            bool canCraft = true;

            foreach (var ingredient in recipe.resources)
            {
                // ingredient.itemId가 items 리스트에 포함돼 있어야 함
                if (!items.Contains(ingredient.itemId))
                {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft)
                return true;
        }

        return false;
    }

    public RecipeData GetTransformRecipe(int id)   // 1:1 변환만 고려
    {
        foreach (var recipe in recipes)
        {
            if (recipe.resources.Count == 1 && recipe.resources[0].itemId == id)
            {
                return recipe;
            }
        }

        return null; // 해당 레시피 없음
    }
    public IEnumerator CraftCoroutine(RecipeData recipe)
    {
        Debug.Log($"Crafting started: output {recipe.outputItemId}");

        foreach (var resource in recipe.resources)
        {
            manager.RemoveItem(resource.itemId);
        }

        yield return new WaitForSeconds(recipe.processTime);

        Debug.Log($"Crafting finished: crafted {recipe.outputItemId}");

        manager.CraftFinished(recipe);
    }
}
