using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotbarClickDetector : MonoBehaviour, IPointerClickHandler {
    public int hotbarIndex;

    public void OnPointerClick(PointerEventData eventData) {
        if (!PentagramManager.Instance.canPlaceOrgans)
            return;
        
        Inventory.Instance.ClickOnItem(hotbarIndex);
    }
}
