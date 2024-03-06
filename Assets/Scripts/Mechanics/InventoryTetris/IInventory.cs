using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory
{
    public void Add(IInventory item);
    public IEnumerable<IInventory> Get();
}
