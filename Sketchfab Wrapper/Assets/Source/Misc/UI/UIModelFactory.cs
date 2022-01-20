using System.Collections.Generic;
using UnityEngine;

public class UIModelFactory
{
    private List<UIModel> m_Stock = new List<UIModel>(50);
    private UIModel m_Prefab;

    public UIModelFactory(UIModel _prefab)
    {
        m_Prefab = _prefab;
    }

    public UIModel Get()
    {
        UIModel toReturn = null;
        if(m_Stock.Count == 0)
        {
            toReturn = GameObject.Instantiate(m_Prefab);
        }
        else
        {
            toReturn = m_Stock[m_Stock.Count - 1];
            m_Stock.RemoveAt(m_Stock.Count - 1);
        }
        toReturn.transform.SetParent(m_Prefab.transform.parent);
        toReturn.gameObject.SetActive(true);
        return toReturn;
    }

    public void Refund(UIModel _model)
    {
        _model.gameObject.SetActive(false);
        _model.gameObject.transform.SetParent(null);
        m_Stock.Add(_model);
    }
}

