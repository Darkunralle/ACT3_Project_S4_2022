using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody playerRigid;

    [SerializeField] private float speed = 5f;

    void Update()
    {
        /*
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);

        playerRigid.velocity = new Vector3(targetVelocity.x, playerRigid.velocity.y, targetVelocity.y);

        playerRigid.

        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        playerRigid.Move(move * speed * Time.deltaTime);*/
    }
}
