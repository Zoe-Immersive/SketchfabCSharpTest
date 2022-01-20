using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIModel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image ImgTumbnail;
    public Text TxtName;
    public Text TxtUID;
    public Text TxtVertextCount;
    public Image Background;

    public Color ColorHover;
    public Color ColorDefault;

    private SketchfabModel m_Model;

    public void Init(SketchfabModel _model)
    {
        m_Model = _model;
        TxtName.text = _model.Name;
        TxtUID.text = _model.Uid;
        TxtVertextCount.text = _model.VertexCount.ToString();

        TestApi.GetAssetThumbnail(_model.Uid, SetThumbnail);
    }

    private void SetThumbnail(Sprite _sprite)
    {
        ImgTumbnail.sprite = _sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Background.color = ColorHover;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TestApi.DownloadModel(m_Model.Uid);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Background.color = ColorDefault;
    }
}
