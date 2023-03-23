using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // Создаём объект контроллера персонажа

    public float playerSpeed = 12f; //Скорость движения
    public float boost = 8f; //Ускорение
    public float gravity = -9.81f; //Скорость падения
    public float JumpHeight = 3.0f; //Высота прыжка

    float speed; // Конечная скорость игрока

    public Transform groundCheck; // Создаём объект, который будет проверять наличие замли под ногами
    public float groundDistance = 0.4f;
    public LayerMask groundMask; // Маска объекта, являющегося землей

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //Проверка на касание с землёй

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = playerSpeed + boost; // Увеличиваем скорость движения при нажатом "Shift"
        }else
        {
            speed = playerSpeed;
        }
        //---------Блок движения игрока------------
        float x = Input.GetAxis("Horizontal"); // Считываем значение x
        float z = Input.GetAxis("Vertical"); // Считываем значение Y

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime); // Движение игрока по осям x и z

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }
        //-----------------------------------------

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
