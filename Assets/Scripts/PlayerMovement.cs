using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // ������ ������ ����������� ���������

    public float playerSpeed = 12f; //�������� ��������
    public float boost = 8f; //���������
    public float gravity = -9.81f; //�������� �������
    public float JumpHeight = 3.0f; //������ ������

    float speed; // �������� �������� ������

    public Transform groundCheck; // ������ ������, ������� ����� ��������� ������� ����� ��� ������
    public float groundDistance = 0.4f;
    public LayerMask groundMask; // ����� �������, ����������� ������

    Vector3 velocity;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //�������� �� ������� � �����

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = playerSpeed + boost; // ����������� �������� �������� ��� ������� "Shift"
        }else
        {
            speed = playerSpeed;
        }
        //---------���� �������� ������------------
        float x = Input.GetAxis("Horizontal"); // ��������� �������� x
        float z = Input.GetAxis("Vertical"); // ��������� �������� Y

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime); // �������� ������ �� ���� x � z

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
        }
        //-----------------------------------------

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
