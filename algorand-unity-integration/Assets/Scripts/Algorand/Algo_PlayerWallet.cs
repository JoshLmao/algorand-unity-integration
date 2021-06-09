using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Algorand
using Algorand.V2;
using AlgodApi = Algorand.V2.AlgodApi;
using Account = Algorand.Account;
using Algorand.Algod.Api;
using System;

public class Algo_PlayerWallet : MonoBehaviour
{
    // Player's account
    private Algorand.Account m_playerAlgoAccount = null;
    // AlgodAPI class
    private AlgodApi m_algodAPI = null;

    private void Awake()
    {
        m_algodAPI = GameObject.FindObjectOfType<AlgorandSceneHelper>().GetAlgodAPI();
        // Load user's wallet
        LoadWallet();
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
    
    // Prints relevant wallet info of the given account
    private void PrintPlayerWalletInfo(Account algoAccount)
    {
        Debug.Log($"Wallet Address = '{m_playerAlgoAccount.Address}'");
        Debug.Log($"Mnemonic = '{m_playerAlgoAccount.ToMnemonic()}");

        if (m_algodAPI != null) 
        {
            ulong microAlgoBalance = GetMicroAlgoBalance(algoAccount.Address.ToString());
            Debug.Log($"Balance: {MicroAlgoToAlgo(microAlgoBalance)} ALGO - {microAlgoBalance} microAlgos");
        }
    }

    // Converts micro algo to algo amount with decimals
    private ulong MicroAlgoToAlgo(ulong microAlgo)
    {
        ulong pow = (ulong)Math.Pow(10, 6);
        return microAlgo / pow;
    }

    private void LoadWallet()
    {
        string algoMnemonic = PlayerPrefs.GetString("algo-mnemonic");
        if (!string.IsNullOrEmpty(algoMnemonic))
        {
            Debug.Log("Using existing mnemonic to load user wallet");

            m_playerAlgoAccount = new Account(algoMnemonic);
            PrintPlayerWalletInfo(m_playerAlgoAccount);
        }
        else
        {
            Debug.Log("Creating new Algo user account");

            m_playerAlgoAccount = new Account();
            PrintPlayerWalletInfo(m_playerAlgoAccount);

            PlayerPrefs.SetString("algo-mnemonic", m_playerAlgoAccount.ToMnemonic());
        }
    }

    // Gets the balance of the wallet in ALGO.
    public ulong GetAlgoBalance(string walletAddress = "")
    {
        return MicroAlgoToAlgo(GetMicroAlgoBalance(walletAddress));
    }

    // Gets the balance of the wallet in micro algo (10^6)
    public ulong GetMicroAlgoBalance(string walletAddress = "")
    {
        //Get address if not given
        string addr = walletAddress;
        if (String.IsNullOrEmpty(addr) && m_playerAlgoAccount != null)
        {
            addr = m_playerAlgoAccount.Address.ToString();
        }

        // Call API and get micro algo amount
        if (m_algodAPI != null)
        {
            var accountInfo = m_algodAPI.AccountInformation(addr);
            return (ulong?)accountInfo.Amount ?? 0;
        }
        return 0;
    }

    public Algorand.Account GetPlayerAccount()
    {
        return m_playerAlgoAccount;
    }
}
