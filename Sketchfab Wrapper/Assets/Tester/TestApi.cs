using System.Collections.Generic;
using UnityEngine;

public class TestApi : MonoBehaviour
{
    private string m_Email = "your@email.com";
    private string m_Password = "pass";
    private string m_AccessToken = string.Empty;

    private string m_UserInformation = string.Empty;
    private List<SketchfabModel> m_ModelList = new List<SketchfabModel>();
    private CameraController m_CameraController;

    private void Start()
    {
        m_CameraController = GameObject.FindObjectOfType<CameraController>();        
    }

    public void Login()
    {
        SketchfabAPI.GetAccessToken(m_Email, m_Password, (SketchfabResponse<SketchfabAccessToken> answer) =>
        {
            if(answer.Success)
            {
                m_AccessToken = answer.Object.AccessToken;
                SketchfabAPI.AuthorizeWithAccessToken(answer.Object);

                GetUserInformation();
                GetModelList();
            }
            else
            {
                Debug.LogError(answer.ErrorMessage);
            }

        });
    }

    public void GetUserInformation()
    {

        SketchfabAPI.GetUserInformation((SketchfabResponse<SketchfabUserInfo> answer) => {
            if (answer.Success)
            {
                m_UserInformation = answer.Object.ToString();
            }
            else
            {
                Debug.Log(answer.ErrorType);
                Debug.Log(answer.ErrorMessage);
            }
        });
    }

    public void GetModelList()
    {
        UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        p.downloadable = true;
        SketchfabAPI.GetModelList(p, ((SketchfabResponse<SketchfabModelList> _answer) =>
         {
             SketchfabResponse<SketchfabModelList> ans = _answer;
             m_ModelList = ans.Object.Models;
         }));

        // Search code
        //UnityWebRequestSketchfabModelList.Parameters p = new UnityWebRequestSketchfabModelList.Parameters();
        //p.downloadable = true;
        //string searchKeyword = "Cat";
        //SketchfabAPI.ModelSearch(((SketchfabResponse<SketchfabModelList> _answer) =>
        //{
        //    SketchfabResponse<SketchfabModelList> ans = _answer;
        //    m_ModelList = ans.Object.Models;
        //}), p, searchKeyword);
    }


    private void DownloadModel(string _guid)
    {
        bool enableCache = true;
        SketchfabAPI.GetModel(_guid, (resp) =>
        {
            SketchfabModelImporter.Import(resp.Object, (obj) =>
            {
                if (obj != null)
                {
                    m_CameraController.PlaceObjectInFront(obj);
                }
            }, enableCache);
        }, enableCache);
    }

    private void OnGUI()
    {

        if (!SketchfabAPI.Authorized)
        {
            GUI.color = Color.white;
            m_Email = GUI.TextField(new Rect(10, 10, 200, 20), m_Email);
            m_Password = GUI.PasswordField(new Rect(10, 30, 200, 20), m_Password, '*');

            if (GUI.Button(new Rect(10, 50, 150, 20), "Login"))
            {
                Login();
            }
        }
        else
        {
            GUI.color = Color.black;
            RenderUserInfo();
            if (GUI.Button(new Rect(10, 130, 250, 20), "Logout"))
            {
                SketchfabAPI.Logout();
            }
            RenderModelList();
        }

    }

    private void RenderUserInfo()
    {
        if(this.m_UserInformation.Length == 0)
        {
            return;
        }
        GUI.Label(new Rect(10, 10, 150, 20), "User Information:");
        GUI.Label(new Rect(10, 30, 250, 100), m_UserInformation);
    }

    private void RenderModelList()
    {
        if (m_ModelList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < m_ModelList.Count; ++i)
        {
            SketchfabModel m = m_ModelList[i];
            if (m.IsDownloadable)
            {
                RenderModelListElement(m_ModelList[i], i);
            }
        }
    }

    private void RenderModelListElement(SketchfabModel _model, int y)
    {
        GUI.color = Color.black;
        GUI.Label(new Rect(Screen.width - 250, y * 20 + 20, 120, 30), _model.Name);
        GUI.color = Color.white;
        if (GUI.Button(new Rect(Screen.width - 250 + 120, y * 30 + 20, 100, 20), "Download"))
        {
            DownloadModel(_model.Uid);
        }
    }

}
