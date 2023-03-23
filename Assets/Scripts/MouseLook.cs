using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Данный скрипт позволяет игроку осметриваться по сторонам
public class MouseLook : MonoBehaviour
{
    public float mouseSens = 100f; //Задаём начальную чувствительность мыши
    public Transform playerBody;    //Создаём объект игрока

    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //Отключаем курсор игрока
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;   //Считываем движение по осям X и Y
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
