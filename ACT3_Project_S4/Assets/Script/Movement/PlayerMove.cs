using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("Charactère controleur du joueur")] 
    private CharacterController m_characterController;

    [SerializeField, Tooltip("Vitesse du joueur en mètre par seconde")] 
    private float m_speed = 5f;

    [SerializeField, Tooltip("La gravité gravité terrestre : 9,8 m/s²")]
    private float m_gravity = 9.8f;

    [SerializeField, Tooltip("Rotation degré par seconde")]
    private float m_rotateSpeed = 45;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 1.5f;

    [SerializeField, Tooltip("Barre d'endurance Float ")]
    private float m_stamBarre = 100f;

    private float m_gravityCalcul = 0;


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
        Debug.Log(m_stamBarre);

        // Faire quelque chose pour le sprint a l'arrière
        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);

        // Pbm mystique le else s'appel tjrs même si la condition du if est vérifié
        if (m_characterController.isGrounded)
        {
            //Debug.Log("sol");

            movement = transform.forward * move.y * m_speed;
            
            ///(m_stamBarre = Mathf.Round(1);) jarrive pas a arrondire la stam </summary>
            
            //activaction si input.pressed et stam sup à 0
            if (playerInput.Player.Sprint.IsPressed() && m_stamBarre > 0)
            {
                m_stamBarre -= 5 * Time.deltaTime;
                movement = transform.forward * move.y * (m_speed * m_speedMulti);
            }

            //activation si !input.pressed = rechargement de la stam
            if (!playerInput.Player.Sprint.IsPressed() && m_stamBarre < 20)
            {
                m_stamBarre += 5 * Time.deltaTime;
            }
            
            //petit bon
            if (playerInput.Player.Jump.IsPressed())
            {
                movement = transform.forward * move.x * (m_speed * m_speedMulti);
                //Debug.Log("Jump");
            }

        }
        else
        {
            m_gravityCalcul = 0;
            m_gravityCalcul -= m_gravity;

            movement.y = m_gravityCalcul;
            //Debug.Log("Gravity");
        }

        m_characterController.Move(movement * Time.deltaTime);
    }
}
