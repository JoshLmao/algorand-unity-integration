using Algorand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private List<ShopItem> Items = new List<ShopItem>();

    public ShopManager()
    {
        Items.Add(new ShopItem("Ice Cream", Utils.AlgosToMicroalgos(0.2)));
        Items.Add(new ShopItem("Radiance", Utils.AlgosToMicroalgos(5150)));
        Items.Add(new ShopItem("Bracer", Utils.AlgosToMicroalgos(5)));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<ShopItem> GetShopItems()
    {
        return Items;
    }
}
