using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    private PlayerCam m_playerCam;
    private CharacterController m_characterController;
    private SphereCollider m_sphereBruit;
    private PauseButton m_zawarudo;

    [SerializeField, Tooltip("Sphere radius marche")]
    private float m_sphereRadWalk = 1f;

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

    [SerializeField, Tooltip("La gravit� gravit� terrestre : -9,8 m/s�")]
    private float m_gravity = -9.81f;

    [SerializeField, Tooltip("Hauteur du saut en m")]
    private float m_jumpHeight = 2.5f;

    [SerializeField, Tooltip("Co�t du saut en stamina")]
    private float m_jumpCost = 10;

    [SerializeField, Tooltip("Rotation degr� par seconde")]
    private float m_rotateSpeed = 90;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour le sprint (1 = vitesse de base) Float ")]
    private float m_speedMulti = 2f;

    [SerializeField, Tooltip("Co�t par seconde du sprint")]
    private float m_sprintCost = 5f;

    [SerializeField, Tooltip("Multiplicateur de vitesse pour la marche arri�re (1 = vitesse de base // 0.5 = 50% de la vitesse de base) Float ")]
    private float m_speedBackReduce = 0.5f;

    [SerializeField, Tooltip("Quantit� d'endurance maximal")]
    private float m_stamMax = 100f;

    [SerializeField, Tooltip("Quantit� d'endurance Float au start")]
    private float m_stam = 100f;

    private Transform m_groundCheck;

    [SerializeField, Tooltip("Float gerant le radius de la sphere GroundCheck")]
    private float m_groundCheckRange = 0.5f;

    [SerializeField, Tooltip("LayerMask du sol")]
    private LayerMask m_groundMask;

    // Calcule l'effet de la gravit� dans un nouveau vecteur
    private Vector3 m_gravityEffect;

    // Initialise la variable de mouvement
    private Vector3 movement = new Vector3(0, 0, 0);

    [SerializeField, Tooltip("Temps en seconde avant de commencer a regen de la stamina sans bouger")]
    private float m_secondeBeforeRegen = 2f;

    [SerializeField, Tooltip("Pourcentage de regen naturelle de la stamina quand on ne bouge pas en pourcentage")]
    private float m_pourcentageRegStam = 20f;

    [SerializeField, Tooltip("Quantit� de stamina regen par seconde quand on ne bouge pas")]
    private float m_stamPerSec = 5f;

    [SerializeField, Tooltip("Pourcentage de stamina regen en marche 0.5 = 50 %")]
    private float m_stamPercentWalk = 0.5f;

    // temps passer immobile initialiser a 0 car bah voila
    private float m_timePassed = 0;

    //Pour �vit� la surd�pense de stam pour le saut
    private bool m_jumped = false;

    //D�termine l'augmentation de la vitesse chaque seconde;
    private float m_speedAugmentPerSec;

    //D�termine la diminution de la vitesse chaque seconde;
    private float m_speedReducePerSec;


    [SerializeField, Tooltip("Barre de stamina")]
    private StamBarre m_stamBarre;

    [SerializeField, Tooltip("Active/D�sactive le recadrage de la cam�ra lors du mouvement ou pas l'utilisation du clic droit")]
    private bool m_activateCameraRedirection = false;

    [SerializeField, Tooltip("Combien de point d'endurance seront r�g�n�r� lors qu'on d�vore un chasseur")]
    private float m_regenOnKill = 100;

    [SerializeField, Tooltip("Nombre de balle que le joueur peut encaisser sans mourir")]
    private int m_lifeMax = 3;

    // vie actuelle du joueur
    private int m_life;

    [SerializeField, Tooltip("Chance que le joueur a d'esquiv� (30 = 30 % etc ...)")]
    private int m_dodge = 30;

    // Timer pour empecher l'actualisation  du radius de la sphere de son via sa fonction private
    private float m_timer = 0;

    // Si le dernier mouvement du joueur est avant ou arri�re
    private bool m_forward;

    // Class contenant les input du joueur
    private PlayerInput playerInput;

    public delegate void m_spawnDelegate();
    public static event m_spawnDelegate m_spawnCp;

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

    /// <summary>
    /// #_Start
    /// </summary>
    /// 
    /// Initialisation du max de la barre de stamina avec le max
    /// Initialisation de la vitesse de base sur la minimal
    /// Calcule de la d�c�l�ration selon la diff�rence entre le min et le max et la p�riode de temps necessaire
    ///
    /// V�rification de la pr�sence des gameobject et leur r�cup�ration
    private void Start()
    {
        m_stamBarre.setMaxStam((int)Mathf.Round(m_stamMax));

        m_speed = m_speedMin;

        m_speedAugmentPerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMax;
        m_speedReducePerSec = (m_speedMax - m_speedMin) / m_timeForSpeedMin;

        m_life = m_lifeMax;


        //V�rif
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
            m_sphereBruit = GetComponentInChildren<SphereCollider>();
            if (m_sphereBruit == null)
            {
                Debug.Log("Tardos il manque la sph�re  MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_playerCam == null)
        {
            m_playerCam = GetComponentInChildren<PlayerCam>();
            if (m_playerCam == null)
            {
                Debug.Log("Tardos il manque la m_playerCam  MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_groundCheck == null)
        {
            //Debug.Log("R�cup Ground check");
            m_groundCheck = this.gameObject.transform.GetChild(3);
            if (m_groundCheck == null)
            {
                Debug.Log("Tardos il manque la m_groundCheck  MERCI");
                throw new System.ArgumentNullException();
            }
        }

        if (m_zawarudo == null)
        {
            m_zawarudo = GetComponent<PauseButton>();
            if (m_zawarudo == null)
            {
                Debug.Log("Tardos il manque le script Pause MERCI");
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
    /// #_D�tection du sol
    /// </summary>
    /// 
    /// Cr�e une sphere invisible qui d�tecte le sol (Dans le layer "Ground") 
    /// Applique une gravit� de -1 en permanence quand on est pour �vit� des probl�me pendant le d�placement et une cumlation infinie de celle-ci
    /// <returns>Retourne true si un sol a �tait d�tect� et sinon false</returns>
    public bool isGrounded()
    {
        // Cr�e une sphere invisible et check si elle colide avec un layer "Ground"
        bool isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundCheckRange, m_groundMask);

        // Force reset de la gravit� a -1
        if (isGrounded && m_gravityEffect.y < 0)
        {

            m_gravityEffect.y = -2f;
        }

        return isGrounded;
    }

    /// <summary>
    /// #_Mouvement avant & arri�re
    /// </summary>
    /// 
    /// Sprint
    /// 
    /// Quand le joueur presse la touche de Sprint, se dirige en avant et poss�de de l'endurance sup�rieur au co�t du sprint par seconde ajoute un multiplicateur a sa vitesse
    /// Enregistre aussi la derni�re direction prise (Utilis� dans la d�c�l�ration)
    /// Augmente la port�e a la quelle le joueur est entendu
    /// 
    /// WIP : Cam�ra controle
    /// 
    /// Walk
    /// 
    /// Selon la touche Avancer ou Reculer calcule le d�placement du joueur
    /// En avant vitesse normal en arri�re un multiplicateur et ajouter pour ralentir le joueur
    /// 
    /// Enregistre aussi la direction pour la d�c�l�ration
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>
    private void movementY(Vector2 p_move)
    {
        if (playerInput.Player.Sprint.IsPressed() && m_stam > 0)
        {
            if (p_move.y > 0)
            {
                m_timePassed = 0;

                m_stam -= m_sprintCost * Time.deltaTime;
                movement = transform.forward * p_move.y * (m_speed * m_speedMulti);

                // Modification de la port�e du son
                sphereRadiusModify(m_sphereRadRun);
                // enregistre la direction "Avant"
                m_forward = true;

                //Wip controle cam�ra
                if (!playerInput.Player.RightClick.IsPressed() && m_activateCameraRedirection)
                {
                    m_playerCam.cameraResetAngle();
                }
                else
                {
                    m_playerCam.resetOnRedirect();
                }
            }

        }
        else
        {
            m_playerCam.resetOnRedirect();

            if (p_move.y >= 0.6f)
            {
                movement = transform.forward * p_move.y * m_speed;
                m_forward = true;
            }
            else if (p_move.y <= -0.6f)
            {
                movement = transform.forward * p_move.y * (m_speed * m_speedBackReduce);
                m_forward = false;
            }
            sphereRadiusModify(m_sphereRadWalk);
        }
    }

    /// <summary>
    /// #_Fonction de saut
    /// </summary>
    /// 
    /// Si le joueur poss�de assez d'endurance (sup�rieur ou �gal a celle du co�t du saut) effectue un saut
    /// Calcule : Hauteur * la gravit� * -2 (Gravit� *-2 pour invers� la gravit�)
    /// 
    /// Aucun air controle
    /// m_jumped permet de faire un seul saut (�vite la d�pense inutile d'endurance) et retire l'endurance
    /// 
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>
    private void jump(Vector2 p_move)
    {
        if (playerInput.Player.Jump.IsPressed() && m_stam >= m_jumpCost)
        {
            m_timePassed = 0;

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
    /// Acc�l�ration du joueur lors de son d�placement (peut �tre passer sur u nsyst�me de v�locit� plus tard a voir) 
    /// 
    /// Quand le joueur arr�te de se d�placer sa vitesse freine en X m par seconde (calcul� dans le start ou avec une fonction a part) permettant un petit moment ou le joueur d�rape 
    /// Si le dernier mouvement du joueur et de reculer ou avancer change la formule du d�placement : ici dans movement = transform.forward * 1 * m_speed; le 1 remplace p_move.y et le -1 en cas de marche arri�re
    /// Si le joueur fait une rotation en m�me temps applique le m�me calcule mais en r�duisant la vitesse 
    /// 
    /// <param name="p_move"> Vector 2 du mouvement (ZQSD) venant de l'update </param>

    private void stamAndSpeedControl(Vector2 p_move)
    {
        //Stam regen
        if (m_stam < m_pourcentageRegStam) m_timePassed += Time.deltaTime;

        if ((p_move.x == 0 && p_move.y == 0) || (p_move.x != 0 && p_move.y == 0))
        {

            if (m_timePassed >= m_secondeBeforeRegen && m_stam <= m_pourcentageRegStam)
            {
                m_stam += m_stamPerSec * Time.deltaTime;
            }

            // D�cc�l�ration du joueur par seconde selon sont dernier d�placement et/ou sa rotation
            if (m_speed > m_speedMin)
            {
                m_speed -= m_speedReducePerSec * Time.deltaTime;
                if (m_forward)
                {
                    if (p_move.x >= 0.6f || p_move.x <= -0.6f)
                    {
                        movement = transform.forward * 1 * m_speed * 0.7f;
                    }
                    else
                    {
                        movement = transform.forward * 1 * m_speed;
                    }

                }
                else
                {
                    if (p_move.x >= 0.6f || p_move.x <= -0.6f)
                    {
                        movement = transform.forward * -1 * (m_speed * m_speedBackReduce) * 0.7f;
                    }
                    else
                    {
                        movement = transform.forward * -1 * (m_speed * m_speedBackReduce);
                    }

                }


            }
            // Arrondie
            else if (m_speed < m_speedMin)
            {
                movement = transform.forward * 0 * (m_speed * m_speedMulti);
                m_speed = m_speedMin;
            }

        }
        // Augmentation de la vitesse
        else
        {
            if (m_timePassed >= m_secondeBeforeRegen && m_stam <= m_pourcentageRegStam)
            {
                m_stam += m_stamPerSec * m_stamPercentWalk * Time.deltaTime;
            }

            if (m_speed < m_speedMax && p_move.y != 0)
            {
                m_speed += m_speedAugmentPerSec * Time.deltaTime;
            }
            // Arrondie
            else if (m_speed > m_speedMax)
            {
                m_speed = m_speedMax;
            }
        }
    }

    /// <summary>
    /// #_Update
    /// </summary>
    /// 
    /// R�cup�ration des d�placement en ZQSD de la classe PlayerInput
    /// 
    /// Si le joueur est bien sur un props dans le layer Ground active la rotation et appelle les 3 fonction li� au mouvement
    /// Sinon applique la gravit� et reset le "compteur" de saut
    /// 
    /// Arrondie la valeur de l'endurance a l'entier le plus proche et l'envoi dans la barre d'endurance (Temporaire)
    /// 
    /// D�placement du joueur et ensuite applique la gravit� (s�par� pour �vit� quelque probl�me de compr�hension ou autre)
    void Update()
    {
        Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();

        if (isGrounded())
        {
            // Rotation du joueur
            m_characterController.transform.Rotate(0, move.x * m_rotateSpeed * Time.deltaTime, 0);


            movementY(move);
            jump(move);
            // Gestion de l'endurance et des acc�l�ration d�cc�l�ration
            stamAndSpeedControl(move);

        }
        else
        {
            // applique la gravit� quand le joueur n'est pas au sol
            m_gravityEffect.y += m_gravity * Time.deltaTime;
            m_jumped = false;
        }

        m_stamBarre.setStam((int)Mathf.Round(m_stam));

        //Application du mouvement
        m_characterController.Move(movement * Time.deltaTime);
        //Application de la gravit�
        m_characterController.Move(m_gravityEffect * Time.deltaTime);

        //Debug.Log(m_stam);
    }
    /// <summary>
    /// #_IdBoxSon fonction (private)
    /// </summary>
    /// 
    /// Actualise le radius de la sphere de d�tection du son
    /// 
    /// Si un timer est actuellement init emp�che le son de s'actualiser et fait baisser le timer
    /// 
    /// Le timer est d�fini dans la surcharge public de cette fonction
    /// 
    /// <param name="p_radius">float d�finissant le radius de la sph�re de d�tection</param>
    private void sphereRadiusModify(float p_radius)
    {
        if (m_timer <= 0)
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
    /// <summary>
    /// #_IdBoxSon fonction (public) Surcharge
    /// </summary>
    /// 
    /// Surcharge public de la fonction pour le bruit
    /// 
    /// Un timer et la pour faire en sorte que le bruit ne osit pas directement �craser, merci de mettre une valeur sup�rieur a z�ro
    /// 
    /// <param name="p_radius">float d�finissant le radius de la sph�re de d�tection</param>
    /// <param name="p_timer">float temps pendant laquel la bruit est audible (en seconde)</param>
    public void sphereRadiusModify(float p_radius, float p_timer)
    {
        m_sphereBruit.radius = p_radius;
        m_timer = p_timer;
    }

    /// <summary>
    /// #_Attaque du joueur
    /// </summary>
    /// 
    /// Si la condition pour tuer est valid� alors on renvoi l'information
    /// 
    /// Condition actuellement "�tre en saut"
    /// 
    /// <returns>Bool qui renvoi si oui ou non l'attaque est autoris�</returns>
    public bool attackPrey(float p_regen)
    {
        if (m_jumped)
        {
            m_stam += p_regen;
            if (m_stam > m_stamMax)
            {
                m_stam = m_stamMax;
            }
            m_stamBarre.setStam((int)Mathf.Round(m_stam));

            m_spawnCp();

            return true;
        }
        return false;
    }

    public float getStam()
    {
        return m_stam;
    }
    public float setStam(float p_stam)
    {
        m_stam += m_regenOnKill;
        if (m_stam > m_stamMax)
        {
            m_stam = m_stamMax;
        }
        m_stamBarre.setStam((int)Mathf.Round(m_stam));
        return m_stam;
    }

    public void beHit(bool p_deathRange)
    {
        if (p_deathRange)
        {
            m_life = 0;
        }
        else
        {
            if (Random.Range(0, 100) > m_dodge)
            {
                m_life--;
            }
        }

        if (m_life == 0)
        {
            //m_zawarudo.isPaused();

            // Temporaire
            SceneManager.LoadScene(0);
        }
        
        
    }
}