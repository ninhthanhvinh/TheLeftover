using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new();
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        slotsOnInterface.UpdateSlotDisplay();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;

        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot _slot)
    {
        if (_slot.item.Id >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplay;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
    public abstract void CreateSlots();
   

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnPointerClick(GameObject obj)
    {
        var item = slotsOnInterface[obj];
        if(item.ItemObject != null)
        {
            if(item.ItemObject.type == ItemType.Food)
            {
                float healGen = 0;
                for (int i = 0; i < item.item.buffs.Length; i++)
                {
                    if(item.item.buffs[i].attributes == Attributes.Healing)
                    {
                        healGen = item.item.buffs[i].value;
                    }
                }
                player.currentHealth += healGen;
                item.RemoveItem();
                Debug.Log(string.Concat("Healing", healGen));
            }

            else if (item.ItemObject.type == ItemType.Weapon)
            {
                var equipSlot = GameObject.Find("Equipment Screen").GetComponent<StaticInterface>();
                if (equipSlot.slotsOnInterface[equipSlot.slots[2]].ItemObject == null)
                {
                    equipSlot.slotsOnInterface[equipSlot.slots[2]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                }

                else
                {
                    var temp = equipSlot.slotsOnInterface[equipSlot.slots[2]].ItemObject;
                    equipSlot.slotsOnInterface[equipSlot.slots[2]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                    item.UpdateSlot(temp.data, 1);
                }
            }

            else if (item.ItemObject.type == ItemType.Helmet)
            {
                var equipSlot = GameObject.Find("Equipment Screen").GetComponent<StaticInterface>();
                if (equipSlot.slotsOnInterface[equipSlot.slots[0]].ItemObject == null)
                {
                    equipSlot.slotsOnInterface[equipSlot.slots[0]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                }

                else
                {
                    var temp = equipSlot.slotsOnInterface[equipSlot.slots[0]].ItemObject;
                    equipSlot.slotsOnInterface[equipSlot.slots[0]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                    item.UpdateSlot(temp.data, 1);
                }
            }

            else if (item.ItemObject.type == ItemType.Armor)
            {
                var equipSlot = GameObject.Find("Equipment Screen").GetComponent<StaticInterface>();
                if (equipSlot.slotsOnInterface[equipSlot.slots[1]].ItemObject == null)
                {
                    equipSlot.slotsOnInterface[equipSlot.slots[1]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                }

                else
                {
                    var temp = equipSlot.slotsOnInterface[equipSlot.slots[1]].ItemObject;
                    equipSlot.slotsOnInterface[equipSlot.slots[1]].UpdateSlot(item.ItemObject.data, 1);
                    item.RemoveItem();
                    item.UpdateSlot(temp.data, 1);
                }
            }
        }

    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;

        if (slotsOnInterface[obj].item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;

    }

    public void OnDragEnd(GameObject obj)
    {

        Destroy(MouseData.tempItemBeingDragged);

       if(MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoveredOver;

}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface)
        {
            if (_slot.Value.item.Id >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}