using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Input;
using Object.Character.Player;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    [Header("Player")]
    public PlayerCondition playerCondition;
    public Transform playerPosition;
    public BuildingActionController buildingActionController;

    [Header("Item Inventory")]
    public InventoryManager inventoryManager;
    public ItemDatabase itemDatabase;
    public InventoryUI inventoryUI;

    [Header("InputAction")]
    private CharacterControls controls;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            
            controls = new CharacterControls();
            controls.Player.OpenInventory.performed += OnOpenInventoryPerformed;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable() => controls?.Enable();
    private void OnDisable() => controls?.Disable();
    private void OnDestroy() => controls.Player.OpenInventory.performed -= OnOpenInventoryPerformed;

    public CharacterControls Controls => controls;

    private void OnOpenInventoryPerformed(InputAction.CallbackContext context)
    {
        if (inventoryUI != null)
        {
            if (inventoryUI.gameObject.activeSelf) // 열린 상태면 닫기
            {
                inventoryUI.Close();
            }
            else // 닫혀있으면 열기
            {
                inventoryUI.Open();
            }
        }
    }

    public void EnterBuildingMode()
    {
        var id = inventoryUI.selectedSlotId.Value;
        var instance = inventoryManager.GetSlotBySlotId(id).itemData.inGamePrefab;

        Debug.Log(id);

        Debug.Log(instance);

        buildingActionController.Simulate(instance);
    }
}
