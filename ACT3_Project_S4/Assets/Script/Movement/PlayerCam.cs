using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField, Tooltip("Transform du joueur")]
    private Transform m_player;

    [SerializeField, Tooltip("Offset de la caméra par rapport au joueur")]
    private Vector3 m_camOffset;

    [SerializeField, Tooltip("Camera")]
    private Camera m_camera;

    [SerializeField, Tooltip("Blocage camaré Vertical en Degré")]
    private int m_blockAngleY = 30;

    [SerializeField, Tooltip("Blocage camaré Horizontal en Degré")]
    private int m_blockAngleX = 60;

    public float m_sensitivity = 0.8f;
    public float m_smoothing = 1.5f;

    private Vector2 m_velocity;
    private Vector2 m_frameVelocity;

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Start()
    {
        // Bloque la souris dans la fenêtre
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 delta = playerInput.Player.Look.ReadValue<Vector2>();

        Vector2 rawFrameVelocity = Vector2.Scale(delta, Vector2.one * m_sensitivity);
        m_frameVelocity = Vector2.Lerp(m_frameVelocity, rawFrameVelocity, 1 / m_smoothing);
        m_velocity += m_frameVelocity;

        //Lock de la caméra
        m_velocity.y = Mathf.Clamp(m_velocity.y, -m_blockAngleY, m_blockAngleY);
        m_velocity.x = Mathf.Clamp(m_velocity.x, -m_blockAngleX, m_blockAngleX);

        transform.localRotation = Quaternion.Euler(-m_velocity.y, m_velocity.x, 0);
    }
}
