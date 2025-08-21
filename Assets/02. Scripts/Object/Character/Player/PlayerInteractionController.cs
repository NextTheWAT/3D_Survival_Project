#nullable disable
using Object.Character.Player;
using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.Input;
public class PlayerInteractionController : MonoBehaviour
{
    private CharacterControls controls;
    [SerializeField] private float interactDistance = 2f;
    private PlayerPerspectiveController controller;
    private EquipItemData? currentEquippedItem;

    private void Awake()
    {
        controls = GameManager.Instance.Controls;
        controller = GetComponent<PlayerPerspectiveController>();
        controls.Player.Interact.performed += OnInteractPerformed;
    }

    private void OnDestroy()
    {
        controls.Player.Interact.performed -= OnInteractPerformed;
    }

    public void SetEquipptedItem(EquipItemData? equippedItem)
    {
        if(equippedItem == null)
        {
            currentEquippedItem = null;
        }
        else
        {
            currentEquippedItem = equippedItem;
        }
    }

    private void OnInteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 origin = controller.PerspectiveCameraRig.position;
        Vector3 direction = controller.PerspectiveCameraRig.forward;

        Debug.DrawRay(origin, direction * interactDistance, Color.green, 1f); //1초만 일단


        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                ResourceObject resource = interactable as ResourceObject;
                if (resource != null)
                {
                    if (currentEquippedItem != null && currentEquippedItem.type == EquipType.Tool)
                    {
                        interactable.OnInteract();
                    }
                }
                else
                {
                    interactable.OnInteract();
                }
            }
        }
    }
}
