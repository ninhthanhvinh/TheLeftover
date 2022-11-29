using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public MouseItem mouseItem = new MouseItem();
    public float movementSpeed = 3f;
    
    public float maxHealth;
    public float currentHealth;

    public Rigidbody2D rb;

    public Animator anim;

    private Transform gunPosition;
    private Transform helmetPosition;
    private Transform armorPosition;
    private Transform helmet;
    private Transform armor;

    Vector2 movement;

    public InventoryObject inventory;
    public InventoryObject equipment;

    public Attribute[] attributes;
    void Start()
    {
        helmetPosition = GameObject.Find("Head").transform;
        armorPosition = GameObject.Find("Chest").transform;
        maxHealth = 100;
        currentHealth = maxHealth;
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.allowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attributes)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Armor:
                break;
            default:
                break;

        }

        if (_slot.ItemObject.characterDisplay != null)
        {
            switch (_slot.allowedItems[0])
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    Destroy(gunPosition.gameObject);
                    break;
                case ItemType.Helmet:
                    Destroy(helmet.gameObject);
                    break;
                case ItemType.Armor:
                    Destroy(armor.gameObject);
                    break;
                case ItemType.Bullet:
                    break;
                default:
                    break;
            }
        }
    }

    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.allowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if(attributes[j].type == _slot.item.buffs[i].attributes)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Armor:
                break;
            default:
                break;

        }

        if(_slot.ItemObject.characterDisplay != null)
        {
            switch (_slot.allowedItems[0])
            {
                case ItemType.Food:
                    break;
                case ItemType.Weapon:
                    gunPosition = Instantiate(_slot.ItemObject.characterDisplay, transform.position, Quaternion.identity, transform).transform;
                    break;
                case ItemType.Helmet:
                    helmet = Instantiate(_slot.ItemObject.characterDisplay, helmetPosition.position, Quaternion.identity, helmetPosition).transform;
                    break;
                case ItemType.Armor:
                    armor = Instantiate(_slot.ItemObject.characterDisplay, armorPosition.position, Quaternion.identity, armorPosition).transform;
                    break;
                case ItemType.Bullet:
                    break;
                default:
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            inventory.Load();
            equipment.Load();
        }

        if (currentHealth <= 0)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if(item)
        {
            Item _item = new(item.item);
            if(inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }

        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, "was updated. Value is now ", attribute.value.ModifiedValue));
    }
    public void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
    }

    public void UseItemDirectly(InventorySlot _slot)
    {
        float healthRegen = 0;
        if (_slot.ItemObject.type == ItemType.Food)
        {
            healthRegen = _slot.ItemObject.characterDisplay.GetComponent<Food>().healthGen;
            _slot.RemoveItem();
        }

        currentHealth += healthRegen;
        if (currentHealth >= maxHealth) 
            currentHealth = maxHealth;
    }
}




[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerController parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerController _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}