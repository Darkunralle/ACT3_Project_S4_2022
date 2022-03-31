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
    private float m_sphereRadWalk = 2f;

    [SerializeField, Tooltip("Sphere radius course")]
    private float m_sphereRadRun = 8f;

    [SerializeField, Tooltip("Vitesse max m/s")]
    private float m_speedMax = 8f;

    [SerializeField, Tooltip("Vitesse minimal m/s")] 
    private float m_speedMin = 2f;

    [SerializeField, Tooltip("Temps avant d'atteindre la vitesse maxium en seconde")]
    private float m_timeForSpeedMax = 2f;

    [SerializeField, Tooltip("Temps avant de revenir a la vitesse minimal")]
    private float m_timeForSpeedMin = 0.4f;

    //Speed atuelle du joueur
    private float m_speed;

    [SerializeField, Tooltip("La gravité gravité terrestre : -9,8 m/s²")]
    private float m_gravity = -9.81f;

    [SerializeField, Tooltip("Hauteur du saut en m")]
    private float m_jumpHeight = 2.5f;

    [SerializeField, Tooltip("Coût du saut en stamina")]
    private float m_jumpCost = 25;

    [SerializeField, Tooltip("Rotation degré par seconde")]
    private float m_rotateSpeed = 90;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 2f;

    [SerializeField, Tooltip("Coût par seconde du sprint")]
    private float m_sprintCost = 5f;

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

    private Vector3 m_gravityEffect;

    private Vector3 movement = new Vector3(0, 0, 0);

    [SerializeField, Tooltip("Temps en seconde avant de commencer a regen de la stamina sans bouger")]
    private float m_secondeBeforeRegen = 2f;

    [SerializeField, Tooltip("Pourcentage de regen naturelle de la stamina quand on ne bouge pas en pourcentage")]
    private float m_pourcentageRegStam = 25f;

    [SerializeField, Tooltip("Quantité de stamina regen par seconde quand on ne bouge pas")]
    private float m_stamPerSec = 5f;

    private float m_timePassed = 0;

    //Pour évité la surdépense de stam pour le saut
    private bool m_jumped = false;

    //Détermine l'augmentation de la vitesse chaque seconde;
    private float m_speedAugmentPerSec ;

    //Détermine la diminution de la vitesse chaque seconde;
    private float m_speedReducePerSec;


    [SerializeField, Tooltip("Barre de stamina")]
    private StamBarre m_stamBarre;

    [SerializeField, Tooltip("Active/Désactive le recadrage de la caméra lors du mouvement ou pas l'utilisation du clic droit")]
    private bool m_activateCameraRedirection = false;

    private float m_timer = 0;

    private bool m_forward;

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
        m_speedReducePerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMin;


        //Vérif
        if (m_characterController == null)
        {
            m_characterController = GetComponent<CharacterController>();
            if (m_characterController == null)
            {
                Debug.Log("Tardos il manque le CharactereController MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_sphereBruit == null)
        {
            m_sphereBruit = GetComponent<SphereCollider>();
            if (m_sphereBruit == null)
            {
                Debug.Log("Tardos il manque la sphère  MERCI");
                throw new System.ArgumentNullException();
            }
        }
    }


    private void refreshSpeedAugmentOrReduce(bool p_augment)
    {
        if (p_augment)
        {
            m_speedAugmentPerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMax;
        }
        else
        {
            m_speedReducePerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMin;
        }
    }

    /// <summary>
    /// #_Détection du sol
    /// </summary>
    /// 
    /// Crée une sphere invisible qui détecte le sol (Dans le layer "Ground") 
    /// Applique une gravité de -1 en permanence quand on est pour évité des problème pendant le déplacement et une cumlation infinie de celle-ci
    /// <returns>Retourne true si un sol a était détecté et sinon false</returns>
    private bool isGrounded()
    {
        // Crée une sphere invisible et check si elle colide avec un layer "Ground"
        bool p_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundCheckRange, m_groundMask);

        // Force reset de la gravité a -1
        if (p_isGrounded && m_gravityEffect.y < 0)
        {

            m_gravityEffect.y = -1f;
        }

        return p_isGrounded;
    }

    /// <summary>
    /// #_Mouvement avant & arrière
    /// </summary>
    /// 
    /// Sprint
    /// 
    /// Quand le joueur presse la touche de Sprint, se dirige en avant et possède de l'endurance supérieur au coût du sprint par seconde ajoute un multiplicateur a sa vitesse
    /// Enregistre aussi la dernière direction prise (Utilisé dans la décélération)
    /// Augmente la portée a la quelle le joueur est entendu
    /// 
    /// WIP : Caméra controle
    /// 
    /// Walk
    /// 
    /// Selon la touche Avancer ou Reculer calcule le déplacement du joueur
    /// En avant vitesse normal en arrière un multiplicateur et ajouter pour ralentir le joueur
    /// 
    /// Enregistre aussi la direction pour la décélération
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>
    private void movementY(Vector2 p_move)
    {
        if (playerInput.Player.Sprint.IsPressed() && m_stam > 0)
        {
            if (p_move.y > 0)
            {
                m_stam -= m_sprintCost * Time.deltaTime;
                movement = transform.forward * p_move.y * (m_speed * m_speedMulti);

                // Modification de la portée du son
                sphereRadiusModify(false, m_sphereRadRun, 0);
                // enregistre la direction "Avant"
                m_forward = true;

                //Wip controle caméra
                if (!playerInput.Player.RightClick.IsPressed() && m_activateCameraRedirection)
                {
                    m_playerCam.cameraResetAngle();
                }
            }

        }
        else
        {
            if (p_move.y == 1)
            {
                movement = transform.forward * p_move.y * m_speed;
                m_forward = true;
            }
            else if (p_move.y == -1)
            {
                movement = transform.forward * p_move.y * (m_speed * m_speedBackReduce);
                m_forward = false;
            }
            sphereRadiusModify(false, m_sphereRadWalk, 0);
        }
    }

    /// <summary>
    /// #_Fonction de saut
    /// </summary>
    /// 
    /// Si le joueur possède assez d'endurance (supérieur ou égal a celle du coût du saut) effectue un saut
    /// Calcule : Hauteur * la gravité * -2 (Gravité *-2 pour inversé la gravité)
    /// 
    /// Aucun air controle
    /// m_jumped permet de faire un seul saut (évite la dépense inutile d'endurance) et retire l'endurance
    /// 
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>
    private void jump(Vector2 p_move)
    {
        if (playerInput.Player.Jump.IsPressed() && m_stam >= m_jumpCost)
        {
            m_gravityEffect.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);

            if (!m_jumped)
            {
                m_stam -= m_jumpCost;
                m_jumped = true;
            }
        }
    }

    /// <summary>
    /// #_Gestion de l'endurance et de la vitesse
    /// </summary>
    /// 
    /// Quand le joueur ne bouge plus regen de l'endurance jusqu'a une certaine valeur quand il bouge pas pendant X Seconde
    /// 
    /// ~WIP
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>

    private void stamAndSpeedControl(Vector2 p_move)
    {
        if (p_move.x == 0 && p_move.y == 0)
        {
            //Stam regen
            if (m_stam < m_pourcentageRegStam)
            {
                m_timePassed += Time.deltaTime;
                if (m_timePassed >= m_secondeBeforeRegen)
                {
                    m_stam += m_stamPerSec * Time.deltaTime;
                }
            }

            if (m_speed > m_speedMin)
            {
                m_speed -= m_speedReducePerSec * Time.deltaTime;
                if (m_forward)
                {
                    movement = transform.forward * 1 * (m_speed);
                }
                else
                {
                    movement = transform.forward * -1 * (m_speed * m_speedBackReduce);
                }


            }
            else if (m_speed < m_speedMin)
            {
                movement = transform.forward * 0 * (m_speed * m_speedMulti);
                m_speed = m_speedMin;
            }

        }
        else
        {
            m_timePassed = 0;
            if (m_speed < m_speedMax)
            {
                m_speed += m_speedAugmentPerSec * Time.deltaTime;
            }
            else if (m_speed > m_speedMax)
            {
                m_speed = m_speedMax;
            }
        }
    }
    void Update()
    {
        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        if (isGrounded())
        {
            // Rotation du joueur
            m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);


            movementY(move);
            jump(move);
            stamAndSpeedControl(move);

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
        if (p_type)
        {
            m_sphereBruit.radius = p_radius;
            m_timer = p_timer;
        }
        else if (m_timer <= 0)
        {
            m_sphereBruit.radius = p_radius;
        }
        else
        {
            m_timer -= Time.deltaTime;
        }

        if (m_timer < 0)
        {
            m_timer = 0;
        }
        
    }
    
}
