using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UIInventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    public UISlot slotPrefab;
    public Transform slotsContainer;
    public float marginPerSlot;
    public List<UISlot> slots = new();

    [Header("Input Actions")]
    public InputSystem_Actions inputActions;

    [Header("Drag Mechanic")]
    [SerializeField] private bool hold = false;
    [SerializeField] private float deadZone = 3f;
    [SerializeField] private GameObject tempSlot;

    private Vector3 dragStartMousePos = Vector3.zero;
    private bool hasDragged = false;

    public UISlot SelectedSlot;

    private int indexStart = -1;
    private int indexFinal = -1;

    private void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();

        inputActions.UI.Click.started += OnClickDown;
        inputActions.UI.Click.performed += OnClick;
        inputActions.UI.Click.canceled += OnClickUp;

    }

    private void OnDisable()
    {
        inputActions.UI.Click.started -= OnClickDown;
        inputActions.UI.Click.performed -= OnClick;
        inputActions.UI.Click.canceled -= OnClickUp;

        inputActions.Disable();
    }

    private void Update()
    {
        HandleDragMechanism();
    }

    public void SetInventory(Inventory inventory)
    {
        for (int i = 0; i < inventory.size; i++)
        {
            UISlot uiSlot = Instantiate(slotPrefab, slotsContainer);
            uiSlot.name = $"Slot{i}";
            uiSlot.SetSlot(i);
            
            slots.Add(uiSlot);

            SetSlot(i, inventory.ItemManager.CurrentSlots[i]);
            uiSlot.SetNeighbors(inventory.ItemManager.CurrentSlots[i], this);
        }
        slotsContainer.GetComponent<GridLayoutGroup>().constraintCount = inventory.rows;
    }

    public void SetSlot(int position, Slot slot)
    {
        if (position < 0 || position >= slots.Count || slot == null || slots[position] == null)
        {
            Debug.LogError($"Invalid slot set at position {position}");
            return;
        }

        slots[position].SetSlot(slot, this);
    }
    public void ClearSlot(int position)
    {
        if (position < 0 || position >= slots.Count)
        {
            Debug.LogError($"Invalid slot clear at position {position}");
            return;
        }
        slots[position].Clear();
    }



    private void HandleDragMechanism()
    {
        if (!hold || SelectedSlot == null || !SelectedSlot.HasContent)
            return;

        Vector3 currentMousePos = Input.mousePosition;
        float distance = Vector3.Distance(dragStartMousePos, currentMousePos);

        if (!hasDragged && distance < deadZone)
            return;

        hasDragged = true;

        if (tempSlot == null)
        {
            tempSlot = new GameObject("TempSlot", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            tempSlot.transform.SetParent(transform, false);

            var image = tempSlot.GetComponent<Image>();
            image.sprite = SelectedSlot.icon.sprite;
            image.raycastTarget = false;
            image.preserveAspect = true;
        }

        tempSlot.transform.position = currentMousePos;
    }

    public void HighlightSlots(List<UISlot> slotList)
    {
        foreach (var item in slotList)
        {
            item.icon.color = item.highlightColor;
        }
    }

    public void TriggerSlotsByIndexes(List<int> indexes)
    {
        foreach (var index in indexes)
        {
            if (index < 0 || index >= slots.Count || slots[index] == null)
            {
                Debug.LogError($"Index {index} out of range");
                continue;
            }
            slots[index].SwitchSelectedState();
        }
    }

    private void OnClickDown(InputAction.CallbackContext ctx)
    {
        var hoveredSlot = GetSlotUnderMouse();
        if (hoveredSlot != null && hoveredSlot.HasContent)
        {
            SelectedSlot = hoveredSlot;
            indexStart = SelectedSlot.UISlotIndex;
            dragStartMousePos = Input.mousePosition;
        }
        hold = true;
        hasDragged = false;
    }

    private void OnClick(InputAction.CallbackContext ctx) 
    {
        
    }

    private void OnClickUp(InputAction.CallbackContext ctx)
    {
        var hoveredSlot = GetSlotUnderMouse();
        if (hoveredSlot != null)
        {
            SelectedSlot = hoveredSlot;
            indexFinal = SelectedSlot.UISlotIndex;
        }

        if (hasDragged && indexStart != -1 && indexFinal != -1)
            GameManager.Instance.inventory.SwapItems(indexStart, indexFinal);
        else
        {
            HandleSimpleClick(GetSlotUnderMouse());
        }

        indexStart = -1;
        indexFinal = -1;
        Destroy(tempSlot);
        tempSlot = null;
        SelectedSlot = null;
        hold = false;
        hasDragged = false;
    }
    private void HandleSimpleClick(UISlot clickedSlot)
    {
        if (clickedSlot != null)
        {
            Debug.Log("Click simple sobre: " + clickedSlot.name);

            //->Descoplar despues con eventos
            GameManager.Instance.inventory.uiSlotInspector.SetSlot(clickedSlot.ItemData);

        }
    }

    private UISlot GetSlotUnderMouse()
    {
        PointerEventData pointerData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent(out UISlot slot))
                return slot;
        }

        return null;
    }
}
