using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    private Camera m_camera;

    private PauseButton m_pause;

    [SerializeField, Tooltip("Blocage camaré Vertical en Degré")]
    private int m_blockAngleY = 30;

    [SerializeField, Tooltip("Blocage camaré Horizontal en Degré")]
    private int m_blockAngleX = 60;

    [SerializeField, Tooltip("Réglage de la sensibilité du regard")]
    private float m_sensitivity = 0.5f;

    [SerializeField, Tooltip("Réglage rendant la caméra plus lisse et évité les acoups")]
    private float m_smoothing = 1.5f;

    [SerializeField, Tooltip("Temps necessaire pour recentrer la caméra en sprint (seconde)")]
    private float m_redirecTime = 2f;

    private float m_moveXPerSec;

    private float m_moveYPerSec;

    //[SerializeField, Tooltip("Active les mouvement spécial de la caméra comme les léger mouvement quand on cour")]
    //private bool m_specialCamMovement = false;

    private bool m_inReset = false;

    private Vector2 m_velocity;
    private Vector2 m_frameVelocity;

    private PlayerInput playerInput;

    private bool m_onRedirect = false;

    private Vector2 delta = new Vector2(0, 0);

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

        if (m_pause == null)
        {
            m_pause = GetComponentInParent(typeof(PauseButton)) as PauseButton;
            if (m_pause == null)
            {
                Debug.Log("Tardos il manque le script de pause MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_camera == null)
        {
            m_camera = GetComponent<Camera>();
            if (m_camera == null)
            {
                Debug.Log("Tardos il manque la camera MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (PlayerPrefs.HasKey("sensitive"))
        {
            m_sensitivity = PlayerPrefs.GetFloat("sensitive");
        }
        else
        {
            PlayerPrefs.SetFloat("sensitive", m_sensitivity);
        }


        // Bloque la souris dans la fenêtre
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void look(Vector2 p_delta)
    {
        p_delta.y = -p_delta.y;

        Vector2 rawFrameVelocity = Vector2.Scale(p_delta, Vector2.one * m_sensitivity);
        m_frameVelocity = Vector2.Lerp(m_frameVelocity, rawFrameVelocity, 1 / m_smoothing);
        m_velocity += m_frameVelocity;
        //Debug.Log($"Before clamp : X {m_velocity.y} Y {m_velocity.x}");
        //Lock de la caméra
        m_velocity.y = Mathf.Clamp(m_velocity.y, -m_blockAngleY, m_blockAngleY);
        m_velocity.x = Mathf.Clamp(m_velocity.x, -m_blockAngleX, m_blockAngleX);
        //Debug.Log($"After clamp : X {m_velocity.y} Y {m_velocity.x}");
        transform.localRotation = Quaternion.Euler(m_velocity.y, m_velocity.x, 0);
        //Debug.Log(new Vector2 (m_velocity.y, m_velocity.x));


    }

    private void Update()
    {
        if (!PauseButton.getGameIsPaused())
        {
            if (!m_onRedirect)
            {
                delta = playerInput.Player.Look.ReadValue<Vector2>();
                look(delta);
            }
            else
            {
                Vector2 newVelocity = new Vector2(m_camera.transform.localRotation.eulerAngles.x, m_camera.transform.localRotation.eulerAngles.y);
                Debug.Log($"Raw : X {newVelocity.y} Y {newVelocity.x}");
                if (newVelocity.x > m_blockAngleY)
                {
                    newVelocity.x -= 360;
                }
                if (newVelocity.y > m_blockAngleX)
                {
                    newVelocity.y -= 360;
                }
                m_velocity = new Vector2(newVelocity.y, newVelocity.x);
                Debug.Log($"Before Correctif : X {newVelocity.y} Y {newVelocity.x}");
                Debug.Log($"Correctif : X {m_velocity.y} Y {m_velocity.x}");
                //Debug.Log(new Vector2(m_velocity.y, m_velocity.x));
            }
            //Debug.Log(transform.localRotation);
            //Debug.Log(playerInput.Player.Look.ReadValue<Vector2>());

            if (PlayerPrefs.HasKey("sensitive"))
            {
                m_sensitivity = PlayerPrefs.GetFloat("sensitive");
            }
            else
            {
                Debug.Log("Impossible de trouver les playerprefs lié a la sensibilité ",this);
            }
        }
    }

    public void cameraResetAngle()
    {
        m_onRedirect = true;
        if (!m_inReset)
        {
            redirectionCalcul();
            m_inReset = true;
        }
        Vector3 newRotate = transform.localRotation.eulerAngles;

        if (newRotate.x != 0)
        {
            if (newRotate.x > 180)
            {
                newRotate.x += m_moveYPerSec * Time.deltaTime;
                if (newRotate.x > 359.8f)
                {
                    newRotate.x = 0;
                }
            }
            else
            {
                newRotate.x -= m_moveYPerSec * Time.deltaTime;
                if (newRotate.x < 0.2f)
                {
                    newRotate.x = 0;
                }
            }
        }

        if (newRotate.y != 0)
        {
            if (newRotate.y > 180)
            {
                newRotate.y += m_moveXPerSec * Time.deltaTime;
                if (newRotate.y > 359.8f)
                {
                    newRotate.y = 0;
                }
            }
            else
            {
                newRotate.y -= m_moveXPerSec * Time.deltaTime;
                if (newRotate.y < 0.2f)
                {
                    newRotate.y = 0;
                }
            }
        }
        //Mouse.current.WarpCursorPosition(new Vector2(tempx, tempy));
        transform.localRotation = Quaternion.Euler(newRotate.x, newRotate.y, 0);
    }

    public void resetOnRedirect()
    {
        m_onRedirect = false;
        m_inReset = false;
        look(new Vector2(0, 0));
    }

    private void redirectionCalcul()
    {
        if (transform.localRotation.eulerAngles.y > 180)
        {
            m_moveXPerSec = (360 - transform.localRotation.eulerAngles.y) / m_redirecTime;
        }
        else
        {
            m_moveXPerSec = transform.localRotation.eulerAngles.y / m_redirecTime;
        }

        if (transform.localRotation.eulerAngles.x > 180)
        {
            m_moveYPerSec = (360 - transform.localRotation.eulerAngles.x) / m_redirecTime;
        }
        else
        {
            m_moveYPerSec = transform.localRotation.eulerAngles.x / m_redirecTime;
        }

    }

    private void camSpecialMovement()
    {
    }
}