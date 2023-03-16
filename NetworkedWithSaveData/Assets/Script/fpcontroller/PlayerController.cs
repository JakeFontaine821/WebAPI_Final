using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController playerController;
    public float speed;
    public float gravity = -9.8f;
    public float jumpHeight = 5.0f;
    Vector3 vel;
    public Transform GC;
    public float groundDist;
    public LayerMask groundMask;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(GC.position, groundDist, groundMask);

        if(isGrounded && vel.y < 0)
        {
            vel.y = -2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        playerController.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            vel.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        vel.y += gravity * Time.deltaTime;
        playerController.Move(vel * Time.deltaTime);
    }
}
