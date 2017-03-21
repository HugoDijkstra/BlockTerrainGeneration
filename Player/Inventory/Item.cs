using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Item()
    {
        this.id = 0;
        this.name = "item.name";
        this.icon = null;
        this.damage = 0;
        this.type = Type.Default;
    }
    public Item(int id, string name, Sprite icon, float damage, Type type)
    {
        this.id = id;
        this.name = name;
        this.icon = icon;
        this.damage = damage;
        this.type = type;
    }
    public Item(int id, string name, string iconPath, float damage, Type type)
    {
        this.id = id;
        this.name = name;
        this.icon = Resources.Load<Sprite>(iconPath);
        this.damage = damage;
        this.type = type;
    }
    public int id;
    public string name;
    public Sprite icon;
    public float damage;
    public Type type;
    public enum Type
    {
        Default,
        Food,
        Weapon,
        Block
    };
}
