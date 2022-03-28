using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("Camera du joueur")]
    private PlayerCam m_playerCam;

    [SerializeField, Tooltip("Charactère controleur du joueur")] 
    private CharacterController m_characterController;

    [SerializeField, Tooltip("Sphere de détection des bruits du joueurs")]
    private SphereCollider m_sphereBruit;

    [SerializeField, Tooltip("Sphere radius marche")]
    private float m_sphereRadWalk;

    [SerializeField, Tooltip("Sphere radius course")]
    private float m_sphereRadRun;

    [SerializeField, Tooltip("Vitesse max m/s")]
    private float m_speedMax = 5f;

    [SerializeField, Tooltip("Vitesse minimal m/s")] 
    private float m_speedMin = 2f;

    [SerializeField, Tooltip("Temps avant d'atteindre la vitesse maxium en seconde")]
    private float m_timeForSpeedMax = 2f;

    //Speed acutelle du joueur
    private float m_speed;

    [SerializeField, Tooltip("La gravité gravité terrestre : -9,8 m/s²")]
    private float m_gravity = -9.8f;

    [SerializeField, Tooltip("Hauteur du saut en m")]
    private float m_jumpHeight = 2.5f;

    [SerializeField, Tooltip("Coût du saut en stamina")]
    private float m_jumpCost = 25;

    [SerializeField, Tooltip("Rotation degré par seconde")]
    private float m_rotateSpeed = 45;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 1.5f;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour la marche arrière (1 = vitesse de base // 0.5 = 50% de la vitesse de base) Float ")]
    private float m_speedBackReduce = 0.5f;

    [SerializeField, Tooltip("Quantité d'endurance Float ")]
    private float m_stam = 100f;

    [SerializeField, Tooltip("Sphere invisible pour détecter la collision avec le bloc (Mettre l'empty Groundcheck)")]
    private Transform m_groundCheck;

    [SerializeField, Tooltip("Float gerant le radius de la sphere GroundCheck")]
    private float m_groundCheckRange = 0.4f;

    [SerializeField, Tooltip("LayerMask du sol")]
    private LayerMask m_groundMask;

    private bool m_isGrounded;

    private Vector3 m_gravityEffect;

    private Vector3 movement = new Vector3(0, 0, 0);

    [SerializeField, Tooltip("Temps en seconde avant de commencer a regen de la stamina sans bouger")]
    private float m_secondeBeforeRegen = 2f;

    [SerializeField, Tooltip("Pourcentage de regen naturelle de la stamina quand on ne bouge pas en pourcentage")]
    private float m_pourcentageRegStam = 20f;

    [SerializeField, Tooltip("Quantité de stamina regen par seconde quand on ne bouge pas")]
    private float m_stamPerSec = 2f;

    private float m_timePassed = 0;

    //Pour évité la surdépense de stam pour le saut
    private bool m_jumped = false;

    //Détermine l'augmentation de la vitesse chaque seconde;
    private float m_speedAugmentPerSec ;


    [SerializeField, Tooltip("Barre de stamina")]
    private StamBarre m_stamBarre;

    [SerializeField, Tooltip("Active/Désactive le recadrage de la caméra lors du mouvement ou pas l'utilisation du clic droit")]
    private bool m_activateCameraRedirection = false;

    private float m_timer = 0;

    // Class contenant les input du joueur
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

    private void Start()
    {
        m_stamBarre.setMaxStam((int)Mathf.Round(m_stam));

        m_speed = m_speedMin;

        m_speedAugmentPerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMax;

        if (m_characterController == null)
        {
            m_characterController = GetComponent<CharacterController>();
            if (m_characterController == null)
            {
                Debug.Log("Tardos il manque le CharactereController MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_characterController == null)
        {
            m_characterController = GetComponent<CharacterController>();
            if (m_characterController == null)
            {
                Debug.Log("Tardos il manque le CharactereController MERCI");
                throw new System.ArgumentNullException();
            }
        }
    }




    void Update()
    {
        // Crée une sphere invisible et check si elle colide avec un layer "Ground"
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundCheckRange, m_groundMask);

        // Reset de la force de gravité au sol (Pour ne pas qu'elle augmente de manière idiote une fois au sol)
        // -1 pour être sur qu'il reste au sol
        if (m_isGrounded && m_gravityEffect.y < 0)
        {

            m_gravityEffect.y = -1f;
        }

        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        // Vérifie si le joueur est au sol pour empecher l'air control et pouvoir suater
        if (m_isGrounded)
        {
            // Rotation du joueur
            m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);         

            //activaction si input.pressed et stam sup à 0
            if (playerInput.Player.Sprint.IsPressed() && m_stam > 0)
            {
                
                if(move.y > 0)
                {
                    m_stam -= 5 * Time.deltaTime;
                    movement = transform.forward * move.y * (m_speed * m_speedMulti);
                    sphereRadiusModify(false, m_sphereRadRun, 0);
                    //m_sphereBruit.radius = m_sphereRadRun;

                    if (!playerInput.Player.RightClick.IsPressed() && m_activateCameraRedirection)
                    {
                        m_playerCam.cameraResetAngle();
                    }
                }

            }
            else
            {
                if (move.y != -1)
                {
                    movement = transform.forward * move.y * m_speed;
                }
                else
                {
                    movement = transform.forward * move.y * (m_speed * m_speedBackReduce);
                }
                sphereRadiusModify(false, m_sphereRadWalk, 0);
                //m_sphereBruit.radius = m_sphereRadWalk;

            }

            //Ajoute une force opposé via une equation pour sauter quand le jouer est au sol
            if (playerInput.Player.Jump.IsPressed() && m_stam >= m_jumpCost)
            {
                if (move.y != -1)
                {
                    m_gravityEffect.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                }
                // en travaux
                else
                {
                    move.y = 0;
                    m_gravityEffect.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
                }

                if (!m_jumped)
                {
                    m_stam -= m_jumpCost;
                    m_jumped = true;
                }
                
                
            }

            //Si on bouge pas regen stam ou bout de x temps
            if (move.x == 0 && move.y == 0)
            {
                if (m_stam < m_pourcentageRegStam)
                {
                    m_timePassed += Time.deltaTime;
                    if (m_timePassed >= m_secondeBeforeRegen)
                    {
                        m_stam += m_stamPerSec * Time.deltaTime;
                    }
                }

                m_speed = m_speedMin;
            }
            else
            {   
                m_timePassed = 0;
                if(m_speed < m_speedMax)
                {
                    m_speed += m_speedAugmentPerSec * Time.deltaTime;
                }
                else if (m_speed > m_speedMax)
                {
                    m_speed = m_speedMax;
                }
            }
        }
        else
        {
            // applique la gravité quand le joueur n'est pas au sol
            m_gravityEffect.y += m_gravity * Time.deltaTime;
            m_jumped = false;
        }

        m_stamBarre.setStam((int)Mathf.Round(m_stam));

        //Application du mouvement
        m_characterController.Move(movement * Time.deltaTime);
        //Application de la gravité
        m_characterController.Move(m_gravityEffect * Time.deltaTime);
    }

    public void sphereRadiusModify(bool p_type, float p_radius, float p_timer)
    {
        
        Debug.Log(m_timer);
        if (p_type)
        {
            m_sphereBruit.radius = p_radius;
            m_timer = p_timer;
            Debug.Log("Trap");
        }
        else if (m_timer <= 0)
        {
            m_sphereBruit.radius = p_radius;
        }
        else
        {
            m_timer -= Time.deltaTime;
        }
        
    }
    
}
