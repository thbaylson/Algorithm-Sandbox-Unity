using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : IInventory
{
    private List<IInventory> items = new();

    public void Add(IInventory item)
    {
        items.Add(item);
    }

    public IEnumerable<IInventory> Get()
    {
        return items;
    }
}
