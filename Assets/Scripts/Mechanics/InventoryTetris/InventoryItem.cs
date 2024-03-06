using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : IInventory
{
    
    public void Add(IInventory item)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<IInventory> Get()
    {
        throw new System.NotImplementedException();
    }
}
