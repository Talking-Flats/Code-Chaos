using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthController health = other.GetComponent<HealthController>();
            if(health != null)
            {
                Debug.Log("Отправляю урон");
                health.takeDamage(20);
            }
        }
    }
}
