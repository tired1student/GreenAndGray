using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image slotImage;
    public Item slotItem;
    public Text slotAmount;

    public static string itemName = "";

    public void ItemOnClick()
    {
        InventoryManager.UpdateItemInformation(slotItem.itemInfo);
        itemName = slotItem.itemName;
    }
}
