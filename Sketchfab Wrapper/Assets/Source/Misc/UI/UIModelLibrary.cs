using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModelLibrary : MonoBehaviour
{

    public InputField InSeach;
    public Button BtnSearch;

    private CanvasGroup m_Group;

    private UIModel m_UiModelPrefab;

    private UIModelFactory m_Factory;
    private List<UIModel> m_ModelList = new List<UIModel>();

    private void Awake()
    {
        m_Group = GetComponent<CanvasGroup>();

        // Get the list element 
        m_UiModelPrefab = GetComponentInChildren<UIModel>();

        // Initialize the factory with the prefab
        m_Factory = new UIModelFactory(m_UiModelPrefab);

        Setup();

        BtnSearch.onClick.AddListener(PerformSearch);
    }

    private void Setup()
    {
        // Hide the prefab
        m_UiModelPrefab.gameObject.SetActive(false);

        // Hide the widget at start
        TestApi_OnLogout();
    }

    private void OnEnable()
    {
        TestApi.OnLogin += TestApi_OnLogin;
        TestApi.OnLogout += TestApi_OnLogout;
        TestApi.OnModelList += TestApi_OnModelList;
    }

    private void OnDisable()
    {
        TestApi.OnLogin -= TestApi_OnLogin;
        TestApi.OnLogout -= TestApi_OnLogout;
        TestApi.OnModelList -= TestApi_OnModelList;
    }

    private void TestApi_OnLogin()
    {
        m_Group.alpha = 1;
        m_Group.blocksRaycasts = true;
    }

    private void TestApi_OnLogout()
    {
        m_Group.alpha = 0;
        m_Group.blocksRaycasts = false;
    }

    private void TestApi_OnModelList(List<SketchfabModel> _models)
    {
        // Remove all the current assets on the list
        ClearList();

        // Show the new list
        foreach(SketchfabModel _mod in _models)
        {
            UIModel uiMod = m_Factory.Get();
            uiMod.Init(_mod);
            m_ModelList.Add(uiMod);
        }
    }

    private void ClearList()
    {
        foreach(UIModel uim in m_ModelList)
        {
            m_Factory.Refund(uim);
        }
        m_ModelList.Clear();
    }

    private void PerformSearch()
    {
        TestApi.SearchModel(InSeach.text);
    }

}
