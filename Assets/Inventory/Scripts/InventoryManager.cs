using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory playerBag;
    public GameObject slotGrid;
    public Slot slotPrefab;
    public Text itemInformation;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void ClearItemInformation()
    {
        instance.itemInformation.text = "";
    }

    private static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab, instance.slotGrid.transform.position, Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);

        //将物品的信息传给背包中的格子里
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        int itemAmount = instance.playerBag.itemAmountList[instance.playerBag.itemList.IndexOf(item)];
        if (itemAmount == 1)
        {
            newItem.slotAmount.text = "";
        } else
        {
            newItem.slotAmount.text = itemAmount.ToString();
        }
    }

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < instance.playerBag.itemList.Count; i++)
        {
            CreateNewItem(instance.playerBag.itemList[i]);
        }
    }

    public static void UpdateItemInformation(string itemInfo)
    {
        instance.itemInformation.text = itemInfo;
    }
}
