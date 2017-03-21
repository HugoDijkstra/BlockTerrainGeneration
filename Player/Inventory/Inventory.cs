using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField]
    GameObject[] slots;
    [SerializeField]
    int inventorySize;
    [SerializeField]
    Item[] inventory;

    GameObject craftingSlot;

    GameObject inventoryUI;
    GameObject hotbar;

    List<GameObject> selected;
    EventSystem es;

    Item CurrentlyCrafting;
    // Use this for initialization
    void Start()
    {
        craftingSlot = GameObject.Find("Crafting");
        selected = new List<GameObject>();
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        GameObject inventoryUI = GameObject.Find("Inventory");
        for (int i = 0; i < inventorySize; i++)
        {
            slots[i] = inventoryUI.transform.Find("Slot_" + i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Select(GameObject GO)
    {
        if (selected.Contains(GO))
        {
            selected.Remove(GO);
        }
        else
        {
            selected.Add(GO);
        }
        List<Item> list = new List<Item>();
        for (int j = 0; j < selected.Count; j++)
        {
            list.Add(selected[j].GetComponent<InventorySlot>().Holding);
        }
        CurrentlyCrafting = CraftingDictionairy.CheckCrafting(list.ToArray());
        if (CurrentlyCrafting != null)
        {
            Sprite icon = CurrentlyCrafting.icon;
            craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().sprite = icon;
            craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().color = new Color(1, 1, 1, 1);
            
        }
        else
        {
            craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().sprite = null;
            craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }
    public void Craft()
    {
        for (int k = 0; k < selected.Count; k++)
        {
            selected[k].GetComponent<InventorySlot>().Holding = null;
        }
        selected[0].GetComponent<InventorySlot>().Holding = CurrentlyCrafting;
        selected.Clear();

        craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().sprite = null;
        craftingSlot.transform.Find("Slot").Find("Item").GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    public List<GameObject> getSelected()
    {
        return selected;
    }
    public bool isSelected(GameObject GO)
    {
        return selected.Contains(GO);
    }
}
