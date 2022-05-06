// GetWalletAddress.cs

using System.Collections;
//using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// use web3.jslib
using System.Runtime.InteropServices;

public class GetWalletAddress : MonoBehaviour
{
    // text in the button
    public GameObject ButtonText;
    public GameObject ProjectAddressTextField;
    public GameObject StakeTextField;
    // use WalletAddress function from web3.jslib
    [DllImport("__Internal")] private static extern string WalletAddress();
    [DllImport("__Internal")] private static extern string getStakedCountAndAmount(byte[] array);
    [DllImport("__Internal")] private static extern string TestCopyToBuffer(byte[] array);
    [DllImport("__Internal")] private static extern string stake(byte[] projectAddress, byte[] _stakeAmount);


    private void Start()
    {
    }

    public void OnClick()
    {
        // get wallet address and display it on the button
        ButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = WalletAddress();
    }

    public void GetStake()
    {
        // StartCoroutine(RequestStakedAmount());
        // get wallet address and display it on the button
        for (int i = 0; i < 1024; i++)
        {
            GlobalVariables.SkatedAmount[i] = 0;
        }
        getStakedCountAndAmount(GlobalVariables.SkatedAmount);
    }
    public void GetStakeResult()
    {
        Debug.Log("GetStakeResult");
        string result = System.Text.Encoding.UTF8.GetString(GlobalVariables.SkatedAmount);
        Debug.Log(result);
        ButtonText.GetComponent<TMPro.TextMeshProUGUI>().text = result;
    }

    public void StakePushed()
    {
        byte[] projectAddress = System.Text.Encoding.UTF8.GetBytes(ProjectAddressTextField.GetComponent<InputField>().text);
        byte[] stakeCount = System.Text.Encoding.UTF8.GetBytes(StakeTextField.GetComponent<InputField>().text);

        stake(projectAddress, stakeCount);
    }

}