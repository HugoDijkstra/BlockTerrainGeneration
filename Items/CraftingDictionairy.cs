using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDictionairy
{

    SortedList<string, List<Item>> items;
    private static CraftingDictionairy instance;
    public static CraftingDictionairy Instance
    {
        get { if (instance == null) instance = new CraftingDictionairy(); return instance; }
    }
    private CraftingDictionairy()
    {
        items = new SortedList<string, List<Item>>();
        items.Add("potatoStick", new List<Item> { ItemDictionairy.getItem("potato"), ItemDictionairy.getItem("stick") });
    }
    public static Item CheckCrafting(Item[] it)
    {
        for (int j = 0; j < Instance.items.Count; j++)
        {
            bool containsAll = true;
            Debug.Log(Instance.items.Values[j].Count + " + " + it.Length);
            if (Instance.items.Values[j].Count == it.Length)
            {
                List<Item> l = new List<Item>(Instance.items.Values[j]);
                for (int i = 0; i < it.Length; i++)
                {
                    Debug.Log(Instance.items.Values[j].Contains(it[i]));
                    if (l.Contains(it[i]))
                    {
                        l.Remove(it[i]);
                    }
                    else
                    {
                        containsAll = false;
                    }
                }
                if (containsAll)
                {
                    Debug.Log("Found");
                    return ItemDictionairy.getItem(instance.items.Keys[j]);
                }
            }
        }
        return null;
    }

}
