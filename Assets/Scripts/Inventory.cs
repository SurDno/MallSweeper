using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory> {
    public List<Image> sprites;
    public List<Item> items;
    public int index;
    
    public void AddItem(Item item) {
        if (index > 5)
            return;
        
        items.Add(item);
        sprites[index].sprite = items[index].Image;
        sprites[index].enabled = true;
        index++;
    }
}
