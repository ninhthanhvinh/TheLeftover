using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectlyUsingItem : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    public void UseItemDirectly(InventorySlot _slot)
    {
        float healthRegen = 0;
        //float foodGen = 0;
        if (_slot.ItemObject.type == ItemType.Food)
        {
            healthRegen = _slot.ItemObject.characterDisplay.GetComponent<Food>().healthGen;
            //foodGen = _slot.ItemObject.characterDisplay.GetComponent<Food>().foodGen;
            _slot.RemoveItem();
        }

        player.currentHealth += healthRegen;
        if (player.currentHealth >= player.maxHealth)
            player.currentHealth = player.maxHealth;




    }
}
