using UnityEngine;

public class UIModelLibrary : MonoBehaviour
{
    private CanvasGroup m_Group;

    private UIModel m_UiModelPrefab;;

    private void Awake()
    {
        m_Group = GetComponent<CanvasGroup>();

        // Get the list element 
        m_UiModelPrefab = GetComponentInChildren<UIModel>();

        // Hide the ui element
        OnLogout();
    }

    private void OnEnable()
    {
        TestApi.OnLogin += OnLogin;
        TestApi.OnLogout += OnLogout;
    }

    private void OnDisable()
    {
        TestApi.OnLogin -= OnLogin;
        TestApi.OnLogout -= OnLogout;
    }

    private void OnLogin()
    {
        m_Group.alpha = 1;
        m_Group.blocksRaycasts = true;
    }

    private void OnLogout()
    {
        m_Group.alpha = 0;
        m_Group.blocksRaycasts = false;
    }

}
