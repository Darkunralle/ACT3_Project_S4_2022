using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("Charactère controleur du joueur")] 
    private CharacterController m_characterController;

    [SerializeField, Tooltip("Vitesse du joueur en mètre par seconde")] 
    private float m_speed = 5f;

    [SerializeField, Tooltip("La gravité gravité terrestre : -9,8 m/s²")]
    private float m_gravity = -9.8f;

    [SerializeField, Tooltip("Hauteur du saut en m")]
    private float m_jumpHeight = 5f;

    [SerializeField, Tooltip("Rotation degré par seconde")]
    private float m_rotateSpeed = 45;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 1.5f;

    [SerializeField, Tooltip("Barre d'endurance Float ")]
    private float m_stamBarre = 100f;

    [SerializeField, Tooltip("Sphere invisible pour détecter la collision avec le bloc (Mettre l'empty Groundcheck)")]
    private Transform m_groundCheck;

    [SerializeField, Tooltip("Float gerant le radius de la sphere GroundCheck")]
    private float m_groundCheckRange = 0.4f;

    [SerializeField, Tooltip("LayerMask du sol")]
    private LayerMask m_groundMask;

    private bool m_isGrounded;

    private Vector3 m_gravityEffect;


    private Vector3 movement = new Vector3(0, 0, 0);

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

    


    void Update()
    {
        // Crée une sphere invisible et check si elle colide avec un layer "Ground"
        m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundCheckRange, m_groundMask);

        // Reset de la force de gravité au sol (Pour ne pas qu'elle augmente de manière idiote une fois au sol)
        if (m_isGrounded && m_gravityEffect.y < 0)
        {

            m_gravityEffect.y = -1f;
        }

        // Faire quelque chose pour le sprint a l'arrière
        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        // Vérifie si le joueur est au sol pour empecher l'air control et pouvoir suater
        if (m_isGrounded)
        {
            // Rotation du joueur
            m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);

            // Calcule du mouvement (a opti car il se fait plusieurs fois)
            movement = transform.forward * move.y * m_speed;

            //(m_stamBarre = Mathf.Round(1);) jarrive pas a arrondire la stam

            //activaction si input.pressed et stam sup à 0
            if (playerInput.Player.Sprint.IsPressed() && m_stamBarre > 0)
            {
                m_stamBarre -= 5 * Time.deltaTime;
                movement = transform.forward * move.y * (m_speed * m_speedMulti);
            }

            //activation si !input.pressed = rechargement de la stam /!\ Temporaire en attendant la mécanique de MANGER /!\
            if (!playerInput.Player.Sprint.IsPressed() && m_stamBarre < 20)
            {
                m_stamBarre += 5 * Time.deltaTime;
            }

            //Ajoute une force opposé via une equation pour sauter quand le jouer est au sol
            if (playerInput.Player.Jump.IsPressed())
            {
                m_gravityEffect.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
            }
        }
        else
        {
            // applique la gravité quand le joueur n'est pas au sol
            m_gravityEffect.y += m_gravity * Time.deltaTime;
        }

        //Application du mouvement
        m_characterController.Move(movement * Time.deltaTime);
        //Application de la gravité
        m_characterController.Move(m_gravityEffect * Time.deltaTime);
    }
}
