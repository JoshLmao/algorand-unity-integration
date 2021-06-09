using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem
{
    public string Name { get; set; }
    public ulong MicroAlgoCost { get; set; }

    public ShopItem(string name, ulong microAlgoCost)
    {
        Name = name;
        MicroAlgoCost = microAlgoCost;
    }
}
