using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Tooltip("Charact�re controleur du joueur")] 
    private CharacterController m_characterController;

    [SerializeField, Tooltip("Vitesse du joueur en m�tre par seconde")] 
    private float m_speed = 5f;

    [SerializeField, Tooltip("La gravit� gravit� terrestre : 9,8 m/s�")]
    private float m_gravity = 9.8f;

    [SerializeField, Tooltip("Rotation degr� par seconde")]
    private float m_rotateSpeed = 45;

    private float m_gravityCalcul = 0;


    void Update()
    {
        m_characterController.transform.Rotate(0, Input.GetAxis("Horizontal") * m_rotateSpeed * Time.deltaTime, 0);

        Vector3 movement = transform.forward * Input.GetAxis("Vertical") * m_speed;


        m_gravityCalcul -= m_gravity + Time.deltaTime;

        movement.y = m_gravityCalcul;

        m_characterController.Move(movement * Time.deltaTime);
    }
}
