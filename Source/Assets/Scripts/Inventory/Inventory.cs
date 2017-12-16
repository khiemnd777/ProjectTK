using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    static Inventory inventory;

    public static Inventory instance
    {
        get
        {
            if (!inventory)
            {
                inventory = FindObjectOfType<Inventory>();
                if (!inventory)
                {
                    Debug.LogError("There needs to be one active Inventory script on a GameObject in your scene.");
                }
                else
                {
                    inventory.items = new List<Item>();
                }
            }
            return inventory;
        }
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;
    public List<Item> items;

    public bool Add(Item item)
    {
        if (item.isDefaultItem)
            return false;
        if (items.Count >= space)
        {
            Debug.Log("Not enough space.");
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
