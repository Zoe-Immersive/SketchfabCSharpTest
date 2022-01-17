using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float Speed;
    public float SpeedH = 2.0f;
    public float SpeedV = 2.0f;

    private float m_Yaw = 0.0f;
    private float m_Pich = 0.0f;

    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += Speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Speed * transform.forward * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Speed * transform.up * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.C))
        {
            transform.position -= Speed * transform.up * Time.deltaTime;
        }

        //m_Pich -= SpeedV * Input.GetAxis("Mouse X");
        //m_Camera.transform.localEulerAngles = new Vector3(45f, m_Pich, 0.0f);
    }

    public void PlaceObjectInFront(GameObject _object)
    {

        _object.transform.position = transform.position + m_Camera.transform.forward * 1;
    }
}
