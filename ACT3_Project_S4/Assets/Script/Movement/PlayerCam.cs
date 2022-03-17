using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField, Tooltip("Transform du joueur")]
    private Transform m_player;

    [SerializeField, Tooltip("Offset de la caméra par rapport au joueur")]
    private Vector3 m_camOffset;

    private void Update()
    {
        if (m_player != null)
        {
            transform.position = m_player.position + m_camOffset;
        }
        else Debug.Log($"Il manque la variable {m_player.name}");

        Vector2 mouse = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        mouse *= Time.deltaTime;

        //Camera rotate (todo)
        //transform.localEulerAngles(mouse.x, mouse.y, 0);
    }
}
