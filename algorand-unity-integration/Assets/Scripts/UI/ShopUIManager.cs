using Algorand;
using Algorand.V2;
using Algorand.V2.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ItemParent;
    [SerializeField]
    private GameObject SingleItemUIPrefab;

    [SerializeField]
    private ShopManager ShopManager;

    private AlgorandSceneHelper m_algoSceneHelper;

    void Start()
    {
        // Get Algo API first
        m_algoSceneHelper = GameObject.FindObjectOfType<AlgorandSceneHelper>();

        List<ShopItem> allItems = ShopManager.GetShopItems();
        if (ItemParent != null && allItems.Count > 0)
        {
            // Destroy any children if exist
            if (ItemParent.transform.childCount > 0)
            {
                foreach (Transform child in ItemParent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            float totalHeight = 0;
            int index = 0;
            foreach(ShopItem item in allItems)
            {
                GameObject instGameObject = Instantiate(SingleItemUIPrefab, ItemParent.transform);
                // Add on height to set correct Y position
                RectTransform rect = instGameObject.GetComponent<RectTransform>();
                float height = rect.sizeDelta.y;
                rect.localPosition += (Vector3.down * (height * index));

                // Set name, cost and listener to buy
                var nameTextGO = instGameObject.transform.Find("NameText");
                nameTextGO.GetComponent<Text>().text = item.Name;

                var costTextGO = instGameObject.transform.Find("CostText");
                costTextGO.GetComponent<Text>().text = Utils.MicroalgosToAlgos((ulong)item.MicroAlgoCost).ToString() + " ALGO";
                
                var buyBtnGO = instGameObject.transform.Find("BuyBtn");
                buyBtnGO.GetComponent<Button>().onClick.AddListener(() => OnBuyItem(item));

                index++;
                totalHeight += height;
            }

            // Set total height of scroll rect
            RectTransform parentRect = ItemParent.GetComponent<RectTransform>();
            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, totalHeight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBuyItem(ShopItem item)
    {
        GameObject player = GameObject.Find("Player");
        Algo_PlayerWallet playerWallet = player.GetComponent<Algo_PlayerWallet>();
        if (playerWallet)
        {
            // Check if player has enough balance for item
            ulong playerMicroAlgoBalance = playerWallet.GetMicroAlgoBalance();
            if (item.MicroAlgoCost > playerMicroAlgoBalance)
            {
                Debug.LogError($"Can't buy '{item.Name}', not enough balance in player's wallet '{playerMicroAlgoBalance}'");
                return;
            }

            var playerAccount = playerWallet.GetPlayerAccount();
            ulong microAlgoAmount = (ulong)item.MicroAlgoCost;
            string txId = m_algoSceneHelper.MakePayment(playerAccount, new Address(AppConstants.APP_WALLET_ADDRESS), microAlgoAmount, $"UNITY GAME TEST -> Buy item '{item.Name}'", () =>
            {
                Debug.Log("Payment complete!");
            });

            Debug.Log($"Player paid for '{item.Name}' item for '{Utils.MicroalgosToAlgos(item.MicroAlgoCost)}' ALGO (TxId: {txId})");
        }
    }
}
