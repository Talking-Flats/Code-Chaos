using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public WeaponController wc;
    public GameObject HitParticle;
    public AudioClip HitSound;

    private void OnTriggerEnter(Collider other){

        if(other.tag == "Enemy" && wc.isAttacking){
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(HitSound);
            Instantiate(HitParticle, new Vector3(other.transform.position.x,transform.position.y,transform.position.z),other.transform.rotation);
        }
    }
}
