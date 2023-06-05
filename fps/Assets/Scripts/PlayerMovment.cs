using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{

    public CharacterController controller;

    public float playerSpeed = 12f;
    public float boost = 8f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    float speed; //Конечная скорость игрока
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;


    Vector3 velocity;

    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position , groundDistance, groundMask);


        if(isGrounded && velocity.y<0){
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = playerSpeed + boost; // Увеличиваем скорость движения при нажатом "Shift"
        }
        else
        {
            speed = playerSpeed;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move*speed*Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity*Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);
    }
}
