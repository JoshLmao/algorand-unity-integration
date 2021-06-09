using Algorand;
using Algorand.V2;
using Address = Algorand.Address;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AlgorandSceneHelper : Singleton<AlgorandSceneHelper>
{
    private AlgodApi m_algodAPI = null;

    public AlgorandSceneHelper()
    {
        m_algodAPI = new AlgodApi(GetAPIAddress(), AppConfig.PURESTAKE_API_KEY);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Gets API address of PureStake
    /// </summary>
    /// <returns></returns>
    private string GetAPIAddress()
    {
        string testnetAddress = "https://testnet-algorand.api.purestake.io/ps2";
        string mainnetAddress = "https://mainnet-algorand.api.purestake.io/ps2";

        // Set to testnet for dev
        string ALGOD_API_ADDR = testnetAddress;
        if (ALGOD_API_ADDR.IndexOf("//") == -1)
        {
            ALGOD_API_ADDR = "http://" + ALGOD_API_ADDR;
        }
        return ALGOD_API_ADDR;
    }

    /// <summary>
    /// Gets the created AlgodAPI class reference
    /// </summary>
    /// <returns></returns>
    public AlgodApi GetAlgodAPI()
    {
        return m_algodAPI;
    }

    /// <summary>
    ///  Makes a payment of the microAlgoAmount, from account -> to address. Callback once transaction is success
    /// </summary>
    /// <param name="fromAccount">Account from payment comes from</param>
    /// <param name="toAddress">Address that payment is going to</param>
    /// <param name="microAlgoAmount">Amount of microALGO to pay</param>
    /// <param name="txMessage">Message to include in transaction</param>
    /// <param name="onTransactionSuccess">Multithread callback once transaction is successful</param>
    /// <returns>Transaction ID of the payment from -> to</returns>
    public string MakePayment(Account fromAccount, Address toAddress, ulong microAlgoAmount, string txMessage, System.Action onTransactionSuccess = null)
    {
        // Check from/to is not null
        if (fromAccount == null || toAddress == null)
        {
            Debug.LogError("From/To account/address is invalid");
            return null;
        }
        // Check address is valid
        if (!Address.IsValid(toAddress.ToString()))
        {
            Debug.LogError("Invalid recieve address. Can't send transaction");
            return null;
        }
        // Check amount to send is more than 0
        if (microAlgoAmount <= 0)
        {
            Debug.LogError("microAlgo Amount is less than 0.");
            return null;
        }

        // Build transaction params, make transaction and sign it
        Algorand.V2.Model.TransactionParametersResponse transactionParameters = m_algodAPI.TransactionParams();
        Transaction transaction = Utils.GetPaymentTransaction(fromAccount.Address, toAddress, microAlgoAmount, txMessage, transactionParameters);
        SignedTransaction signedTx = fromAccount.SignTransaction(transaction);

        try
        {
            var id = Utils.SubmitTransaction(m_algodAPI, signedTx);
            Debug.Log($"Submitted transaction '{id.TxId}'");

            // Launch thread to wait for complete transaction, callback once done
            Task.Run(() =>
            {
                var resp = Utils.WaitTransactionToComplete(m_algodAPI, id.TxId);
                if (resp != null && resp.ConfirmedRound != null)
                {
                    onTransactionSuccess?.Invoke();
                }
            });
        }
        catch (Algorand.Client.ApiException e)
        {
            // This is generally expected, but should give us an informative error message.
            Debug.LogError("Exception when calling algod#rawTransaction: " + e.Message);
            return null;
        }

        return signedTx.transactionID;
    }
}
