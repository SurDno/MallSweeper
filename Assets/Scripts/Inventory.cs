using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory>
{
    public List<Image> hotbarSprites;
    public List<Item> items;
    public int hotbarIndex;
    
    private Item selectedItem;
    
    public void AddItem(Item item)
    {
        if (hotbarIndex >= hotbarSprites.Count)
            return;
        
        items.Add(item);
        hotbarSprites[hotbarIndex].sprite = item.Image;
        hotbarSprites[hotbarIndex].enabled = true;
        hotbarIndex++;
    }
    
    public void SelectItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return;
        
        selectedItem = items[index];
        Debug.Log($"Selected item: {selectedItem.name}");
        
        // You can now use the selected item in a different script
        UseSelectedItem();
    }
    
    private void UseSelectedItem()
    {
        // Add your logic to use the selected item here
        // For example, you could call a method in another script
    }
    
    public void DeselectItem()
    {
        selectedItem = null;
    }
}
