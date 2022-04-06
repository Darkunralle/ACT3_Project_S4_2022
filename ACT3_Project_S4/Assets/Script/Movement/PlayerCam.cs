using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCam : MonoBehaviour
{
    private Camera m_camera;

    [SerializeField, Tooltip("Blocage camaré Vertical en Degré")]
    private int m_blockAngleY = 30;

    [SerializeField, Tooltip("Blocage camaré Horizontal en Degré")]
    private int m_blockAngleX = 60;

    [SerializeField, Tooltip("Réglage de la sensibilité du regard")]
    private float m_sensitivity = 0.8f;

    [SerializeField, Tooltip("Réglage rendant la caméra plus lisse et évité les acoups")]
    private float m_smoothing = 1.5f;

    [SerializeField, Tooltip("Correction de l'axe X en degré par seconde de la caméra lors du sprint (si l'option dans le joueur est activé")]
    private float m_xCorrection = 30f;

    [SerializeField, Tooltip("Correction de l'axe Y en degré par seconde de la caméra lors du sprint (si l'option dans le joueur est activé")]
    private float m_yCorrection = 10f;

    private Vector2 m_velocity;
    private Vector2 m_frameVelocity;

    private PlayerInput playerInput;

    private bool m_onRedirect = false;

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
        if (m_camera == null)
        {
            m_camera = GetComponent<Camera>();
            if (m_camera == null)
            {
                Debug.Log("Tardos il manque la camera MERCI");
                throw new System.ArgumentNullException();
            }
        }

        // Bloque la souris dans la fenêtre
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (!m_onRedirect)
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
        

        

        Debug.Log(playerInput.Player.Look.ReadValue<Vector2>());
    }

    public void cameraResetAngle()
    {
        m_onRedirect = true;
        Vector3 newRotate = transform.localRotation.eulerAngles;

        if (newRotate.x != 0 )
        {
            if (transform.localRotation.eulerAngles.x > 180)
            {
                newRotate.x += m_yCorrection * Time.deltaTime;
                if (newRotate.x > 359.8f)
                {
                    newRotate.x = 0;
                }
            }
            else
            {
                newRotate.x -= m_yCorrection * Time.deltaTime;
                if (newRotate.x < 0.2f)
                {
                    newRotate.x = 0;
                }
            }
        }

        if (newRotate.y != 0)
        {
            if (transform.localRotation.eulerAngles.y > 180)
            {
                newRotate.y += m_xCorrection * Time.deltaTime;
                if (newRotate.y > 359.8f)
                {
                    newRotate.y = 0;
                }
            }
            else
            {
                newRotate.y -= m_xCorrection * Time.deltaTime;
                if (newRotate.y < 0.2f)
                {
                    newRotate.y = 0;
                }
            }
        }
        //Mouse.current.WarpCursorPosition(new Vector2(0, 0));
        transform.localRotation = Quaternion.Euler(newRotate.x, newRotate.y, 0);
    }

    public void resetOnRedirect()
    {
        m_onRedirect = false;
    }
}
