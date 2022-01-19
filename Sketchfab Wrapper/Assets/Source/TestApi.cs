using System;
using System.Collections.Generic;
using UnityEngine;


public class TestApi : MonoBehaviour
{

    public static event Action OnLogin;
    public static event Action OnLogout;
    public static event Action<SketchfabUserInfo> OnUserInformation;

    public static void Login(string _email, string _password)
    {
        SketchfabAPI.GetAccessToken(_email, _password, (SketchfabResponse<SketchfabAccessToken> answer) =>
        {
            if(answer.Success)
            {
                string token = answer.Object.AccessToken;
                SketchfabAPI.AuthorizeWithAccessToken(token);
                
                // Notify other components that we just login
                OnLogin?.Invoke();

                GetAccountInformation();
            }
            else
            {
                Debug.LogError(answer.ErrorMessage);
            }
        });
    }

    public static void Logout()
    {
        SketchfabAPI.Logout();
        OnLogout?.Invoke();
    }

    public static void GetAccountInformation()
    {

        SketchfabAPI.GetUserInformation((SketchfabResponse<SketchfabUserInfo> answer) => {
            if (answer.Success)
            {
                SketchfabUserInfo userInformation = answer.Object;
                OnUserInformation?.Invoke(userInformation);
            }
            else
            {
                Debug.Log(answer.ErrorType);
                Debug.Log(answer.ErrorMessage);
            }
        });
    }

    public static void GetModelList()
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        SketchfabAPI.GetModelList(p, ((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            SketchfabResponse<SketchfabModelList> ans = _answer;
            List<SketchfabModel> modelList = ans.Object.Models;
        }));
    }

    public static void SearchModel(string _query)
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        SketchfabAPI.ModelSearch(((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            SketchfabResponse<SketchfabModelList> ans = _answer;
            List<SketchfabModel> m_ModelList = ans.Object.Models;
        }), p, _query);
    }


    private static void DownloadModel(string _guid)
    {
        bool enableCache = true;
        SketchfabAPI.GetModel(_guid, (resp) =>
        {
            SketchfabModelImporter.Import(resp.Object, (obj) =>
            {
                if (obj != null)
                {
                    CameraController.Instance.PlaceObjectInFront(obj);
                }
            }, enableCache);
        }, enableCache);
    }
}
