using Algorand;
using Algorand.V2;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAlgoInfoUI : MonoBehaviour
{
    [SerializeField]
    Text m_balanceText;

    [SerializeField]
    Text m_walletText;

    [SerializeField]
    Button m_copyWalletBtn;

    private AlgodApi m_algodAPI;
    private AlgorandSceneHelper m_algoSceneHelper;
    private Algo_PlayerWallet m_player;

    private string m_uiMessage;
    private string m_playerWalletAddress;

    // Start is called before the first frame update
    void Start()
    {
        m_algoSceneHelper = GameObject.FindObjectOfType<AlgorandSceneHelper>();
        m_algodAPI = m_algoSceneHelper.GetAlgodAPI();

        m_player = GameObject.FindObjectOfType<Algo_PlayerWallet>();

        InvokeRepeating(nameof(UpdateBalance), 0.0f, 2.0f);

        if (m_player)
        {
            if (m_walletText)
            {
                m_playerWalletAddress = m_player.GetPlayerAccount()?.Address.ToString();
                m_walletText.text = $"Player Wallet: {m_playerWalletAddress}";
            }

            if (m_copyWalletBtn)
            {
                m_copyWalletBtn.onClick.AddListener(() =>
                {
                    TextEditor te = new TextEditor();
                    te.text = m_playerWalletAddress;
                    te.SelectAll();
                    te.Copy();

                    Debug.Log("Copied player wallet to clipboard");
                });
            }
        }
    }

    private void Update()
    {
        // Update text on main thread
        m_balanceText.text = m_uiMessage;
    }

    void UpdateBalance()
    {
        // Async get latest balance and update variable
        Task.Run( () =>
            {
                if (m_player)
                {
                    ulong microAlgoBalance = m_player.GetMicroAlgoBalance();
                    double algoBalance = Utils.MicroalgosToAlgos(microAlgoBalance);
                    string msg = $"ALGO: {algoBalance} microALGO: {microAlgoBalance}";
                    m_uiMessage = msg;
                }
                else
                {
                    m_uiMessage = "Unable to update.";
                }
            }
        );
    }
}
