// GetWalletAddress.cs

using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
// use web3.jslib
using System.Runtime.InteropServices;

public class GetWalletAddress : MonoBehaviour
{
    // text in the button
    public GameObject ButtonText;
    public GameObject ProjectAddressTextField;
    public GameObject StakeTextField;
    public GameObject ProjectSelector;
    public GameObject StakeSelector;
    public string[] projectAddressList;
    public float[] stakedByProject;
    public float[] apyByProject;
    public float stakeUnit = 0.0001F;

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
        Debug.Log(ProjectSelector.GetComponent<TMPro.TMP_Dropdown>().value);
        string target = projectAddressList[ProjectSelector.GetComponent<TMPro.TMP_Dropdown>().value];
        int stakeNum = StakeSelector.GetComponent<TMPro.TMP_Dropdown>().value;
        stakeNum += 1;
        float stakeFloat = (float)stakeNum;
        stakeFloat *= stakeUnit;
        byte[] projectAddress = System.Text.Encoding.UTF8.GetBytes(target);
        byte[] stakeCount = System.Text.Encoding.UTF8.GetBytes(stakeFloat.ToString());

        stake(projectAddress, stakeCount);
    }

    public void GetProjectStatus()
    {
        // e.g.curl "https://us-central1-metaverstake.cloudfunctions.net/projects?address=0x854fb5E2E490f22c7e0b8eA0aD4cc8758EA34Bc9&address=0x92561F28Ec438Ee9831D00D1D59fbDC981b762b2"
        // -> [{ "totalStaked":0.0011,"apy":120},{ "totalStaked":0.1,"apy":120}]


        StartCoroutine(CallProjectMethod());
    }

    private IEnumerator CallProjectMethod()
    {

        string requestString = "https://us-central1-metaverstake.cloudfunctions.net/projects?";
        for ( int i = 0; i < projectAddressList.Length; i ++)
        {
            requestString += "address=";
            requestString += projectAddressList[i];
            if ( i < projectAddressList.Length - 1)
            {
                requestString += "&";
            }
        }

        // Debug.Log(requestString);

        //1.UnityWebRequestを生成
        UnityWebRequest request = UnityWebRequest.Get(requestString);

        //2.SendWebRequestを実行し、送受信開始
        yield return request.SendWebRequest();

        //3.isNetworkErrorとisHttpErrorでエラー判定
        if ( request.result == UnityWebRequest.Result.ProtocolError )
        {
            //4.エラー確認
            Debug.Log(request.error);
        }
        else
        {
            //4.結果確認
            Debug.Log(request.downloadHandler.text);
            ProjectStatusJson[] test = JsonConvert.DeserializeObject<ProjectStatusJson[]>(request.downloadHandler.text);

            Debug.Log(test.Length);
            for ( int i = 0; i < test.Length; i ++)
            {
                Debug.Log(test[i].totalStaked);
                Debug.Log(test[i].apy);
            }
        }

    }
}