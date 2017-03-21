using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : EventTrigger
{
    Inventory inventory;
    Item holding;
    public Item Holding
    {
        get { return this.holding; }
        set { this.holding = value; }
    }

    GameObject itemIcon;
    static Sprite selectSprite;
    static Sprite normalSprite;
    // Use this for initialization
    void Start()
    {
        itemIcon = gameObject.transform.Find("Item").gameObject;
        if (Random.Range(0.0f, 1.0f) < 0.5)
            holding = ItemDictionairy.getItem("potato");
        else
            holding = ItemDictionairy.getItem("stick");

        inventory = GameObject.Find("Player").GetComponent<Inventory>();
        if (selectSprite == null)
        {
            selectSprite = Resources.Load<Sprite>("Art/ui/Selected");
        }
        if (normalSprite == null)
        {
            normalSprite = Resources.Load<Sprite>("Art/ui/Slot");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(holding != null)
        {
            itemIcon.GetComponent<Image>().sprite = holding.icon;
            itemIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
        else
        {
            itemIcon.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
        if (IsSelected())
        {
            GetComponent<Image>().sprite = selectSprite;
        }
        else
        {
            GetComponent<Image>().sprite = normalSprite;
        }
    }

    public bool GetIsHolding()
    {
        return holding != null;
    }

    public bool IsSelected()
    {
        return inventory.isSelected(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        GetComponent<Image>().color += new Color(0, 0, 0, 0.2f);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        GetComponent<Image>().color -= new Color(0, 0, 0, 0.2f);
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        GameObject.Find("Player").GetComponent<Inventory>().Select(gameObject);
    }


}
