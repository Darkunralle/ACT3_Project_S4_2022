using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("Charact�re controleur du joueur")] 
    private CharacterController m_characterController;

    [SerializeField, Tooltip("Vitesse max m/s")]
    private float m_speedmax = 5f;

    [SerializeField, Tooltip("Vitesse minimal m/s")] 
    private float m_speedmin = 2f;

    [SerializeField, Tooltip("Temps avant d'atteindre la vitesse maxium en seconde")]
    private float m_timeForSpeedMax = 2f;

    //Speed acutelle du joueur
    private float m_speed = 5f;

    [SerializeField, Tooltip("La gravit� gravit� terrestre : -9,8 m/s�")]
    private float m_gravity = -9.8f;

    [SerializeField, Tooltip("Hauteur du saut en m")]
    private float m_jumpHeight = 2.5f;

    [SerializeField, Tooltip("Rotation degr� par seconde")]
    private float m_rotateSpeed = 45;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 1.5f;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour la marche arri�re (1 = vitesse de base // 0.5 = 50% de la vitesse de base) Float ")]
    private float m_speedBackReduce = 0.5f;

    [SerializeField, Tooltip("Barre d'endurance Float ")]
    private float m_stamBarre = 100f;

    [SerializeField, Tooltip("Sphere invisible pour d�tecter la collision avec le bloc (Mettre l'empty Groundcheck)")]
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

    [SerializeField, Tooltip("Quantit� de stamina regen par seconde quand on ne bouge pas")]
    private float m_stamPerSec = 2f;

    private float m_timePassed = 0;

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
        // Cr�e une sphere invisible et check si elle colide avec un layer "Ground"
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundCheckRange, m_groundMask);

        // Reset de la force de gravit� au sol (Pour ne pas qu'elle augmente de mani�re idiote une fois au sol)
        // -1 pour �tre sur qu'il reste au sol
        if (m_isGrounded && m_gravityEffect.y < 0)
        {

            m_gravityEffect.y = -1f;
        }

        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        // V�rifie si le joueur est au sol pour empecher l'air control et pouvoir suater
        if (m_isGrounded)
        {
            // Rotation du joueur
            m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);         

            //activaction si input.pressed et stam sup � 0
            if (playerInput.Player.Sprint.IsPressed() && m_stamBarre > 0)
            {
                
                if(move.y != -1)
                {
                    m_stamBarre -= 5 * Time.deltaTime;
                    movement = transform.forward * move.y * (m_speed * m_speedMulti);
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
                
            }
            /*
            //activation si !input.pressed = rechargement de la stam /!\ Temporaire en attendant la m�canique de MANGER /!\
            if (!playerInput.Player.Sprint.IsPressed() && m_stamBarre < 20)
            {
                m_stamBarre += 5 * Time.deltaTime;
            }*/

            //Ajoute une force oppos� via une equation pour sauter quand le jouer est au sol
            if (playerInput.Player.Jump.IsPressed())
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
                
            }

            //Si on bouge pas regen stam ou bout de x temp
            if (move.x == 0 && move.y == 0)
            {
                if (m_stamBarre < m_pourcentageRegStam)
                {
                    m_timePassed += Time.deltaTime;
                    if (m_timePassed >= m_secondeBeforeRegen)
                    {
                        m_stamBarre += m_stamPerSec * Time.deltaTime;
                    }
                }
                
            }
            else
            {
                m_timePassed = 0;

            }
        }
        else
        {
            // applique la gravit� quand le joueur n'est pas au sol
            m_gravityEffect.y += m_gravity * Time.deltaTime;
        }

        // Arrondie de la stam a l'unit�
        Mathf.Round(m_stamBarre);

        Debug.Log(move.y);

        //Application du mouvement
        m_characterController.Move(movement * Time.deltaTime);
        //Application de la gravit�
        m_characterController.Move(m_gravityEffect * Time.deltaTime);
    }
}
