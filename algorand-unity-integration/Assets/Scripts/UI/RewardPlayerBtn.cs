using Algorand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPlayerBtn : MonoBehaviour
{
    [SerializeField]
    private Button m_rewardBtn;

    private AlgorandSceneHelper m_algorandSceneHelper;

    // Start is called before the first frame update
    void Start()
    {
        m_algorandSceneHelper = GameObject.FindObjectOfType<AlgorandSceneHelper>();

        if (m_rewardBtn)
        {
            m_rewardBtn.onClick.AddListener(() => OnRewardPlayer());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRewardPlayer()
    {
        Account gameAccount = new Account(AppConfig.APP_WALLET_MNEMONIC);
        Account playerAccount = GameObject.FindObjectOfType<Algo_PlayerWallet>().GetPlayerAccount();

        // Reward player with 0.05 ALGO
        double ALGOrewardAmount = 0.05;
        ulong microAlgoRewardAmount = Utils.AlgosToMicroalgos(ALGOrewardAmount);
        string txId = m_algorandSceneHelper.MakePayment(gameAccount, playerAccount.Address, microAlgoRewardAmount, "UNITY GAME TEST -> Reward", () =>
        {
            Debug.Log($"Reward transaction complete");
        });
        Debug.Log($"Rewarded player '{ALGOrewardAmount}' ALGO (TxId: {txId})");
    }
}
