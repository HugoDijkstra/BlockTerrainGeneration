using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionairy
{

    Dictionary<string, Item> items;
    private static ItemDictionairy instance;
    public static ItemDictionairy Instance
    {
        get { if (instance == null) instance = new ItemDictionairy(); return instance; }
    }
    private ItemDictionairy()
    {
        items = new Dictionary<string, Item>();
        items.Add("potato", new Item(0, "food.potato", "Art/items/Potato", 0, Item.Type.Food));
        items.Add("stick", new Item(0, "Item.stick", "Art/items/stick", 0, Item.Type.Default));
        items.Add("potatoStick", new Item(0, "weapon.potatoStick", "Art/items/PotatoStick", 0, Item.Type.Weapon));
    }
    public static Item getItem(string name)
    {
        return Instance.items[name];
    }
}
