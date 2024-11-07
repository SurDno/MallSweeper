using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PentagramManager : MonoBehaviour, IPointerClickHandler
{
    public int hotbarIndex;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory.Instance.SelectItem(hotbarIndex);
    }
}
