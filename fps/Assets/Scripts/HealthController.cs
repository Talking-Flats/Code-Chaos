using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    [Range(10, 5000)]
    public float hp = 100;
    public float maxHp;

    public TextMeshProUGUI hpText; //Хп текста
    public GameObject deathPanel; //Панель смерти игрока

    public void takeDamage(float damage)
    {
        Debug.Log("taken damage: "+ damage);
        hp -= damage;
        if(gameObject.tag == "Player")
        {
            showHpText();
        }
        if (hp <= 0)
        {
            if (gameObject.tag == "Player")
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                deathPanel.SetActive(true);
                gameObject.GetComponent<PlayerMovment>().enabled = false;
                gameObject.GetComponentInChildren<MouseLook>().enabled = false;

                foreach(MonoBehaviour script in gameObject.GetComponentsInChildren<MonoBehaviour>())
                {
                    script.enabled = false;
                }
            }
            else
            {
                Die();
            }
            
        }
    }
    void showHpText()
    {
        hpText.SetText("HP: " + hp + "/" + maxHp);
    }
    void Die()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        maxHp = hp;
        showHpText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
