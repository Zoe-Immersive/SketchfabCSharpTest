using UnityEngine;
using UnityEngine.UI;

public class UIModel : MonoBehaviour
{
    public Image ImgTumbnail;
    public Text TxtName;
    public Text TxtUID;
    public Text TxtVertextCount;

    public void Init(SketchfabModel _model)
    {
        TxtName.text = _model.Name;
        TxtUID.text = _model.Uid;
        TxtVertextCount.text = _model.VertexCount.ToString();
    }
}
