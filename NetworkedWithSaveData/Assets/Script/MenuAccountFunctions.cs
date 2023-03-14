using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using TMPro;

public class MenuAccountFunctions : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField L_Username;
    public TMP_InputField L_Password;
    public TextMeshProUGUI L_NotFound;

    [Header("New Account")]
    public TMP_InputField NA_Username;
    public TMP_InputField NA_Password;

    #region Log In Post
    IEnumerator SearchLogin(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/login", json))
        {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            // handle response
            var newData = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            var getRequestData = JsonUtility.FromJson<SaveList>(newData);

            if (getRequestData.accountsaves.Length == 0)
            {
                L_NotFound.text = "Login Not Found";
            }
            else if(getRequestData.accountsaves[0].password != L_Password.text)
            {
                L_NotFound.text = "Login Incorrect";
            }
            else
            {
                //Now we have reference to account data from database
                AccountManager.Instance.Account = getRequestData.accountsaves[0];
            }

            //error check
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("DataObj Posted");
            }
            request.uploadHandler.Dispose();
        }
    }

    public void Login()
    {
        DataClass account = new DataClass();
        account.username = L_Username.text;
        account.password = L_Password.text;

        string json = JsonUtility.ToJson(account);
        StartCoroutine(SearchLogin(json));
    }
    #endregion

    #region New Account
    IEnumerator AddAccount(string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/newAccount", json))
        {
            request.SetRequestHeader("content-type", "application/json");
            request.uploadHandler.contentType = "application/json";
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

            yield return request.SendWebRequest();

            var newData = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
            newData = newData.Remove(0, 1);
            newData = newData.Remove(newData.Length-1, 1);

            AccountManager.Instance.Account._id = newData;            

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("DataObj Posted");
            }
            request.uploadHandler.Dispose();
        }
    }
    public void NewAccount()
    {
        DataClass account = new DataClass();
        account.username = NA_Username.text;
        AccountManager.Instance.Account.username = NA_Username.text;

        account.password = NA_Password.text;
        AccountManager.Instance.Account.password = NA_Password.text;

        string json = JsonUtility.ToJson(account);
        StartCoroutine(AddAccount(json));
    }
    #endregion
}
