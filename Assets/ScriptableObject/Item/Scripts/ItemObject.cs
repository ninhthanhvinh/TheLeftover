using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Weapon,
    Helmet,
    Armor,
    Bullet
}

public enum Attributes
{
    Agility,
    Intellect,
    Stamina,
    Strength,
    Healing
}
[CreateAssetMenu(fileName = "Create New Item", menuName = "Inventory System/Items/Create")]
public class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public Item data = new();
    public GameObject characterDisplay;
    public Item CreateItem()
    {
        Item newItem = new(this);
        return newItem;
    }

}

[System.Serializable]
public class Item
{
    public string name;
    public int Id = -1;
    public ItemBuff[] buffs;

    public Item()
    {
        name = "";
        Id = -1;
    }

    public Item(ItemObject _item)
    {
        name = _item.name;
        Id = _item.data.Id;
        buffs = new ItemBuff[_item.data.buffs.Length];
        for(int i = 0; i < buffs.Length; i++)
        {            
            buffs[i] = new ItemBuff(_item.data.buffs[i].min, _item.data.buffs[i].max);
            buffs[i].attributes = _item.data.buffs[i].attributes;
        }
    }
}
[System.Serializable]
public class ItemBuff : IModifier
{
    public Attributes attributes;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }
}