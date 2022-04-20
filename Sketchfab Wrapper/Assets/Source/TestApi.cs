using System;
using System.Collections.Generic;
using UnityEngine;


public class TestApi : MonoBehaviour
{

    public static TestApi Instance;
    public static event Action OnLogin;
    public static event Action OnLogout;
    public static event Action<SketchfabUserInfo> OnUserInformation;
    public static event Action<List<SketchfabModel>> OnModelList;

    public int MaxVertexCount = 10000;
    public int MaxFaceCount = 10000;

    public int MaxTextureResolution = 1024;


    private void Awake()
    {
        Instance = this;
    }

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
                GetDefaultModelList();
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

    private static UnityWebRequestSketchfabModelList.Parameters GetParameters()
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        p.archives_texture_max_resolution = Instance.MaxTextureResolution;
        p.maxVertexCount = Instance.MaxVertexCount;
        p.maxFaceCount = Instance.MaxFaceCount;
        return p;
    }

    public static void GetDefaultModelList()
    {

        SketchfabAPI.GetModelList(GetParameters(), ((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            SketchfabResponse<SketchfabModelList> ans = _answer;
            List<SketchfabModel> modelList = ans.Object.Models;
            OnModelList?.Invoke(modelList);
        }));
    }

    public static void SearchModel(string _query)
    {
        SketchfabAPI.ModelSearch(((SketchfabResponse<SketchfabModelList> _answer) =>
        {
            SketchfabResponse<SketchfabModelList> ans = _answer;
            List<SketchfabModel> modelList = ans.Object.Models;
            OnModelList?.Invoke(modelList);
        }), GetParameters(), _query);
    }


    public static void DownloadModel(string _guid)
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

    public static void GetAssetThumbnail(string _guid, Action<Sprite> _onThumbnailDownloaded)
    {
        SketchfabAPI.GetModel(_guid, async (SketchfabResponse<SketchfabModel> _sketchfabModel) =>
        {
            if (_sketchfabModel.Success)
            {
                Sprite sprite = await SketchfabThumbnailImporter.ImportAsync(_sketchfabModel.Object.Thumbnails.ClosestThumbnailToSizeWithoutGoingBelow(256, 256));

                _onThumbnailDownloaded?.Invoke(sprite);
            }
            else
            {
                Debug.LogError(_sketchfabModel.ErrorMessage);
            }
        }, true); // Enable the cache system as we don't need to download the asset info again
    }
}
