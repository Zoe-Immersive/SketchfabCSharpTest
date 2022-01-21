using UnityEngine;

/*
Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
Converted to C# 27-02-13 - no credit wanted.
Simple flycam I made, since I couldn't find any others made public.  
Made simple to use (drag and drop, done) for regular keyboard layout  
wasd : basic movement
shift : Makes camera accelerate
space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

public class CameraController : MonoBehaviour
{

    private float m_Speed = 10.0f;
    private float m_ShiftSpeed = 25.0f;
    private float m_MaxSpeed = 1000.0f; 
    private float m_MouseSensitivity = 0.25f;
    private Vector3 m_LastMousePosition = new Vector3(255, 255, 255);
    private float m_TotalRun = 1.0f;

    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        m_LastMousePosition = Input.mousePosition - m_LastMousePosition;
        m_LastMousePosition = new Vector3(-m_LastMousePosition.y * m_MouseSensitivity, m_LastMousePosition.x * m_MouseSensitivity, 0);
        m_LastMousePosition = new Vector3(transform.eulerAngles.x + m_LastMousePosition.x, transform.eulerAngles.y + m_LastMousePosition.y, 0);
        transform.eulerAngles = m_LastMousePosition;
        m_LastMousePosition = Input.mousePosition;

        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_TotalRun += Time.deltaTime;
            p = p * m_TotalRun * m_ShiftSpeed;
            p.x = Mathf.Clamp(p.x, -m_MaxSpeed, m_MaxSpeed);
            p.y = Mathf.Clamp(p.y, -m_MaxSpeed, m_MaxSpeed);
            p.z = Mathf.Clamp(p.z, -m_MaxSpeed, m_MaxSpeed);
        }
        else
        {
            m_TotalRun = Mathf.Clamp(m_TotalRun * 0.5f, 1f, 1000f);
            p = p * m_Speed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { 
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    public void PlaceObjectInFront(GameObject _object)
    {
        _object.transform.position = transform.position + transform.forward * 1;
    }
}
