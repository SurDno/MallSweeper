using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : Singleton<Inventory> {
    public List<Image> hotbarSprites;
    public List<Item> items;
    public int hotbarIndex;
    public bool allOrgansCollected;
    
    public void AddItem(Item item) {
        if (items.Count == 6)
            return;
        
        if (hotbarIndex >= hotbarSprites.Count)
            return;

        items.Add(item);
        hotbarSprites[hotbarIndex].sprite = item.Image;
        hotbarSprites[hotbarIndex].enabled = true;
        hotbarIndex++;

        if (hotbarIndex > 3)
            LightsOutManager.Instance.TurnLightsOff();
        
        var organsLeft = 6 - items.Count;
        GoalManager.Instance.SetNewGoal($"Find {organsLeft} more organs to perform the ritual.");

        if (items.Count == 6) {
            allOrgansCollected = true;
            GoalManager.Instance.SetNewGoal($"Find the pentagram to perform the ritual.");
        }
    }

    public void ClickOnItem(int index) {
        if (index < 0 || index >= items.Count)
            return;

        var selectedItem = items[index];
        items.RemoveAt(index);
        for (int i = 0; i < hotbarSprites.Count; i++) {
            hotbarSprites[i].sprite = items.Count > i ? items[i].Image : null;
            hotbarSprites[i].enabled = items.Count > i;
        }
        PentagramManager.Instance.PlaceOrgan(selectedItem);

        hotbarIndex--;
    }
}
