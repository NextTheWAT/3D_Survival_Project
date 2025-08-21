using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSystem
{
    private List<RecipeData> recipes = new List<RecipeData>();
    private bool isCrafting = false;

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

    public RecipeData GetTransformRecipe(int itemId)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.resources.Count == 1 && recipe.resources[0].itemId == itemId)
                return recipe;
        }
        return null;
    }
    public IEnumerator CraftCoroutine(RecipeData recipe)
    {
        if (isCrafting)
        {
            Debug.Log("Try again after finishing previous crafting.");
            yield break;
        }

        isCrafting = true;

        Debug.Log($"Crafting started: output {recipe.outputItemId}");

        // 재료 제거: slot 단위로 제거
        foreach (var resource in recipe.resources)
        {
            // 해당 itemId를 가진 슬롯 중 첫 번째 사용 가능한 슬롯 찾기
            var slot = manager.GetSlotDatas().Find(s => s.itemData.id == resource.itemId && s.count > 0);
            if (slot != null)
            {
                manager.RemoveOneItemFromSlot(slot.slotId);
            }
            else
            {
                Debug.LogWarning($"Not enough items for crafting: itemId {resource.itemId}");
                isCrafting = false;
                yield break;
            }
        }

        yield return new WaitForSeconds(recipe.processTime);

        Debug.Log($"Crafting finished: crafted {recipe.outputItemId}");
        manager.CraftFinished(recipe);

        isCrafting = false;
    }
}
